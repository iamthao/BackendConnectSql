'use strict';
app.controller('resetPasswordUserController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "resetPasswordUserController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            common.activateController(null, $scope.controllerId).then(function () {
                //log(messageLanguage.resetPasswordUser);
            });
            
        }

       
    }]
);