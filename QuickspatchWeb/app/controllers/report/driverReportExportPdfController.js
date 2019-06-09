'use strict';
app.controller('driverReportExportPdfController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage', '$http',
    function ($scope, masterfileService, $state, common, config, messageLanguage, $http) {
        var events = config.events;
        $scope.controllerId = "driverReportExportPdfController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        var logError = getLogFn($scope.controllerId, "error");

        $scope.Cancel = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }

    }]);