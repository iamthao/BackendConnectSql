app.controller('changeRestorePasswordController', ['$scope', '$http', 'common', 'messageLanguage', function ($scope, $http, common, messageLanguage) {
    var controllerId = "changeRestorePasswordController";
    var getLogFn = common.logger.getLogFn;
    $scope.User = new ChangePasswordViewModel();


    $scope.getShareViewData = function () {
        return { SharedParameter: JSON.stringify($scope.User) };
    };

    $scope.IsProcessing = false;

    $scope.Save = function () {
        var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
        var isExistMess = false;

        if ($scope.User.NewPassword.trim() === "") {
            mess += messageLanguage.enterNewPass + "<br/>";
            isExistMess = true;
        }
        if ($scope.User.NewPassword.length < 6) {
            mess += messageLanguage.passwordLenght + "<br/>";
            isExistMess = true;
        }
        if ($scope.User.NewPassword !== $scope.User.ConfirmNewPassword) {
            mess += messageLanguage.passIsNotMatch + "<br/>";
            isExistMess = true;
        }
        if (isExistMess == true) {
            logError(mess);
            return false;
        }
        if ($scope.IsProcessing == false) {
            $scope.IsProcessing = true;
            $http.post('/Authentication/SaveChangeRestorePassword', { param: JSON.stringify($scope.User) })
                .success(function (result) {
                    $scope.IsProcessing = false;
                    if (result.Error === undefined || result.Error === '') {
                        var logSuccess = getLogFn(controllerId, "success");
                        logSuccess(messageLanguage.changePassSuccess);
                        $scope.User = new ChangePasswordViewModel();
                        setTimeout(function () {
                            location.href = "/Authentication/Signin";
                        }, 2500);
                    }
                }).error(function (error) {
                });
        }

    };


}]);