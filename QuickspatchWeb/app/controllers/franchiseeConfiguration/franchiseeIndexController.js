'use strict';
app.controller('franchiseeIndexController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage', '$stateParams',
    function ($scope, masterfileService, $state, common, config, messageLanguage, $stateParams) {
        var events = config.events;
        $scope.controllerId = "franchiseeIndexController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);

        //1. franchisee 2. billing 3. billing history

        $scope.tabIndex = "";
        $scope.selectTab = function (val) {
            $scope.tabIndex = val;
        };
        function activate() {
            //console.log('$stateParams.tabIndex = ' + $stateParams.tabIndex);
            if ($stateParams.tabIndex == null || $stateParams.tabIndex == undefined) {
                $scope.tabIndex = 1;
            } else {
                $scope.tabIndex = 2;
                document.getElementById("select-tab-billing").click();
            }

            //console.log('$scope.tabIndex1 = ' + $scope.tabIndex);
        }
        activate();
        //console.log('$scope.tabIndex2 = ' + $scope.tabIndex);



    }]);