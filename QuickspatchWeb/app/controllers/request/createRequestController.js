'use strict';
app.controller('createRequestController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', 'masterfileService', '$http',
    function ($rootScope, $scope, common, messageLanguage, $window, masterfileService, $http) {
        $scope.controllerId = "createRequestController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        var requestStartTime = $("#RequestStartTime").data("kendoTimePicker");
        var requestEndTime = $("#RequestEndTime").data("kendoTimePicker");
        $scope.currentMousePos = { x: -1, y: -1 };
        $scope.popConfirmOpen = false;
        $scope.IsShowLoading = false;
        $scope.IsUpdate = false;

        $scope.LocationFromDefault = 0;
        $scope.LocationToDefault = 0;

        $scope.LocationFromDefaultName = "";
        $scope.LocationToDefaultName = "";

        function activate() {

            // common.activateController(null, controllerId).then(function () { log(messageLanguage.listrequest); });
            $(document).mousemove(function (event) {
                $scope.currentMousePos.x = event.pageX;
                $scope.currentMousePos.y = event.pageY;
            });

            $http.get("FranchiseeConfiguration/GetLocationDefault")
               .then(function (result) {
                   $scope.LocationFromDefault = result.data.LocationFromId;
                   $scope.LocationToDefault = result.data.LocationToId;

                   $scope.LocationFromDefaultName = result.data.LocationFromName;
                   $scope.LocationToDefaultName = result.data.LocationToName;
               });

        }

        $scope.setSpatchTimeDefault = function() {
            $http.get("Request/GetSpatchTimeDefault")
             .then(function (result) {
                 //console.log('send time', $scope.Request.SendingTime);
                 if ($scope.Request.SendingTime == '' || $scope.Request.SendingTime == undefined) {
                 $scope.Request.SendingTime = common.getValueOfTimeAMPM(result.data.SpatchTimeDefault);
                 }
               
             });
           
        };

        $scope.ShowRequestAutoAssign = true;

        $scope.Request = new RequestViewModel();

        $scope.getData = function () {
            return { SharedParameter: JSON.stringify($scope.Request) };
        };

        //$watch location from
        $scope.$watch("Request.SetDefaultFrom", function () {
            //console.log($scope.Request.SendingTime = "05:15 PM");
            //console.log(common.getValueOfTimeAMPM($scope.Request.SendingTime));
            if ($scope.Request.SetDefaultFrom == true) {
                if ($scope.LocationFromDefault > 0) {
                    $scope.$broadcast('LocationFrom_Change', [{ KeyId: $scope.LocationFromDefault, DisplayName: $scope.LocationFromDefaultName }]);
                } else {
                    $scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
                }
            }

        });

        //$watch location to
        $scope.$watch("Request.SetDefaultTo", function () {

            if ($scope.Request.SetDefaultTo == true) {
                if ($scope.LocationToDefault > 0) {
                    $scope.$broadcast('LocationTo_Change', [{ KeyId: $scope.LocationToDefault, DisplayName: $scope.LocationToDefaultName }]);
                } else {
                    $scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);
                }
            }
        });

        $scope.$watch("Request.IsStat", function () {
            var sendingTime = $("#SendingTime").data("kendoTimePicker");
            if ($scope.Request.IsStat == true) {
                
                // $scope.Request.SendingTime = kendo.toString(new Date(), "HH:mm");
                sendingTime.enable(false);
            } else {
                //$scope.Request.SendingTime = '';
                sendingTime.enable();
            }

        });

        $scope.$watch("Request.AutoAssign", function () {
            if ($scope.Request.AutoAssign == true) {
                $.ajax({
                    url: "/courier/getautoassigncourier"
                })
              .done(function (data) {
                  if (data != undefined && data.Data != undefined && data.Data.length > 0) {
                      $scope.$broadcast('Courier_ChangeDataSource', data.Data[0].Id);
                      //$scope.$broadcast('Courier_Change', [{ KeyId: data.Data[0].Id, DisplayName: data.Data[0].DisplayName }]);
                  }
              });

            }

        });
        $scope.$watch("Request.CourierId", function (newValue, oldValue) {
            if (newValue !== undefined && newValue !== null && newValue != "" && newValue != oldValue && oldValue != 0 && newValue != 0) {

                $scope.Request.AutoAssign = false;
            }


        });
        $scope.$watch("Request.StartTime", function (newValue, oldValue) {
            if (newValue !== undefined && newValue !== null && newValue != "" && newValue != "__:__ __") {
                $scope.Request.StartTime = common.getValueOfTimeAMPM(newValue);
            }
            //if ($scope.Request.EndTime!='' && $scope.Request.StartTime > $scope.Request.EndTime) {
            //    $scope.Request.StartTime = '';
            //    requestStartTime.value('');
            //}
        });
        $scope.$watch("Request.EndTime", function (newValue, oldValue) {
            if (newValue !== undefined && newValue !== null && newValue != "" && newValue != "__:__ __") {
                $scope.Request.EndTime = common.getValueOfTimeAMPM(newValue);
            }
            //if ($scope.Request.StartTime > $scope.Request.EndTime) {
            //    $scope.Request.EndTime = '';
            //    requestEndTime.value('');
            //}
        });
        $scope.$watch("Request.SendingTime", function (newValue, oldValue) {
            if (newValue !== undefined && newValue !== null && newValue != "" && newValue != "__:__ __") {
                $scope.Request.SendingTime = common.getValueOfTimeAMPM(newValue);
            }
        });

        function callBackAfterCreateSuccess() {
            $scope.$parent.$emit("ReloadRequestGrid");
        }

        $scope.sendData = {};
        $scope.setConfirm = function(val) {
            $scope.Request.Confirm = val;
        }
        $scope.timeout = null;
        $scope.SendRequest = function () {
            if ($scope.popConfirmOpen == false) {
                $scope.popConfirmOpen = true;

                angular.copy($scope.Request, $scope.sendData);
                var currentDate = new Date();
                var endOfDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59);

                if ($scope.Request.SendingTime !== undefined && $scope.Request.SendingTime !== '') {
                    var tempSending = $scope.sendData.SendingTime.split(":");
                    var hourAddSending = parseInt(tempSending[0]);
                    var secondAddSending = parseInt(tempSending[1].split(" ")[0]);
                    if (tempSending[1].split(" ")[1] == 'PM' && hourAddSending != 12) {
                        hourAddSending += 12;
                    }
                    if (tempSending[1].split(" ")[1] == 'AM' && hourAddSending == 12) {
                        hourAddSending = 0;
                    }
                    $scope.sendData.SendingTime = (new Date((new Date()).setHours(hourAddSending, secondAddSending))).toUTCString();
                }

                if ($scope.Request.StartTime !== undefined && $scope.Request.StartTime !== '') {
                    var tempStart = $scope.sendData.StartTime.split(":");
                    var hourAddStart = parseInt(tempStart[0]);
                    var secondAddStart = parseInt(tempStart[1].split(" ")[0]);

                    if (tempStart[1].split(" ")[1] == 'PM' && hourAddStart != 12) {
                        hourAddStart += 12;
                    }
                    if (tempStart[1].split(" ")[1] == 'AM' && hourAddStart == 12) {
                        hourAddStart = 0;
                    }
                    $scope.sendData.StartTime = (new Date((new Date()).setHours(hourAddStart, secondAddStart))).toUTCString();
                }

                if ($scope.Request.EndTime !== undefined && $scope.Request.EndTime !== '') {
                    var tempEnd = $scope.sendData.EndTime.split(":");
                    var hourAddEnd = parseInt(tempEnd[0]);
                    var secondAddEnd = parseInt(tempEnd[1].split(" ")[0]);

                    if (tempEnd[1].split(" ")[1] == 'PM' && hourAddEnd != 12) {
                        hourAddEnd += 12;
                    }
                    if (tempEnd[1].split(" ")[1] == 'AM' && hourAddEnd == 12) {
                        hourAddEnd = 0;
                    }
                    $scope.sendData.EndTime = (new Date((new Date()).setHours(hourAddEnd, secondAddEnd))).toUTCString();

                }
                $scope.sendData.ExpiredTime = Math.round((endOfDate - currentDate) / 1000);

                $scope.IsShowLoading = true;
                clearTimeout($scope.timeout);

                $scope.timeout = setTimeout(function () {
                    masterfileService.create('Request').perform({ parameters: { SharedParameter: JSON.stringify($scope.sendData) } }).$promise.then(function (result) {
                        $scope.IsShowLoading = false;
                        
                        if (result.WarningInfo != null) {
                            showWarning(result.WarningInfo);
                        } else {
                        if (result.Error === undefined || result.Error === '') {
                            $scope.Request = new RequestViewModel();
                            $scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
                            $scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);
                            $scope.$broadcast('Courier_Change', [{ KeyId: 0, DisplayName: '' }]);
                            $scope.$parent.$broadcast("HideFormRequest", {});
                            var logSuccess = getLogFn($scope.controllerId, "success");
                            logSuccess(messageLanguage.createRequestSuccess);
                            callBackAfterCreateSuccess();
                        }
                            $("#popupWindow").data("kendoWindow").close();
                        $scope.popConfirmOpen = false;
                        }
                    });
                }, 1000);

            }
        };

        $scope.EditRequest = function () {
            if ($scope.popConfirmOpen == false) {
                $scope.popConfirmOpen = true;

                angular.copy($scope.Request, $scope.sendData);
                var currentDate = new Date();
                var endOfDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59);


                if ($scope.Request.SendingTime !== undefined && $scope.Request.SendingTime !== '') {
                    var tempSending = $scope.sendData.SendingTime.split(":");
                    var hourAddSending = parseInt(tempSending[0]);
                    var secondAddSending = parseInt(tempSending[1].split(" ")[0]);
                    if (tempSending[1].split(" ")[1] == 'PM') {
                        hourAddSending += 12;
                    }
                    if (tempSending[1].split(" ")[1] == 'AM' && hourAddSending == 12) {
                        hourAddSending = 0;
                    }
                    $scope.sendData.SendingTime = (new Date(currentDate.setHours(hourAddSending, secondAddSending))).toUTCString();
                }

                if ($scope.Request.StartTime !== undefined && $scope.Request.StartTime !== '') {
                    var tempStart = $scope.sendData.StartTime.split(":");
                    var hourAddStart = parseInt(tempStart[0]);
                    var secondAddStart = parseInt(tempStart[1].split(" ")[0]);

                    if (tempStart[1].split(" ")[1] == 'PM' && hourAddStart != 12) {
                        hourAddStart += 12;
                    }
                    if (tempStart[1].split(" ")[1] == 'AM' && hourAddStart == 12) {
                        hourAddStart = 0;
                    }
                    $scope.sendData.StartTime = (new Date(currentDate.setHours(hourAddStart, secondAddStart))).toUTCString();
                }
                if ($scope.Request.EndTime !== undefined && $scope.Request.EndTime !== '') {
                    var tempEnd = $scope.sendData.EndTime.split(":");
                    var hourAddEnd = parseInt(tempEnd[0]);
                    var secondAddEnd = parseInt(tempEnd[1].split(" ")[0]);

                    if (tempEnd[1].split(" ")[1] == 'PM' && hourAddEnd != 12) {
                        hourAddEnd += 12;
                    }
                    if (tempEnd[1].split(" ")[1] == 'AM' && hourAddEnd == 12) {
                        hourAddEnd = 0;
                    }
                    $scope.sendData.EndTime = (new Date(currentDate.setHours(hourAddEnd, secondAddEnd))).toUTCString();
                }

                $scope.sendData.ExpiredTime = Math.round((endOfDate - currentDate) / 1000);

                $scope.IsShowLoading = true;
                clearTimeout($scope.timeout);
                $scope.timeout = setTimeout(function () {
                    masterfileService.update('Request').perform({ parameters: { SharedParameter: JSON.stringify($scope.sendData) } }).$promise.then(function (result) {

                        $scope.IsShowLoading = false;
                        
                        if (result.WarningInfo != null) {
                            showWarning(result.WarningInfo);
                        } else {
                        if (result.Error === undefined || result.Error === '') {
                            $scope.Request = new RequestViewModel();
                            $scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
                            $scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);
                            $scope.$broadcast('Courier_Change', [{ KeyId: 0, DisplayName: '' }]);
                            $scope.$parent.$broadcast("HideFormRequest", {});
                            var logSuccess = getLogFn($scope.controllerId, "success");
                            logSuccess(messageLanguage.updateRequestSuccess);
                            callBackAfterCreateSuccess();
                        }
                            $("#popupWindow").data("kendoWindow").close();
                        $scope.popConfirmOpen = false;
                        }
                        
                    });
                }, 1000);

            }
        };

        function showWarning(obj) {
            var title = '<span class="fa fa-exclamation-triangle"></span> Request conflict' ;
            var data = Base64.encode(JSON.stringify(obj));
            //console.log(data);
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: "600px",
                height: "250px",
                title: title,
                content: {
                    url: "/Request/Warning?data=" + data
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
        $scope.cancelRequest = function () {
            $scope.IsUpdate = false;
            $scope.Request = new RequestViewModel();
            $scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
            $scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);
            $scope.$broadcast('Courier_Change', [{ KeyId: 0, DisplayName: '' }]);
            //$scope.ShowFormRequest = false;
            $scope.$parent.$broadcast("HideFormRequest", {});
        };
        $scope.AddLookupLocationFrom = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: "600px",
                height: "500px",
                title: "Create Location",
                content: { url: "/Location/Create?type=from" },
                activate: $scope.onActivate

            });
            popup.open();
        }
        $scope.EditLookupLocationFrom = function () {
            if ($scope.Request.LocationFrom != undefined && $scope.Request.LocationFrom != null && $scope.Request.LocationFrom != 0) {
                var popup = $("#popupWindow").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "500px",
                    title: "Update Location",
                    content: { url: "/Location/Update/" + $scope.Request.LocationFrom + "?type=from" },
                    activate: $scope.onActivate
                });
                popup.open();
            }
        }
        $scope.AddLookupLocationTo = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: "600px",
                height: "500px",
                title: "Create Location",
                content: { url: "/Location/Create?type=to" },
                activate: $scope.onActivate

            });
            popup.open();
        }
        $scope.EditLookupLocationTo = function () {
            if ($scope.Request.LocationTo != undefined && $scope.Request.LocationTo != null && $scope.Request.LocationTo != 0) {
                var popup = $("#popupWindow").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "500px",
                    title: "Update Location",
                    content: { url: "/Location/Update/" + $scope.Request.LocationTo + "?type=to" },
                    activate: $scope.onActivate
                });
                popup.open();
            }
        }
        $scope.AddLookupCourier = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: "500px",
                height: "400px",
                title: "Create Courier",
                content: { url: "/Courier/Create" },
                activate: $scope.onActivate

            });
            popup.open();
        }
        $scope.EditLookupCourier = function () {
            if ($scope.Request.Courier != undefined && $scope.Request.Courier != null && $scope.Request.Courier != 0) {
                var popup = $("#popupWindow").data("kendoWindow");
                popup.setOptions({
                    width: "500px",
                    height: "400px",
                    title: "Update Courier",
                    content: { url: "/Courier/Update/" + $scope.Request.Courier },
                    activate: $scope.onActivate
                });
                popup.open();
            }
        }
        $scope.onActivate = function (e) {
            //$(".k-window-titlebar.k-header").show();
            //$(".k-overlay").attr('style', 'opacity: 0.5 !important');
            //$(".k-widget.k-window").css({'left' : ($scope.currentMousePos.x) + 'px !important','top' : $scope.currentMousePos.y + 'px !important'});
        }

        $scope.$on("CourierSelectedId", function (event, val) {
            //$("#RequestAutoAssign").attr('disabled', 'disabled');
            $scope.ShowRequestAutoAssign = false;
            $scope.$broadcast('Courier_ChangeDataSource', val);
        });

        $scope.$on("ResetFormRequestParent", function (event, args) {
            $scope.Request = new RequestViewModel();
            $scope.$broadcast('LocationFrom_Change', [{ KeyId: 0, DisplayName: '' }]);
            $scope.$broadcast('LocationTo_Change', [{ KeyId: 0, DisplayName: '' }]);
            $scope.$broadcast('Courier_Change', [{ KeyId: 0, DisplayName: '' }]);
        });

        $scope.$on("CopyRequest", function (event, args) {
            //var startDate = new Date(args.dataItem.StartTime + 'Z');
            //var endDate = new Date(args.dataItem.EndTime + 'Z');

            $scope.$broadcast("Courier_ChangeDataSource", args.dataItem.CourierId);
            $scope.$broadcast("LocationFrom_ChangeDataSource", args.dataItem.LocationFromId);
            $scope.$broadcast("LocationTo_ChangeDataSource", args.dataItem.LocationToId);

            $scope.Request.CourierId = args.dataItem.CourierId;
            $scope.Request.RequestLocationFrom = args.dataItem.LocationFromId;
            $scope.Request.RequestLocationFrom = args.dataItem.LocationToId;
            $scope.Request.IsStat = args.dataItem.Type == "STAT" ? true : false;
            //$scope.Request.StartTime = kendo.toString(startDate, "MM/dd/yyyy HH:mm");
            //$scope.Request.EndTime = kendo.toString(endDate, "MM/dd/yyyy HH:mm");
            $scope.Request.Description = args.dataItem.Note;
            if (args.dataItem.DisableCourier) {
                $scope.ShowRequestAutoAssign = false;
                var requestCourierId = $("#RequestCourierId").data("kendoComboBox");
                requestCourierId.enable(false);
            }
        });
        $scope.$on("EditRequest", function (event, args) {
            $scope.IsUpdate = true;
            var sendingTime = new Date(args.dataItem.SendingTime + 'Z');
            var startDate = new Date(args.dataItem.StartTime + 'Z');
            var endDate = new Date(args.dataItem.EndTime + 'Z');

            $scope.$broadcast("Courier_ChangeDataSource", args.dataItem.CourierId);
            $scope.$broadcast("LocationFrom_ChangeDataSource", args.dataItem.LocationFromId);
            $scope.$broadcast("LocationTo_ChangeDataSource", args.dataItem.LocationToId);

            $scope.Request.Id = args.dataItem.Id;
            $scope.Request.CourierId = args.dataItem.CourierId;
            $scope.Request.RequestLocationFrom = args.dataItem.LocationFromId;
            $scope.Request.RequestLocationFrom = args.dataItem.LocationToId;
            $scope.Request.IsStat = args.dataItem.Type == "STAT" ? true : false;

            $scope.Request.SendingTime = kendo.toString(sendingTime, "hh:mm tt");
            $scope.Request.StartTime = kendo.toString(startDate, "hh:mm tt");
            $scope.Request.EndTime = kendo.toString(endDate, "hh:mm tt");

            $scope.Request.Description = args.dataItem.Note;
            if (args.dataItem.DisableCourier) {
                $scope.ShowRequestAutoAssign = false;
                var requestCourierId = $("#RequestCourierId").data("kendoComboBox");
                requestCourierId.enable(false);
            }
        });
        $scope.$on("DisableCourier", function (event, val) {
            var requestCourierId = $("#RequestCourierId").data("kendoComboBox");
            requestCourierId.enable(false);
        });
    }]);