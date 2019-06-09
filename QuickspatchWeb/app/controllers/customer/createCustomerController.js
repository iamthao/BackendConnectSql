'use strict';
app.controller('createCustomerController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "createCustomerController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.createCustomer); });
        }

        function callBackAfterCreateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }
        $scope.CreateMasterfileData($scope.controllerId, 'Customer', messageLanguage.createCustomerSuccess, callBackAfterCreateSuccess);

        $scope.getData = function() {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'customerShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);