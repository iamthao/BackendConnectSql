'use strict';
app.controller('createContactController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createContactController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
        }

        function callBackAfterCreateSuccess() {
            var scope = angular.element("#franchisee-configuration-controller").scope();
            if (scope !== undefined && scope !== null) {
                scope.refreshGrid();
            }
        }
        $scope.CreateMasterfileData($scope.controllerId, 'Contact', messageLanguage.createContactSuccess, callBackAfterCreateSuccess);

        $scope.getData = function () {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'contactShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        }

    }]
);