app.controller('notifyDeclineController', ['$scope', '$http', 'common', '$timeout', '$document', 'masterfileService',
    function ($scope, $http, common, $timeout, $document, masterfileService) {
        var getLogFn = common.logger.getLogFn;
        $scope.TimeOld = (new Date()).toUTCString();
        $scope.ShowClickDecline = 0;
        var schemaFields = {
            Id: { editable: false },
            CreatedDate: { editable: false, type: "date" }
        };

        $scope.getNotifyDeclineDataSource = function () {
            //$timeout.cancel($scope.timeout);
            $scope.NotifyDeclineDataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/Common/GetNotifyDecline",
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
                                Time: $scope.TimeOld
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
                pageSize: 10,

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
                        //console.log(response);
                        $scope.TotalRowCount = response.TotalRowCount.toString();
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
            return $scope.NotifyDeclineDataSource;
        };
        $scope.notifyDeclineGridOptions = {

            dataSource: $scope.getNotifyDeclineDataSource(),
            pageable: true,

            columns: [
                {
                    field: "Event",
                    title: "Event",
                    width: "350px"
                    //template: kendo.template($("#eventTemplate").html())
                },
                    {
                        field: "CreatedDate",
                        title: "Created Date",
                        width: "160px",
                        format: "{0:MM/dd/yyyy hh:mm tt}"
                    }
            ],
            dataBinding: function (e) {
                //$scope.timeout = $timeout(function () {
                //    $("#notifyDeclineGrid").data("kendoGrid").dataSource.read();
                //}, 10000);
            }
        };

        $document.on('click', function (event) {
            if ($("#notify-decline").hasClass("open")) {
                $scope.TimeOld = (new Date()).toUTCString();
                $scope.TotalRowCount = "";
                $("#notifyDeclineGrid").data("kendoGrid").dataSource.data([]);
            }

        });
        $scope.IsShowNotifyDecline = "";
        $scope.showNotifyDecline = function() {
            if ($scope.IsShowNotifyDecline == "") {
                $scope.IsShowNotifyDecline = "open";
            } else {
                $scope.IsShowNotifyDecline = "";
                $scope.TimeOld = (new Date()).toUTCString();
                $scope.TotalRowCount = "";
                $("#notifyDeclineGrid").data("kendoGrid").dataSource.data([]);
            }
        };
        $scope.checkSumRequest = 0;
        $scope.checkSumEventDecline = 0;
        $scope.checkSumCourier = 0;
        $scope.CheckChangeTable = function () {
            $timeout.cancel($scope.timeout);
            masterfileService.callWithUrl('/Common/CheckChangeTable').perform({}).$promise.then(function (result) {
                if (result.Data.CheckSumRequest !== $scope.checkSumRequest) {
                    $scope.checkSumRequest = result.Data.CheckSumRequest;
                    $scope.$root.$broadcast("HasChangeRequest");
                }
                if (result.Data.CheckSumEventDecline !== $scope.checkSumEventDecline) {
                    $scope.checkSumEventDecline = result.Data.CheckSumEventDecline;
                    $scope.$root.$broadcast("HasChangeEventDecline");
                }
                if (result.Data.CheckSumCourier !== $scope.checkSumCourier) {
                    $scope.checkSumCourier = result.Data.CheckSumCourier;
                    $scope.$root.$broadcast("HasChangeCourier");
                }
                $scope.timeout = $timeout(function () { $scope.CheckChangeTable(); }, 5000);
            });
        };
        if (getCookie("IsCamino").toString() == "false") {
            $scope.CheckChangeTable();
        }
        
        $scope.$on("HasChangeEventDecline", function () {
            $("#notifyDeclineGrid").data("kendoGrid").dataSource.read();
        });
    }]);