'use strict';
app.controller('updateFranchiseeConfigurationController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateFranchiseeConfigurationController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateUser); });
        }
        
        function callBackAfterUpdateSuccess() {
            $scope.$root.$broadcast("ReloadConfig");
        }
        
        $scope.UpdateMasterfileData($scope.controllerId, 'FranchiseeConfiguration', messageLanguage.updateFranchiseeConfigSuccess, callBackAfterUpdateSuccess);

        $scope.getData = function() {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'franchiseeConfigurationShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);