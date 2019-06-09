app.directive('routeLoadingIndicator', function ($rootScope) {
    return {
        restrict: 'E',
        template: "<div ng-show='isRouteLoading' class='loading-indicator'>" +
        "<div class='loading-indicator-body'>" +
        "<h3 class='loading-title'>Loading...</h3>" +
        "<div class='spinner'><rotating-plane-spinner></rotating-plane-spinner></div>" +
        "</div>" +
        "</div>",
        replace: true,
        link: function (scope, elem, attrs) {
            scope.isRouteLoading = false;

            $rootScope.$on('$stateChangeStart', function () {
                scope.isRouteLoading = true;
            });
            $rootScope.$on('$stateChangeSuccess', function () {
                scope.isRouteLoading = false;
            });
            $rootScope.$on('$stateChangeError', function () {
                scope.isRouteLoading = false;
            });
        }
    };
});