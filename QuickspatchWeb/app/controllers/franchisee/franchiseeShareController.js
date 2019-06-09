'use strict';
app.controller('franchiseeShareController', ['$rootScope', '$scope', '$resource',
    function ($rootScope, $scope, $resource) {
        $scope.controllerId = "franchiseeShareController";

        $scope.Franchisee = new FranchiseeViewModel();


        $scope.sendData = {};
        $scope.getShareViewData = function () {
            angular.copy($scope.Franchisee, $scope.sendData);
            if ($scope.Franchisee.StartActiveDate !== undefined && $scope.Franchisee.StartActiveDate !== '') {
                var startOfDate = new Date($scope.Franchisee.StartActiveDate);
                
                $scope.sendData.StartActiveDate = (new Date(startOfDate.getFullYear(), startOfDate.getMonth(), startOfDate.getDate(), 0, 0, 0)).toUTCString();
            }
            if ($scope.Franchisee.EndActiveDate !== undefined && $scope.Franchisee.EndActiveDate !== '') {
                var endOfDate = new Date($scope.Franchisee.EndActiveDate);
                $scope.sendData.EndActiveDate = (new Date(endOfDate.getFullYear(), endOfDate.getMonth(), endOfDate.getDate(), 23, 59, 59)).toUTCString();
            }

            return { SharedParameter: JSON.stringify($scope.sendData) };
        };

        $scope.generateKey = function () {
            $resource('/FranchiseeTenant/GenerateLicenseKey')
                .get()
                .$promise
                .then(function (result) {
                    $scope.Franchisee.LicenseKey = result.Data;
                });

            EnableCreateFooterButton(true);
        };
    }]
);