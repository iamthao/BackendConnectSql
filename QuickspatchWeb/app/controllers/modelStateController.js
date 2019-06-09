app.controller('modelStateController', ['$scope', 'ModelStateService', function ($scope, ModelStateService) {
    $scope.feedback = {};
    $scope.isShown = false;

    ModelStateService.onElevatedValidateModelState($scope,
    function (feedback) {
        $scope.feedback = feedback.Data;
        if ($scope.feedback != null) {
            $scope.isShown = true;
        }
        //console.log($scope.feedback.ModelState);
    });
    $scope.closeValidation=function() {
        $scope.isShown = false;
    }
}]);