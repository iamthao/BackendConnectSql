'use strict';
app.controller('contactUsController', ['$rootScope', '$scope', 'messageLanguage', 'common', '$http',
    function ($rootScope, $scope, messageLanguage, common, $http) {
        $scope.controllerId = "contactUsController";

        $scope.User = new UserViewModel();
        var controllerId = "contactUsController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSuccess = getLogFn(controllerId, "success");
        var logError = getLogFn(controllerId, "error");

        function validateEmail(email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }

        $scope.Send = function () {
            console.log(validateEmail($scope.User.Email));
            var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:";
            mess += "<ol>";
            var isExistMess = false;
            if ($scope.User.FullName == undefined || $scope.User.FullName == null || $scope.User.FullName === "") {
                mess += "<li>" + messageLanguage.fullNameRequired + "</li>";
                isExistMess = true;
            }
            if ($scope.User.Email == undefined || $scope.User.Email == null || $scope.User.Email === "") {
                mess += "<li>" + messageLanguage.emailRequired + "</li>";
                isExistMess = true;
            }
            else if (validateEmail($scope.User.Email) == false) {
                mess += "<li>" + messageLanguage.emailInvalid + "</li>";
                isExistMess = true;
            }
            if ($scope.User.Subject == undefined || $scope.User.Subject == null || $scope.User.Subject === "") {
                mess += "<li>" + messageLanguage.subjectRequired + "</li>";
                isExistMess = true;
            }
            if ($scope.User.Content == undefined || $scope.User.Content == null || $scope.User.Content === "") {
                mess += "<li>" + messageLanguage.contentRequired + "</li>";
                isExistMess = true;
            }
            mess += "</ol>";
            if (isExistMess == true) {
                logError(mess);
                return;
            }

            $http.get("Common/SendContactUs",
                { params: { fullname: $scope.User.FullName, email: $scope.User.Email, subject: $scope.User.Subject, content: $scope.User.Content } })
                    .then(function (result) {
                        //console.log(result);
                        if (result.data == "true") {
                            logSuccess(messageLanguage.sendMessageSuccessfully);
                            var popup = $("#popupWindow").data("kendoWindow");
                                   popup.close();
                        }
            });

        };

        $scope.Cancel = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }


    }]
);