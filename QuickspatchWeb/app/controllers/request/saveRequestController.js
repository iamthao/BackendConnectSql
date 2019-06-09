'use strict';
app.controller('SaveRequestController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', 'masterfileService', '$http', '$timeout', '$interval',
    function ($rootScope, $scope, common, messageLanguage, $window, masterfileService, $http, $timeout, $interval) {
        $scope.controllerId = "SaveRequestController";
        $scope.loadDataComplete = false;
        $scope.isLoading = true;
        function SaveRequestViewModel() {
            var self = this;
            self.Id = 0;
            self.LocationFrom = 0;
            self.RequestNo = '',
            self.LocationTo = 0;
            self.CourierId = 0;
            self.SendingTime = '';
            self.StartTime = '';
            self.EndTime = '';
            self.Description = '';
            self.Status = 60;
            self.ExpiredTime = 0;
            self.IsStat = false;
            self.HoldingRequestId = null;
        }
        $scope.data = new SaveRequestViewModel();
        var objUpdate = $("#popupWindow").data('dataItemUpdate');
        var objCopy = $("#popupWindow").data('dataItemCopy');
        var objSendHolding = $("#popupWindow").data('sendholding');

        var obj = new SaveRequestViewModel();
        if (objUpdate != undefined && objUpdate != null) {
            obj.Id = objUpdate.Id;
            obj.LocationFrom = objUpdate.LocationFromId;
            obj.LocationTo = objUpdate.LocationToId;
            obj.CourierId = objUpdate.CourierId;
            obj.SendingTime = moment(objUpdate.TimeNoFormat).format('MM/DD/YYYY HH:mm');
            obj.StartTime = moment(objUpdate.StartTimeNoFormat).format('MM/DD/YYYY HH:mm');
            obj.EndTime = moment(objUpdate.EndTimeNoFormat).format('MM/DD/YYYY HH:mm');
            obj.RequestNo = objUpdate.RequestNo;
            obj.Status = objUpdate.StatusId;
            obj.ExpiredTime = objUpdate.ExpiredTime;
            $("#popupWindow").data('dataItemUpdate', null);
        }
        else if (objCopy != undefined && objCopy != null) {
            obj.LocationFrom = objCopy.LocationFromId;
            obj.LocationTo = objCopy.LocationToId;
            obj.CourierId = objCopy.CourierId;
            obj.Status = objCopy.StatusId;
            $("#popupWindow").data('dataItemCopy', null);
        }
        else if (objSendHolding != undefined && objSendHolding != null) {
            obj.Id = 0;
            obj.LocationFrom = objSendHolding.LocationFromId;
            obj.LocationTo = objSendHolding.LocationToId;
            obj.CourierId = 0;
            obj.SendingTime = '';
            obj.StartTime = moment(objSendHolding.StartTimeNoFormat).format('MM/DD/YYYY HH:mm');
            obj.EndTime = moment(objSendHolding.EndTimeNoFormat).format('MM/DD/YYYY HH:mm');
            obj.RequestNo = '';
            obj.Status = 60;
            obj.ExpiredTime = 0;
            obj.HoldingRequestId = objSendHolding.Id;
            $("#popupWindow").data('sendholding', null);

        }

        function addZero(num) {
            return (num >= 0 && num < 10) ? "0" + num : num + "";
        }

        function convertTimeToFormat(dt) {

            var formatted = '';

            if (dt) {
                var hours24 = dt.getHours();
                var hours = ((hours24 + 11) % 12) + 1;
                formatted = [formatted, [addZero(hours), addZero(dt.getMinutes())].join(":"), hours24 > 11 ? "PM" : "AM"];
            }
            return formatted[1] + " " + formatted[2];

        };
        $scope.$watch('data.IsStat', function (nval, oval) {
            $('#dispatch-time').data('kendoTimePicker').enable(!nval);
            if (nval != oval && nval == true) {
                var now = new Date();
                $('#dispatch-time').data('kendoTimePicker').value(convertTimeToFormat(now));
                $scope.data.SendingTime = common.getValueOfTimeAMPM(convertTimeToFormat(now));//moment(result.data.SpatchTimeDefault, 'h:m A').format('MM/DD/YYYY HH:mm');

            }
        });

        $scope.setSpatchTimeDefault = function () {
            $http.get("Request/GetSpatchTimeDefault")
             .then(function (result) {
                 if ($scope.data.SendingTime == '' || $scope.data.SendingTime == undefined) {
                     $('#dispatch-time').data('kendoTimePicker').value(result.data.SpatchTimeDefault);
                     $scope.data.SendingTime = common.getValueOfTimeAMPM(result.data.SpatchTimeDefault);//moment(result.data.SpatchTimeDefault, 'h:m A').format('MM/DD/YYYY HH:mm');
                 }
             });
        };

        $scope.setSpatchTimeDefault();
        $scope.getAllDefaultLocation = function () {
            $http.get("FranchiseeConfiguration/GetLocationDefault")
              .then(function (result) {

                  $scope.LocationFromDefault = result.data.LocationFromId;
                  $scope.LocationToDefault = result.data.LocationToId;
              });
        }
        $scope.getAllDefaultLocation();

        $scope.setAutoAssign = function () {
            $http.get("/courier/getautoassigncourier")
              .then(function (result) {
                  $scope.data.CourierId = result.data.Data[0].Id;
                  $('#courier').data('kendoDropDownList').value($scope.data.CourierId);
                  $('#courier').data('kendoDropDownList').trigger("change");
              });
        }

        $scope.$watch("data.CourierId", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if ($('#courier').data('kendoDropDownList') != undefined || $('#to').data('kendoDropDownList') != null) {
                    $('#courier').data('kendoDropDownList').trigger("change");
                }

            }
        });

        $scope.setFromDefault = function () {

            var defaultToId = parseInt($('#from').data('kendoDropDownList').value());

            if (defaultToId != $scope.LocationFromDefault && $scope.LocationFromDefault > 0) {
                $scope.data.LocationFrom = $scope.LocationFromDefault;
                $('#from').data('kendoDropDownList').value($scope.LocationFromDefault);
                $('#from').data('kendoDropDownList').trigger("change");
            }

        }

        $scope.$watch("data.LocationFrom", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if ($('#from').data('kendoDropDownList') != undefined || $('#from').data('kendoDropDownList') != null) {
                    $('#from').data('kendoDropDownList').trigger("change");
                }

            }
        });
        $scope.setToDefault = function () {
            var defaultFromId = parseInt($('#from').data('kendoDropDownList').value());
            if (defaultFromId != $scope.LocationToDefault && $scope.LocationToDefault > 0) {
                $scope.data.LocationTo = $scope.LocationToDefault;
                $('#to').data('kendoDropDownList').value($scope.LocationToDefault);
                $('#to').data('kendoDropDownList').trigger("change");
            }
        }

        $scope.$watch("data.LocationTo", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if ($('#to').data('kendoDropDownList') != undefined || $('#to').data('kendoDropDownList') != null) {
                    $('#to').data('kendoDropDownList').trigger("change");
                }

            }
        });

        function checkdate(input) {
            var formats = ['MM/DD/YYYY hh:mm tt'];
            return moment(input, formats).isValid();
        }
        $scope.$watch("data.SendingTime", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if ($scope.data.SendingTime != null && $scope.data.SendingTime != "") {
                    var stringMin = '';
                    if ($scope.data.SendingTime.indexOf('/') > 0) {
                        var splitDate = $scope.data.SendingTime.split(" ");
                        stringMin = splitDate[1];

                    } else {
                        stringMin = moment($scope.data.SendingTime, 'h:m A').format("HH:mm");
                    }

                    var sethour = '';
                    var setmin = '';
                    var splitMin = stringMin.split(":");
                    if (splitMin[1] >= 0) {
                        sethour = parseInt(splitMin[0]);
                        setmin = 30;
                    }
                    if (splitMin[1] > 30) {
                        sethour = parseInt(splitMin[0]) + 1;
                        setmin = 0;
                    }
                    var now = new Date();
                    var timeMinDes = new Date(now.getFullYear(), now.getMonth(), now.getDay(), sethour, setmin, 0);
                    if ($("#start-time").data("kendoTimePicker") != undefined || $("#start-time").data("kendoTimePicker") != null) {
                        var timepickerStart = $("#start-time").data("kendoTimePicker");
                        timepickerStart.min(timeMinDes);
                    }
                    if ($("#end-time").data("kendoTimePicker") != undefined || $("#end-time").data("kendoTimePicker") != null) {
                        var timepickerEnd = $("#end-time").data("kendoTimePicker");
                        timepickerEnd.min(timeMinDes);
                    }
                } else {
                    var now1 = new Date();
                    var timeMinDes1 = new Date(now1.getFullYear(), now1.getMonth(), now1.getDay(), 0, 0, 0);
                    if ($("#start-time").data("kendoTimePicker") != undefined || $("#start-time").data("kendoTimePicker") != null) {
                        var timepickerStart1 = $("#start-time").data("kendoTimePicker");
                        timepickerStart1.min(timeMinDes1);
                    }
                    if ($("#end-time").data("kendoTimePicker") != undefined || $("#end-time").data("kendoTimePicker") != null) {
                        var timepickerEnd1 = $("#end-time").data("kendoTimePicker");
                        timepickerEnd1.min(timeMinDes1);
                    }
                }
            }
        });
        function convertUtcTime() {
            //Thao         
            //TH tra ve 02:03 PM
            if ($scope.data.SendingTime.indexOf('AM') > 0 || $scope.data.SendingTime.indexOf('PM') > 0) {

                var tempStart = $scope.data.SendingTime.split(":");
                var hourAddStart = parseInt(tempStart[0]);
                var secondAddStart = parseInt(tempStart[1].split(" ")[0]);

                if (tempStart[1].split(" ")[1] == 'PM' && hourAddStart != 12) {
                    hourAddStart += 12;
                }
                if (tempStart[1].split(" ")[1] == 'AM' && hourAddStart == 12) {
                    hourAddStart = 0;
                }

                $scope.data.SendingTime = (new Date((new Date()).setHours(hourAddStart, secondAddStart))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.data.SendingTime != '' && $scope.data.SendingTime != undefined) {
                var a = new Date(Date.parse($scope.data.SendingTime));
                $scope.data.SendingTime = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();
            }
            if ($scope.data.StartTime != '' && $scope.data.StartTime != undefined) {
                var b = new Date(Date.parse($scope.data.StartTime));
                $scope.data.StartTime = (new Date((new Date()).setHours(b.getHours(), b.getMinutes()))).toUTCString();
            }
            if ($scope.data.EndTime != '' && $scope.data.EndTime != undefined) {
                var c = new Date(Date.parse($scope.data.EndTime));
                $scope.data.EndTime = (new Date((new Date()).setHours(c.getHours(), c.getMinutes()))).toUTCString();
            }

            //A NGhiep
            //if (typeof ($scope.data.SendingTime) == "object") {
            //    $scope.data.SendingTime = $scope.data.SendingTime != null && $scope.data.SendingTime != '' ? $scope.data.SendingTime.utc().format('MM/DD/YYYY HH:mm') : '';
            //}
            //if (typeof ($scope.data.SendingTime) == "object") {
            //    $scope.data.StartTime = $scope.data.StartTime != null && $scope.data.StartTime != '' ? $scope.data.StartTime.utc().format('MM/DD/YYYY HH:mm') : '';
            //}
            //if (typeof ($scope.data.SendingTime) == "object") {
            //    $scope.data.EndTime = $scope.data.EndTime != null && $scope.data.EndTime != '' ? $scope.data.EndTime.utc().format('MM/DD/YYYY HH:mm') : '';
            //}

            var currentDate = new Date();
            var endOfDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59);
            $scope.data.ExpiredTime = Math.round((endOfDate - currentDate) / 1000);
        }
        function showWarning(obj) {
            var title = '<span class="fa fa-exclamation-triangle"></span> Request conflict';
            var data = Base64.encode(JSON.stringify(obj));

            var popup = $("#popupWindowChildOptions").data("kendoWindow");
            popup.setOptions({
                width: "600px",
                height: "250px",
                title: title,
                content: {
                    url: "/Request/Warning?data=" + data
                },
                close: function (e) {
                    popup.content('');
                    //$scope.cancelRequest();
                    $scope.popConfirmOpen = false;
                },
                animation: false
            });
            popup.open();
        }

        $scope.send = function () {

            $scope.isLoading = true;
            convertUtcTime();
            masterfileService.create('Request').perform({ parameters: { SharedParameter: JSON.stringify($scope.data) } }).$promise.then(function (result) {

                if (result.WarningInfo != null) {
                    showWarning(result.WarningInfo);
                } else {
                    $scope.isLoading = false;
                    if (result.Error === undefined || result.Error === '') {
                        $scope.cancel();

                    }
                    if (objSendHolding != undefined && objSendHolding != null) {
                        var scope = angular.element("#holding-request-controller").scope();
                        scope.Search();
                    }
                }
            });
        }
        $scope.update = function () {
            $scope.isLoading = true;
            convertUtcTime();
            masterfileService.update('Request').perform({ parameters: { SharedParameter: JSON.stringify($scope.data) } }).$promise.then(function (result) {
                if (result.WarningInfo != null) {
                    showWarning(result.WarningInfo);
                } else {
                    $scope.isLoading = false;
                    if (result.Error === undefined || result.Error === '') {
                        $scope.cancel();
                    }


                }
            });
        }

        $scope.cancel = function () {
            $timeout(function () {
                var popup = $("#popupWindow").data("kendoWindow");
                popup.close();
            });

        }
        //Init control success;
        var intrval = $interval(function () {
            var fromCtr = $("#from").data('kendoDropDownList');
            var toCtr = $("#to").data('kendoDropDownList');
            var courierCtr = $("#courier").data('kendoDropDownList');
            var dispatchCtr = $('#dispatch-time').data('kendoTimePicker');
            var startCtr = $('#start-time').data('kendoTimePicker');
            var endCtr = $('#end-time').data('kendoTimePicker');
            var initControlOk = true;
            initControlOk &= fromCtr != undefined;
            initControlOk &= toCtr != undefined;
            initControlOk &= courierCtr != undefined;
            initControlOk &= dispatchCtr != undefined;
            initControlOk &= startCtr != undefined;
            initControlOk &= endCtr != undefined;
            if (initControlOk) {
                $timeout(function () {
                    $scope.$apply(function () {
                        $scope.data = obj;
                    });
                });
                fromCtr.value(obj.LocationFrom);
                toCtr.value(obj.LocationTo);
                courierCtr.value(obj.CourierId);
                dispatchCtr.value(obj.SendingTime != null && obj.SendingTime != '' ? moment(obj.SendingTime).format('h:mm A') : obj.SendingTime);
                startCtr.value(obj.StartTime != null && obj.StartTime != '' ? moment(obj.StartTime).format('h:mm A') : obj.StartTime);
                endCtr.value(obj.EndTime != null && obj.EndTime != '' ? moment(obj.EndTime).format('h:mm A') : obj.EndTime);
                $timeout(function () {
                    $scope.$apply(function () {
                        $scope.loadDataComplete = true;
                        $scope.isLoading = false;
                        $scope.setSpatchTimeDefault();
                    });
                });

                $interval.cancel(intrval);
            }
        }, 200);

        $scope.getMapForRequest = function (dataItem) {

        }

    }]);