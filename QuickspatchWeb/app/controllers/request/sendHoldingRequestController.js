'use strict';
app.controller('sendHoldingRequestController', ['$rootScope', '$scope', 'common', 'masterfileService', function ($rootScope, $scope, common, masterfileService) {
    var controllerId = "sendHoldingRequestController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    var logError = getLogFn(controllerId, "Error");
    activate();


    function activate() {

    }

    $scope.IsShowHoldingForm = false;
    $scope.IsShowHoldingToRequestForm = false;
    $scope.SearchTextEncoded = "";
    $scope.modelName = "HoldingRequest";
    //$scope.HoldingRequestSelectedId = 0;
    $scope.IsClickEdit = false;
    $scope.IsShowLoading = false;

    //generate Calendar Filter


    // create Holding Request
    $scope.ShowHoldingForm = function (type) {
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: 650,
            height: 365,
            title: "Create Holding Request",
            content: {
                url: "/Request/PartialCreateHoldingRequest/" + 0
            }
        });
        popup.open();
        //Old 
        //$scope.IsShowHoldingForm = type;
        //$("#holdingGrid").css({ height: $scope.IsShowHoldingForm ? $(window).height() - 443 : $(window).height() - 217 });
        //$("#holdingGrid").children(".k-grid-content").height($scope.IsShowHoldingForm ? $(window).height() - 482 : $(window).height() - 255);

        //if (type) {
        //    $scope.$root.$broadcast("HideFormRequest", {});
        //    $scope.$emit("ResetFormHoldingRequest");
        //} else {
        //    $scope.IsClickEdit = false;
        //    $scope.$broadcast("ResetFormHoldingRequestParent");
        //}
        //$scope.HoldingRequestSelectedId = '';
    };

    $scope.HoldingToRequest = new HoldingToRequestViewModel();

    $scope.$watch("HoldingToRequest.SendingTime", function (newValue, oldValue) {
        //console.log(newValue, oldValue);
        if (newValue !== undefined && newValue !== null && newValue != "" && newValue != "__:__") {
            $scope.HoldingToRequest.SendingTime = common.getValueOfTime(newValue);
        }
    });

    $scope.$watch("HoldingToRequest.AutoAssign", function () {
        if ($scope.HoldingToRequest.AutoAssign == true) {
            $scope.HoldingToRequest.CourierId = 0;
            $.ajax({
                url: "/courier/getautoassigncourier"
            })
          .done(function (data) {
              if (data != undefined && data.Data != undefined && data.Data.length > 0) {
                  $scope.HoldingToRequest.CourierId = data.Data[0].Id;
                  $scope.$broadcast('CourierHoldingToRequest_ChangeDataSource', data.Data[0].Id);
              }
          });

        }

    });
    $scope.$watch("HoldingToRequest.CourierId", function (newValue, oldValue) {
        if (newValue !== undefined && newValue !== null && newValue != "" && newValue != oldValue && oldValue != 0 && newValue != 0) {

            $scope.HoldingToRequest.AutoAssign = false;
        }


    });

    $scope.$watch("HoldingToRequest.IsStat", function (newValue, oldValue) {
        var timePicker = $('#HoldingRequestSendingTime').data("kendoTimePicker");

        if (newValue) {
            //$scope.HoldingToRequest.SendingTime = kendo.toString(new Date(), "HH:mm");
            timePicker.enable(false);
        } else {
            //$scope.HoldingToRequest.SendingTime = '';
            timePicker.enable();
        }
    });

    $scope.cancel = function () {
        var popup = $("#popupWindow").data("kendoWindow");
        popup.close();
    }
    $scope.sendHoldingRequest = function () {
        var scope = angular.element("#holding-request-controller").scope();
       // console.log(scope);

        var currentDate = new Date();
        var endOfDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59);

        var sendingTime = '';
        if ($scope.HoldingToRequest.SendingTime !== undefined && $scope.HoldingToRequest.SendingTime !== '') {
            var tempSending = $scope.HoldingToRequest.SendingTime.split(":");
            var hourAddSending = parseInt(tempSending[0]);
            var secondAddSending = parseInt(tempSending[1].split(" ")[0]);
            if (tempSending[1].split(" ")[1] == 'PM') {
                hourAddSending += 12;
            }
            if (tempSending[1].split(" ")[1] == 'AM' && hourAddSending == 12) {
                hourAddSending = 0;
            }
            sendingTime = (new Date((new Date()).setHours(hourAddSending, secondAddSending))).toUTCString();
        }

        var url = '/HoldingRequest/SendHoldingRequest';
        var sendData = {
            HoldingRequestSelectedId: $scope.HoldingToRequest.Id,
            CourierId: $scope.HoldingToRequest.CourierId,
            SendingTime: sendingTime,
            IsStat: $scope.HoldingToRequest.IsStat,
            ExpiredTime: Math.round((endOfDate - currentDate) / 1000)
        };

        if ($scope.HoldingRequestSelectedId !== 0 || arr.length > 0) {
            clearTimeout($scope.timeout);
            $scope.timeout = setTimeout(function () {
                masterfileService.callWithUrl(url).perform({ data: JSON.stringify(sendData) }).$promise.then(function (data) {

                    if (data.Error === undefined || data.Error === '') {
                        var logSuccess = getLogFn(controllerId, "success");
                        if ($scope.$parent.deleteMessage != undefined) {
                            $scope.$root.$broadcast("ReloadRequestGrid");
                            logSuccess($scope.$parent.deleteMessage);
                            $scope.cancel();
                        } else {
                            logSuccess('Send holding request successfully');
                            $scope.cancel();
                            scope.Search();
                        }

                    } else {
                        $scope.HoldingRequestSelectedId = sendData.HoldingRequestSelectedId;
                    }

                });
            }, 1000);
        }

    };


}]);