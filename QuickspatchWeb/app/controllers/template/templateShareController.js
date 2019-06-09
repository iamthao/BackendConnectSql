'use strict';
app.controller('templateShareController', ['$rootScope', '$scope', 'uiGmapGoogleMapApi', 'common', 'masterfileService',
    function ($rootScope, $scope, uiGmapGoogleMapApi, common, masterfileService) {
        $scope.controllerId = "templateShareController";

        var getLogFn = common.logger.getLogFn;


        $scope.Data = new TemplateViewModel();

        $scope.getShareViewData = function () {
            return { SharedParameter: JSON.stringify($scope.Data) };
        };
        
       function activate() {
           $('a[data-function="FOOTER_ACTION_SAVE"]').removeAttr('disabled');
           $('a[data-function="FOOTER_ACTION_SAVE"]').removeClass('k-state-disabled');
       }

        activate();
    }]
);