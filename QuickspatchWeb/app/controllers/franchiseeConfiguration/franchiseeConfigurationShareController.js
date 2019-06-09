'use strict';
app.controller('franchiseeConfigurationShareController', ['$rootScope', '$scope',
    function ($rootScope, $scope) {
        $scope.controllerId = "franchiseeConfigurationShareController";

        $scope.FranchiseeConfiguration = new FranchiseeConfigurationViewModel();

        $scope.getShareViewData = function () {
            return { SharedParameter: JSON.stringify($scope.FranchiseeConfiguration) };
        };

    }]
);