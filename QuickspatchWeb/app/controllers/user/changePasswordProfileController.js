'use strict';
app.controller('changePasswordProfileController', ['$rootScope', '$scope', 'messageLanguage', 'common', '$http', 'masterfileService',
    function ($rootScope, $scope, messageLanguage, common, $http, masterfileService) {
        $scope.controllerId = "changePasswordProfileController";

        $scope.User = new UserViewModel();
        var controllerId = "changePasswordProfileController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSuccess = getLogFn(controllerId, "success");
        var logError = getLogFn(controllerId, "error");


        $scope.Save = function () {
            var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
            var isExistMess = false;
            if ($scope.User.CurrentPassword == undefined || $scope.User.CurrentPassword == null || $scope.User.CurrentPassword === "") {
                mess += messageLanguage.currentpasswordrequired + "<br/>";
                isExistMess = true;
            }

            if ($scope.User.Password == undefined || $scope.User.Password == null || $scope.User.Password === "") {
                mess += messageLanguage.passwordrequired + "<br/>";
                isExistMess = true;
            }
            else if ($scope.User.Password.length < 6) {
                mess += messageLanguage.passwordLenght + "<br/>";
                isExistMess = true;

            }

            if ($scope.User.RePassword === undefined || $scope.User.RePassword.trim() === "") {
                mess += messageLanguage.rePasswordrequired + "<br/>";
                isExistMess = true;
            }
            else if ($scope.User.Password !== $scope.User.RePassword) {
                mess += messageLanguage.comparePassword + "<br/>";
                isExistMess = true;
            }

            if (isExistMess == true) {
                logError(mess);
                return;
            }

            var param = JSON.stringify($scope.User);
            masterfileService.callWithUrl("/User/SaveChangePasswordProfile")
                    .perform({ parameters: param })
                    .$promise.then(function (result) {
                        //console.log(result.Status);
                        if (result.Status == true) {
                            var popup = $("#popupWindow").data("kendoWindow");
                            popup.close();
                            logSuccess(messageLanguage.resetPasswordUserSuccess);
                        } else {
                            logError(messageLanguage.currentpasswordinvalid);
                        }
                    });

        };

        $scope.Cancel = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }

        $scope.ResetPassword = function () {

            $.ajax({
                method: "POST",
                url: "/User/SaveChangePasswordProfile",
                data: { password: 123456 }
            })
               .done(function (result) {
                   if (result == true) {

                       logSuccess(messageLanguage.resetPasswordUserSuccess);
                       var popup = $("#popupWindow").data("kendoWindow");
                       popup.close();
                   }
               });
        }

    }]
);