'use strict';
app.controller('userShareController', ['$rootScope', '$scope',
    function ($rootScope, $scope) {
        $scope.controllerId = "userShareController";

        $scope.User = new UserViewModel();

        $scope.getShareViewData = function () {
            return { SharedParameter: JSON.stringify($scope.User) };
        };

    }]
);