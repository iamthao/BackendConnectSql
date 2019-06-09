'use strict';

app.controller('reportController', ['$rootScope', '$scope', 'common', 'masterfileService', 'messageLanguage', '$sce', '$http', '$templateCache', '$interval',
    function ($rootScope, $scope, common, masterfileService, messageLanguage, $sce, $http, $templateCache, $interval) {
        $scope.controllerId = "reportController";
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn($scope.controllerId, "Error");
        $scope.HideTool = true;
        activate();

        function activate() {
            //$scope.$broadcast('Courier_Change', [{ KeyId: 0, DisplayName: '---Select All---' }]);
            $(".content-report").css({ height: $(window).height() - 250, "overflow-y": "auto" });
        }

        $scope.FullName = "";
        $scope.Email = "";
        $scope.HomePhone = "";
        $scope.MobilePhone = "";

        $scope.Content = $sce.trustAsHtml("NO DATA");
        function ReportViewModel() {
            var self = this;
            self.Courier = -1;
            self.From = '';
            self.To = '';
        }
        $scope.Tracking = new ReportViewModel();
        $scope.ShowPreviewPdf = true;

        $scope.ShowHideTool = function (type) {
            $scope.HideTool = type;
        };
        $scope.CurrentDate = new Date();
        $scope.TotalActualDistance = 0;

        $scope.IsProcessing = false;
        var tempCourierId = "";
        var tempFromDate = "";
        var tempToDate = "";

        function checkdate(input) {
            var formats = ['MM/DD/YYYY'];
            return moment(input, formats).isValid();
        }

        $scope.getData = function () {
            //console.log(isNaN(Date.parse($scope.Tracking.From)));
            //console.log(new Date($scope.Tracking.From).getTime() >= new Date($scope.Tracking.To).getTime());

            var comboxCourier = $("#Courier").data("kendoComboBox");
            //console.log('a' + comboxCourier.text() + 'a');
            var mess = "<ol>";
            var isExistMess = false;

            if (comboxCourier.text() == "" || $scope.Tracking.Courier < 0) {
                //mess += "<li>" + messageLanguage.driverReportError + "</li>";
                mess += "<li> The " + $rootScope.CourierDisplayName + " field is required.</li>";
                isExistMess = true;
            }
            if ($scope.Tracking.From === '' || $scope.Tracking.From === undefined) {
                mess += "<li>" + messageLanguage.fromReportError + "</li>";
                isExistMess = true;
            } else if (isNaN(Date.parse($scope.Tracking.From))) {
                mess += "<li>" + messageLanguage.fromReportErrorNaN + "</li>";
                isExistMess = true;
            }
            else if (!checkdate($scope.Tracking.From)) {
                mess += "<li>" + messageLanguage.fromReportErrorNaN + "</li>";
                isExistMess = true;
            }

            if ($scope.Tracking.To === '' || $scope.Tracking.To === undefined) {
                mess += "<li>" + messageLanguage.toReportError + "</li>";
                isExistMess = true;
            }
            else if (isNaN(Date.parse($scope.Tracking.To))) {
                mess += "<li>" + messageLanguage.toReportErrorNaN + "</li>";
                isExistMess = true;
            }
            else if (!checkdate($scope.Tracking.To)) {
                mess += "<li>" + messageLanguage.toReportErrorNaN + "</li>";
                isExistMess = true;
            }
            else if (new Date($scope.Tracking.From).getTime() > new Date($scope.Tracking.To).getTime()) {
                mess += "<li>" + messageLanguage.toGreaterThanFrom + "</li>";
                isExistMess = true;
            }
            mess += "</ol>";

            if (isExistMess == true) {
                logError(mess);
                return;
            }

            //set gia tri temp de view PDF
            tempCourierId = $scope.Tracking.Courier;
            tempFromDate = $scope.Tracking.From;
            tempToDate = $scope.Tracking.To;

            $("#report-driver-grid").data("kendoGrid").setDataSource($scope.DataSource);
            $scope.ShowPreviewPdf = false;

            //$scope.TotalActualDistance = 0;
            //var url = '/Report/GetHtmlReport';
            //masterfileService.callWithUrl(url).perform({ courierId: $scope.Tracking.Courier, fromDate: $scope.Tracking.From, toDate: $scope.Tracking.To }).$promise.then(function (data) {
            //    if (data.Error === undefined || data.Error === '') {
            //        //set gia tri temp de view PDF
            //        tempCourierId = $scope.Tracking.Courier;
            //        tempFromDate = $scope.Tracking.From;
            //        tempToDate = $scope.Tracking.To;

            //        $scope.Content = $sce.trustAsHtml(data.Data);
            //        $(".content-report").css({ height: $(window).height() - 250, "overflow-y": "auto" });
            //        $scope.ShowPreviewPdf = false;
            //    }
            //});

        };

        $scope.clearData = function () {
            if ($scope.Tracking != undefined) {
                $scope.Tracking.From = '';
                $scope.Tracking.To = '';
                $scope.Tracking.Courier = -1;

                $scope.FullName = "";
                $scope.Email = "";
                $scope.HomePhone = "";
                $scope.MobilePhone = "";
                $("#report-driver-grid").data("kendoGrid").setDataSource($scope.DataSource);
            }      
            $scope.$broadcast('Courier_Change', [{ KeyId: -1, DisplayName: '' }]);
            $scope.ShowPreviewPdf = true;
        };

        $scope.pdf = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: "1000px",
                height: "520px",
                title: $rootScope.CourierDisplayName +" Report",
                content: {
                    url: "/Report/ExportPdf/?courierId=" + tempCourierId + "&fromDate=" + tempFromDate + "&toDate=" + tempToDate + "&displayName=" + $rootScope.CourierDisplayName
                },
                animation: false
            });
            popup.open();
        };

        
        //View detail
        function showDetail(dataItem) {
            $templateCache.remove("/Request/Detail");
            var popup = $("#popupWindow").data("kendoWindow");
            //set RequestItem;
            $("#popupWindow").data("RequestItem", dataItem);
            $("#popupWindow").data("VisibleTracking", false);
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
                    //console.log(e);
                    if (intervalObject != null) {
                        $interval.cancel(intervalObject);
                        if ($scope.requestGrid != undefined) {
                            $scope.requestGrid.clearSelection();
                        }

                    }
                    
                },
                animation: false
            });
          
            popup.open();

        }

        $scope.viewDetail = function (id) {
            $http.get('/Request/GetRequestForTracking?requestId=' + id).then(function (data) {
                showDetail(data.data);
            });
        }
        //grid                                                                       
        var schemaFields = {
            Id: { editable: false },
            RequestDate: { editable: false, type: "date" }          
        };

        $scope.PageSizeSelected = 20;
        $scope.DataSource = {};
        $scope.getDataSource = function () {
            $scope.DataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/Report/GetListRequestForReport",
                        type: "POST"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var searchString = $scope.SearchTextPackage;
                            $scope.PageSizeSelected = options.pageSize;
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                SearchString: searchString,
                                CourierId: $scope.Tracking.Courier,
                                FromDate: $scope.Tracking.From,
                                ToDate: $scope.Tracking.To,
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
                    total: "TotalRowCount"
                } ,
                
            });
            console.log('vo',$scope.DataSource)
            return $scope.DataSource;
        };

        function gridDataBound(e) {
            var grid = e.sender;
            if (grid.dataSource.total() == 0) {
                var colCount = grid.columns.length;
                $(e.sender.wrapper)
                    .find('tbody')
                    .append('<tr class="kendo-data-row"><td colspan="' + colCount + '" class="no-data">No data</td></tr>');
            }
        };
        $scope.reportDriverGridOptions = {
            dataSource: $scope.getDataSource(),
            sortable: false,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, 100],
                buttonCount: 5
            },
            reorderable: true,
            resizable: true,
            height: $(window).height() - 240,
            dataBound: gridDataBound,
            columns: [
                 {
                     field: "RequestNo",
                     title: "Request#",
                     width: "130px",
                     locked: true
                 },
                {
                    field: "FullName",
                    title: "Name",
                    width: "120px",
                    locked: true
                },
                {
                    field: "",
                    width: "80px",
                    template: kendo.template($("#templateButtons").html()), attributes: { style: "text-align:center;" },
                    locked: true
                },
                {
                    field: "LocationFrom",
                    title: "From",
                    width: "250px"

                },
                {
                    field: "LocationTo",
                    title: "To",
                    width: "250px"

                },
                {
                    field: "RequestDateFormat",
                    title: "Request Date",
                    width: "100px",              
                    headerAttributes: { style: "text-align:right" },
                    attributes: { style: "text-align:right;" },
                    
                }
                ,
                {
                    field: "EstimateTime",
                    title: "Estimate Time (hours)",
                    width: "150px",
                    headerAttributes: { style: "text-align:right" },
                    attributes: { style: "text-align:right;" }
                }
                ,
                {
                    field: "RealTime",
                    title: "Real Time (hours)",
                    width: "150px",
                    headerAttributes: { style: "text-align:right" },
                    attributes: { style: "text-align:right;" }
                }
                ,
                {
                    field: "TextEstimateDistance",
                    title: "Estimate Distance (miles)",
                    width: "170px",
                    headerAttributes: { style: "text-align:right" },
                    attributes: { style: "text-align:right;" }
                },
                {
                    field: "TextActualDistance",
                    title: "Real Distance (miles)",
                    width: "150px",
                    headerAttributes: { style: "text-align:right" },
                    attributes: { style: "text-align:right;" }
                }
            ],
        };

    }]);