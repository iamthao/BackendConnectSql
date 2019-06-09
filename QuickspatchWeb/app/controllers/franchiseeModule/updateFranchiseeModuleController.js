'use strict';
app.controller('updateFranchiseeModuleController', ['$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
            function ($scope, masterfileService, common, config, messageLanguage) {
    var events = config.events;
    $scope.controllerId = "updateFranchiseeModuleController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn($scope.controllerId);
    activate();
    var dataSource = {};
    $scope.init = function (userRoleId) {
    //    $scope.userRoleId = userRoleId;
    //    $scope.urlToGetDataForUserRoleGrid = "/UserRole/GetRoleFunction?id=" + userRoleId;
    };

    function activate() {
        //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateFranchiseeModule); });
    }

    function callBackAfterUpdateSuccess() {
        $scope.$root.$broadcast("ReloadGrid");
    }
                
    $scope.UpdateMasterfileData($scope.controllerId, 'FranchiseeModule', messageLanguage.updateFranchiseeModuleSuccess, callBackAfterUpdateSuccess);
    $scope.getData = function () {
        var shareData = {};
        for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
            // get $$childHead first and then iterate that scope's $$nextSiblings
            if (childScope.controllerId != undefined && childScope.controllerId == 'franchiseeModuleShareController') {
                shareData = childScope.getShareViewData();
                break;
            }
        }
        return shareData;
    }
}]);