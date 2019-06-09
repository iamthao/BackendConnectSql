'use strict';
app.controller('updateProfileController', ['$rootScope', '$scope','$state', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope,$state, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateProfileController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, controllerId).then(function () { log(messageLanguage.editProfile); });
        }
        $scope.User = new UpdateProfileViewModel();

        function callBackAfterUpdateSuccess() {
            $state.reload();
        }
        $scope.UpdateMasterfileData($scope.controllerId, 'User', messageLanguage.updateProfileSuccess, callBackAfterUpdateSuccess);

        $scope.getData = function() {
            return { SharedParameter: JSON.stringify($scope.User) };
        };

    }]
);