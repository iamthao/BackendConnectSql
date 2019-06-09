'use strict';
app.controller('createLocationController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createLocationController";       
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();
        $scope.Type = "";
        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.createLocation); });
        }

        function callBackAfterCreateSuccess(data) {

            if ($scope.Type == "from") {
                $scope.$root.$broadcast('LocationFrom_Change', [{ KeyId: data.id, DisplayName: data.name }]);
                $scope.$root.$broadcast('setLocationFromAfterAdd', data);
            }
            if ($scope.Type == "to") {
                $scope.$root.$broadcast('LocationTo_Change', [{ KeyId: data.id, DisplayName: data.name }]);
                $scope.$root.$broadcast('setLocationToAfterAdd', data);
            }
            $scope.$root.$broadcast("ReloadGrid");
        }
        $scope.CreateMasterfileData($scope.controllerId, 'Location', messageLanguage.createLocationSuccess, callBackAfterCreateSuccess);

        $scope.getData = function() {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'locationShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };

    }]
);