app.controller('footerController', ['$scope', 'common', function ($scope, common) {
    $scope.Save = function (controllerId) {
        common.formSaveDataEvent(controllerId);
        EnableCreateFooterButton(false);
    };

    $scope.Cancel = function (controllerId) {
        common.formCancelDataEvent(controllerId);
    };

    $scope.CreateNewOrder = function (controllerId) {
        if (controllerId == "updatePatientController") {
            $scope.$root.$broadcast("CreateNewOrderFromPatient");
        }
    }
}]);