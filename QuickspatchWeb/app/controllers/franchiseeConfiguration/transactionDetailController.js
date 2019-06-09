'use strict';
app.controller('transactionDetailController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage', '$http',
    function ($scope, masterfileService, $state, common, config, messageLanguage, $http) {
        var events = config.events;
        $scope.controllerId = "transactionDetailController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        var logError = getLogFn($scope.controllerId, "error");
        activate();

        function activate() {
            
        }

        $scope.closePopUp = function () {
            //console.log(123);
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }

    }]);