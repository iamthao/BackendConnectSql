'use strict';
app.controller('navigationController', function ($scope, $location) {
    $scope.isActive = function (path) {
        return $location.path().substr(0, path.length) == path;
    };
});

app.controller('pageAnimationController', function ($scope, $location) {
    $scope.pageClass = function () {
        if ($location.path().indexOf("/user")) {
            return "page page-contact";
        }
        return "page page-home";
    };
});