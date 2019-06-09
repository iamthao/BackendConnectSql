'use strict';
app.controller('scheduleController', ['$rootScope', '$scope', 'common', 'messageLanguage', 'masterfileService', '$http', '$templateCache', '$interval',
    function ($rootScope, $scope, common, messageLanguage, masterfileService, $http, $templateCache, $interval) {
        var controllerId = "scheduleController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSuccess = getLogFn(controllerId, "Success");


        function activate() {
           // common.activateController(null, controllerId).then(function () { log('Show schedule'); });
            $scope.formatschedule();
        }

        $scope.IsShowLoadingRoute = false;
        $scope.IsShowLoading = false;

        $scope.FirstDayInit = common.getFirstDayInWeek(new Date());
        $scope.LastDayInit = common.getLastDayInWeek(new Date());
        $scope.FirstDayMonthInit = common.getFirstDayInMonth(new Date());
        $scope.LastDayMonthInit = common.getLastDayInMonth(new Date());

        $scope.FromDate = ($scope.FirstDayInit.getMonth() + 1) + "/" + $scope.FirstDayInit.getDate() + "/" + $scope.FirstDayInit.getFullYear();
        $scope.ToDate = ($scope.LastDayInit.getMonth() + 1) + "/" + $scope.LastDayInit.getDate() + "/" + $scope.LastDayInit.getFullYear();

        $scope.FromDateTime = ($scope.FirstDayInit.getMonth() + 1) + "/" + $scope.FirstDayInit.getDate() + "/" + $scope.FirstDayInit.getFullYear() + " 12:00:00 AM";
        $scope.ToDateTime = ($scope.LastDayInit.getMonth() + 1) + "/" + $scope.LastDayInit.getDate() + "/" + $scope.LastDayInit.getFullYear() + " 11:59:59 PM";
        $scope.FromMonthDateTime = ($scope.FirstDayMonthInit.getMonth() + 1) + "/" + $scope.FirstDayMonthInit.getDate() + "/" + $scope.FirstDayMonthInit.getFullYear() + " 12:00:00 AM";
        $scope.ToMonthDateTime = ($scope.LastDayMonthInit.getMonth() + 1) + "/" + $scope.LastDayMonthInit.getDate() + "/" + $scope.LastDayMonthInit.getFullYear() + " 11:59:59 PM";

        $scope.CourierSelected = "";
        $scope.CourierIdSelected = 0;
        $scope.FrequencySelected = 'daily';
        $scope.CurrentPageSelected = 1;
        $scope.FrequencyWeek = {};
        $scope.Frequency = {};
        $scope.FrequencyDaysOfMonthArray = [];
        $scope.IndexCourierSelected = 0;


        $scope.GenerateArray = function (min, max) {
            var input = [];
            for (var i = min; i <= max; i++) input.push(i);
            return input;
        };
        $scope.DayOfWeekArray = [
            {
                ShortText: "MON",
                FullText: "Monday"
            },
            {
                ShortText: "TUE",
                FullText: "Tuesday"
            },
            {
                ShortText: "WED",
                FullText: "Wednesday"
            },
            {
                ShortText: "THU",
                FullText: "Thursday"
            },
            {
                ShortText: "FRI",
                FullText: "Friday"
            },
            {
                ShortText: "SAT",
                FullText: "Saturday"
            },
            {
                ShortText: "SUN",
                FullText: "Sunday"
            }
        ];
        $scope.createScheduleMonth = function (date) {
            $scope.FirstDayMonthInit = common.getFirstDayInMonth(date);
            $scope.LastDayMonthInit = common.getLastDayInMonth(date);
            $scope.FromDate = ($scope.FirstDayMonthInit.getMonth() + 1) + "/" + $scope.FirstDayMonthInit.getDate() + "/" + $scope.FirstDayMonthInit.getFullYear();
            $scope.ToDate = ($scope.LastDayMonthInit.getMonth() + 1) + "/" + $scope.LastDayMonthInit.getDate() + "/" + $scope.LastDayMonthInit.getFullYear();
            $scope.FromDateTime = ($scope.FirstDayMonthInit.getMonth() + 1) + "/" + $scope.FirstDayMonthInit.getDate() + "/" + $scope.FirstDayMonthInit.getFullYear() + " 12:00:00 AM";
            $scope.ToDateTime = ($scope.LastDayMonthInit.getMonth() + 1) + "/" + $scope.LastDayMonthInit.getDate() + "/" + $scope.LastDayMonthInit.getFullYear() + " 11:59:59 PM";

            $(".table-month").find("div.k-listview").html("");
            $(".table-month").find("div.date").html("");
            if ($scope.CourierId > 0) {
                $scope.IsShowLoading = true;
       
                var i = 7;
                var offset = new Date().getTimezoneOffset();

                // Process for each list
                var datasourcedata = [];
                masterfileService.callWithUrl('/schedule/GetDetailScheduleMonthly').perform({ courierId: $scope.CourierId, fromDate: $scope.FromDateTime, toDate: $scope.ToDateTime, timezone: offset, scheduleType: $scope.ScheduleType }).$promise.then(function (result) {
                    if (result != undefined && result != null) {
                        datasourcedata = result.Data;
                        if (datasourcedata != null) {
                            var dayOfWeekStart = $scope.FirstDayMonthInit.getDay();
                            if (dayOfWeekStart > 0) {
                                for (var j = 0; j < dayOfWeekStart; j++) {
                                    i++;
                                }
                            }
                            for (var d = $scope.FirstDayMonthInit ; d <= $scope.LastDayMonthInit; d.setDate(d.getDate() + 1)) {
                                var r = parseInt(i / 7);
                                var data = $scope.filterRouteMonthList(datasourcedata, d.getDate());
                                switch (d.getDay()) {
                                    case 0:
                                        $("#date-sunday-" + r).html(d.getDate());
                                        $("#preview-table-sunday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                    case 1:
                                        $("#date-monday-" + r).html(d.getDate());
                                        $("#preview-table-monday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                    case 2:
                                        $("#date-tuesday-" + r).html(d.getDate());
                                        $("#preview-table-tuesday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                    case 3:
                                        $("#date-wednesday-" + r).html(d.getDate());
                                        $("#preview-table-wednesday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                    case 4:
                                        $("#date-thursday-" + r).html(d.getDate());
                                        $("#preview-table-thursday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                    case 5:
                                        $("#date-friday-" + r).html(d.getDate());
                                        $("#preview-table-friday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                    case 6:
                                        $("#date-saturday-" + r).html(d.getDate());
                                        $("#preview-table-saturday-" + r).kendoListView({
                                            dataSource: { data: data },
                                            template: kendo.template($("#routeMonthReviewTemplate").html())
                                        });
                                        break;
                                }
                                i++;
                            }

                            var dayOfWeekEnd = $scope.LastDayMonthInit.getDay();
                            if (dayOfWeekEnd < 6) {
                                for (var l = dayOfWeekEnd; l < 6; l++) {
                                    i++;
                                }
                            }
                            $(".tile").hover($scope.tileHoverIn, $scope.tileHoverOut);
                            $(".tile").find(".schedule-content").click($scope.tileFocus);
                            $scope.FirstDayMonthInit = common.getFirstDayInMonth(date);
                            $scope.LastDayMonthInit = common.getLastDayInMonth(date);
                        }
                    }

                    $scope.IsShowLoading = false;
                });
            }
        };
        $scope.createScheduleWeek = function (date) {
            $scope.FirstDayInit = common.getFirstDayInWeek(date);
            $scope.LastDayInit = common.getLastDayInWeek(date);
            $scope.FromDate = ($scope.FirstDayInit.getMonth() + 1) + "/" + $scope.FirstDayInit.getDate() + "/" + $scope.FirstDayInit.getFullYear();
            $scope.ToDate = ($scope.LastDayInit.getMonth() + 1) + "/" + $scope.LastDayInit.getDate() + "/" + $scope.LastDayInit.getFullYear();
            $scope.FromDateTime = ($scope.FirstDayInit.getMonth() + 1) + "/" + $scope.FirstDayInit.getDate() + "/" + $scope.FirstDayInit.getFullYear() + " 12:00:00 AM";
            $scope.ToDateTime = ($scope.LastDayInit.getMonth() + 1) + "/" + $scope.LastDayInit.getDate() + "/" + $scope.LastDayInit.getFullYear() + " 11:59:59 PM";

            $(".table-week").find("div.k-listview").html("");
            $(".table-week").find("div.date").html("");
            if ($scope.CourierId > 0) {
                $scope.IsShowLoading = true;
                var offset = new Date().getTimezoneOffset();
                // Process for each list
                var datasourcedata = [];
                masterfileService.callWithUrl('/schedule/GetDetailScheduleWeekly').perform({ courierId: $scope.CourierId, fromDate: $scope.FromDateTime, toDate: $scope.ToDateTime, timezone: offset, scheduleType: $scope.ScheduleType }).$promise.then(function (result) {
                    if (result != undefined && result != null) {
                        datasourcedata = result.Data;
                        if (datasourcedata != null) {
                            var rIdMonday = [];
                            var rIdTuesday = [];
                            var rIdWednesday = [];
                            var rIdThursday = [];
                            var rIdFriday = [];
                            var rIdSaturday = [];
                            var rIdSunday = [];

                            // Process for each list
                            for (var j = 0; j < datasourcedata.length; j++) {
                                var item = datasourcedata[j];
                                if (item !== undefined && item != null) {
                                    if (item.Monday == true) {
                                        rIdMonday.push(item.Id);
                                    }
                                    if (item.Tuesday == true) {
                                        rIdTuesday.push(item.Id);
                                    }
                                    if (item.Wednesday == true) {
                                        rIdWednesday.push(item.Id);
                                    }
                                    if (item.Thursday == true) {
                                        rIdThursday.push(item.Id);
                                    }
                                    if (item.Friday == true) {
                                        rIdFriday.push(item.Id);
                                    }
                                    if (item.Saturday == true) {
                                        rIdSaturday.push(item.Id);
                                    }
                                    if (item.Sunday == true) {
                                        rIdSunday.push(item.Id);
                                    }
                                }
                            }
                            $("#preview-table-sunday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdSunday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });
                            $("#preview-table-monday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdMonday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });
                            $("#preview-table-tuesday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdTuesday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });
                            $("#preview-table-wednesday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdWednesday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });
                            $("#preview-table-thursday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdThursday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });
                            $("#preview-table-friday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdFriday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });
                            $("#preview-table-saturday").kendoListView({
                                dataSource: { data: $scope.filterRouteList(datasourcedata, rIdSaturday) },
                                template: kendo.template($("#routeReviewTemplate").html())
                            });

                            $(".tile").hover($scope.tileHoverIn, $scope.tileHoverOut);

                            $(".tile").find(".schedule-content").click($scope.tileFocus);
                        }

                    }
                    $scope.IsShowLoading = false;
                });
            }

        };
        $scope.ScheduleType = "Weekly";
        $scope.configOptions = {
            tabPosition: "bottom",
            animation: { open: { effects: "fadeIn" } },
            activate: function (e) {
                $scope.ScheduleType = $(e.item).find("> .k-link").text().trim();
                if ($scope.ScheduleType == "Weekly") {
                    $scope.createScheduleWeek($scope.FirstDayInit);

                } else {

                    $scope.createScheduleMonth($scope.FirstDayMonthInit);
                }
                $scope.$apply();
            }
        }
        $scope.formatschedule = function () {
            $("#content-container").css({ height: $(window).height() - 122 });
            $("#courier-list-wrapper").css({ height: $(window).height() - 212 });
            $("#route-card-content").css({ height: $(window).height() - 250 });
            $(".preview-table-wrapper").css({ height: $(window).height() - 422, "overflow-y": "auto" });
        };
        activate();

        $scope.getCouriersDataSource = function () {
            $scope.CourierDataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/courier/getcouriersforschedule",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var searchString = $scope.SearchCourier;
                            var result = {
                                skip: options.skip,
                                take: options.take,
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
                pageSize: 12,
                page:   $scope.CurrentPageSelected,
                schema: {
                    model: {
                        fields: {
                            Id: 'Id'
                        }
                    },
                    data: "Data",
                    total: "TotalRowCount"
                }
            });

            return $scope.CourierDataSource;
        };

        $scope.search = function () {
            //$scope.getCouriersDataSource().read();
            //$("#courier-list").data("kendoListView").setDataSource($scope.CourierDataSource);
            $scope.loadCouriers();
            $scope.$root.CourierId = 0;

        };
        $scope.LoadCourierFirst = false;

        $scope.loadCouriers = function () {

            if ($("#courier-list").data("kendoListView"))
                $("#courier-list").data("kendoListView").destroy();
            $("#courier-list").kendoListView({
                dataSource: $scope.getCouriersDataSource(),
                template: kendo.template($("#courierListViewTemplate").html()),
                selectable: "single",
                change: function (e) {
                    var index = this.select().index();
                    $scope.IndexCourierSelected = index;
                    var dataItem = $scope.CourierDataSource.at(index);
                    if (dataItem != undefined) {
                        $scope.CourierSelected = dataItem.Name;
                        $scope.clickCourier(dataItem.Id);

                    }
                },
                dataBound: function () {
                    var pager = $("#pager").kendoPager({
                        dataSource: $scope.CourierDataSource,
                        buttonCount: 1
                    }).data("kendoPager");
                    //if ($("#pager").data("kendoPager"))
                    //    $("#pager").data("kendoPager").destroy();
                    if ($scope.CurrentPageSelected == pager.page()) {
                        var dataItem = this.dataSource.view()[$scope.IndexCourierSelected];//$scope.CourierDataSource.at($scope.CurrentPageSelected);
                        if (dataItem != undefined) {
                            $scope.CourierSelected = dataItem.Name;
                            if ($scope.LoadCourierFirst == false) {
                                $scope.getScheduleOfCouriersDataSource(dataItem.Id);

                                $scope.LoadCourierFirst = true;
                            }
                            $scope.CourierIdSelected = dataItem.Id;
                            $scope.$apply();
                        } else {
                            $scope.CourierIdSelected = 0;
                            $scope.$apply();
                        }
                        this.select(this.element.children().eq($scope.IndexCourierSelected));
                    }

                   
                }
            });
        };

        $scope.clickCourier = function (id) {
            $scope.CourierId = id;
            $scope.$root.CourierId = id;
            $scope.getScheduleOfCouriersDataSource(id);
            if ($scope.ScheduleType == "Weekly") {
                $scope.createScheduleWeek($scope.FirstDayInit);

            } else {
                $scope.createScheduleMonth($scope.FirstDayMonthInit);
            }

            var pager = $("#pager").kendoPager({
                dataSource: $scope.CourierDataSource
            }).data("kendoPager");

            $scope.CurrentPageSelected = pager.page();
            $scope.$apply();
        };

        $scope.getScheduleOfCouriersDataSource = function (courierId) {
            if (courierId === undefined) {
                courierId = 0;
                $scope.CourierId = courierId;
                //$scope.IsShowLoadingRoute = false;
            }
            $scope.ScheduleOfCourierDataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/schedule/getschedulesofcourier?courierId=" + courierId,
                        type: "GET"
                    }
                },
                schema: {
                    model: {
                        id: "Id",
                    },
                    data: "Data",
                    total: "TotalRowCount"
                }
            });
            return $scope.ScheduleOfCourierDataSource;
        };

        $scope.showWarning = function(dataItem) {
            $http.get('/Schedule/WarningInfo?scheduleId=' + dataItem.Id + '&courierId=' + $scope.CourierId).then(function(result) {
                var obj = result.data;
                var title = '<span class="fa fa-exclamation-triangle"></span>';
                var data = Base64.encode(JSON.stringify(obj));
                var popup = $("#popupWindow").data("kendoWindow");
                popup.setOptions({
                    width: "600px",
                    height: "250px",
                    title: title,
                    content: {
                        url: "/Schedule/WarningDetail?data=" + data
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

        $scope.tileHoverIn = function () {
            $('.tile[name="' + $(this).attr('name') + '"]').addClass('hover');
        };

        $scope.tileHoverOut = function () {
            $('.tile[name="' + $(this).attr('name') + '"]').removeClass('hover').removeClass('k-state-selected');
        };

        $scope.tileFocus = function () {
            if ($(this).parent().hasClass('active')) {
                $('.tile.active').removeClass('active');
            } else {
                $('.tile.active').removeClass('active');
                $('.tile[name="' + $(this).parent().attr('name') + '"]').addClass('active');
            }
        };
        $scope.filterRouteList = function (routeList, idArray) {
            var data =  _.filter(routeList, function (item) {
                if (item !== undefined && item != null) {
                    return _.find(idArray, function (num) {
                        return num == item.Id;
                    });
                }
                return null;
            });
           
            for (var i = 0; i < data.length; i++) {
                data[i]["RequestNo"] = '';
            }

            var listRequestHistory = _.filter(data, function (item) { return item.HistoryScheduleId != null; });
            
            var result = _.reject(data, function(item1) {
                return _.find(listRequestHistory, function (item2) {
                    return item2.HistoryScheduleId == item1.Id;
                });
            });

            for (var j = 0; j < result.length; j++) {
                var obj = _.findWhere(data, { Id: result[j].HistoryScheduleId, IsSchedule: true });
                if (obj) {
                    result[j].RequestNo = result[j].Name;
                    result[j].Name = obj.Name;
                }
            }
            return result;
        };
        $scope.filterRouteMonthList = function (routeList, date) {
            var data =  _.filter(routeList, function (item) {
                if (item !== undefined && item != null && item.Date == date) {
                    return item;
                }
                return null;
            });
            for (var i = 0; i < data.length; i++) {
                data[i]["RequestNo"] = '';
                
            }

            var listRequestHistory = _.filter(data, function (item) { return item.HistoryScheduleId != null; });

            var result = _.reject(data, function (item1) {
                return _.find(listRequestHistory, function (item2) {
                    return item2.HistoryScheduleId == item1.Id;
                });
            });

            for (var j = 0; j < result.length; j++) {
                var obj = _.findWhere(data, { Id: result[j].HistoryScheduleId, IsSchedule: true });
                if (obj) {
                    result[j].RequestNo = result[j].Name;
                    result[j].Name = obj.Name;
                }
            }
            return result;
        };

        $(document).click(function (e) {
            var tile = $('.tile');
            if (!tile.is(e.target) && tile.has(e.target).length == 0) {
                //$('.tile.active').removeClass('active');
            }
        });

        $scope.addNewRoute = function () {
            //collect days of week
            var frequencyDaysOfWeekArray = [];
            _.each($scope.FrequencyWeek, function (element, index) {
                if (element) {
                    frequencyDaysOfWeekArray.push(index);
                }
            });

            //collect days of month
            var frequencyDaysOfMonthArray = [];
            _.each($scope.FrequencyDaysOfMonthArray, function (element) {
                if (element) {
                    frequencyDaysOfMonthArray.push(element);
                }
            });

            $scope.Frequency = common.generateCrontabString($scope.FrequencySelected, frequencyDaysOfWeekArray.join(), frequencyDaysOfMonthArray.join(), 23, 59, 59);
            var currentDate = new Date();
            bootbox.confirm("Are you sure that you want to create new route?", function (result) {
                if (result) {
                    var url = '/Schedule/Create';

                    var startTime = '';
                    var endTime = '';
                    if ($scope.StartTime !== undefined) {
                        startTime = (new Date(currentDate.setHours($scope.StartTime.split(":")[0], $scope.StartTime.split(":")[1]))).toUTCString();
                    }
                    if ($scope.EndTime !== undefined) {
                        endTime = (new Date(currentDate.setHours($scope.EndTime.split(":")[0], $scope.EndTime.split(":")[1]))).toUTCString();
                    }

                    var param = {
                        SharedParameter: JSON.stringify({
                            Name: $scope.RouteName,
                            LocationFrom: $scope.LocationFrom,
                            LocationTo: $scope.LocationTo,
                            Frequency: $scope.Frequency,
                            CourierId: $scope.CourierId,
                            StartTime: startTime,
                            EndTime: endTime,
                            DurationStart: $scope.DurationStart,
                            DurationEnd: $scope.DurationEnd,
                            IsNoDurationEnd: $scope.IsNoDurationEnd,
                            Description: $scope.Description
                        })
                    };

                    masterfileService.callWithUrl(url).perform({ parameters: param }).$promise.then(function (data) {
                        if (data.Error === undefined || data.Error === '') {
                            $scope.search();
                            logSuccess('Create route successfully');
                        }
                    });
                }
            });
        };

        $scope.PopupViewModel = new PopupViewModel();
        $scope.modelName = 'Schedule';
        $scope.Add = function () {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: $(window).width() > $scope.PopupViewModel.PopupWidth ? $scope.PopupViewModel.PopupWidth + "px" : $(window).width() + "px",
                height: $scope.PopupViewModel.PopupHeight + "px",
                title: "Add route",
                content: {
                    url: "/" + $scope.modelName + "/Create"
                },
                animation: false
            });
            popup.open();
        };

        $scope.DeleteSchedule = function (id) {
            common.bootboxConfirm("Are you sure that you want to delete this record?", function () {
                $scope.IsShowLoadingRoute = true;
                masterfileService.deleteById($scope.modelName).perform({ id: id }).$promise.then(function (data) {
                    if (data.Error === undefined || data.Error === '') {
                        var logSuccess = getLogFn(controllerId, "success");
                        if ($scope.$parent.deleteMessage != undefined) {
                            $scope.IsShowLoadingRoute = false;
                            logSuccess($scope.$parent.deleteMessage);
                        } else {
                            $scope.IsShowLoadingRoute = false;
                            logSuccess('Delete route successfully');
                        }

                        $scope.getScheduleOfCouriersDataSource($scope.CourierId);
                        $scope.search();
                    }
                });
            }, function () { }).modal('show');

        };

        $scope.EditSchedule = function (id) {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: $(window).width() > $scope.PopupViewModel.PopupWidth ? $scope.PopupViewModel.PopupWidth + "px" : $(window).width() + "px",
                height: $scope.PopupViewModel.PopupHeight + "px",
                title: "Update route",
                content: {
                    url: "/" + $scope.modelName + "/Update/" + id
                },
                animation: false
            });
            popup.open();
        };

        $scope.Pre = function () {
            if ($scope.ScheduleType == "Weekly") {
                $scope.createScheduleWeek($scope.FirstDayInit.addDays(-7));
            } else {
                $scope.createScheduleMonth($scope.FirstDayMonthInit.addDays(-1));
            }
        };
        $scope.Next = function () {
            if ($scope.ScheduleType == "Weekly") {
                $scope.createScheduleWeek($scope.FirstDayInit.addDays(7));
            } else {
                $scope.createScheduleMonth($scope.LastDayMonthInit.addDays(1));
            }

        };
        Date.prototype.addDays = function (days) {
            var dat = new Date(this.valueOf());
            dat.setDate(dat.getDate() + days);
            return dat;
        };

        $scope.$on("ReloadScheduleGrid", function () {
            //$scope.getScheduleOfCouriersDataSource($scope.CourierId);
            $scope.loadCouriers();
        });


        //Nghiep: Implement click show detail

        function show(dataItem) {
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
                    if (intervalObject != null) {
                        $interval.cancel(intervalObject);
                        if ($scope.requestGrid != undefined) {
                            $scope.requestGrid.clearSelection();
                        }
                    }
                    popup.content('');
                },
                animation: false
            });
            popup.open();
        }
        $scope.showDetail = function (name, requestNo, isSchedule, historyRequestId) {
            if (!isSchedule) {
                $http.get('/Request/GetRequestNoForTracking?requestNo=' + (historyRequestId == null || requestNo == '' ? name : requestNo)).then(function (data) {
                    show(data.data);
                });
            }
        }
    }]);