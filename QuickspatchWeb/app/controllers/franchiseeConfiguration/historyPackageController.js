'use strict';
app.controller('historyPackageController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage',
    function ($scope, masterfileService, $state, common, config, messageLanguage) {
        var events = config.events;
        $scope.controllerId = "historyPackageController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            //common.activateController(null, $scope.controllerId).then(function () { log(messageLanguage.updateFranchiseeModule); });
        }

        //view Detail
        $scope.viewDetail = function (id) {
            $scope.setRequestId(id);
            $scope.setActionHistory(2);
        };
        //search
        $scope.Search = function ($event) {
            if ($event != null) {
                var currentKey = $event.which || $event.charCode;
                if (currentKey === 13) {
                    $("#package-grid").data("kendoGrid").setDataSource($scope.DataSource);
                }
            } else {
                $("#package-grid").data("kendoGrid").setDataSource($scope.DataSource);
            }
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
                        url: "/FranchiseeConfiguration/GetListChangePackage",
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

        $scope.packageGridOptions = {
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
                     field: "RequestId",
                     title: "Request#",
                     width: "80px"

                 },
                {
                    field: "PackageName",
                    title: "New Package",
                    width: "150px"

                },
                {
                    field: "OldPackageName",
                    title: "Old Package",
                    width: "150px"

                },
                {
                    field: "ChangeDate",
                    title: "Change Date",
                    width: "100px"

                },
                {
                    field: "IsApply",
                    title: "Apply",
                    width: "50px",
                    template: kendo.template($("#packageTemplate").html())
                },
                {
                    field: "", width: "50px", template: kendo.template($("#withdrawButtons").html())
                }
            ],
        };


    }]);