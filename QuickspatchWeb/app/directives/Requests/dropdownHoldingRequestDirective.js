'use strict';
app.directive('dropdownHoldingRequest', ['$q', '$http', 'requestDataService', '$timeout', function ($q, $http, requestDataService, $timeout) {
    return {
        restrict: "E",
        scope: true,
        template: function (element, attrs) {
            return '<div class="input-group">'+
                        '<input type="text" class="form-control dropdown-cus" id="' + attrs["dropId"] + '">' +
                        '<span class="input-group-btn">'+
                        '<button style="height: 16px;"  class="btn btn-default btn-action" type="button" ng-click="Add()" ng-if="selectedId == 0"><span class="fa fa-plus"></span></button>' +
                        '<button style="height: 16px;" class="btn btn-default btn-action" type="button" ng-click="Edit()" ng-if="selectedId != 0"><span class="fa fa-edit"></span></button>' +
                        '</span>'+
                    '</div>';
        },
        link: function ($scope, $element, $attrs) {

            $scope.selectedId = 0;
            $scope.data = [];
            $scope.init = function () {
                var deferred = $q.defer();
                if ($attrs["dropType"] == 'location') {
                    requestDataService.getDataLocation().then(function (data) {
                        $scope.data = data;
                        deferred.resolve($scope.data);
                    });
                } else {
                    requestDataService.getDataCourier().then(function (data) {
                        $scope.data = data;
                        deferred.resolve($scope.data);
                    });
                }
                return deferred.promise;
            }
            function refresh() {
                $scope.init().then(function (data) {
                    var deferred = $q.defer();
                    requestDataService.getDataLocationNew().then(function (data) {
                        $scope.data = data;
                        deferred.resolve($scope.data);
                    });
                    if ($attrs["bindVal"] == 'LocationFrom' && $attrs["dropId"] === 'from') {                       
                        if ($scope.data != null && $scope.data != undefined) {
                            if ($scope.data.length > 0) {
                                for (var i = 0; i < $scope.data.length; i++) {
                                    if ($scope.data[i].KeyId == $scope.$parent.HoldingRequest.LocationTo) {
                                        $scope.data.splice(i, 1);
                                    }
                                    if (kendoControl != null) {
                                        kendoControl.setDataSource($scope.data);
                                        kendoControl.value($scope.selectedId);
                                    }
                                }
                            }
                        }
                    }
                    if ($attrs["bindVal"] == 'LocationTo' && $attrs["dropId"] === 'to') {
                      
                        if ($scope.data != null && $scope.data != undefined) {
                            if ($scope.data.length > 0) {
                                for (var i = 0; i < $scope.data.length; i++) {
                                    if ($scope.data[i].KeyId == $scope.$parent.HoldingRequest.LocationFrom) {
                                        $scope.data.splice(i, 1);
                                    }
                                    if (kendoControl != null) {
                                        kendoControl.setDataSource($scope.data);
                                        kendoControl.value($scope.selectedId);
                                    }
                                }
                                
                            }
                        }
                    }
                 
                    //if (kendoControl != null) {
                    //    kendoControl.setDataSource($scope.data);
                    //    kendoControl.value($scope.selectedId);
                    //}
                });

            }
            
            $scope.$root.$on('setLocationFromAfterAdd', function (event, data) {
                if ($attrs["dropId"] === 'from') {
                    $scope.init().then(function (result) {
                        if (kendoControl != null) {
                            kendoControl.setDataSource($scope.data);
                            kendoControl.value(data.id);
                            $scope.selectedId = data.id;
                            $timeout(function () {
                                $scope.$apply(function () {
                                    if ($attrs["bindObj"] == 'HoldingRequest') {
                                        $scope.$parent.HoldingRequest[$attrs["bindVal"]] = data.id;
                                    }
                                });
                            });
                        }
                    });
                }
                
            });

            $scope.$root.$on('setLocationToAfterAdd', function (event, data) {
                if ($attrs["dropId"] === 'to') {
                    $scope.init().then(function(result) {
                        if (kendoControl != null) {
                            kendoControl.setDataSource($scope.data);
                            kendoControl.value(data.id);
                            $scope.selectedId = data.id;
                            $timeout(function() {
                                $scope.$apply(function() {
                                    if ($attrs["bindObj"] == 'HoldingRequest') {
                                        $scope.$parent.HoldingRequest[$attrs["bindVal"]] = data.id;
                                    } 
                                });
                            });
                        }
                    });
                }
            });
            $scope.Add = function () {
                var popup = $("#popupWindowChild").data("kendoWindow");
                if ($attrs["dropType"] == 'location') {
                    popup.setOptions({
                        width: "800px",
                        height: "500px",
                        title: "Create Location",
                        content: { url: "/Location/Create?type=" + $attrs["dropId"] },
                        close: function (e) {
                            popup.content('');
                            refresh();
                        },

                    });
                    popup.open();
                } else {
                    popup.setOptions({
                        width: "1000px",
                        height: "400px",
                        title: "Create Courier",
                        content: { url: "/Courier/Create" },
                        close: function (e) {
                            popup.content('');
                            refresh();
                        },
                    });
                    popup.open();
                }
                
            }

            $scope.Edit = function () {
                var popup = $("#popupWindowChild").data("kendoWindow");
                if ($attrs["dropType"] == 'location') {
                    popup.setOptions({
                        width: "800px",
                        height: "500px",
                        title: "Update Location",
                        content: { url: "/Location/Update/" + $scope.selectedId + "?type=" },
                        close: function (e) {
                            popup.content('');
                            refresh();
                        },

                    });
                    popup.open();
                } else {
                    popup.setOptions({
                        width: "1000px",
                        height: "400px",
                        title: "Update Courier",
                        content: { url: "/Courier/Update/" + $scope.selectedId },
                        close: function (e) {
                            popup.content('');
                            refresh();
                        },
                    });
                    popup.open();
                }

            }
            function setDataSourceWhenChangeValue(val) {
               
                if ($attrs["dropType"] == 'location') {
                    $scope.data = requestDataService.data.Locations;
                } else {
                    $scope.data = requestDataService.data.Couriers;
                }
                if (val != 0) {
                    $scope.data = _.reject($scope.data, function (item) { return item.KeyId == val; });
                }
                if (kendoControl != null) {
                    kendoControl.setDataSource($scope.data);
                }
            }
            var kendoControl = null;
            function initControlerWithData() {
              
                $scope.init().then(function() {
                    kendoControl =  $("#" + $attrs["dropId"]).kendoDropDownList({
                        //filter: "startswith",
                        optionLabel: "Select",
                        dataTextField: "DisplayName",
                        dataValueField: "KeyId",
                        dataSource: {
                            data: $scope.data
                        },
                        change: function (e) {

                            var value = this.value() == '' ? 0 : parseInt(this.value());
                            $scope.selectedId = value;
                            $timeout(function() {
                                $scope.$apply(function () {
                                    if ($attrs["bindObj"] == 'HoldingRequest') {
                                        $scope.$parent.HoldingRequest[$attrs["bindVal"]] = value;
                                    } 
                                });
                            });
                        }
                    }).data('kendoDropDownList');

                    if ($attrs["bindObj"] == 'HoldingRequest') {
                        $scope.$parent.$watch('HoldingRequest.' + $attrs["referVal"], function (nval, oval) {
                            if (nval != undefined) {
                                setDataSourceWhenChangeValue(nval);
                            }
                           
                        });
                    } else {
                        $scope.$parent.$watch($attrs["referVal"], function (nval, oval) {
                            if (nval != undefined) {
                                setDataSourceWhenChangeValue(nval);
                            }
                        });
                        
                    }
                });
            }
            initControlerWithData();
        }
    }
}]);