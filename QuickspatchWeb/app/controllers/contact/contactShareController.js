'use strict';
app.controller('contactShareController', ['$rootScope', '$scope',
    function ($rootScope, $scope) {
        $scope.controllerId = "contactShareController";

        $scope.Contact = new ContactViewModel();

        $scope.getShareViewData = function () {
            return { SharedParameter: JSON.stringify($scope.Contact) };
        };

    }]
);