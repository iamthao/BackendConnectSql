'use strict';
app.controller('moduleShareController', ['$rootScope', '$scope',
    function ($rootScope, $scope) {
        $scope.controllerId = "moduleShareController";

        $scope.Module = new ModuleViewModel();

        $scope.getShareViewData = function () {
            return { SharedParameter: JSON.stringify($scope.Module) };
        };

    }]
);