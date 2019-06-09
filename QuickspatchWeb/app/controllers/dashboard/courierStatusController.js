'use strict';
app.controller('courierStatusController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', 'masterfileService', '$templateCache', '$interval','$http',
    function ($rootScope, $scope, common, messageLanguage, $window, masterfileService, $templateCache, $interval, $http) {
        $scope.controllerId = "courierStatusController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        var logSuccess = getLogFn($scope.controllerId, "success");
        var logError = getLogFn($scope.controllerId, "error");
        $scope.classAll = '';
        $scope.isAllDay = true;
        activate();

        $scope.currentMousePos = { x: -1, y: -1 };
        $scope.popConfirmOpen = false;
        
        function activate() {
            $(document).mousemove(function (event) {
                $scope.currentMousePos.x = event.pageX;
                $scope.currentMousePos.y = event.pageY;
            });
            $(document).on('click', function (e) {
                
                if ($(e.target).closest(".btnCallRequestCard").length === 0 && $(e.target).closest(".btn-copy-request").length === 0 && $(e.target).closest("#form-request").length === 0 && $(e.target).closest(".k-list-container").length === 0 && $(e.target).closest(".k-window").length === 0 && $(e.target).closest(".popup-footer").length === 0) {
                    $scope.ShowFormRequest = false;
                    $scope.$apply();
                }

                if ($(e.target).closest(".btn-reassign-request").length === 0 && $(e.target).closest("#re-assign-courier").length === 0 && $(e.target).closest(".k-list-container").length === 0) {
                    $scope.IsShowReAssignCourierForm = false;
                    $scope.$apply();
                }
                if ($(e.target).closest(".btn-cancel-request").length === 0 && $(e.target).closest("#cancel-request").length === 0 && $(e.target).closest(".k-list-container").length === 0) {
                    $scope.IsShowCancelRequestForm = false;
                    $scope.$apply();
                }
                if ($(e.target).closest(".btn-note-request").length === 0 && $(e.target).closest("#show-note-request").length === 0 && $(e.target).closest(".k-list-container").length === 0) {
                    $scope.IsShowNoteRequestForm = false;
                    $scope.$apply();
                }
            });
            //$("[name='chk-courier-status']").bootstrapSwitch();
        }

        $scope.SelectedCourier = "";
        $scope.CourierList = {};
        $scope.CourierId = null;
        $scope.ShowFormRequest = false;
        $scope.IsShowNoteRequestForm = false;
        $scope.IsShowCancelRequestForm = false;
        $scope.IsShowReAssignCourierForm = false;
        $scope.dataItemSelected = null;
        $scope.LoadCourierFirst = false;
        $scope.timeout = null;
        $scope.hasDataBound = false;
        $scope.clickCourier = false;

        function saveRequest(dataItem) {
            var title = "Create Request";
            $("#popupWindow").data('dataItemCopy', dataItem);
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: $(window).width() - 100,
                height: $(window).height() - 100,
                title: title,
                content: {
                    url: "/Request/SaveRequest"
                },
                close: function (e) {
                    popup.content('');
                    $scope.refreshCourierStatus();
                },
                animation: false
            });
            popup.open();
        }

        $scope.callSendRequestCard = function () {
            saveRequest({ LocationFromId: 0, LocationToId: 0, CourierId: $scope.CourierId, StatusId: 60, });
            //var scope = angular.element("#form-request").scope();
            //if (scope !== undefined && scope !== null) {
            //    scope.setSpatchTimeDefault();
            //}

            //$scope.$broadcast("CourierSelectedId", $scope.CourierId);
            //$scope.$broadcast("DisableCourier", true);
            //$scope.ShowFormRequest = true;
            //$scope.$apply();
            //var top = $scope.currentMousePos.y - 40;
            //if (top > $(window).height() - 400) {
            //    top = top - 350;
            //    if (top < 0)
            //        top = 84;
            //}
            //$("#form-request").css({ "left": $scope.currentMousePos.x + 10, "top": top });

        };



        $scope.$on("ReloadRequestGrid", function (event, val) {

            $("#courier-route").data('kendoGrid').dataSource.read();
        });
        $scope.$on("HideFormRequest", function (event, val) {

            $scope.ShowFormRequest = false;
        });
        $scope.$on("ReAssignCourier_DataBound", function (event, val) {
            if ($scope.CourierId > 0) {
                $scope.$broadcast("ReAssignCourier_RemoveItem", { KeyId: $scope.CourierId });
            }

        });

        $scope.getCourierList = function () {
            $scope.CourierList = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/Courier/GetListCourierForDashboard",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                SearchString: $scope.SearchCourier
                            };

                            if (options.sort) {
                                for (var i = 0; i < options.sort.length; i++) {
                                    result["sort[" + i + "].field"] = options.sort[i].field;
                                    result["sort[" + i + "].dir"] = options.sort[i].dir;
                                }
                            }
                            return result;
                        }
                    }
                },
                serverPaging: true,
                serverSorting: true,
                batch: true,
                pageSize: 50,
                schema: {
                    model: {
                        fields: {
                            name: "FullName",
                            Status: "Status"
                        }
                    },
                    data: "Data",
                    total: "TotalRowCount"
                }
            });
            return $scope.CourierList;
        };

        $scope.showCourierList = function () {
            clearTimeout($scope.timeout);
            if ($("#courier-list").data("kendoListView")) {
                $("#courier-list").data("kendoListView").destroy();
            }
            
            $("#courier-list").kendoListView({
                dataSource: $scope.getCourierList(),
                sort: { field: "Status", dir: "asc" },
                template: kendo.template($("#courierListViewTemplate").html()),
                selectable: "single",
                dataBound: function (e) {
                    $scope.hasDataBound = true;
                    $scope.clickCourier = false;
                    
                    if ($scope.LoadCourierFirst == true) {
                        var dataItem = this.dataSource.view()[0];
                        if (dataItem != undefined) {
                            $scope.SelectedCourier = dataItem.name;
                            $scope.CourierId = dataItem.Id;

                            // retrieve data correspond to the new selection
                            $("#courier-route").data('kendoGrid').dataSource.read();
                            $scope.setSelectedCourierById($scope.CourierId);

                        } else {
                            $("#courier-route").data("kendoGrid").dataSource.data([]);
                            $scope.SelectedCourier = "";
                            $scope.CourierId = 0;
                        }
                    } else {
                        $scope.setSelectedCourierById($scope.CourierId);
                    }

                    $scope.$emit("LoadCourierStatusDone");
                    //$scope.$apply();
                },
                change: function (e) {
                    if ($scope.hasDataBound==false) {
                        $scope.clickCourier = true;
                    }
                    $scope.hasDataBound = false;
                    var index = this.select().index();
                    var dataItem = this.dataSource.view()[index];
                    if (dataItem != undefined) {
                        $scope.SelectedCourier = dataItem.name;
                        if (dataItem.Status == 2) {
                            $scope.setPostionCurrentOfCourier(dataItem.Id);
                        } else {
                            $scope.$root.$broadcast('Dashboard_Courier_Changed', { lat: 0, lng: 0, fullName: '', avatar: '', requestNo: '', currentVelocity: '' });
                        }

                        $scope.CourierId = dataItem.Id;
                        // retrieve data correspond to the new selection
                        $("#courier-route").data('kendoGrid').dataSource.read();
                        $scope.LoadCourierFirst = false;
                    }
                }
            }).undelegate(".btnCallRequestCard", "mousedown").delegate(".btnCallRequestCard", "mousedown", function (e) {
                //e.stopPropagation();
                $(this).parent().parent().parent().parent().find("div").removeClass("k-state-selected");
                $(this).parent().parent().parent().addClass("k-state-selected");

                $scope.SelectedCourier = $(this).parent().parent().find(".courier-name").text();
                $scope.CourierId = $(this).attr("id").replace("courier-", "");
                // retrieve data correspond to the new selection
                $("#courier-route").data('kendoGrid').dataSource.read();
                $scope.callSendRequestCard();
            }).undelegate(".btnForceSignOut", "mousedown").delegate(".btnForceSignOut", "mousedown", function (e) {
                if ($scope.popConfirmOpen == false) {
                    $scope.popConfirmOpen = true;

                    e.stopPropagation();
                    $scope.CourierId = $(this).attr("id").replace("courierSignOut-", "");


                    bootbox.hideAll();
                    common.bootboxConfirm("Are you sure that you want to Force Sign Out this courier?", function () {
                        $.ajax({
                            method: "POST",
                            url: "/Courier/CleanImei",
                            data: { id: $scope.CourierId }
                        })
                           .done(function (result) {
                               if (result == true) {
                                   logSuccess('Force Sign Out successfully');
                                   $scope.showCourierList();
                               }
                           });
                    }, function () { }).modal('show');
                    
                    $scope.popConfirmOpen = false;
                }
                
            });

            kendo.ui.progress($("#courier-list"), false);
        };
        $scope.showCourierList();
        $scope.setSelectedCourierById = function (id) {
            if (id == null || id == 0) {
                $scope.CourierId = 0;
                $scope.SelectedCourier = "All";
                if ($("#courier-route").data('kendoGrid') != undefined && $("#courier-route").data('kendoGrid') != null) {
                    $("#courier-route").data('kendoGrid').dataSource.read();
                }
               
                return;
            }
            var listView = $("#courier-list").data("kendoListView");
            var children = listView.dataSource.view();
            var index = 0;
            for (var x = 0; x < children.length; x++) {
                if (children[x].Id == id) {
                    index = x;
                    break;
                }
            }

            // selects first list view item
            listView.select(listView.element.children().eq(index));
        };
        $scope.search = function () {
            clearTimeout($scope.timeout);
            $("#courier-list").data("kendoListView").dataSource.read();

        };
        var schemaFields = {
            Id: { editable: false },
            CreatedDateNoFormat: { editable: false, type: "date" }
        };

        var schemaFieldsForRequest = {
            Id: { editable: false },
            TimeNoFormat: { editable: false, type: "date" },
            StartTime: { editable: false, type: "date" },
            EndTime: { editable: false, type: "date" },
        };
        //re-assign request
        $scope.callReAssignCourier = function (dataItem, type) {
            $scope.dataItemSelected = dataItem;
            $scope.IsShowCancelRequestForm = false;
            $scope.IsShowReAssignCourierForm = type;

            $scope.$broadcast('ReAssignCourier_Change', [{ KeyId: 0, DisplayName: '' }]);
            var top = $scope.currentMousePos.y - 40;
            if (top > $(window).height() - 200) {
                top = top - 150;
            }
            $("#re-assign-courier").css({ "right": '45px', "top": top });
        };
        $scope.callReAssignCourier = function (dataItem, type) {
            $scope.dataItemSelected = dataItem;
            $scope.IsShowCancelRequestForm = false;
            $scope.IsShowReAssignCourierForm = type;

            $scope.$broadcast('ReAssignCourier_Change', [{ KeyId: 0, DisplayName: '' }]);
            var top = $scope.currentMousePos.y - 40;
            if (top > $(window).height() - 200) {
                top = top - 150;
            }
            $("#re-assign-courier").css({ "right": '45px', "top": top });
        };
        $scope.assignCourier = function () {
            if ($scope.ReAssignCourierId != undefined && $scope.ReAssignCourierId > 0 && $scope.dataItemSelected != null) {
                $.ajax({
                    method: "POST",
                    url: "/request/reassigncourier",
                    data: { id: $scope.dataItemSelected.Id, courierId: $scope.ReAssignCourierId }
                })
                    .done(function (result) {
                        if (result == true) {
                            $scope.ReAssignCourierId = 0;
                            $scope.IsShowReAssignCourierForm = false;
                            logSuccess(messageLanguage.reassignCourierSuccess);
                            if ($("#courier-route").data('kendoGrid') != undefined) {
                                $("#courier-route").data('kendoGrid').dataSource.read();
                            }
                        } else {
                            logError(messageLanguage.reassignCourierFail);
                        }

                    });
            } else {
                var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
                mess += "-The Courier field is required<br/>";
                logError(mess);
            }
        };
        //cancel request
        $scope.cancelRequestForm = function (dataItem, type) {

            $scope.IsShowCancelRequestForm = type;
            $scope.IsShowReAssignCourierForm = false;
            $scope.dataItemSelected = dataItem;
            var top = $scope.currentMousePos.y - 40;
            if (top > $(window).height() - 200) {
                top = top - 150;
            }
            $("#cancel-request").css({ "right": '80px', "top": top });
            if (type) {
                $scope.ContentCancel = "Are you sure that you want to cancel this request #: " + $scope.dataItemSelected.RequestNo + "?";
            }

        };
        $scope.cancelRequest = function () {
           
            $.ajax({
                method: "POST",
                url: "/request/cancelrequest",
                data: { id: $scope.dataItemSelected.Id }
            })
                .done(function (result) {
                    if (result == "") {
                        logSuccess("This request is cancelled");
                    } else {
                        logError(result);
                    }
                    $scope.IsShowCancelRequestForm = false;
                    $("#courier-route").data('kendoGrid').dataSource.read();

                });
        };
        //show note request
        $scope.NoteDataSource = {};
        $scope.getNoteDataSource = function () {
            $scope.NoteDataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/NoteRequest/GetNoteRequest",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                requestId: $scope.dataItemSelected != null && $scope.dataItemSelected != undefined ? $scope.dataItemSelected.Id : 0
                            };

                            if (options.sort) {
                                for (var i = 0; i < options.sort.length; i++) {
                                    result["sort[" + i + "].field"] = options.sort[i].field;
                                    result["sort[" + i + "].dir"] = options.sort[i].dir;
                                }
                            }
                            return result;
                        }
                    }
                },
                serverPaging: true,
                serverSorting: true,
                batch: true,
                pageSize: 50,

                schema: {
                    model: {
                        fields: {
                            Id: 'Id',
                            fields: schemaFields
                        }
                    },
                    data: "Data",
                    total: "TotalRowCount",
                    parse: function (response) {
                        //get list field which have data type is datetime
                        var listFieldDateType = [];
                        $.each(schemaFields, function (itemIdx, item) {
                            if (item.type && item.type == "date") {
                                listFieldDateType.push(itemIdx);
                            }
                        });
                        
                        //process date to correct timezone
                        $.each(response, function (idx, elem) {
                            $.each(elem, function (itemIdx, item) {
                                for (var name in item) {
                                    if (_.contains(listFieldDateType, name)) {
                                        var d = new Date(item[name] + 'Z');
                                        if (d && d.getFullYear() > 1970) {
                                            response[idx][itemIdx][name] = d;
                                        }
                                    }
                                }
                            });
                        });
                        return response;
                    }
                }
            });
            return $scope.NoteDataSource;
        };
        $scope.showNoteRequest = function (dataItem, type) {
            showDetail(dataItem);
            //$scope.dataItemSelected = dataItem;
            //$scope.IsShowCancelRequestForm = false;
            //$scope.IsShowReAssignCourierForm = false;
            //$scope.IsShowNoteRequestForm = type;
            //$('#noterequest-grid').data('kendoGrid').dataSource.read();
            //$('#noterequest-grid').data('kendoGrid').refresh();


            //var top = $scope.currentMousePos.y - 40;
            //if (top > $(window).height() - 300) {
            //    top = top - 200;
            //}
            //$("#show-note-request").css({ "right": '110px', "top": top });

        };
        //Copy request
        $scope.copyRequest = function (dataItem) {
            saveRequest(dataItem);
            //$scope.ShowFormRequest = true;
            //var top = $scope.currentMousePos.y - 40;
            //if (top > $(window).height() - 400) {
            //    top = top - 350;
            //    if (top < 0)
            //        top = 84;
            //}
            //dataItem.DisableCourier = true;
            ////$("#form-request").css({ "left": $scope.currentMousePos.x + 10, "top": top });
            //if (($(window).width() - $scope.currentMousePos.x) < 1000) {
            //    $("#form-request").css({ "right": 5, "top": top + 60 });
            //    $("#form-request").css("left", '');
            //} else {
            //    $("#form-request").css({ "left": $scope.currentMousePos.x + 10, "top": top });
            //    $("#form-request").css("right", '');
            //}
            //$scope.$broadcast("CopyRequest", { dataItem: dataItem});
        };
        //Set location of courier on map
        $scope.setPostionCurrentOfCourier = function (courierId) {
            masterfileService.callWithUrl("/Courier/getPositionCurrentOfCourier").perform({ courierId: courierId }).$promise.then(function (result) {

                var lat = 0;
                var lng = 0;
                var fullName = '';
                var avatar = '';
                var requestNo = "";
                var currentVelocity = 0;
                if (result != undefined && result != '' && result.Lat != null && result.Lat != '') {
                    lat = result.Lat;
                    lng = result.Lng;
                    fullName = result.FullName;
                    avatar = result.AvatarImage;
                    requestNo = result.CurrentRequestNo;
                    currentVelocity = result.CurrentVelocity;

                } else {
                    //logError(messageLanguage.UnableDetermineCourier);
                }
                $scope.$root.$broadcast('Dashboard_Courier_Changed', { lat: lat, lng: lng, fullName: fullName, avatar: avatar, requestNo: requestNo, currentVelocity: currentVelocity, clickCourier: $scope.clickCourier });
            });
        };
        $scope.RequestDataSource = {};
        $scope.getRequestDataSource = function () {
            $scope.RequestDataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/Request/GetRequestListByCourier",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                QueryId: $scope.CourierId
                            };

                            if (options.sort) {
                                for (var i = 0; i < options.sort.length; i++) {
                                    result["sort[" + i + "].field"] = options.sort[i].field;
                                    result["sort[" + i + "].dir"] = options.sort[i].dir;
                                }
                            }
                            return result;
                        }
                    }
                },
                serverPaging: true,
                serverSorting: true,
                batch: true,
                pageSize: 50,
                schema: {
                    model: {
                        fields: {
                            Id: 'Id',
                            fields: schemaFieldsForRequest
                        }
                    },
                    data: "Data",
                    total: "TotalRowCount",
                    parse: function (response) {
                        //get list field which have data type is datetime
                        var listFieldDateType = [];
                        $.each(schemaFieldsForRequest, function (itemIdx, item) {
                            if (item.type && item.type == "date") {
                                listFieldDateType.push(itemIdx);
                            }
                        });
                        
                        //Nghiep update for all button
                        //$scope.classAll = 'inactive';
                        //$scope.isAllDay = false;
                        //CourierId
                        //var courierid = 0, request= 0, inactive = false;
                        //for (var i = 0; i < response.Data.length; i++) {
                        //    if (response.Data[i].CourierId != courierid) {
                        //        if (courierid == 0) {
                        //            courierid = response.Data[i].CourierId;
                        //            inactive = true;
                        //        } else {
                        //            inactive = false;
                        //            break;
                        //        }
                        //    }
                        //    if (response.Data[i].Id != request) {
                        //        if (request == 0) {
                        //            request = response.Data[i].Id;
                        //            inactive = true;
                        //        } else {
                        //            inactive = false;
                        //            break;
                        //        }
                        //    }
                        //}
                        $scope.isAllDay = $scope.CourierId == null || $scope.CourierId == 0;
                        $scope.classAll = $scope.CourierId  > 0 ? 'inactive' : '';

                        //process date to correct timezone
                        $.each(response, function (idx, elem) {
                            $.each(elem, function (itemIdx, item) {
                                for (var name in item) {
                                    if (_.contains(listFieldDateType, name)) {
                                        var d = new Date(item[name] + 'Z');
                                        if (d && d.getFullYear() > 1970) {
                                            response[idx][itemIdx][name] = d;
                                        }
                                    }
                                }
                            });
                        });
                        return response;
                    }
                }
            });
            return $scope.RequestDataSource;
        };


        $scope.noteGridOptions = {
            dataSource: $scope.getNoteDataSource(),
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: [10, 25, 50, 100, 1000],
                buttonCount: 3
            },
            reorderable: true,
            resizable: true,
            height: "200px",

            columns: [
                {
                    field: "Description",
                    title: "Description",
                    width: "200px"
                },
                {
                    field: "CreatedBy",
                    title: "Created By",
                    width: "100px"
                },
                {
                    field: "CreatedDateNoFormat",
                    title: "Created Date",
                    width: "160px",
                    format: "{0:hh:mm tt}"
                }
            ],
            dataBinding: function (e) {
                var tmp = e.sender.dataItems();
                _.each($scope.currentAnswers, function (obj) {
                    var udRow = _.findWhere(tmp, { Id: obj.Id });
                    if (udRow != undefined) {
                        udRow.Answer = obj.Answer;
                    }
                });

            }
        };
        $scope.showWarning = function (dataItem) {
            $http.get('/Request/WarningInfo?requestId=' + dataItem.Id + '&courierId=' + dataItem.CourierId).then(function(result) {
                var obj = result.data;
                var title = '<span class="fa fa-exclamation-triangle"></span>';
                var data = Base64.encode(JSON.stringify(obj));
                var popup = $("#popupWindow").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "250px",
                    title: title,
                    content: {
                        url: "/Request/WarningDetail?data=" + data
                    },
                    close: function(e) {
                        popup.content('');
                        //$scope.cancelRequest();
                        $scope.popConfirmOpen = false;
                    },
                    animation: false
                });
                popup.open();
            });
        }
        // get a list of Request based on the Courier
        $scope.mainGridOptions = {
            // define the grid 
            dataSource: $scope.getRequestDataSource(),
            height: "100%",
            sortable: true,
            resizable: true,
            columns: [
                {
                    field: "RequestNo",
                    title: "Req #",
                    width: "140px",
                    template: kendo.template($("#requestNo").html())
                },
                {
                    field: "LocationFromName",
                    title: "From",
                    width: "80px"
                },
                {
                    field: "LocationToName",
                    title: "To",
                    width: "80px"
                },
                {
                    field: "Status",
                    width: "80px",
                    template: kendo.template($("#requestStatus").html())
                },
                {
                    field: "StartTime",
                    title: "Arr-From",
                    format: "{0:hh:mm tt}"
                },
                {
                    field: "EndTime",
                    title: "Arr-To",
                    format: "{0:hh:mm tt}"
                },
                {
                    field: "",
                    width: "150px",
                    template: kendo.template($("#requestButtons").html())
                }
            ],
            dataBound: function (e) {
                var data = this.dataSource.data();
                $.each(data, function (i, row) {
                    if (row.IsExpired == true)
                        $('.k-grid-content tr[data-uid="' + row.uid + '"] td:first-child').addClass("row-expired");
                });
                //$scope.$root.$broadcast("RefreshRequestStatus", { courierId: $scope.CourierId, courierName: $scope.SelectedCourier, clickCourier: $scope.clickCourier });
                $scope.$root.$broadcast("RefreshRequestStatus", { courierId: $scope.CourierId, courierName: $scope.SelectedCourier, clickCourier: $scope.CourierId != null && $scope.CourierId  > 0});
                $('.k-grid-content tr td').bind('click', function (ec) {
                    if (!$(this).children('div').hasClass('btn-group') && !$(ec.target).hasClass('fa-exclamation-triangle')) {
                        var uid = $(this).parent('tr').attr('data-uid');
                        var dataItem = e.sender.dataSource.getByUid(uid);
                        showDetail(dataItem);
                    }
                });
            },
        };



        function showDetail(dataItem) {
            $templateCache.remove("/Request/Detail");
            var popup = $("#popupWindow").data("kendoWindow");
            $('#popupWindow').data('VisibleTracking', false);
            //set RequestItem;
            $("#popupWindow").data("RequestItem", dataItem);
            var title = '<span>' + dataItem.RequestNo + '</span> ';
            title += '<span class="account-section" id="loading-request-detail" style="padding-left: 30%;">' +
                            '<img src="/Content/quickspatch/img/loading-blue.gif" />' +
                        '</span>';
            popup.setOptions({
                width: ($(document).width() - 200) + "px",
                height: ($(document).height() - 200) + "px",
                title: title,
                content: {
                    url: "/Request/Detail"
                },
                close: function (e) {
                    if (intervalObject != null) {
                        $interval.cancel(intervalObject);
                        //if ($scope.requestGrid != undefined) {
                        //    $scope.requestGrid.clearSelection();
                        //}
                        
                    }
                    popup.content('');
                },
                animation: false
            });
            popup.open();
        }

        $scope.refreshCourierStatus = function () {
            clearTimeout($scope.timeout);
            $scope.showCourierList();
        };
        $scope.$on("RefreshCourierStatus", function () {
            $scope.refreshCourierStatus();
        });
        $scope.$on("Chart_Click", function () {
            $scope.IsShowNoteRequestForm = false;
        });

        //Implement all
        //$scope.classAll = 'inactive';
        //$scope.isAllDay = true;
        $scope.showAllRequest = function() {
            if (!$scope.isAllDay) {
                $scope.CourierId = 0;
                $scope.SelectedCourier = "All";
                $("#courier-route").data('kendoGrid').dataSource.read();
                $("#courier-list").data("kendoListView").clearSelection();
            }
        }
    }]);