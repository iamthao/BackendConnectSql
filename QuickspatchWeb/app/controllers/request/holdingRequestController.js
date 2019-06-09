'use strict';
app.controller('holdingRequestController', ['$rootScope', '$scope', 'common', 'masterfileService', function ($rootScope, $scope, common, masterfileService) {
    var controllerId = "holdingRequestController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    var logError = getLogFn(controllerId, "Error");
    activate();

    $scope.currentMousePos = { x: -1, y: -1 };
    function activate() {
        //common.activateController(null, controllerId).then(function () { log( messageLanguage.listrequest); });
        $(document).mousemove(function (event) {
            $scope.currentMousePos.x = event.pageX;
            $scope.currentMousePos.y = event.pageY;
        });
        $(document).on('click', function (e) {

            if ($(e.target).closest("#dlink").length === 0 && $(e.target).closest("#btn-export").length === 0) {
                if ($(e.target).closest(".btn-holding-request").length === 0 && $(e.target).closest("#send-holding-request").length === 0 && $(e.target).closest(".k-list-container").length === 0) {

                    $scope.popCofirmOpen = false;
                    $scope.IsShowHoldingToRequestForm = false;
                    $scope.$apply();
                }
            }

        });
    }

    $scope.IsShowDeleteAll = true;
    $scope.IsShowHoldingForm = false;
    $scope.IsShowHoldingToRequestForm = false;
    $scope.SearchTextEncoded = "";
    $scope.modelName = "HoldingRequest";
    $scope.HoldingRequestSelectedId = 0;
    $scope.IsClickEdit = false;
    $scope.IsShowLoading = false;

    //generate Calendar Filter
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.createdDate = function () {
        $('#reportrangeholding').daterangepicker({
            startDate: moment(),//.subtract('days', 29),
            endDate: moment(),
            minDate: '01/01/2012',
            maxDate: '12/31/5000',
            dateLimit: { days: 60 },
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                'Last 7 Days': [moment().subtract('days', 6), moment()],
                'Last 30 Days': [moment().subtract('days', 29), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
            },
            opens: 'left',
            buttonClasses: ['btn btn-default'],
            applyClass: 'btn-small btn-primary',
            cancelClass: 'btn-small',
            format: 'MM/DD/YYYY',
            separator: ' to ',
            locale: {
                applyLabel: 'Submit',
                fromLabel: 'From',
                toLabel: 'To',
                customRangeLabel: 'Custom Range',
                daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                firstDay: 1
            }
        },
            function (start, end) {
                //console.log("Callback has been called!");
                $('#reportrangeholding span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                $scope.StartDate = start.format('MM/DD/YYYY 00:00:00');
                $scope.EndDate = end.format('MM/DD/YYYY 23:59:59');
                // $scope.search();
            }
        );

        //set khoang thoi gian mac dinh 
        //$('#reportrangeholding span').html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        $('#reportrangeholding span').html(moment().format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));

        $("#reportrangeholding").on("click", function () {
            $('body').click();
        });
        $scope.StartDate = moment().format('MM/DD/YYYY 00:00:00'); //moment().subtract('days', 29).format('MM/DD/YYYY 00:00:00');
        $scope.EndDate = moment().format('MM/DD/YYYY 23:59:59');
    };
    $scope.createdDate();

    function compareTime(h1, m1, h2, m2) {
        if (h1 < h2) {
            return false;
        }
        if (h1 == h2) {
            if (m1 < m2)
                return false;
        }
        return true;
    };

    function saveRequest(id, dataItem, key) {
        var title = "Send Holding Request";
        $("#popupWindow").data(key, dataItem);
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
            },
            animation: false
        });
        popup.open();
    }

    function updateHodingRequest(id, dataItem, key) {
        var title = "Update Holding Request";
        $("#popupWindow").data(key,dataItem);
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: 650,
            height: 365,
            title: "Update Holding Request",
            content: {
                url: "/Request/PartialCreateHoldingRequest/" + dataItem.Id
            },
            close: function (e) {
                popup.content('');
            },
            animation: false
        });
        popup.open();
    }

    $scope.callSendHoldingRequestCard = function (dataItem) {
        saveRequest(null, dataItem, 'sendholding');
        //var today = new Date();
        //var startTime = dataItem.StartTimeNoFormat;
        //if (compareTime(startTime.getHours(), startTime.getMinutes(), today.getHours(), today.getMinutes()) == false) {
        //    logError('The holding request is out of time. Please update Arrival From.');
        //    return;
        //}

        //var popup = $("#popupWindow").data("kendoWindow");
        //popup.setOptions({
        //    width: 400,
        //    height: 200,
        //    title: "Send Holding Request",
        //    content: {
        //        url: "/Request/PartialSendHoldingRequest/" + dataItem.Id
        //    }
        //});
        //popup.open();


        //$scope.IsShowHoldingToRequestForm = type;
        //$scope.HoldingRequestSelectedId = id;
        //var top = $scope.currentMousePos.y - 40;
        //if (top > $(window).height() - 250) {
        //    top = top - 180;
        //}
        //$("#send-holding-request").css({ "left": $scope.currentMousePos.x -300, "top": top });
        //if (type) {
        //    $scope.HoldingToRequest.SendingTime = '';
        //    $scope.HoldingToRequest.CourierId = 0;
        //    $scope.HoldingToRequest.IsStat = false;
        //    $scope.$broadcast('CourierHoldingToRequest_Change', [{ KeyId: 0, DisplayName: '' }]);
        //    $scope.HoldingToRequest.AutoAssign = false;
        //    $scope.$root.$broadcast("HideFormRequest", {});
        //}
    };
    $scope.callSendHoldingRequestCardMultiSelected = function () {

        var arr = [];
        var grid = $('#holdingGrid').data("kendoGrid");
        var listDataItems = grid.dataItems();
        var isError = false;
        _.each(listDataItems, function (obj) {

            if (obj.CheckedBox == true) {
                if (obj.OutOfDate) {
                    isError = true;
                }
                arr.push(obj.id);
            }
        });

        if (isError) {
            logError('One of selected holding request is out of date');
            return;
        }

        if (arr.length > 0) {

            $scope.HoldingRequestSelectedId = -1;
            $scope.IsShowHoldingToRequestForm = true;
            var top = $scope.currentMousePos.y - 40;
            if (top > $(window).height() - 250) {
                top = top - 180;
            }
            $("#send-holding-request").css({ "left": $scope.currentMousePos.x, "top": top });
            $scope.HoldingToRequest.SendingTime = '';
            $scope.HoldingToRequest.CourierId = 0;
            $scope.HoldingToRequest.IsStat = false;
            $scope.$broadcast('CourierHoldingToRequest_Change', [{ KeyId: 0, DisplayName: '' }]);
            $scope.HoldingToRequest.AutoAssign = false;
            $scope.$root.$broadcast("HideFormRequest", {});
        } else {
            logError('Please select the holding request');
        }
    };

    // create Holding Request
    $scope.ShowHoldingForm = function (type) {
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: 650,
            height: 365,
            title: "Create Holding Request",
            content: {
                url: "/Request/PartialCreateHoldingRequest/" + 0
            },

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
    $scope.popCofirmOpen = false;
    $scope.sendHoldingRequest = function () {

        var sendData;
        var url;

        var arr = [];
        var grid = $('#holdingGrid').data("kendoGrid");
        var listDataItems = grid.dataItems();
        _.each(listDataItems, function (obj) {
            if (obj.CheckedBox == true) {
                arr.push(obj.id);
            }
        });

        var currentDate = new Date();
        var endOfDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59);

        var sendingTime = '';
        //if ($scope.HoldingToRequest.SendingTime !== undefined && $scope.HoldingToRequest.SendingTime!="") {
        //    sendingTime = (new Date(currentDate.setHours($scope.HoldingToRequest.SendingTime.split(":")[0], $scope.HoldingToRequest.SendingTime.split(":")[1]))).toUTCString();
        //}
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
        if (($scope.HoldingRequestSelectedId == undefined || $scope.HoldingRequestSelectedId <= 0) && arr.length > 0) {
            url = '/HoldingRequest/SendListHoldingRequest';
            sendData = {
                HoldingRequestSelectedIds: arr,
                CourierId: $scope.HoldingToRequest.CourierId,
                SendingTime: sendingTime,
                IsStat: $scope.HoldingToRequest.IsStat,
                ExpiredTime: Math.round((endOfDate - currentDate) / 1000)
            };
        } else if ($scope.HoldingRequestSelectedId !== 0) {
            url = '/HoldingRequest/SendHoldingRequest';
            sendData = {
                HoldingRequestSelectedId: $scope.HoldingRequestSelectedId,
                CourierId: $scope.HoldingToRequest.CourierId,
                SendingTime: sendingTime,
                IsStat: $scope.HoldingToRequest.IsStat,
                ExpiredTime: Math.round((endOfDate - currentDate) / 1000)
            };
        }


        if ($scope.HoldingRequestSelectedId !== 0 || arr.length > 0) {
            if ($scope.popCofirmOpen == false) {
                $scope.popCofirmOpen = true;
                common.bootboxConfirm("Are you sure that you want to send this holding request?", function () {
                    $scope.IsShowLoading = true;
                    clearTimeout($scope.timeout);
                    $scope.timeout = setTimeout(function () {

                        masterfileService.callWithUrl(url).perform({ data: JSON.stringify(sendData) }).$promise.then(function (data) {
                            $scope.IsShowLoading = false;
                            if (data.Error === undefined || data.Error === '') {
                                var logSuccess = getLogFn(controllerId, "success");
                                if ($scope.$parent.deleteMessage != undefined) {
                                    $scope.$root.$broadcast("ReloadRequestGrid");
                                    logSuccess($scope.$parent.deleteMessage);
                                } else {
                                    logSuccess('Send holding request successfully');
                                }
                                $scope.IsShowHoldingToRequestForm = false;
                                $("#holdingGrid").data("kendoGrid").dataSource.read();
                                $("#request-grid").data("kendoGrid").dataSource.read();
                            } else {
                                $scope.HoldingRequestSelectedId = sendData.HoldingRequestSelectedId;
                            }
                            $scope.popCofirmOpen = false;
                        });
                    }, 1000);

                }, function () {
                    $scope.popCofirmOpen = false;
                }).modal('show');

            }
        }

        $scope.$root.$broadcast("HideFormRequest", {});
        $scope.$root.$broadcast("HideUpdateHoldingRequest");
        $scope.$emit("ResetFormHoldingRequest");
    };

    $scope.ShowHideDeleteAll = function () {
        //show/hide button delete all      
        var arr = [];
        var grid = $('#holdingGrid').data("kendoGrid");
        var listDataItems = grid.dataItems();
        _.each(listDataItems, function (obj) {
            if (obj.CheckedBox == true) {
                arr.push(obj.id);
            }
        });

       // console.log(arr.length);
        if (arr.length > 0) {
            $scope.IsShowDeleteAll = false;
        }
        else{
            $scope.IsShowDeleteAll = true;           
        }
    }

    $scope.choose = function (dataItem) {
        if (dataItem.CheckedBox == true) {
            //console.log('vo');
            $('#holding-all').prop('checked', false);
        }

        var grid = $('#holdingGrid').data('kendoGrid');
        var listDataItems = grid.dataItems();
        _.each(listDataItems, function (obj) {
            if (obj.Id == dataItem.Id) {
                obj.CheckedBox = !obj.CheckedBox;
                $scope.ShowHideDeleteAll();
            }
        });


    }
    $scope.checkAllRows = function () {
        var grid = $('#holdingGrid').data('kendoGrid');
        grid.table.find("tr")
            .find("td:first-child input")
            .prop("checked", $("#holding-all").is(':checked'))
            .trigger("change");
        var listDataItems = grid.dataItems();
        _.each(listDataItems, function (obj) {
            obj.CheckedBox = $("#holding-all").is(':checked');
        });

        $scope.ShowHideDeleteAll();

    };
    var schemaFields = {
        Id: { editable: false },
        LocationFrom: { editable: false },
        LocationTo: { editable: false },
        Description: { editable: false },
        SendDate: { editable: false, type: "date" },
        StartTimeNoFormat: { editable: false, type: "date" },
        EndTimeNoFormat: { editable: false, type: "date" },
    };

    var columns = [
        {
            title: "<input type='checkbox' name='holding-request-check' ng-click='checkAllRows()' class='k-checkbox' id='holding-all' /><label for='holding-all' class='k-checkbox-label'></label>",
            template: kendo.template($("#holdingRequestcheckBox").html()),
            width: "37px"
        },
        {
            field: "LocationFrom",
            title: "From",
            width: "200px",
            template: '<div style="white-space: nowrap;word-wrap: break-word;text-overflow: ellipsis;width:100%;overflow: hidden;" title="#= LocationFrom#" data-container="body" data-toggle="tooltip" data-placement="top">#= LocationFrom #</div>'
        },
        {
            field: "LocationTo",
            title: "To",
            width: "200px",
            template: '<div style="white-space: nowrap;word-wrap: break-word;text-overflow: ellipsis;width:100%;overflow: hidden;" title="#= LocationTo#" data-container="body" data-toggle="tooltip" data-placement="top">#= LocationTo #</div>'

        },
        //{ field: "Description", title: "Note" },
        { field: "SendDate", title: "Dispatch Date", width: "100px", format: "{0:MM/dd/yyyy}" },
        { field: "StartTimeNoFormat", title: "From Time", width: "100px", format: "{0:hh:mm tt}" },
         { field: "EndTimeNoFormat", title: "To Time", width: "100px", format: "{0:hh:mm tt}" },
        { template: kendo.template($("#holdingRequestButtons").html()), width: "120px" }
    ];

    var dataSource = [];

    $scope.shareInit = function () {
        $scope.urlToGetData = '/Request/GetDataForHoldingRequest/';
        dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: $scope.urlToGetData,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                },
                parameterMap: function (options, operation) {
                    if (operation == "read") {
                        var result = {
                            pageSize: options.pageSize,
                            skip: options.skip,
                            take: options.take,
                            StartDate: (new Date($scope.StartDate)).toUTCString(),
                            EndDate: (new Date($scope.EndDate)).toUTCString(),
                            SearchString: $scope.SearchTextEncoded
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
            serverSorting: true,
            batch: true,
            emptyMsg: 'No Record',
            table: "#holdingGrid",

            //change: changeValueInGrid,
            schema: {
                model: {
                    id: "Id",
                    fields: schemaFields
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
        //console.log(dataSource)
        $scope.mainGridOptions = {
            dataSource: dataSource,
            columns: columns,
            scrollable: { virtual: true },
            editable: false,
            sortable: true,
            resizable: true,
            dataBound: function () {
                var data = this.dataSource.data();
                $.each(data, function (i, row) {
                    if (row.OutOfDate == true)
                        $('tr[data-uid="' + row.uid + '"] td:nth-child(2)').addClass("row-out-of-date");
                });
            },
            height: $(window).height() - 180//$scope.IsShowHoldingForm ? $(window).height() - 455 : $(window).height() - 217,

        };
    };

    $scope.Search = function ($event) {
        if ($event != null) {
            var currentKey = $event.which || $event.charCode;
            if (currentKey === 13) {
                $scope.$broadcast("ReloadGridWhenSearch", { SearchText: $scope.SearchText });
            }
        } else {
            $scope.$broadcast("ReloadGridWhenSearch", { SearchText: $scope.SearchText });
        }
    };

    $scope.Delete = function (id) {
        if ($scope.IsClickEdit) {
            $scope.$root.$broadcast("HideUpdateHoldingRequest");
        }

        common.bootboxConfirm("Are you sure that you want to delete this record?", function () {
            masterfileService.deleteById($scope.modelName).perform({ id: id }).$promise.then(function (data) {
                if (data.Error === undefined || data.Error === '') {
                    var logSuccess = getLogFn(controllerId, "success");
                    if ($scope.$parent.deleteMessage != undefined) {
                        logSuccess($scope.$parent.deleteMessage);
                    } else {
                        logSuccess('Delete holding request successfully');
                    }
                    $("#holdingGrid").data("kendoGrid").dataSource.read();
                }
            });
        }, function () { }).modal('show');


    };

    $scope.DeleteSelected = function () {
        var arr = [];
        var grid = $('#holdingGrid').data("kendoGrid");
        var listDataItems = grid.dataItems();
        _.each(listDataItems, function (obj) {
            if (obj.CheckedBox == true) {
                arr.push(obj.id);
            }
        });

        //console.log(arr.length);

        if (arr.length > 0) {
            common.bootboxConfirm("Are you sure that you want to delete these records?", function () {
                masterfileService.deleteMultiByIds($scope.modelName).perform({ selectedRowIdArray: JSON.stringify(arr) }).$promise.then(function (data) {
                    if (data.Error === undefined || data.Error === '') {
                        var logSuccess = getLogFn(controllerId, "success");
                        if ($scope.$parent.deleteMessage != undefined) {
                            logSuccess($scope.$parent.deleteMessage);
                        } else {
                            logSuccess('Delete holding request(s) successfully');
                        }
                        $("#holdingGrid").data("kendoGrid").dataSource.read();
                    }
                });
            }, function () { }).modal('show');

        } else {
            logError('Please select the holding request');
        }
    };
    $scope.Edit = function (dataItem) {
        //console.log(dataItem);
        updateHodingRequest(dataItem.Id, dataItem, 'updateholdingrequest');
        //var popup = $("#popupWindow").data("kendoWindow");
        //popup.setOptions({
        //    width: 650,
        //    height: 365,
        //    title: "Update Holding Request",
        //    content: {
        //        url: "/Request/PartialCreateHoldingRequest/" + dataItem.Id
        //    }
        //});
        //popup.open();

        //$scope.$broadcast("EditHoldingRequest", { dataItem: dataItem });
        //$scope.IsClickEdit = true;
        //$scope.IsShowHoldingForm = true;
        ////$("#btn-delete-holding-request-" + $scope.HoldingRequestSelectedId).attr("disabled", null);
        //// $scope.HoldingRequestSelectedId = dataItem.Id;
        ////$("#btn-delete-holding-request-" + $scope.HoldingRequestSelectedId).attr("disabled", "disabled");
        //$("#holdingGrid").css({ height: $scope.IsShowHoldingForm ? $(window).height() - 443 : $(window).height() - 217 });
        //$("#holdingGrid").children(".k-grid-content").height($scope.IsShowHoldingForm ? $(window).height() - 482 : $(window).height() - 255);
    };
    $scope.$on("ReloadGridWhenSearch", function (event, args) {
        var searchText = args.SearchText;
        searchText = "<SearchTerms>" + Encoder.htmlEncode(searchText) + "</SearchTerms>";
        searchText = Base64.encode('<AdvancedQueryParameters>' + searchText + '</AdvancedQueryParameters>');
        $scope.SearchTextEncoded = searchText;
        dataSource.read();
    });

    $scope.$on("ReloadGrid", function () {
        dataSource.read();
    });
    $scope.$on("HideFormHoldingRequest", function (event, val) {

        $scope.ShowHoldingForm(false);
    });
    $scope.$on("HideFormHoldingToRequestForm", function (event, val) {

        $scope.popCofirmOpen = false;
        $scope.IsShowHoldingToRequestForm = false;
    });

    $scope.$watch("HoldingToRequest.SendingTime", function (newValue, oldValue) {
        if (newValue !== undefined && newValue !== null && newValue != "" && newValue != "__:__") {
            $scope.HoldingToRequest.SendingTime = common.getValueOfTime(newValue);
        }
    });

    //$scope.$watch("HoldingToRequest.IsStat", function (newValue, oldValue) {
    //    var timePicker = $('#HoldingToRequestSendingTime').data("kendoTimePicker");

    //    if (newValue) {
    //        //$scope.HoldingToRequest.SendingTime = kendo.toString(new Date(), "HH:mm");
    //        timePicker.enable(false);
    //    } else {
    //        $scope.HoldingToRequest.SendingTime = '';
    //        timePicker.enable();
    //    }
    //});

    //kiem tra filter calendar change
    $scope.$watch("StartDate", function (newValue, oldValue) {
        if (newValue != oldValue) {
            $scope.Search();
        }
    });

    $scope.$watch("EndDate", function (newValue, oldValue) {
        if (newValue != oldValue) {
            $scope.Search();
        }
    });
}]);