'use strict';
app.controller('eventController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', function ($rootScope, $scope, common, messageLanguage, $window) {
    $scope.controllerId = "eventController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn($scope.controllerId);
    activate();

    function activate() {
       
        // common.activateController(null, controllerId).then(function () { log(messageLanguage.listdashboard); });
    }

    var schemaFields = {
        Id: { editable: false },
        CreatedOnDateTime: { editable: false, type: "date" }
    };
    $scope.getDataSouce = function() {
        return new kendo.data.DataSource({
            type: "json",
            transport: {
                read: "/Dashboard/GetEvents",
            },
            schema: {
                model: {
                    fields: {
                        Id: 'Id',
                        //fields: schemaFields
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
                                    var d = moment(kendo.toString(kendo.parseDate(item[name]), 'u')).toDate();
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
    };
    $scope.timeout = null;
    $scope.loadEventEnd = function () {
        clearTimeout($scope.timeout);
        $("#event-list").kendoListView({
            dataSource: $scope.getDataSouce(),
            template: kendo.template($("#eventListViewTemplate").html()),
            selectable: "single",
            change: function (e) {
                var index = this.select().index();
                var dataItem = this.dataSource.at(index);

            },
            dataBound: function () {
                $("#event-list").css({ "height": "100%" });
                $scope.timeout=setTimeout(function () { $scope.loadEventEnd(); }, 10000);
            }
        });
        kendo.ui.progress($("#event-list"), false);

    };
    //$scope.loadEventEnd();
    //$scope.refreshEventGrid = function () {
    //    //$scope.diaryGrid.dataSource.read();
    //};

    //$scope.getListEvent = function() {
    //    var eventList = [
    //       { description: "'Admin' send request 'AAA' to 'Courier 2' at '06/15/2015 12:25 AM'" },
    //       { description: "'Lam Nguyen' send request 'BBB' to 'Courier 5' at '06/15/2015 12:25 AM'" },
    //       { description: "Admin send request 'CCC' to 'Courier 2' at '06/15/2015 12:25 AM'" },
    //       { description: "Admin send request 'DDD' to 'Courier 2' at '06/15/2015 12:25 AM'" }
    //    ];
    //    $("#event-list").kendoListView({
    //        dataSource: datasouce,
    //        template: kendo.template($("#eventListViewTemplate").html()),
    //        selectable: "single",
    //        change: function (e) {
    //            var index = this.select().index();
    //            var dataItem = this.dataSource.at(index);

    //        }
    //    });
    //};
    //$scope.getListEvent();
}]);