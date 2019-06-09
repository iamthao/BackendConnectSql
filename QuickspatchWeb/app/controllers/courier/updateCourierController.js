'use strict';
app.controller('updateCourierController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateCourierController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
           // common.activateController(null, $scope.controllerId).then(function () { log('Update ' + $rootScope.CourierDisplayName); });
        }

        function callBackAfterUpdateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }

        $scope.UpdateMasterfileData($scope.controllerId, 'Courier', 'Update ' + $rootScope.CourierDisplayName + ' successfully', callBackAfterUpdateSuccess);

        $scope.getData = function () {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'courierShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);