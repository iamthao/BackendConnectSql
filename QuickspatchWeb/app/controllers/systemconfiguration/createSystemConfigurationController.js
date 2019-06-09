'use strict';
app.controller('createSystemConfigurationController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createSystemConfigurationController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();
        $scope.Type = "";
        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.createUser); });
        }

        function callBackAfterCreateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }
        $scope.CreateMasterfileData($scope.controllerId, 'SystemConfiguration', messageLanguage.createSystemConfigurationSuccess, callBackAfterCreateSuccess);

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
        }

    }]
);