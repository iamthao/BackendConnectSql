'use strict';
app.controller('updateSystemConfigurationController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateSystemConfigurationController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();
        $scope.Type = "";
        function activate() {
          
        }
    
        function callBackAfterUpdateSuccess() {
            //$scope.$root.$broadcast("ReloadGrid");
            var scope = angular.element("#system-configuration-controller").scope();
            if (scope != undefined ) {
                scope.refreshGrid();
            }
        }

        $scope.UpdateMasterfileData($scope.controllerId, 'SystemConfiguration', messageLanguage.updateTSystemConfigurationSuccess, callBackAfterUpdateSuccess);

        $scope.getData = function () {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'systemConfigurationShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);