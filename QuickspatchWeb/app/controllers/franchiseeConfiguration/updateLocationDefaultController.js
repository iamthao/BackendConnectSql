'use strict';
app.controller('updateLocationDefaultController', ['$rootScope', '$scope', 'messageLanguage', 'common', '$http', 'masterfileService',
    function ($rootScope, $scope, messageLanguage, common, $http, masterfileService) {
        $scope.controllerId = "updateLocationDefaultController";

        $scope.Location = new LocationViewModel();
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        var logSuccess = getLogFn($scope.controllerId, "success");
        var logError = getLogFn($scope.controllerId, "error");

        function activate() {
            $http.get("FranchiseeConfiguration/GetLocationDefault")
                .then(function (result) {
                    //console.log(result.data);
                    var from = $("#froms").data("kendoDropDownList");
                    var to = $("#tos").data("kendoDropDownList");
                    from.value(result.data.LocationFromId);
                    to.value(result.data.LocationToId);
                });
        }

        activate();
        $scope.Cancel = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }

        $scope.Update = function () {
            var from = $("#froms").data("kendoDropDownList");
            var to = $("#tos").data("kendoDropDownList");
            if (from.value() != 0 && to.value() != 0 && from.value() == to.value()) {
                logError(messageLanguage.compareLocationDefault);
            } else {
                $http.get("FranchiseeConfiguration/UpdateDataLocationDefault", { params: { fromId: from.value(), toId: to.value() } })
                 .then(function (result) {
                     if (result.data == "true") {
                         var scope = angular.element("#franchisee-configuration-controller").scope();
                         if (scope !== undefined && scope !== null) {
                             scope.getLocationDefault();
                         }
                         var popup = $("#popupWindow").data("kendoWindow");
                         popup.close();
                         logSuccess(messageLanguage.updateLocationSuccess);
                     }
                 });
            }        
        }

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

    }]
);