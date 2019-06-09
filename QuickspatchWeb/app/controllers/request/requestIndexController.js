'use strict';
app.controller('requestIndexController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', function ($rootScope, $scope, common, messageLanguage, $window) {
    var controllerId = "requestIndexController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
  

    function activate() {
        // common.activateController(null, controllerId).then(function () { log(messageLanguage.listrequest); });
    }
    activate();
    $scope.tabIndex = 1;
    $scope.selectTab = function (val) {
        $scope.tabIndex = val;
    };

    //$scope.$on("ResetFormRequest", function (event, args) {

    //    $scope.$broadcast("ResetFormRequestParent");
    //});
    //$scope.$on("ResetFormHoldingRequest", function (event, args) {

    //    $scope.$broadcast("ResetFormHoldingRequestParent");
    //});
}]);