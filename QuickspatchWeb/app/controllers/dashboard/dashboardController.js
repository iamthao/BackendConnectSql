'use strict';
app.controller('dashboardController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', function ($rootScope, $scope, common, messageLanguage, $window) {
    var controllerId = "dashboardController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    $scope.RefreshCourierStatusFirst = true;
    $scope.RefreshCourierOnlineFirst = true;
    $scope.RefreshRequestStatusFirst = true;

    function activate() {
        $(window).resize();
        $(document).bind('webkitfullscreenchange mozfullscreenchange fullscreenchange', function (e) {
            $scope.formatPage();
        });
        //createScrollBarForList('courier-list-wrapper');
        // common.activateController(null, controllerId).then(function () { log(messageLanguage.listdashboard); });
       
    }

    //$scope.$on("LoadCourierStatusDone", function () {
    //    if ($scope.RefreshCourierOnlineFirst == false) {

    //        if ($scope.RefreshCourierOnlineFirst == false) {
    //            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
    //                // get $$childHead first and then iterate that scope's $$nextSiblings
    //                if (childScope.controllerId != undefined && childScope.controllerId == 'requestStatusController') {
    //                    childScope.refreshRequestStatus();
    //                    break;
    //                }
    //            }
    //            $scope.RefreshCourierOnlineFirst = true;
    //        }
    //    }

    //});
    //$scope.$on("LoadRequestStatusDone", function () {
    //    setTimeout(function () {

    //        if ($scope.RefreshCourierOnlineFirst == false) {
    //            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
    //                // get $$childHead first and then iterate that scope's $$nextSiblings
    //                if (childScope.controllerId != undefined && childScope.controllerId == 'requestStatusController') {
    //                    childScope.refreshRequestStatus();
    //                    break;
    //                }
    //            }
    //            $scope.RefreshCourierOnlineFirst = true;
    //        }
    //    }, 60000);

    //});

    activate();
    $scope.formatPage = function() {
        var fullScreen = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
        if (!fullScreen) {
            var panel = $(".screen-full").closest('.panel');
            panel.find("span[name=panel-minimize]").hide();
            panel.find("span[name=panel-maximize]").show();

            panel.find(".screen-fit-1").addClass("screen-fit-2").removeClass("screen-fit-1 screen-full");
            $(window).resize();
        }

    };

    $scope.fullscreenOn = function (el) {
        var panel = $(el.target).closest('.panel');

        panel.find("span[name=panel-minimize]").show();
        panel.find("span[name=panel-maximize]").hide();

        panel.find(".screen-fit-2").addClass("screen-fit-1 screen-full").removeClass("screen-fit-2");
        window.isFullScreen = true;
        panel.fullScreen(true);
    }

    $scope.fullscreenOff = function (el) {
        var panel = $(el).closest('.panel');
        panel.find("span[name=panel-minimize]").hide();
        panel.find("span[name=panel-maximize]").show();

        panel.find(".screen-fit-1").addClass("screen-fit-2").removeClass("screen-fit-1 screen-full");
        window.isFullScreen = false;
        $(document).fullScreen(false);
    }
}]);