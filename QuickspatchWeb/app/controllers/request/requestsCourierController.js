'use strict';
app.controller('RequestsCourierController', ['$q', '$rootScope', '$scope', 'common', 'messageLanguage', '$window', 'masterfileService', '$http', '$timeout', 'requestDataService',
function ($q, $rootScope, $scope, common, messageLanguage, $window, masterfileService, $http, $timeout, requestDataService) {
    $scope.controllerId = "RequestsCourierController";

    $scope.requests = [];
    $scope.courierId = 0;
    $scope.courierName = '';
    function setLocalTime(item) {
        var d = new Date();
        var n = d.getTimezoneOffset();
        var startTime = moment(moment(item.SendingTime).format("YYYY-MM-DD") + " " + moment(item.StartTime).format('HH:mm'));
        var endTime = moment(moment(item.SendingTime).format("YYYY-MM-DD") + " " + moment(item.EndTime).format('HH:mm'));
        item.StartTime = startTime.add(-n, 'minutes');
        item.EndTime = endTime.add(-n, 'minutes');
        item.SendingTime = moment(item.SendingTime).add(-n, 'minutes');
    }

    function setRequestObj(item) {
        return {
            RequestId: item.Id,
            RequestNo: item.RequestNo,
            IsWarning: item.IsWarning,
            TimeNoFormat: item.SendingTime,
            StartTime: item.StartTime,
            EndTime: item.EndTime,
            TravelTime: null,
            LocationFromName: item.LocationFromName,
            LocationFromLat: item.LocationFromLat,
            LocationFromLng: item.LocationFromLng,
            LocationToName: item.LocationToName,
            LocationToLat: item.LocationToLat,
            LocationToLng: item.LocationToLng,
            IsStat: false,
            IsInvalid: false,
            IsUpdate: false,
        }
    }

    $scope.getDistance = function (latlngA, latlngB) {
        var deferred = $q.defer();
        var directionsService = new google.maps.DirectionsService();
        var request = {
            origin: latlngA.latitude + ',' + latlngA.longitude,
            destination: latlngB.latitude + ',' + latlngB.longitude,
            travelMode: google.maps.DirectionsTravelMode.DRIVING
        };
        directionsService.route(request, function (response, status) {
            var condition = status == google.maps.DirectionsStatus.OK;
            condition &= response.routes.length > 0;
            condition &= response.routes[0].legs != undefined && response.routes[0].legs != null;
            condition &= response.routes[0].legs.length > 0;
            condition &= response.routes[0].legs[0].distance != undefined && response.routes[0].legs[0].distance != null;
            condition &= response.routes[0].legs[0].duration != undefined && response.routes[0].legs[0].duration != null;
            if (condition) {
                var data = {
                    distance: response.routes[0].legs[0].distance.value,
                    duration: response.routes[0].legs[0].duration.value,
                    index1: latlngA.index,
                    index2: latlngB.index,
                }
                deferred.resolve(data);
            }
        });
        return deferred.promise;
    }
    function checkConflit() {
        var firstRequest = null, currentRequest = null, lastRequest = null;
        var deferred = $q.defer();
        if ($scope.requests.length > 1) {
            for (var i = 0; i < $scope.requests.length; i++) {
                var objfirstto = null, objcurrentfrom = null, objcurrentto = null, objlastfrom = null, objlastto = null;
                if ($scope.requests[i].RequestId == $scope.$parent.data.Id) {
                    if (($scope.requests[i].EndTime == null || $scope.requests[i].StartTime == null) || $scope.requests[i].LocationFromLat == null || $scope.requests[i].LocationToLat == null) {
                        deferred.resolve($scope.requests);

                        break;
                    } else {
                        objcurrentfrom = { latitude: $scope.requests[i].LocationFromLat, longitude: $scope.requests[i].LocationFromLng };
                        objcurrentto = { latitude: $scope.requests[i].LocationToLat, longitude: $scope.requests[i].LocationToLng };
                        if (i == 0) {
                            objfirstto = null;
                        } else {
                            objfirstto = { latitude: $scope.requests[i - 1].LocationToLat, longitude: $scope.requests[i - 1].LocationToLng };
                        }
                        if ($scope.requests[i + 1] != null) {
                            objlastfrom = { latitude: $scope.requests[i + 1].LocationFromLat, longitude: $scope.requests[i + 1].LocationFromLng };
                            objlastto = { latitude: $scope.requests[i + 1].LocationToLat, longitude: $scope.requests[i + 1].LocationToLng };
                        }


                        if (objfirstto != null) {
                            firstRequest = $scope.requests[i - 1];
                            currentRequest = $scope.requests[i];

                            if (objlastfrom == null) {
                                currentRequest.IsWarning = false;
                                $scope.getDistance(objfirstto, objcurrentfrom).then(function (result) {
                                    $scope.getDistance(objcurrentfrom, objcurrentto).then(function (result2) {
                                        var rankDateTime = null;
                                        rankDateTime = moment(firstRequest.EndTime.format('YYYY-MM-DD HH:mm')),
                                        rankDateTime.add(result.duration + result2.duration, 'seconds');
                                        currentRequest.IsWarning = rankDateTime.format('YYYYMMDDHHmm').localeCompare(currentRequest.EndTime.format('YYYYMMDDHHmm')) > 0;

                                        deferred.resolve($scope.requests);
                                    });
                                });

                                break;
                            } else {
                                lastRequest = $scope.requests[i + 1];
                                currentRequest.IsWarning = false;
                                lastRequest.IsWarning = false;
                                $scope.getDistance(objfirstto, objcurrentfrom).then(function (result) {
                                    $scope.getDistance(objcurrentfrom, objcurrentto).then(function (result2) {
                                        var rankDateTime = null;
                                        rankDateTime = moment(firstRequest.EndTime.format('YYYY-MM-DD HH:mm')),
                                        rankDateTime.add(result.duration + result2.duration, 'seconds');
                                        currentRequest.IsWarning = rankDateTime.format('YYYYMMDDHHmm').localeCompare(currentRequest.EndTime.format('YYYYMMDDHHmm')) > 0;
                                        $scope.getDistance(objcurrentto, objlastfrom).then(function (result3) {
                                            $scope.getDistance(objlastfrom, objlastto).then(function (result4) {
                                                rankDateTime = null;
                                                rankDateTime = moment(currentRequest.EndTime.format('YYYY-MM-DD HH:mm')),
                                                rankDateTime.add(result3.duration + result4.duration, 'seconds');
                                                lastRequest.IsWarning = rankDateTime.format('YYYYMMDDHHmm').localeCompare(lastRequest.EndTime.format('YYYYMMDDHHmm')) > 0;
                                                deferred.resolve($scope.requests);
                                            });
                                        });
                                    });
                                });

                                break;
                            }

                        } else {
                            currentRequest = $scope.requests[i];
                            lastRequest = $scope.requests[i + 1];
                            $scope.getDistance(objcurrentfrom, objcurrentto).then(function (result) {
                                $scope.getDistance(objcurrentto, objlastfrom).then(function (result2) {
                                    var rankDateTime = null;
                                    rankDateTime = moment(currentRequest.EndTime.format('YYYY-MM-DD HH:mm')),
                                    rankDateTime.add(result.duration + result2.duration, 'seconds');
                                    lastRequest.IsWarning = rankDateTime.format('YYYYMMDDHHmm').localeCompare(lastRequest.EndTime.format('YYYYMMDDHHmm')) > 0;
                                    deferred.resolve($scope.requests);
                                });
                            });

                            break;
                        }
                    }
                }
            }


        } else {
            deferred.resolve($scope.requests);
        }
        checkConflit123(0);
        return deferred.promise;
    }

    function checkConflit123(count) {
        var lengthArray = $scope.requests.length;
        if (count == 0 && lengthArray > 0) {
            count = count + 1;
            $scope.requests[0].IsWarning = false;
            return checkConflit123(count);
        }
        if (count >= lengthArray) {
            var grid = $('#requests-courier-grid').data('kendoGrid');
            if (grid != null && grid != undefined) {
                var dataSource = new kendo.data.DataSource({
                    data: $scope.requests
                });
                grid.setDataSource(dataSource);
                grid.refresh();
            }

            return 0;
        }
        if ($scope.requests[count - 1].EndTime == null || $scope.requests[count - 1].EndTime == undefined) {
            var grid1 = $('#requests-courier-grid').data('kendoGrid');
            if (grid1 != null && grid1 != undefined) {
                var dataSource1 = new kendo.data.DataSource({
                    data: $scope.requests
                });
                grid1.setDataSource(dataSource1);
                grid1.refresh();
            }
            return 0;
        }
        if ($scope.requests[count].EndTime == null || $scope.requests[count].EndTime == undefined) {
            var grid1 = $('#requests-courier-grid').data('kendoGrid');
            if (grid1 != null && grid1 != undefined) {
                var dataSource1 = new kendo.data.DataSource({
                    data: $scope.requests
                });
                grid1.setDataSource(dataSource1);
                grid1.refresh();
            }
            return 0;
        }
        var rankDateTime = null;
        
        if ($scope.requests.length > 1 && count < lengthArray) {
            var datenow = new Date();
            if ($scope.requests[count - 1].StartTime._d < datenow) {
                count = count + 1;
                return checkConflit123(count);
            }
      
            if ($scope.requests[count - 1].LocationToLat != null && $scope.requests[count - 1].LocationToLng != null
                && $scope.requests[count].LocationToLat != null && $scope.requests[count].LocationToLng != null) {
                var link = '/Common/GetDistanceGoogle?prevToLat=' + $scope.requests[count - 1].LocationToLat + '&prevToLng=' + $scope.requests[count - 1].LocationToLng +
                    '&currFromLat=' + $scope.requests[count].LocationFromLat + '&currFromLng=' + $scope.requests[count].LocationFromLng;
                $http.get(link).then(function(result) {
                    if (result.data.status == 'OK') {
                        var link1 = '/Common/GetDistanceGoogle?prevToLat=' + $scope.requests[count].LocationFromLat + '&prevToLng=' + $scope.requests[count].LocationFromLng +
                            '&currFromLat=' + $scope.requests[count].LocationToLat + '&currFromLng=' + $scope.requests[count].LocationToLng;
                        $http.get(link1).then(function(result1) {
                            if (result1.data.status == 'OK') {
                                rankDateTime = null;
                                rankDateTime = moment($scope.requests[count - 1].EndTime.format('YYYY-MM-DD HH:mm')),
                                    rankDateTime.add(result.data.rows[0].elements[0].duration.value + result1.data.rows[0].elements[0].duration.value, 'seconds');
                                $scope.requests[count].IsWarning = rankDateTime.format('YYYYMMDDHHmm').localeCompare($scope.requests[count].EndTime.format('YYYYMMDDHHmm')) > 0;
                                count = count + 1;
                                return checkConflit123(count);
                            } else {
                                console.log('Error google.');
                            }
                        });
                    } else {
                        console.log('Error google.');
                    }
                });
            } else {
                count = $scope.requests.length;
                return checkConflit123(count);
            }
        }

    }

    function calculaTravelTime() {
        var deferred = $q.defer();
        var objfirstto = null, objcurrentfrom = null, objcurrentto = null;
        var totalRequest = $scope.requests.length;
        for (var i = 0; i < totalRequest; i++) {
            objcurrentfrom = { latitude: $scope.requests[i].LocationFromLat, longitude: $scope.requests[i].LocationFromLng, index: i };
            objcurrentto = { latitude: $scope.requests[i].LocationToLat, longitude: $scope.requests[i].LocationToLng, index: i };
            if (i > 0) {

                objfirstto = { latitude: $scope.requests[i - 1].LocationToLat, longitude: $scope.requests[i - 1].LocationToLng, index: i - 1 };
                $scope.getDistance(objfirstto, objcurrentfrom).then(function (result) {
                    $scope.getDistance(objcurrentfrom, objcurrentto).then(function (result2) {
                        var currentRequest = $scope.requests[result2.index1];
                        var firstRequest = $scope.requests[result.index1];
                        var rankDateTime = null;
                        rankDateTime = moment(firstRequest.EndTime.format('YYYY-MM-DD HH:mm')),
                        rankDateTime.add(result.duration + result2.duration, 'seconds');
                        currentRequest.TravelTime = rankDateTime;
                        if (i + 1 == totalRequest) {
                            deferred.resolve($scope.requests);
                        }
                    });
                });
            } else {
                $scope.getDistance(objcurrentfrom, objcurrentto).then(function (result) {
                    var currentRequest = $scope.requests[result.index1];
                    var rankDateTime = null;
                    rankDateTime = moment(currentRequest.EndTime.format('YYYY-MM-DD HH:mm')),
                    rankDateTime.add(result.duration, 'seconds');
                    currentRequest.TravelTime = rankDateTime;
                    if (i + 1 == totalRequest) {
                        deferred.resolve($scope.requests);
                    }
                });
            }
        }
        return deferred.promise;
    }
    function setControlGrid() {
        $scope.mainGridOptions = {
            dataSource: {
                data: $scope.requests
            },
            height: $('#popupWindow').height() / 2 - 55,
            sortable: true,
            columns: [
                {
                    field: "RequestNo",
                    title: "#",
                    width: 120,
                }, {
                    field: "LocationFromName",
                    title: "From",
                }, {
                    field: "LocationToName",
                    title: "To",
                }, {
                    //    field: "TravelTime",
                    //    title: "Travel Time",
                    //}, {
                    field: "StartTime",
                    title: "Arrival From",
                }, {
                    field: "EndTime",
                    title: "Arrival To",
                }
            ],
            rowTemplate: kendo.template($("#rowTemplate").html()),
        };

        checkConflit().then(function (result) {
            $timeout(function () {
                var grid = $('#requests-courier-grid').data('kendoGrid');
                var dataSource = new kendo.data.DataSource({
                    data: result
                });
                grid.setDataSource(dataSource);
                grid.refresh();
            }, 200);
        });

    }

    function getTextDropdown(id) {
        var drop = $('#' + id).data('kendoDropDownList');
        return drop != undefined ? drop.text() : '';
    }
    function getValueDropdown(id) {
        var drop = $('#' + id).data('kendoDropDownList');
        return drop != undefined ? parseInt(drop.value()) : 0;
    }

    function getAppendObject() {
        var timeFormat = moment($scope.$parent.data.SendingTime);
        if (!timeFormat.isValid()) {
            timeFormat = moment($scope.$parent.data.SendingTime, 'h:mm A');
            if (!timeFormat.isValid()) {
                timeFormat = null;
            }
        }

        var startTime = moment($scope.$parent.data.StartTime);
        if (!startTime.isValid()) {
            startTime = moment($scope.$parent.data.StartTime, 'h:mm A');
            if (!startTime.isValid()) {
                startTime = null;
            }
        }

        var endTime = moment($scope.$parent.data.EndTime);
        if (!endTime.isValid()) {
            endTime = moment($scope.$parent.data.EndTime, 'h:mm A');
            if (!endTime.isValid()) {
                endTime = null;
            }
        }
        var valid = true;
        if (timeFormat != null) {
            valid &= timeFormat.format('YYYYMMDDHHmm').localeCompare(moment().format('YYYYMMDDHHmm')) > -1;
        }
        if (timeFormat != null && startTime != null) {
            valid &= startTime.format('YYYYMMDDHHmm').localeCompare(timeFormat.format('YYYYMMDDHHmm')) > -1;
        }
        if (startTime != null && endTime != null) {
            valid &= endTime.format('YYYYMMDDHHmm').localeCompare(startTime.format('YYYYMMDDHHmm')) > 0;
        }
        var fromObj = requestDataService.getLocationById(getValueDropdown('from'));
        var toObj = requestDataService.getLocationById(getValueDropdown('to'));

        var result = {
            RequestId: $scope.$parent.data.Id,
            RequestNo: $scope.$parent.data.Id == 0 ? "New" : $scope.$parent.data.RequestNo,
            IsWarning: false,
            TimeNoFormat: timeFormat,
            StartTime: startTime,
            EndTime: endTime,
            TravelTime: null,
            LocationFromName: fromObj != undefined ? fromObj.DisplayName : 'Select',
            LocationFromLat: fromObj != undefined ? fromObj.Lat : null,
            LocationFromLng: fromObj != undefined ? fromObj.Lng : null,
            LocationToName: toObj != undefined ? toObj.DisplayName : 'Select',
            LocationToLat: toObj != undefined ? toObj.Lat : null,
            LocationToLng: toObj != undefined ? toObj.Lng : null,
            IsStat: $scope.$parent.data.IsStat,
            IsInvalid: !valid,
            IsUpdate: $scope.$parent.data.Id != 0,
        }
        return result;
    }
    function checkEnterData() {
        var obj = getAppendObject();
        var hasEnterData = false;
        hasEnterData |= obj.LocationFromName != 'Select';
        hasEnterData |= obj.LocationToName != 'Select';
        hasEnterData |= obj.TimeNoFormat != null;
        hasEnterData |= obj.StartTime != null;
        hasEnterData |= obj.EndTime != null;
        return hasEnterData;
    }

    $scope.$parent.$watch('data.CourierId', function (nval, oval) {
        if (nval != oval) {
            $scope.courierId = 0;
            $scope.courierId = parseInt(nval);
            $scope.requests = [];
            if ($scope.courierId == 0) {
                setControlGrid();
            } else {
                $scope.courierName = $("#courier").data('kendoDropDownList').text();
                $scope.requests = [];
                $http.get('Request/GetRequestCourierForCreate?courierId=' + $scope.courierId).then(function (result) {
                    $scope.requests = [];
                    if (result.data != null && result.data.Data.length > 0) {
                        var hasRequest = false;
                        for (var i = 0; i < result.data.Data.length; i++) {
                            setLocalTime(result.data.Data[i]);
                            var obj = setRequestObj(result.data.Data[i]);
                            obj.IsUpdate = $scope.$parent.data.Id != 0 && $scope.$parent.data.Id == result.data.Data[i].Id;
                            if (obj.IsUpdate) {
                                hasRequest = true;
                            }
                            $scope.requests.push(obj);
                        }
                        if ($scope.$parent.data.Id == 0 || !hasRequest) {
                            var objAppend = getAppendObject();
                            objAppend.IsUpdate = !hasRequest;
                            $scope.requests.push(objAppend);
                            $scope.requests.sort(function (left, right) {
                                var a = moment(left.StartTime).format('YYYYMMDDHHmm');
                                var b = moment(right.StartTime).format('YYYYMMDDHHmm');
                                return a.localeCompare(b);
                            });
                        }
                        setControlGrid();
                    } else {
                        if (checkEnterData()) {
                            $scope.requests.push(getAppendObject());
                            setControlGrid();
                        }
                    }
                });
            }
        }
    });
    $scope.$parent.$watch('data.LocationFrom', function (nval, oval) {

        if ($scope.$parent.loadDataComplete) {
            changeDataForGrid();
        }

    });
    $scope.$parent.$watch('data.LocationTo', function (nval, oval) {
        if ($scope.$parent.loadDataComplete) {
            changeDataForGrid();
        }
    });
    $scope.$parent.$watch('data.SendingTime', function (nval, oval) {

        if ($scope.$parent.loadDataComplete) {
            changeDataForGrid();
        }
    });
    $scope.$parent.$watch('data.StartTime', function (nval, oval) {
        if ($scope.$parent.loadDataComplete) {
            changeDataForGrid();
        }
    });
    $scope.$parent.$watch('data.EndTime', function (nval, oval) {
        if ($scope.$parent.loadDataComplete) {
            changeDataForGrid();
        }
    });
    $scope.$parent.$watch('data.IsStat', function (nval, oval) {
        if ($scope.$parent.loadDataComplete) {
            changeDataForGrid();
        }
    });

    function changeDataForGrid() {
        var objFind;
        if ($scope.$parent.data.CourierId != 0 && checkEnterData() && $scope.$parent.data.Id == 0) {
            objFind = _.findWhere($scope.requests, { RequestId: 0 });
            if (objFind == null) {
                $scope.requests.push(getAppendObject());
                setControlGrid();
            }
        }

        var grid = $('#requests-courier-grid').data('kendoGrid');
        if (grid != undefined) {
            $scope.requests = _.reject($scope.requests, function (item) { return item.RequestId == $scope.$parent.data.Id; });
            var obj = getAppendObject();
            obj.IsUpdate = $scope.$parent.data.Id != 0;
            $scope.requests.push(obj);
            $scope.requests.sort(function (left, right) {
                var a = moment(left.StartTime).format('YYYYMMDDHHmm');
                var b = moment(right.StartTime).format('YYYYMMDDHHmm');
                return a.localeCompare(b);
            });

            checkConflit().then(function (result) {
                var dataSource = new kendo.data.DataSource({
                    data: result
                });
                grid.setDataSource(dataSource);
                grid.refresh();
            });

        }
    }
}]);