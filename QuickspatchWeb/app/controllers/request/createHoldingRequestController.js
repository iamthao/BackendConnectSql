'use strict';
app.controller('createHoldingRequestController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', 'masterfileService', '$http', '$timeout','$interval',
function ($rootScope, $scope, common, messageLanguage, $window, masterfileService, $http, $timeout,$interval) {
        $scope.controllerId = "createHoldingRequestController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();      

        $scope.IsCreate = false;
        $scope.setCreateOrUpdate = function (val) {
            $scope.IsCreate = val;
        }
        $scope.ShowUpdateHoldingRequest = false;

        $scope.HoldingRequest = new HoldingRequestViewModel();

        $scope.LocationFromDefault = 0;
        $scope.LocationToDefault = 0;

        $scope.LocationFromDefaultName = "";
        $scope.LocationToDefaultName = "";

        $scope.IsUpdate = false;
        $scope.StartTimeBinding = "";
        $scope.EndTimeBinding = "";
        function activate() {
            // common.activateController(null, controllerId).then(function () { log(messageLanguage.listrequest); });
            $http.get("FranchiseeConfiguration/GetLocationDefault")
              .then(function (result) {
                  $scope.LocationFromDefault = result.data.LocationFromId;
                  $scope.LocationToDefault = result.data.LocationToId;

                  $scope.LocationFromDefaultName = result.data.LocationFromName;
                  $scope.LocationToDefaultName = result.data.LocationToName;

                  if ($scope.HoldingRequest.Id > 0) {
                      $scope.$broadcast('LocationFrom_Change', [{ KeyId: $scope.HoldingRequest.LocationFrom, DisplayName: $scope.HoldingRequest.LocationFromName }]);
                      $scope.$broadcast('LocationTo_Change', [{ KeyId: $scope.HoldingRequest.LocationTo, DisplayName: $scope.HoldingRequest.LocationToName }]);
                  }
              });                               
        }

        function callBackAfterCreateSuccess() {
            $scope.$root.$broadcast("ReloadGrid");
        }

        var objUpdate = $("#popupWindow").data('updateholdingrequest');
        var obj = new HoldingRequestViewModel();
        if (objUpdate != undefined && objUpdate != null) {
            //console.log(objUpdate.StartTimeNoFormat)
            obj.LocationFrom = objUpdate.LocationFromId;
            obj.LocationTo = objUpdate.LocationToId;
            obj.StartTime = moment(objUpdate.StartTimeNoFormat).format('MM/DD/YYYY HH:mm');
            obj.EndTime = moment(objUpdate.EndTimeNoFormat).format('MM/DD/YYYY HH:mm');
            $("#popupWindow").data('updateholdingrequest', null);
        }

        var intrval = $interval(function () {
            var fromCtr = $("#from").data('kendoDropDownList');
            var toCtr = $("#to").data('kendoDropDownList');               
            var startCtr = $('#start-time').data('kendoTimePicker');
            var endCtr = $('#end-time').data('kendoTimePicker');
            var initControlOk = true;
            initControlOk &= fromCtr != undefined;
            initControlOk &= toCtr != undefined;
            initControlOk &= startCtr != undefined;
            initControlOk &= endCtr != undefined;
            if (initControlOk) {
                $timeout(function () {
                    $scope.$apply(function () {
                        $scope.HoldingRequest.LocationFrom = obj.LocationFromId;
                        $scope.HoldingRequest.LocationTo = obj.LocationToId;
                        $scope.HoldingRequest.StartTime = obj.StartTime;
                        $scope.HoldingRequest.EndTime = obj.EndTime;
                        
                    });
                });
                fromCtr.value(obj.LocationFrom);
                toCtr.value(obj.LocationTo);             
                startCtr.value(obj.StartTime != null && obj.StartTime != '' ? moment(obj.StartTime).format('h:mm A') : obj.StartTime);
                endCtr.value(obj.EndTime != null && obj.EndTime != '' ? moment(obj.EndTime).format('h:mm A') : obj.EndTime);
                $timeout(function () {
                    $scope.$apply(function () {                      
                      
                    });
                });

                $interval.cancel(intrval);
            }
        }, 200);


        //$watch location from
        $scope.setFromDefault = function () {
            //if ($scope.LocationFromDefault > 0) {
            //    $scope.$broadcast('LocationFrom_Change', [{ KeyId: $scope.LocationFromDefault, DisplayName: $scope.LocationFromDefaultName }]);
            //} 
            var defaultToId = parseInt($('#from').data('kendoDropDownList').value());

            if (defaultToId != $scope.LocationFromDefault && $scope.LocationFromDefault > 0) {
                $scope.HoldingRequest.LocationFrom = $scope.LocationFromDefault;
                $('#from').data('kendoDropDownList').value($scope.LocationFromDefault);
                $('#from').data('kendoDropDownList').trigger("change");
            }
        }
        $scope.$watch("HoldingRequest.LocationFrom", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if ($('#from').data('kendoDropDownList') != undefined || $('#from').data('kendoDropDownList') != null) {
                    $('#from').data('kendoDropDownList').trigger("change");
                }

            }
        });

        //$watch location to
        $scope.setToDefault = function () {
            var defaultToId = parseInt($('#to').data('kendoDropDownList').value());

            if (defaultToId != $scope.LocationToDefault && $scope.LocationToDefault > 0) {
                $scope.HoldingRequest.LocationTo = $scope.LocationToDefault;
                $('#to').data('kendoDropDownList').value($scope.LocationToDefault);
                $('#to').data('kendoDropDownList').trigger("change");
            }
        }

        $scope.$watch("HoldingRequest.LocationTo", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if ($('#to').data('kendoDropDownList') != undefined || $('#to').data('kendoDropDownList') != null) {
                    $('#to').data('kendoDropDownList').trigger("change");
                }

            }
        });

        function convertUtcTime() {         
            //Thao         
            //TH tra ve 02:03 PM
            if ($scope.HoldingRequest.StartTime.indexOf('AM') > 0 || $scope.HoldingRequest.StartTime.indexOf('PM') > 0) {

                var tempStart = $scope.HoldingRequest.StartTime.split(":");
                var hourAddStart = parseInt(tempStart[0]);
                var secondAddStart = parseInt(tempStart[1].split(" ")[0]);

                if (tempStart[1].split(" ")[1] == 'PM' && hourAddStart != 12) {
                    hourAddStart += 12;
                }
                if (tempStart[1].split(" ")[1] == 'AM' && hourAddStart == 12) {
                    hourAddStart = 0;
                }

                $scope.HoldingRequest.StartTime = (new Date((new Date()).setHours(hourAddStart, secondAddStart))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.HoldingRequest.StartTime != '' && $scope.HoldingRequest.StartTime != undefined) {
                var a = new Date(Date.parse($scope.HoldingRequest.StartTime));
                $scope.HoldingRequest.StartTime = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();              
            }

            if ($scope.HoldingRequest.EndTime.indexOf('AM') > 0 || $scope.HoldingRequest.EndTime.indexOf('PM') > 0) {

                var tempStart1 = $scope.HoldingRequest.EndTime.split(":");
                var hourAddStart1 = parseInt(tempStart1[0]);
                var secondAddStart1 = parseInt(tempStart1[1].split(" ")[0]);

                if (tempStart1[1].split(" ")[1] == 'PM' && hourAddStart1 != 12) {
                    hourAddStart1 += 12;
                }
                if (tempStart1[1].split(" ")[1] == 'AM' && hourAddStart1 == 12) {
                    hourAddStart1 = 0;
                }

                $scope.HoldingRequest.EndTime = (new Date((new Date()).setHours(hourAddStart1, secondAddStart1))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.HoldingRequest.EndTime != '' && $scope.HoldingRequest.EndTime != undefined) {
                var a = new Date(Date.parse($scope.HoldingRequest.EndTime));
                $scope.HoldingRequest.EndTime = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();
            }
        }

        $scope.getShareViewData = function () {
            convertUtcTime();

            return { SharedParameter: JSON.stringify($scope.HoldingRequest) };
        };

        $scope.setTimeFrom = function(value) {
            $scope.StartTimeBinding = value;
        }
        $scope.setTimeTo = function (value) {
            $scope.EndTimeBinding = value;
        }
        $scope.AddHoldingRequest = function () {
            masterfileService.create('HoldingRequest').perform({ parameters: $scope.getShareViewData() }).$promise.then(function (result) {
                if (result.Error === undefined || result.Error === '') {
                    var logSuccess = getLogFn($scope.controllerId, "success");
                    logSuccess(messageLanguage.createHoldingRequestSuccess);
                    $scope.cancel();
                    $scope.HoldingRequest = new HoldingRequestViewModel();
                    //$scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
                    //$scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);
                    callBackAfterCreateSuccess();
                    // $scope.ShowHoldingForm(false);

                }
            });

        };

        $scope.UpdateHoldingRequest = function () {
            //console.log($scope.HoldingRequest)
            masterfileService.update('HoldingRequest').perform({ parameters: $scope.getShareViewData() }).$promise.then(function (result) {
                if (result.Error === undefined || result.Error === '') {
                    var logSuccess = getLogFn($scope.controllerId, "success");
                    logSuccess(messageLanguage.updateHoldingRequestSuccess);
                    $scope.cancel();
                    $scope.HoldingRequest = new HoldingRequestViewModel();
                    callBackAfterCreateSuccess();                  
                }
            });
        };

        $scope.cancel = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.close();
        }

        $scope.sendData = {};
      

        $scope.AddLookupLocationFrom = function () {
            var popup = $("#popupWindowChild").data("kendoWindow");
            popup.setOptions({
                width: 800,
                height: 500,
                title: "Create Location",
                content: { url: "/Location/Create?type=from" },
                activate: $scope.onActivate,
                close: function (e) {
                    popup.content('');
                },
            });
            popup.open();
        };

        $scope.EditLookupLocationFrom = function () {
            if ($scope.HoldingRequest.LocationFrom != undefined && $scope.HoldingRequest.LocationFrom != null && $scope.HoldingRequest.LocationFrom != 0) {
                var popup = $("#popupWindowChild").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "500px",
                    title: "Update Location",
                    content: { url: "/Location/Update/" + $scope.HoldingRequest.LocationFrom + "?type=from" },
                    activate: $scope.onActivate,
                    close: function (e) {
                        popup.content('');
                    },
                });
                popup.open();
            }
        };

        $scope.AddLookupLocationTo = function () {
            var popup = $("#popupWindowChild").data("kendoWindow");
            popup.setOptions({
                width: "600px",
                height: "500px",
                title: "Create Location",
                content: { url: "/Location/Create?type=to" },
                activate: $scope.onActivate,
                close: function (e) {
                    popup.content('');
            
                },
            });
            popup.open();
        };

        $scope.EditLookupLocationTo = function () {
            if ($scope.HoldingRequest.LocationTo != undefined && $scope.HoldingRequest.LocationTo != null && $scope.HoldingRequest.LocationTo != 0) {
                var popup = $("#popupWindowChild").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "500px",
                    title: "Update Location",
                    content: { url: "/Location/Update/" + $scope.HoldingRequest.LocationTo + "?type=to" },
                    activate: $scope.onActivate,
                    close: function (e) {
                        popup.content('');
                    },
                });
                popup.open();
            }
        };

        $scope.onActivate = function (e) {
            //$(".k-window-titlebar.k-header").show();
            //$(".k-overlay").attr('style', 'opacity: 0.5 !important');
            //$(".k-widget.k-window").css({'left' : ($scope.currentMousePos.x) + 'px !important','top' : $scope.currentMousePos.y + 'px !important'});
        };

        $scope.$on("ResetFormHoldingRequestParent", function (event, args) {
            $scope.Request = new RequestViewModel();
            $scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
            $scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);

            $scope.HoldingRequest.LocationFrom = 0;
            $scope.HoldingRequest.LocationTo = 0;
            $scope.HoldingRequest.StartTime = '';
            $scope.HoldingRequest.EndTime = '';
            $scope.HoldingRequest.Description = '';
            $scope.ShowUpdateHoldingRequest = false;
        });

        $scope.$on("HideUpdateHoldingRequest", function (event, args) {
            $scope.ShowHoldingForm(false);
            $scope.ShowUpdateHoldingRequest = false;
        });

        $scope.$on("EditHoldingRequest", function (event, args) {
            var startDate = new Date(args.dataItem.StartTime + 'Z');
            var endDate = new Date(args.dataItem.EndTime + 'Z');

            $scope.$broadcast("LocationFrom_ChangeDataSource", args.dataItem.LocationFromId);
            $scope.$broadcast("LocationTo_ChangeDataSource", args.dataItem.LocationToId);

            $scope.HoldingRequest.Id = args.dataItem.Id;
            $scope.HoldingRequest.LocationFrom = args.dataItem.LocationFromId;
            $scope.HoldingRequest.LocationTo = args.dataItem.LocationToId;
            $scope.HoldingRequest.StartTime = kendo.toString(startDate, "MM/dd/yyyy HH:mm");
            $scope.HoldingRequest.EndTime = kendo.toString(endDate, "MM/dd/yyyy HH:mm");
            $scope.HoldingRequest.Description = args.dataItem.Description;
            $scope.ShowUpdateHoldingRequest = true;
        });

        function checkdate(input) {
            var formats = ['MM/DD/YYYY'];
            return moment(input, formats).isValid();
        }
        $scope.$watch("HoldingRequest.SendDate", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (!checkdate($scope.HoldingRequest.SendDate)) {
                    $scope.HoldingRequest.SendDate = "";
                }
            }
        });
    }]);