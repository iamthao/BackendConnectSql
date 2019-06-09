'use strict';
app.controller('franchiseeBillingController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage',
    function ($scope, masterfileService, $state, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "franchiseeBillingController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateFranchiseeModule); });
        }

        //1. billing  2.Change package
        $scope.action = 1;
        $scope.setActionBilling = function(val) {
            $scope.action = val;
        };

        $scope.packageCurrentId = 0;
        $scope.setPackageCurrent = function (val) {
            $scope.packageCurrentId = val;
        };

        $scope.isCloseAccount = false;
        $scope.setIsCloseAccount = function (val) {
            $scope.isCloseAccount = val;
        };

    }]);