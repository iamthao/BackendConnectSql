'use strict';
app.controller('userRoleController', ['$scope', 'common', 'messageLanguage', function ($scope, common, messageLanguage) {
    var controllerId = "userRoleController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    activate();

    function activate() {
        //common.activateController(null, controllerId).then(function () { log(messageLanguage.listUserRole); });
    }

    $scope.deleteMessage = messageLanguage.deleteUserRoleSuccess;
}]);