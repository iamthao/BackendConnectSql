'use strict';
app.controller('userSharePasswordController', ['$rootScope', '$scope', 'messageLanguage', 'common',
    function ($rootScope, $scope, messageLanguage, common) {
        $scope.controllerId = "userSharePasswordController";

        $scope.User = new UserViewModel();
        var controllerId = "userSharePasswordController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSuccess = getLogFn(controllerId, "success");
        var logError = getLogFn(controllerId, "error");

        $scope.IsProcessing = false;

        $scope.Save = function () {
            var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
            var isExistMess = false;
            if ($scope.User.Password == undefined || $scope.User.Password == null || $scope.User.Password === "") {
                mess += messageLanguage.passwordrequired + "<br/>";
                isExistMess = true;
            } else {
                if ($scope.User.Password.length < 6) {
                    mess += messageLanguage.passwordLenght + "<br/>";
                    isExistMess = true;

                }
            }
            if ($scope.User.RePassword === undefined || $scope.User.RePassword.trim() === "") {
                mess += messageLanguage.rePasswordrequired + "<br/>";
                isExistMess = true;
            } else if ($scope.User.Password !== $scope.User.RePassword) {
                mess += messageLanguage.comparePassword + "<br/>";
                isExistMess = true;
            }
            if (isExistMess == true) {
                logError(mess);
                return;
            }
            
            if ($scope.IsProcessing == false) {
                $scope.IsProcessing = true;
                $.ajax({
                    method: "POST",
                    url: "/User/resetpassword",
                    data: { id: $scope.User.Id, password: $scope.User.Password, username: $scope.User.UserName }
                }).done(function (result) {
                    $scope.IsProcessing = false;
                    if (result == true) {
                        logSuccess(messageLanguage.resetPasswordUserSuccess);
                        var popup = $("#popupWindow").data("kendoWindow");
                        popup.close();
                    }
                });
            }
        };

        $scope.Cancel = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }

        $scope.ResetPassword = function () {

            $.ajax({
                method: "POST",
                url: "/User/resetpassword",
                data: { id: $scope.User.Id, password: 123456, username: $scope.User.UserName }
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