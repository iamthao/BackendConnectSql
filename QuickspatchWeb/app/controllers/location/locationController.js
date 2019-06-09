'use strict';
app.controller('locationController', ['$rootScope', '$scope', 'common', 'messageLanguage', function ($rootScope, $scope, common, messageLanguage) {
    var controllerId = "locationController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    activate();

    function activate() {
        //common.activateController(null, controllerId).then(function () { log(messageLanguage.listLocation); });
        formatHtmlBodyMain();
        
    }

    
   
}]);