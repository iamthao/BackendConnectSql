'use strict';
app.controller('updateScheduleController', ['$rootScope', '$scope', 'masterfileService', 'common', 'config', 'messageLanguage',
    function ($rootScope, $scope, masterfileService, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "updateScheduleController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateSchedule); });
        }
        
        function callBackAfterUpdateSuccess() {
            $scope.$root.$broadcast("ReloadScheduleGrid");
        }

        function callbackError(result) {
            var obj = result.WarningInfo;
            if (obj != undefined) {
                var title = '<span class="fa fa-exclamation-triangle"></span> Schedule conflict';
                var data = Base64.encode(JSON.stringify(obj));
                var popup = $("#popupWindowChild").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "250px",
                    title: title,
                    content: {
                        url: "/Schedule/Warning?data=" + data
                    },
                    close: function (e) {
                        popup.content('');
                        //$scope.cancelRequest();
                        $scope.popConfirmOpen = false;
                    },
                    animation: false
                });
                popup.open();
            }
        }

        $scope.UpdateMasterfileData($scope.controllerId, 'Schedule', messageLanguage.updateScheduleSuccess, callBackAfterUpdateSuccess, callbackError);

        $scope.getData = function() {
            var shareData = {};
            for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                // get $$childHead first and then iterate that scope's $$nextSiblings
                if (childScope.controllerId != undefined && childScope.controllerId == 'scheduleShareController') {
                    shareData = childScope.getShareViewData();
                    break;
                }
            }
            return shareData;
        };
    }]
);