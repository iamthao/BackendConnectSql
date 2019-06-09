'use strict';
app.controller('historyTransactionController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage',
    function ($scope, masterfileService, $state, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "historyTransactionController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //console.log($scope.requestId);
        }

        //search
        $scope.Search = function ($event) {
            $scope.setRequestId(0);
            if ($event != null) {
                var currentKey = $event.which || $event.charCode;
                if (currentKey === 13) {
                    $("#transaction-grid").data("kendoGrid").setDataSource($scope.DataSource);
                }
            } else {
                $("#transaction-grid").data("kendoGrid").setDataSource($scope.DataSource);
            }
        };
        //
        $scope.viewDetail1 = function (id) {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: "700px",
                height: "325px",
                title: "Transaction Information",
                content: {
                    url: "/FranchiseeConfiguration/TransactionDetail/" + id
                },
                animation: false
            });
            popup.open();
        };
        //grid                                                                       
        var schemaFields = {
            Id: { editable: false },
            CreatedDateNoFormat: { editable: false, type: "date" },
            TimeNoFormat: { editable: false, type: "date" }
        };

        $scope.PageSizeSelected = 20;
        $scope.DataSource = {};
        $scope.getDataSource = function () {
            $scope.DataSource = new kendo.data.DataSource({
                type: "json",
                transport: {
                    read: {
                        url: "/FranchiseeConfiguration/GetListTransaction",
                        type: "POST"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {
                            var searchString = $scope.SearchTextTransaction;
                            $scope.PageSizeSelected = options.pageSize;
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                SearchString: searchString,
                                RequestId: $scope.requestId
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
                }
            });
            return $scope.DataSource;
        };

        $scope.transactionGridOptions = {
            dataSource: $scope.getDataSource(),
            sortable: false,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, 100],
                buttonCount: 5
            },
            reorderable: true,
            resizable: true,
            height: $(window).height() - 180,

            columns: [
                {
                    field: "TransactionId",
                    title: "Transaction#",
                    width: "200px"

                },
                {
                    field: "SubmittedDate",
                    title: "Charge On",
                    width: "150px"

                },             
                {
                    field: "RequestId",
                    title: "Request#",
                    width: "150px"
                }
                ,
                {
                    field: "", width: "50px", template: kendo.template($("#withdrawButtons").html())
                }

            ],
        };

    }]);