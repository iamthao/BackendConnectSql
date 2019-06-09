(function () {
    'use strict';

    var controllerId = 'shell';
    angular.module('app').controller(controllerId,
        ['$rootScope', '$scope', 'common', 'config', '$location', shell]);
    function shell($rootScope, $scope, common, config, $location) {
        var vm = this;
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn("shell");
        var events = config.events;
        vm.busyMessage = 'Please wait ...';
        vm.isBusy = true;
        vm.spinnerOptions = {
            radius: 10,
            lines: 7,
            length: 0,
            width: 8,
            speed: 1.7,
            corners: 1.0,
            trail: 100,
            color: '#7761a7'
        };

        $scope.userRoleId = 0;
        $rootScope.IsQuickTour = false;
        $scope.init = function (isQuickTour) {
            $rootScope.IsQuickTour = isQuickTour;
            //$scope.userRoleId = userRole;

            window.onbeforeunload = null;
            $rootScope.$$listeners.$locationChangeStart = [];
            activate();
        }

        function activate() {
            //logSuccess('Template loaded', null, true);
            common.activateController([], controllerId);
        }

        function toggleSpinner(on) {
            vm.isBusy = on;
        }

        $rootScope.$on('$stateChangeStart',
            function (event, next, current) {
                //if ($scope.userRoleId == 1 && $location.$$url == "/scheduler") {
                //    $location.path("/user");
                //}
                toggleSpinner(true);
            }
        );

        $rootScope.$on(events.controllerActivateSuccess,
            function (data) {
                toggleSpinner(false);
            }
        );

        $rootScope.$on(events.spinnerToggle,
            function (data) { toggleSpinner(data.show); }
        );

    };
})();