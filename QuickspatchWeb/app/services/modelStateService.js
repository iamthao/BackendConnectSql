'use strict';
app.factory('ModelStateService', function ($rootScope) {
    var elevatedValidateModelState = function (feedback) {
        $rootScope.$broadcast(Postbox.VALIDATION_MODEL_STATE, {
            Data: feedback
        });
    };

    var onElevatedValidateModelState = function ($scope, handler) {
        $scope.$on(Postbox.VALIDATION_MODEL_STATE, function (event, feedback) {
            // note that the handler is passed the problem domain parameters 
            handler(feedback);
        });
    };
    return {
        elevatedValidateModelState: elevatedValidateModelState,
        onElevatedValidateModelState: onElevatedValidateModelState
    }
});