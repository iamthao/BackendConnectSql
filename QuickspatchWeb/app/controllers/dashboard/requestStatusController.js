
app.controller('requestStatusController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', '$http', '$timeout',
    function ($rootScope, $scope, common, messageLanguage, $window, $http, $timeout) {

    $scope.controllerId = "requestStatusController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn($scope.controllerId);
    var logSuccess = getLogFn($scope.controllerId, "success");
    var logError = getLogFn($scope.controllerId, "error");
    activate();
    $scope.currentMousePos = { x: -1, y: -1 };
    function activate() {
        // common.activateController(null, controllerId).then(function () { log(messageLanguage.listdashboard); });
        $(document).mousemove(function (event) {
            $scope.currentMousePos.x = event.pageX;
            $scope.currentMousePos.y = event.pageY;
            
        });
        $(document).click(function(event) {
            if ($(event.target).attr('data-val') != 'reassign' && $(event.target).children().attr('data-val') != 'reassign' && $(event.target).attr('id') != 'reassign-courier') {
                var parents = $(event.target).parents();
                for (var i = 0; i < parents.length; i++) {
                    if ($(parents[i]).attr('id') == 'reassign-courier' || $(parents[i]).attr('id') == 'ReAssignCourierId-list') {
                        return;
                    }
                }
                $('#reassign-courier').hide();
            }

        });
    }

    $scope.StatusSelectedId = 0; //8 compeleted
    $scope.DataSourceChart = [];
    $scope.CourierId = 0;
    $scope.isRefresh = false;
    $scope.isViewAll = false;
    $scope.getChartDataSource = function() {
        $scope.DataSourceChart= new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: "/Request/GetPieChartData?courierId=" + ($scope.CourierId == 0 ? null : $scope.CourierId),
                    type: "GET"
                }
            },
            sort: {
                field: "Category",
                dir: "asc"
            },
            schema: {
                model: {
                    fields: {
                        Id: 'Id',
                    }
                },
                data: "Data"
            }
        });
        return $scope.DataSourceChart;
    };
    $scope.loadChart = function () {
        $("#donut-chart").kendoChart({
            theme: "bootstrap",
            transitions: false,
            legend: {
                visible: true,
                position: "bottom",
                labels: {
                    font: "12px Lato"
            },
                field:"Value"
            },
            chartArea: { background: "transparent" },
            seriesDefaults: {
                overlay: { gradient: null }
            },
            dataSource: $scope.DataSourceChart,
            series: [{
                name: "Donut chart",
                type: 'pie',
                field: "Value",
                categoryField: "Category",
                explodeField: "Selected",
                colorField: "Color"
            }],
            tooltip: { visible: true, template: "#: category #: #: value #" },
            seriesClick: function (e) {
                $scope.$parent.$broadcast("Chart_Click");
                $scope.changeCategory(e, e.category);
                clearTimeout($scope.timeout);
            },

            legendItemClick: function (e) {
                $scope.$parent.$broadcast("Chart_Click");
                e.preventDefault();
                $scope.changeCategory(e, e.text);
                clearTimeout($scope.timeout);
                
            },
            render: function (e) {

                var i = 1;
                var isSelected = false;
                var data = e.sender.dataSource.data();
                if (data != null && data.length > 0) {
                    _.each(data, function(item) {

                        if (item.Selected == true) {
                            $scope.StatusSelectedId = item.Id;
                            $scope.SelectedRequestStatus = (item.Category + ' (' + item.Value + ')');
                           
                            //if (Abandoned)
                            if (item.Id != 10) {
                                $("#request-completion").data("kendoGrid").hideColumn(5);
                            } else {
                                $("#request-completion").data("kendoGrid").showColumn(5);
                            }
                           
                            $("#request-completion").data("kendoGrid").dataSource.read();
                            isSelected = true;
                        }

                        if (i == e.sender.dataSource.data().length && ($scope.StatusSelectedId == 0 || isSelected == false)) {
                            item.Selected = true;
                            $scope.loadChart();
                        }
                        $scope.$emit("LoadRequestStatusDone");
                        //if ($scope.isRefresh == true) {
                        //    $scope.isRefresh = false;
                        //}

                        i++;
                    });
                } else {
                    $scope.StatusSelectedId = 0;
                    $scope.SelectedRequestStatus = "";
                    //$scope.showAllRequestStatus();
                    
                    $scope.$apply();
                    $("#request-completion").data("kendoGrid").dataSource.read();
                }

            }
        });
    };

    $scope.myGrid = $("#request-completion");
    
    $scope.changeCategory = function (e,category) {
        _.each(e.sender.dataSource.data(), function (item) {
            if (item.Category != category) {
                item.Selected = false;
            }
            else {
                item.Selected = true;
            }
          
        });
        $scope.loadChart();
    };

    var schemaFields = {
        Id: { editable: false },
        RequestNo: { editable: false, type: "string" },
        LocationFromName: { editable: false, type: "string" },
        LocationToName: { editable: false, type: "string" },
        CourierSearch: { editable: false, type: "string" },
        TimeNoFormat: { editable: false, type: "date" }
    };
    
    $scope.getGridDataSource = function() {
        return new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: "/Request/GetCurrentDataRequests",
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
                            statusId: $scope.StatusSelectedId,
                            courierId: ($scope.CourierId == 0 ? null : $scope.CourierId)
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
                    if (response != undefined && response.Data != undefined && response.Data != null && response.Data.length > 0) {
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
                    }
                    return response;
                }
            }
        });
    };
        $scope.assignObjItem = null;
        $scope.reassign = function (item) {
            $scope.assignObjItem = item;
        var widthEle = $('#reassign-courier').width();
        var heightEle = $('#reassign-courier').height();
        var mouseX = $scope.currentMousePos.x;
        var mouseY = $scope.currentMousePos.y;
        $('#reassign-courier').css({ "left": (mouseX - (widthEle)) + "px", "top": (mouseY - (heightEle + 80)) + "px" });
        $('#reassign-courier').show();
        
    }

        $scope.timeout = null;
        $scope.showWarning = function (dataItem) {
            $http.get('/Request/WarningInfo?requestId=' + dataItem.Id + '&courierId=' + dataItem.CourierId).then(function (result) {
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
                    close: function (e) {
                        popup.content('');
                        //$scope.cancelRequest();
                        $scope.popConfirmOpen = false;
                    },
                    animation: false
                });
                popup.open();
            });
        }
    $scope.mainGridOptions = {
        dataSource: $scope.getGridDataSource(),
        sortable: true,
        resizable: true,
        pageable: {
            refresh: true,
            pageSizes: [10, 25, 50, 100, 1000],
            buttonCount: 5
        },
        height: "100%",

        columns: [
            {
                field: "RequestNo",
                title: "Req #",
                width: "150px",
                template: "${RequestNo} <span ng-if='dataItem.IsWarning' style='color:\\#FFDC00;cursor: pointer;' ng-click='showWarning(dataItem);' class='fa fa-exclamation-triangle'></span>",
            },
            {
                field: "LocationFromName",
                title: "From",
                width: "120px",
            },
            {
                field: "LocationToName",
                title: "To",
                width: "120px",
            },
            {
                field: "CourierSearch",
                title: $rootScope.CourierDisplayName,
                width: "150px",
            },
            {
                field: "TimeNoFormat",
                title: "Sending",
                width: "120px",
                format: "{0:hh:mm tt}"
            },
            //{
            //    field: " ",
            //    title: "",
            //    width: "50px",
            //    template: '#if(!IsSchedule) {# <a href="javascript:void(0)" class="btn btn-sm btn-default pull-right" title="Reassign" ng-click="reassign(dataItem)"><span data-val="reassign" class="fa fa-user"></span></a>#}#'
            //}
        ],
        dataBound: function (e) {
            $scope.timeout=setTimeout(function () {
                $scope.$root.$broadcast("RefreshCourierStatus");
            }, 60000);
            //createScrollBarForGrid('request-completion');
        }
    };
    $scope.showAllRequestStatus = function () {
        $scope.isViewAll = true;
        $scope.CourierId = null;
        $scope.SelectedCourier = "All";
        
        $scope.refreshRequestStatus();
    };
    $scope.refreshRequestStatus = function () {
        clearTimeout($scope.timeout);
        $scope.isRefresh = true;
        $scope.getChartDataSource();
        $scope.loadChart();
    };


    $scope.$on("RefreshRequestStatus", function (event, val) {

        if (val.clickCourier) {
            $scope.isViewAll = false;
        } 
        $scope.SelectedCourier = $scope.isViewAll == true ? "All" : val.courierName;
        $scope.CourierId = $scope.isViewAll == true ? null : val.courierId;
        $scope.refreshRequestStatus();
    });

    $scope.ReAssignCourierId = 0;
    $scope.callReAssignCourier = function () {
        $('#reassign-courier').hide();
        $scope.$broadcast('ReAssignCourier_Change', [{ KeyId: 0, DisplayName: '' }]);
        
    };
    $scope.assignCourier = function () {
        if ($scope.ReAssignCourierId != undefined && $scope.ReAssignCourierId > 0 && $scope.assignObjItem != null) {
            $.ajax({
                method: "POST",
                url: "/request/reassigncourier",
                data: { id: $scope.assignObjItem.Id, courierId: $scope.ReAssignCourierId }
            })
                .done(function (result) {
                    if (result == true) {
                        $scope.ReAssignCourierId = 0;
                        $scope.IsShowReAssignCourierForm = false;
                        logSuccess(messageLanguage.reassignCourierSuccess);
                        $('#all-dashboard').click();
                        //$scope.refreshRequestStatus();
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
    
}]);