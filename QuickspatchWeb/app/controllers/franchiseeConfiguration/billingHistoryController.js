'use strict';
app.controller('billingHistoryController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage',
    function ($scope, masterfileService, $state, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "billingHistoryController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateFranchiseeModule); });
        }

        //1. Package  2.Transaction
        $scope.requestId = 0;
        $scope.setRequestId = function (val) {
            $scope.requestId = val;
        };

        $scope.actionHistory = 1;
        $scope.setActionHistory = function (val) {

            $scope.actionHistory = val;
        };

        $scope.selectRadioButton = function (val) {
            $scope.requestId = 0;
        };
        
    }]);