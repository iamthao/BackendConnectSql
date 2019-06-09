'use strict';
app.controller('systemConfigurationShareController', ['$rootScope', '$scope', 'common', 'masterfileService', '$timeout', '$interval',
    function ($rootScope, $scope, common, masterfileService, $timeout, $interval) {
        $scope.controllerId = "systemConfigurationShareController";

        var getLogFn = common.logger.getLogFn;
        $scope.Data = new SystemConfigurationViewModel();
        //console.log($scope.Data)
        if ($scope.Data != null && $scope.Data != undefined) {
            if ($scope.Data.SystemConfigTypeId == 3 || $scope.Data.SystemConfigTypeId == 4) {
                var intrval1 = $interval(function() {
                    if ($("#froms").data("kendoDropDownList") != undefined && $("#froms").data("kendoDropDownList") != null) {
                        $timeout(function() {
                            $scope.$apply(function() {

                            });
                        });
                        $("#froms").data("kendoDropDownList").value($scope.Data.Value);
                        $interval.cancel(intrval1);

                    } else if ($("#tos").data("kendoDropDownList") != undefined && $("#tos").data("kendoDropDownList") != null) {
                        $timeout(function() {
                            $scope.$apply(function() {

                            });
                        });
                        $("#tos").data("kendoDropDownList").value($scope.Data.Value);
                        $interval.cancel(intrval1);
                    } else {
                        $interval.cancel(intrval1);
                    }
                }, 200);
            } else {
                var intrval = $interval(function () {                   
                        $interval.cancel(intrval);                  
                }, 200);
            }
        }
               
        EnableCreateFooterButton(true);
        //droplist from
        $scope.locationsDataSource = {
            transport: {
                read: {
                    url: "Location/GetListLocation"
                }
            }
        };

        $scope.locationFromOptions = {
            dataSource: $scope.locationsDataSource,
            dataTextField: "Name",
            dataValueField: "Id",
            template: '<span class="k-state-default"><h4 style="font-size: 1.2em; font-weight: normal;margin: 0 0 1px 0; padding: 0;">{{dataItem.Name}}</h4>' +
                '<p style="margin: 0;padding: 0;font-size: 0.8em;">{{dataItem.FullAddress}}</p></span>',
        };

        //droplist to    
        $scope.locationToOptions = {
            dataSource: $scope.locationsDataSource,
            dataTextField: "Name",
            dataValueField: "Id",
            template: '<span class="k-state-default"><h4 style="font-size: 1.2em; font-weight: normal;margin: 0 0 1px 0; padding: 0;">{{dataItem.Name}}</h4>' +
                '<p style="margin: 0;padding: 0;font-size: 0.8em;">{{dataItem.FullAddress}}</p></span>',
        };

        $scope.getShareViewData = function () {
            //console.log($("#froms").data("kendoDropDownList"), $("#tos").data("kendoDropDownList"));
            if ($scope.Data != null && $scope.Data != undefined) {
                if ($("#froms").data("kendoDropDownList")) {
                    $scope.Data.Value = $("#froms").data("kendoDropDownList").value();
                }
                if ($("#tos").data("kendoDropDownList")) {
                    $scope.Data.Value = $("#tos").data("kendoDropDownList").value();
                }
            }
            return { SharedParameter: JSON.stringify($scope.Data) };
        };

    }]
);