'use strict';
app.controller('updateUserRoleController', ['$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
            function ($scope, masterfileService, common, config, messageLanguage) {
    var events = config.events;
    $scope.controllerId = "updateUserRoleController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn($scope.controllerId);
    activate();
    $scope.userRoleId = 0;
    var dataSource = {};
    $scope.init = function (userRoleId) {
        $scope.userRoleId = userRoleId;
        $scope.urlToGetDataForUserRoleGrid = "/UserRole/GetRoleFunction?id=" + userRoleId;
    }

    function activate() {
      //  common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateUserRole); });
    }

    function callBackAfterUpdateSuccess() {
        
        $scope.$root.$broadcast("ReloadGrid");
    }
    $scope.UpdateMasterfileData($scope.controllerId, 'UserRole', messageLanguage.updateUserRoleSuccess, callBackAfterUpdateSuccess);
    $scope.getData = function () {
        var shareData = {};
        for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
            // get $$childHead first and then iterate that scope's $$nextSiblings
            if (childScope.controllerId != undefined && childScope.controllerId == 'userRoleShareController') {
                shareData = childScope.getShareViewData();
                break;
            }
        }
        return shareData;
    }
}]);