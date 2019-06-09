app.controller('notifyPackageController', ['$rootScope','$scope', '$http', 'common', '$timeout', '$document',
    function ($rootScope,$scope, $http, common, $timeout, $document) {
        var getLogFn = common.logger.getLogFn;

        if ($rootScope.IsShowAlertExtended == true) {
            //console.log('vo');
            $("#alert-extended").css({ display: "block" });
        }
        $('[data-toggle="tooltip"]').tooltip();
        $scope.IsShowNotifyAlert = "";
        $scope.showNotifyAlertPaclage = function () {
            if ($("#notify-extended").hasClass("open")) {
                $("#notify-extended").removeClass("open");
                $("#notify-decline").removeClass("open");
            } else {
                $("#notify-extended").addClass("open");
                $("#notify-decline").removeClass("open");
            }
        };
        $scope.Content = "Your package will be expired in 7 day(s). " +
            "Click to continue using QuickSpatch.";
    }]);