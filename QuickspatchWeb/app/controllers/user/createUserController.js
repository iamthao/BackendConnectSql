'use strict';
app.controller('createUserController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createUserController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.createUser); });
        }

        function callBackAfterCreateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }
        $scope.CreateMasterfileData($scope.controllerId, 'User', messageLanguage.createUserSuccess, callBackAfterCreateSuccess);

        $scope.getData = function () {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'userShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        }

    }]
);