'use strict';
app.controller('customerShareController', ['$rootScope', '$scope',
    function ($rootScope, $scope) {
        $scope.controllerId = "customerShareController";

        $scope.Customer = new CustomerViewModel();

        $scope.getShareViewData = function () {
            return { SharedParameter: JSON.stringify($scope.Customer) };
        };

    }]
);