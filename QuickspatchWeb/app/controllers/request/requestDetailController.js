'use strict';
app.controller('requestDetailController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', '$timeout',
    function ($rootScope, $scope, common, messageLanguage, $window, $timeout) {
    var controllerId = "requestDetailController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);


    $scope.tabIndex = 0;
    $scope.selectTab = function (val) {
        $scope.tabIndex = val;
    };
    $scope.visibleTracking = false;
    $scope.classTracking = 'active';
    $scope.classDetail = '';
    function activate() {
        var visibleTracking = $('#popupWindow').data('VisibleTracking');
        if (visibleTracking != undefined && visibleTracking) {
            $scope.visibleTracking = visibleTracking;
            $scope.tabIndex = 2;
            $scope.classTracking = '';
            $scope.classDetail = 'active';
        } else {
            $scope.tabIndex = 1;
            $scope.classTracking = 'active';
            $scope.classDetail = '';
        }
    }

    activate();
    


}]);