app.controller('restorePasswordController', ['$scope', '$http', 'common', function ($scope, $http, common) {
    var getLogFn = common.logger.getLogFn;
    $scope.email = "";
    $scope.click = function () {
        if ($scope.email === "") {
            common.showErrorModelState(new FeedbackViewModel("error", "Please input email", "", []));
            return;
        }
        //console.log($scope.email);
        $http.post("/Authentication/RestorePassword", { email: $scope.email }).success(function (result) {
            if (result.Error === undefined || result.Error === '') {
                var logSuccess = getLogFn("restorePasswordController", "success");
                logSuccess("Please check your email inbox");
                setTimeout(function () {
                    location.href = "/Authentication/Signin";
                }, 3500);
            }
        });
    }
}]);