'use strict';

app.controller('baseFormPopupController', ['$scope', 'common', 'masterfileService', 'config', function ($scope, common, masterfileService, config) {
    var getLogFn = common.logger.getLogFn;
    var events = config.events;

    function closePopup() {
        var eles = $('div[data-ng-controller=baseFormPopupController]').parents();
        var popup = null;
        for (var i = 0; i < eles.length; i++) {
            if ($(eles[i]).attr('id') == 'popupWindow') {
                popup = $("#popupWindow").data("kendoWindow");
                popup.close();
                break;
            }
            if ($(eles[i]).attr('id') == 'popupWindowChild') {
                popup = $("#popupWindowChild").data("kendoWindow");
                popup.close();
                break;
            }
        }
    }
    $scope.UpdateMasterfileData = function (controllerId, model, successMessage, callback, callbackError) {
        var deRegisterUpdateEvent = $scope.$on(events.controllerFormSaveDataEvent, function (event, data) {
            if (data.controllerId == controllerId) {
                var param = {};
                // Get data from child
                for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                    // get $$childHead first and then iterate that scope's $$nextSiblings
                    if (childScope.controllerId != undefined && childScope.controllerId == controllerId) {
                        param = childScope.getData();
                        break;
                    }
                }
                masterfileService.update(model).perform({ parameters: param }).$promise.then(function (result) {
                    if (result.Error === undefined || result.Error === '') {
                        deRegisterUpdateEvent();
                        var logSuccess = getLogFn(controllerId, "success");
                        logSuccess(successMessage);
                        //var popup = $("#popupWindow").data("kendoWindow");
                        //popup.close();
                        closePopup();
                        callback(result);
                    }
                    else {
                        if (typeof callbackError == "function") {
                            callbackError(result);
                        }
                    }
                });
            }
        });
        var deRegisterCancelEvent = $scope.$on(events.controllerFormCancelSaveDataEvent, function (event, data) {
            if (data.controllerId == controllerId) {
                if (typeof deRegisterUpdateEvent == "function") {
                    deRegisterUpdateEvent();
                }
                deRegisterCancelEvent();
                //var popup = $("#popupWindow").data("kendoWindow");
                //popup.close();
                closePopup();

            }
        });
        var deRegisterEvent = $scope.$on("closePopupRegisterEvent_popupOptions", function () {
            if (typeof deRegisterUpdateEvent == "function") {
                deRegisterUpdateEvent();
            }
            if (typeof deRegisterCancelEvent == "function") {
                deRegisterCancelEvent();
            }
            deRegisterEvent();
        });
    };
    $scope.CreateMasterfileData = function (controllerId, model , successMessage, callback, callbackError) {
        var deRegisterCreateEvent = $scope.$on(events.controllerFormSaveDataEvent, function (event, data) {
            if (data.controllerId == controllerId) {
                var param= {};
                // Get data from child
                for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                    // get $$childHead first and then iterate that scope's $$nextSiblings
                    if (childScope.controllerId != undefined && childScope.controllerId == controllerId) {
                        param = childScope.getData();
                        break;
                    }
                }
                masterfileService.create(model).perform({ parameters: param }).$promise.then(function (result) {
                    if (result.Error === undefined || result.Error === '') {
                        deRegisterCreateEvent();
                        var logSuccess = getLogFn(controllerId, "success");
                        logSuccess(successMessage);
                        //var popup = $("#popupWindow").data("kendoWindow");
                        //popup.close();
                        closePopup();
                        callback(result);
                    } else {
                        if (typeof callbackError == "function") {
                            callbackError(result);
                        }
                        
                    }
                });
            }
        });
        var deRegisterCancelEvent = $scope.$on(events.controllerFormCancelSaveDataEvent, function (event, data) {
            if (data.controllerId == controllerId) {
                if (typeof deRegisterCreateEvent == "function") {
                    deRegisterCreateEvent();
                }
                deRegisterCancelEvent();
                //var popup = $("#popupWindow").data("kendoWindow");
                //popup.close();
                closePopup();
            }
        });
        var deRegisterEvent = $scope.$on("closePopupRegisterEvent_popupOptions", function () {
            if (typeof deRegisterCreateEvent == "function") {
                deRegisterCreateEvent();
            }
            if (typeof deRegisterCancelEvent == "function") {
                deRegisterCancelEvent();
            }
            deRegisterEvent();
        });

    };

}]);