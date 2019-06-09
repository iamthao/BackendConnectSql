'use strict';
app.controller('updateModuleDocumentTypeOperationShareControllerController', ['$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
            function ($scope, masterfileService, common, config, messageLanguage) {
    var events = config.events;
    $scope.controllerId = "updateModuleDocumentTypeOperationShareControllerController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn($scope.controllerId);
    activate();
    var dataSource = {};
    $scope.init = function (moduleId) {
    //    $scope.userRoleId = userRoleId;
    //    $scope.urlToGetDataForUserRoleGrid = "/UserRole/GetRoleFunction?id=" + userRoleId;
    };

    function activate() {
        //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateModuleOperation); });
    }

    function callBackAfterUpdateSuccess() {
        $scope.$root.$broadcast("ReloadGrid");
    }
                
    $scope.UpdateMasterfileData($scope.controllerId, 'ModuleDocumentTypeOperation', messageLanguage.updateFranchiseeModuleOperationSuccess, callBackAfterUpdateSuccess);
    $scope.getData = function () {
        var shareData = {};
        for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
            // get $$childHead first and then iterate that scope's $$nextSiblings
            if (childScope.controllerId != undefined && childScope.controllerId == 'moduleDocumentTypeOperationShareController') {
                shareData = childScope.getShareViewData();
                break;
            }
        }
        return shareData;
    }
}]);