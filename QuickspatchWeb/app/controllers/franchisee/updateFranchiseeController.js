'use strict';
app.controller('updateFranchiseeController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateFranchiseeController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
           // common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateFranchisee); });
        }
        
        function callBackAfterUpdateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }
        
        $scope.UpdateMasterfileData($scope.controllerId, 'FranchiseeTenant', messageLanguage.updateFranchiseeSuccess, callBackAfterUpdateSuccess);

        $scope.getData = function() {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'franchiseeShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);