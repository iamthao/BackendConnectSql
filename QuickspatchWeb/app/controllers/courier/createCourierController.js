'use strict';
app.controller('createCourierController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createCourierController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
           // common.activateController(null, $scope.controllerId).then(function () { log('Create ' + $rootScope.CourierDisplayName); });
        }

        function callBackAfterCreateSuccess(data) {
            $scope.$root.$broadcast('setCourierAfterAdd', data);
            $scope.$root.$broadcast("ReloadGrid");
        }
        $scope.CreateMasterfileData($scope.controllerId, 'Courier', 'Create ' + $rootScope.CourierDisplayName + ' successfully', callBackAfterCreateSuccess);

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
        }

    }]
);