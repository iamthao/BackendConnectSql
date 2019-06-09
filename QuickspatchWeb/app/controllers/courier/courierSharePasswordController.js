'use strict';
app.controller('courierSharePasswordController', ['$rootScope', '$scope', 'messageLanguage', 'common',
    function ($rootScope, $scope, messageLanguage, common) {
        $scope.controllerId = "courierSharePasswordController";

        $scope.Courier = new CourierViewModel();
        var controllerId = "courierSharePasswordController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSuccess = getLogFn(controllerId, "success");
        var logError = getLogFn(controllerId, "error");
        $scope.IsProcessing = false;
        $scope.Save = function () {
            var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
            var isExistMess = false;
            if ($scope.Courier.Password == undefined || $scope.Courier.Password == null || $scope.Courier.Password === "") {
                mess += messageLanguage.passwordrequired + "<br/>";
                isExistMess = true;
            } else {
                if ($scope.Courier.Password.length < 6) {
                    mess += messageLanguage.passwordLenght + "<br/>";
                    isExistMess = true;
                }
            }
            if ($scope.Courier.RePassword === undefined || $scope.Courier.RePassword.trim() === "") {
                mess += messageLanguage.rePasswordrequired + "<br/>";
                isExistMess = true;
            }
            else if ($scope.Courier.Password !== $scope.Courier.RePassword) {
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
                    url: "/Courier/resetpassword",
                    data: { id: $scope.Courier.Id, password: $scope.Courier.Password, username: $scope.Courier.UserName }
                }).done(function (result) {
                    if (result == true) {
                        $scope.IsProcessing = false;
                        logSuccess(messageLanguage.resetPasswordUserSuccess);
                        var popup = $("#popupWindow").data("kendoWindow");
                        popup.close();
                    }
                });
            }
        };

        $scope.Cancel = function() {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        };

        $scope.ResetPassword = function() {
            //$scope.Courier.Password = "123456";
            $.ajax({
                method: "POST",
                url: "/Courier/resetpassword",
                data: { id: $scope.Courier.Id, password: 123456, username: $scope.Courier.UserName }
            }).done(function(result) {
                if (result == true) {
                    logSuccess(messageLanguage.resetPasswordUserSuccess);
                    var popup = $("#popupWindow").data("kendoWindow");
                    popup.close();
                }
            });

        };

    }]
);