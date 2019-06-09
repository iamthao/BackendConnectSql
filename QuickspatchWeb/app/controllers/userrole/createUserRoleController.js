'use strict';
app.controller('createUserRoleController', ['$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function($scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createUserRoleController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();
        $scope.urlToGetDataForUserRoleGrid = "/UserRole/GetRoleFunction?id=0";

        function activate() {
           // common.activateController(null, $scope.controllerId).then(function() { log(messageLanguage.createUserRole); });
        }

        function callBackAfterCreateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }

        $scope.CreateMasterfileData($scope.controllerId, 'UserRole', messageLanguage.createUserRoleSuccess, callBackAfterCreateSuccess);

        $scope.getData = function() {
            // Get data from share controller
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'userRoleShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]);