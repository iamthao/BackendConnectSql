'use strict';
app.controller('updateTemplateController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateTemplateController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();
        $scope.Type = "";
        function activate() {
          
        }

       

        function callBackAfterUpdateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }

        $scope.UpdateMasterfileData($scope.controllerId, 'Template', messageLanguage.updateTemplateSuccess, callBackAfterUpdateSuccess);

        $scope.getData = function () {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'templateShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);