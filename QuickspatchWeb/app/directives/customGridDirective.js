'use strict';
app.directive('pantherCustomGrid', ['$window', function () {
    return {
        restrict: "E",
        link: function (scope, element, attrs) {
            var readUrl = scope.readUrl;
            var readDataConfig = {};
            var columns = [];
            var pageSize = scope.otherConfig.pageSize;
            var serverPaging = scope.otherConfig.serverPaging;
            var serverSorting = scope.otherConfig.serverSorting;
            var sortable = scope.otherConfig.sortable;
            var pageable = scope.otherConfig.pageable;
            var batch = scope.otherConfig.batch;
            var height = scope.otherConfig.height;

            scope.$watch("configs", function () {
                scope.isLoaded = false;
                columns = scope.configs.gridColumns;
                readDataConfig = scope.configs.readDataConfig;

                var dataSource = new kendo.data.DataSource({
                    type: "json",
                    transport: {
                        read: readUrl,
                        parameterMap: function (options, operation) {
                            if (operation !== "read" && options.models) {
                                return { models: kendo.stringify(options.models) };
                            } else if (operation === "read") {
                                return readDataConfig(options);
                            }
                        }
                    },
                    pageSize: pageSize,
                    serverPaging: serverPaging,
                    serverSorting: serverSorting,
                    batch: batch,
                    schema: {
                        data: "Data",
                        total: "TotalRowCount"
                    }
                });

                scope.customGridOptions = {
                    autoBind: false,
                    height: height,
                    dataSource: dataSource,
                    sortable: sortable,
                    pageable: pageable,
                    columns: columns,
                    dataBound: function () {
                        scope.isLoaded = true;
                    }
                };

                if (readDataConfig !== undefined) {
                    scope.customGridOptions.dataSource.read();
                }
            });
        }
    }
}]);