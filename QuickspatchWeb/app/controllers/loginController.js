app.controller('loginController', ['$scope', 'common', 'messageLanguage', '$http', function ($scope, common, messageLanguage, $http) {
    var controllerId = "loginController";
    $scope.isShowForgotPassword = false;
    $scope.userName = getCookie("USRX").toString();
    $scope.password = getCookie("PSRX").toString();
    $scope.rememberMe = Boolean(getCookie("CSRX").toString());
    $scope.IsShowLoading = false;
    $scope.email = "";
    $scope.returnUrl = '';
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);

    $scope.login = function () {
        if ($scope.userName === "" || $scope.password === "") {
            common.showErrorModelState(new FeedbackViewModel("error", messageLanguage.fillInUserAndPass, "", []));
            return;
        }
        $scope.IsShowLoading = true;
        setTimeout(function () {
            var url = "/Authentication/QuickspatchSignIn";
            var data =
             {
                 userName: $scope.userName,
                 password: $scope.password,
                 rememberMe: $scope.rememberMe,
                 returnUrl: $scope.returnUrl
             };
            var request = $.ajax({ cache: false, async: false, url: url, type: "POST", data: data, dataType: "json" });
            request.done(function (result) {
                $scope.IsShowLoading = false;
                if (result.Error === undefined || result.Error === '') {
                    if ($scope.rememberMe == true) {
                        setCookie("USRX", $scope.userName, 365);
                        setCookie("PSRX", $scope.password, 365);
                        setCookie("CSRX", "true", 365);
                    } else {
                        setCookie("USRX", "", 365);
                        setCookie("PSRX", "", 365);
                        setCookie("CSRX", "false", 365);
                    }
                    setCookie("IsCamino", result.isCamino, 365);
                    location.href = "/";
                } else {
                   // console.log(result);
                    if (result.Data != null && result.Data.Type != null && result.Data.Type != '' && result.Data.KeyCode != null && result.Data.KeyCode != '') {
                        location.href = "/licenceextension?keyCode=" + result.Data.KeyCode;
                    } else {
                        common.showErrorModelState(new FeedbackViewModel(result.Status, result.Error, result.StackTrace, result.ModelStateErrors));
                        $scope.$apply();
                    }
                    
                }
            });
        }, 1000);
        
    };

    $scope.autoLogin = function ($event) {
        var currentKey = $event.which || $event.charCode;
        if (currentKey === 13) {
            $scope.login();
        }
    }
    $scope.showForgotPassowrd = function (type) {
        $("#send-reset-code").css({ display: "block" });
        $scope.isShowForgotPassword = type;

    }
    $scope.resetPassword = function() {
        $http.post("/Authentication/RestorePassword", { email: $scope.email }).success(function (result) {
            if (result.Error === undefined || result.Error === '') {
                var logSuccess = getLogFn("restorePasswordController", "success");
                logSuccess("Please check your email inbox");
                setTimeout(function () {
                    location.href = "/Authentication/Signin";
                }, 2500);
            }
        });
    };
}]);