'use strict';
app.controller('courierShareController', ['$rootScope', '$scope',
    function ($rootScope, $scope) {
        $scope.controllerId = "courierShareController";

        $scope.Courier = new CourierViewModel();
      
        $scope.getShareViewData = function () {
            
            return { SharedParameter: JSON.stringify($scope.Courier) };
        };

        //
        

    }]
);