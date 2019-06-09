'use strict';
app.controller('updateContactController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateContactController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();
        
        function activate() {
            
        }
        
        function callBackAfterUpdateSuccess() {
            var scope = angular.element("#franchisee-configuration-controller").scope();
            if (scope !== undefined && scope !== null) {
                scope.refreshGrid();
            }
        }
        
        $scope.UpdateMasterfileData($scope.controllerId, 'Contact', messageLanguage.updateContactSuccess, callBackAfterUpdateSuccess);

        $scope.getData = function() {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'contactShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);