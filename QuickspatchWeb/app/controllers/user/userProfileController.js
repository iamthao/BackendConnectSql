'use strict';
app.controller('userProfileController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config','messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "userProfileController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.userProfile); });
        }

        $scope.ShowChangePasswordForm = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: 650,
                height: 220,
                title: "Change password",
                content: {
                    url: "/User/ChangePasswordProfile"
                }
            });
            popup.open();
        }
        $scope.EditProfile = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: 650,
                height: 280,
                title: "Edit Profile",
                content: {
                    url: "/User/UpdateProfile"
                }
            });
            //log(messageLanguage.editProfile);
            popup.open();
        }
        $scope.EditAvatar=function() {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: 600,
                height: 500,
                title: "Edit Profile Picture",
                content: {
                    url: "/User/EditAvatar"
                }
            });
            //log(messageLanguage.editAvatar);
            popup.open();
        }
    }]
);