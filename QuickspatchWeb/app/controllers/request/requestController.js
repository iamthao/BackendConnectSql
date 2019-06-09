'use strict';

app.controller('requestController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$http', 'masterfileService', '$timeout', '$templateCache', '$interval',
    function ($rootScope, $scope, common, messageLanguage, $http, masterfileService, $timeout, $templateCache, $interval) {
        var controllerId = "requestController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSuccess = getLogFn(controllerId, "success");
        var logError = getLogFn(controllerId, "error");
        
        activate();
        
        $scope.currentMousePos = { x: -1, y: -1 };

        $scope.ShowFormRequest = false;
        $scope.CourierTypeName = "Courier";
        $scope.callSendRequestCard = function (type, dataItem, key) {
            saveRequest(null, dataItem, key);

        //toggleDropMenu('#new-request-card');
        //$scope.ShowFormRequest = type;
        //if (type) {
        //    var scope = angular.element("#form-request").scope();
        //    if (scope !== undefined && scope !== null) {
        //        scope.setSpatchTimeDefault();
        //    }
        //    $scope.$root.$broadcast("HideFormHoldingRequest", {});
        //    $scope.$emit("ResetFormRequest");

        //}
        };

        function saveRequest(id, dataItem, key) {
            var title = "Create Request";
            if (key === 'dataItemUpdate') {
                title = "Update Request";
            }
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
        function activate() {
            // common.activateController(null, controllerId).then(function () { log(messageLanguage.listrequest); });
            
            $(document).mousemove(function (event) {
                $scope.currentMousePos.x = event.pageX;
                $scope.currentMousePos.y = event.pageY;
            });
            $('#dropdown-export a').on('click', function (event) {
                event.stopPropagation();
            });
            $(document).on('click', function (e) {
                var kendoWindowClosest = $(e.target).closest(".k-window") == 0;
                if ($(e.target).closest("#dlink").length === 0 && $(e.target).closest("#btn-export").length === 0) {
                    if ($(e.target).closest("#btn-new-request").length === 0 && $(e.target).closest(".btn-edit-request").length === 0 && $(e.target).closest(".btn-copy-request").length === 0 && $(e.target).closest("#form-request").length === 0 && $(e.target).closest(".k-list-container").length === 0 && kendoWindowClosest && $(e.target).closest(".k-window").length === 0 && $(e.target).closest('div.k-overlay').length == 0) {
                        
                        $scope.ShowFormRequest = false;
                        for (var childScope = $scope.$$childHead; childScope; childScope = childScope.$$nextSibling) {
                            // get $$childHead first and then iterate that scope's $$nextSiblings
                            if (childScope.controllerId != undefined && childScope.controllerId == 'createRequestController') {
                                childScope.IsUpdate = false;
                                break;
                            }
                        }
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
                }

            });

        }
        $scope.init = function (name) {
            $scope.CourierTypeName = $rootScope.CourierDisplayName; //name;

        }
        var schemaFields = {
            Id: { editable: false },
            CreatedDateNoFormat: { editable: false, type: "date" },
            TimeNoFormat: { editable: false, type: "date" },
            StartTimeNoFormat: { editable: false, type: "date" },
            EndTimeNoFormat: { editable: false, type: "date" },
            Status: { editable: false }
        };

        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.PageSizeSelected = 50;
        $scope.DataSource = {};
        $scope.getDataSource = function () {
            
            $scope.DataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/Request/GetListRequest",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var searchString = $scope.SearchRequest;
                            searchString = "<SearchTerms>" + Encoder.htmlEncode(searchString) + "</SearchTerms>";
                            searchString = Base64.encode('<AdvancedQueryParameters>' + searchString + '</AdvancedQueryParameters>');
                            $scope.PageSizeSelected = options.pageSize;
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                StartDate: (new Date($scope.StartDate)).toUTCString(),
                                EndDate: (new Date($scope.EndDate)).toUTCString(),
                                SearchString: searchString
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
                pageSize: $scope.PageSizeSelected,

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
            return $scope.DataSource;
        };
        $scope.search = function () {
            $scope.getDataSource().read();
            $("#request-grid").data("kendoGrid").setDataSource($scope.DataSource);
            //console.log($scope.DataSource);
        };
        $scope.createdDate = function () {
            $('#reportrange').daterangepicker({
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
                    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                    $scope.StartDate = start.format('MM/DD/YYYY 00:00:00');
                    $scope.EndDate = end.format('MM/DD/YYYY 23:59:59');
                    $scope.search();
                }
            );

            //Set the initial state of the picker label
            $('#reportrange span').html(moment().format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));

            $("#reportrange").on("click", function () {
                $('body').click();
            });

            $scope.StartDate = moment().format('MM/DD/YYYY 00:00:00'); //moment().subtract('days', 29).format('MM/DD/YYYY 00:00:00');
            $scope.EndDate = moment().format('MM/DD/YYYY 23:59:59');
        };
        $scope.createdDate();
        $scope.IsShowCancelRequestForm = false;
        $scope.dataItemSelected = null;
        $scope.cancelRequestForm = function (dataItem, type) {
            $scope.IsShowCancelRequestForm = type;
            $scope.IsShowReAssignCourierForm = false;
            $scope.dataItemSelected = dataItem;
            var top = $scope.currentMousePos.y - 40;
            if (top > $(window).height() - 200) {
                top = top - 150;
            }
            $("#cancel-request").css({ "left": '330px', "top": top });
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
                    $("#request-grid").data('kendoGrid').dataSource.read();

                });

        };
        $scope.IsShowReAssignCourierForm = false;
        $scope.callReAssignCourier = function (dataItem, type) {
            $scope.dataItemSelected = dataItem;
            $scope.IsShowCancelRequestForm = false;
            $scope.IsShowReAssignCourierForm = type;
            //if (dataItem != null) {
            //    $scope.$broadcast('ReAssignCourier_Change', [{ KeyId: dataItem.Id, DisplayName: dataItem.Courier }]);
            //}
            var top = $scope.currentMousePos.y - 40;
            if (top > $(window).height() - 200) {
                top = top - 150;
            }
            $("#re-assign-courier").css({ "left": '1000px', "top": top });
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
                        $scope.$broadcast('ReAssignCourier_Change', [{ KeyId: 0, DisplayName: '' }]);
                        $scope.IsShowReAssignCourierForm = false;
                        logSuccess(messageLanguage.reassignCourierSuccess);
                        $scope.search();
                    } else {
                        logError(messageLanguage.reassignCourierFail);
                    }

                });
            } else {
                var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
                mess += "-The " + $scope.CourierTypeName + " field is required<br/>";
                logError(mess);
            }
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
        $scope.IsShowNoteRequestForm = false;
        $scope.showNoteRequest = function (dataItem, type) {

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
                    }
                    popup.content('');
                },
                animation: false
            });
            popup.open();
            
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
            //$("#show-note-request").css({ "right": '130px', "top": top });

        };
        $scope.export = function () {
            var gridId = "#request-grid";
            var grid = $(gridId).data("kendoGrid");
            var gridColumnsConfig = _.filter(grid.columns, function (obj) {
                return obj.field != "Command" && typeof obj.template != "function";
            });

            var gridColumns = _.map(gridColumnsConfig, function (obj) {
                return { Field: obj.field, Title: obj.title };
            });

            var sort = grid.dataSource.sort();

            var searchString = $scope.SearchRequest;
            searchString = "<SearchTerms>" + Encoder.htmlEncode(searchString) + "</SearchTerms>";
            searchString = Base64.encode('<AdvancedQueryParameters>' + searchString + '</AdvancedQueryParameters>');
            var total = grid.dataSource.total();
            var selected = $("#dropdown-export input[type='radio']:checked");
            if (selected.length > 0) {
                if (parseInt(selected.val()) > 0) {
                    total = parseInt(selected.val());
                }
            }

            var queryInfo = {
                StartDate: (new Date($scope.StartDate)).toUTCString(),
                EndDate: (new Date($scope.EndDate)).toUTCString(),
                SearchString: searchString,
                Sort: sort,
                Take: total
            };

            var exportUrl = "/request/ExportExcel";
            var downloadExcelUrl = "/request/DownloadExcelFile";

            $http.post(exportUrl, { queryInfo: queryInfo, gridColumns: gridColumns }).success(function (result) {
                $.fileDownload(downloadExcelUrl, {
                    httpMethod: "POST",
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    data: { 'fileName': result.FileNameResult }
                });
            }).error(function (result) {
            });
        };
        $scope.clickExport = function () {
            $scope.ShowFormRequest = false;
            $scope.IsShowReAssignCourierForm = false;
            $scope.IsShowCancelRequestForm = false;
            $scope.IsShowNoteRequestForm = false;
            $scope.$root.$broadcast("HideFormHoldingToRequestForm", {});
            $scope.$root.$broadcast("HideFormHoldingRequest", {});
        };
        $scope.$on("ReloadRequestGrid", function (event, val) {
            $scope.search();
        });
        $scope.$on("HideFormRequest", function (event, val) {
            $scope.callSendRequestCard(false);
        });
        $scope.$on("ReAssignCourier_DataBound", function (event, val) {
            if ($scope.dataItemSelected != undefined && $scope.dataItemSelected != null) {
                $scope.$broadcast("ReAssignCourier_RemoveItem", { KeyId: $scope.dataItemSelected.CourierId });
            }

        });
        //Copy request
        $scope.copyRequest = function (dataItem) {
            $scope.callSendRequestCard(true, dataItem, 'dataItemCopy');
            //$scope.$broadcast("CopyRequest", {dataItem:dataItem});
        };

        //Edit request
        $scope.editRequest = function (dataItem) {
            $scope.callSendRequestCard(true, dataItem, 'dataItemUpdate');
            //$scope.$broadcast("EditRequest", { dataItem: dataItem });
        };

        //view eliveryAgreement
        $scope.viewDeliveryAgreement = function (dataItem) {
            //console.log(dataItem);
            $templateCache.remove("/Report/PrintPdfFile?parameters=" + JSON.stringify({ QueryId: dataItem.Id }) + "&type=1");
            common.showPopupKendo(1000, 500, "Delivery Agreement", "/Report/PrintPdfFile?parameters=" + JSON.stringify({ QueryId: dataItem.Id }) + "&type=1");
        };
        
        $scope.noteGridOptions = {

            dataSource: $scope.getNoteDataSource(),
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: [10, 25, 50, 100],
                buttonCount: 5
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
                    format: "{0:MM/dd/yyyy hh:mm tt}"
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
            
            $http.get('/Request/WarningInfo?requestId=' + dataItem.Id + '&courierId=' + dataItem.CourierId).then(function (result) {
                var obj = result.data;
                //console.log(obj);
                var title = '<span class="fa fa-exclamation-triangle"></span>';
                var data = Base64.encode(JSON.stringify(obj));
                //console.log(JSON.stringify(obj));
                //console.log(data);
                var popup = $("#popupWindow").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "250px",
                    title: title,
                    content: {
                        url: "/Request/WarningDetail?data=" + data
                    },
                    close: function (e) {
                        popup.content('');
                        //$scope.cancelRequest();
                        $scope.popConfirmOpen = false;
                    },
                    animation: false
                });
                popup.open();
            })
            
        }
        $scope.mainGridOptions = function (displayForCourier) {
            return {
                dataSource: $scope.getDataSource(),
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: [10, 25, 50, 100],
                    buttonCount: 5
                },
                reorderable: true,
                resizable: true,
                height: $(window).height() - 177,

                columns: [
                    {
                        field: "RequestNo",
                        title: "Req #",
                        width: "140px",
                        template: "<span ng-if='dataItem.IsWarning' style='color:\\#FFDC00;cursor: pointer;' ng-click='showWarning(dataItem);' class='fa fa-exclamation-triangle'></span> ${RequestNo}",

                    },
                    {
                        field: "Courier",
                        title: $rootScope.CourierDisplayName,
                        width: "120px",

                    },
                    {
                        field: "Status", title: "Status", width: "90px", template: kendo.template($("#requestStatus").html())
                    },
                    {
                        field: "LocationFromName",
                        title: "From",
                        width: "190px",
                        template: '<div style="white-space: nowrap;word-wrap: break-word;text-overflow: ellipsis;width:100%;overflow: hidden;" title="#= LocationFromName#" data-container="body" data-toggle="tooltip" data-placement="top">#= LocationFromName #</div>'                       
                    },
                    {
                        field: "LocationToName",
                        title: "To",
                        width: "190px"  ,
                        template: '<div style="white-space: nowrap;word-wrap: break-word;text-overflow: ellipsis;width:100%;overflow: hidden;" title="#= LocationToName#" data-container="body" data-toggle="tooltip" data-placement="top">#= LocationToName #</div>'
                    },
                    //{
                    //    field: "Type",
                    //    title: "Type",
                    //    width: "100px"
                    //},
                    {
                        field: "TimeNoFormat",
                        title: "Dispatch",
                        width: "150px",
                        format: "{0:MM/dd/yyyy hh:mm tt}"
                    },
                    {
                        field: "StartTimeNoFormat",
                        title: "Arrival From",
                        width: "150px",
                        format: "{0: hh:mm tt}"
                    },
                    {
                        field: "EndTimeNoFormat",
                        title: "Arrival To",
                        width: "150px",
                        format: "{0: hh:mm tt}"
                    },
                    {
                        field: "", width: "150px", template: kendo.template($("#requestButtons").html())
                   }
                    //{
                    //    field: "Note",
                    //    title: "Note",
                    //    width: "100px"
                    //},
                    //{
                    //    field: "CreatedBy",
                    //    title: "Created By",
                    //    width: "100px"
                    //},
                    //{
                    //    field: "CreatedDateNoFormat",
                    //    title: "Created Date",
                    //    width: "160px",
                    //    format: "{0:MM/dd/yyyy hh:mm tt}"
                    //}
                ],
                dataBound: function () {
                    //$("#notifyDeclineGrid").data("kendoGrid").dataSource.read();
                    //$scope.timeout = $timeout(function () { $scope.CheckChangeRequestTable(); }, 5000);
                    var data = this.dataSource.data();
                    $.each(data, function (i, row) {
                        if (row.IsExpired == true)
                            $('.k-grid-content-locked tr[data-uid="' + row.uid + '"] td:first-child').addClass("row-expired");
                    });
                },
                dataBinding: function (e) {
                    //var tmp = e.sender.dataItems();
                    //_.each($scope.currentAnswers, function (obj) {
                    //    var udRow = _.findWhere(tmp, { Id: obj.Id });
                    //    if (udRow != undefined) {
                    //        udRow.Answer = obj.Answer;
                    //    }
                    //});

                }
            };
        };
        $scope.$on("HasChangeRequest", function () {
            $("#request-grid").data("kendoGrid").dataSource.read();
        });
        //$scope.checkSumRequest = 0;
        //$scope.CheckChangeRequestTable = function () {
        //    $timeout.cancel($scope.timeout);
        //    masterfileService.callWithUrl('/Request/CheckChangeRequestTable').perform({}).$promise.then(function (result) {
        //        if (result.Data != $scope.checkSumRequest) {
        //            $scope.checkSumRequest = result.Data;
        //            $("#request-grid").data("kendoGrid").dataSource.read();
        //        } else {
        //            $scope.timeout = $timeout(function () { $scope.CheckChangeRequestTable(); }, 5000);
        //        }
        //    });
        //};
    }]);