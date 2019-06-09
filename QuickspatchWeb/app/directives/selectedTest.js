'use strict';
app.directive('selectedTest', [function () {
    return {
        restrict: "E",
        templateUrl: function(element, attrs) {
            return '<div class="k-list-row" style="width: 100%; display: flex;"> '+
            '<div style="padding-right:10px; text-align:left; width: 80%; display: flex; flex-direction: column; justify-content: center">'+
                '<span style="vertical-align: middle;text-overflow: ellipsis; white-space: nowrap;display: block;overflow: hidden">{{Name}}</span>'+
            '</div>'+
            '<div style="width: 40%;text-align: right">'+
             '<button class="btn btn-default btn-primary" type="button" name="BtnRemoveTest">Remove</button>'+
            '</div>'+
        '</div>';
        },
        link: function (scope, element, attrs) {
            // Get attribute and assign for grid
            var modelName = attrs.modelName;
            var strSchemaFields = attrs.viewSchemaConfigData;
            var schemaFields = JSON.parse(strSchemaFields);
            var columnStr = attrs.viewColumnsConfigData;
            var columns = JSON.parse(columnStr);
            scope.columns = columns;
            _.each(columns, function (item) {
                if (item.template != null && item.template != '') {
                    item.template = kendo.template($("#" + item.template).html());
                }
            });

            // Get information for grid config view model
            scope.userId = attrs.userId;
            scope.modelName = modelName;
            scope.documentTypeId = attrs.documentTypeId;
            scope.gridInternalName = attrs.gridInternalName;
            scope.gridConfigViewModel = new GridConfigViewModel(0, scope.userId, scope.documentTypeId, scope.gridInternalName);
            var columnDataStr = attrs.viewColumnsData;
            scope.viewColumnData = JSON.parse(columnDataStr);
            scope.idGrid = attrs.gridId;
            var gridID = "#" + scope.idGrid;

            var getUrl = "/" + modelName + "/" + "GetDataForGrid";
            var changeColumnConfig = function (e) {
                scope.changeColumnConfigEvent = e;
                scope.$apply("changeColumnConfig()");
            }

            function changeColumnPosition(e) {
                scope.changeColumnPositionEvent = e;
                scope.$apply("changeColumnPosition()");
            }

            function getGridConfig() {
                scope.$apply("getGridConfig()");
            }

            scope.SearchTextEncoded = "";

            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: getUrl,
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {

                            // Limitation of nested json object; temporarily have to modify the json to
                            // pass in sort information correctly.
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                SearchString: scope.SearchTextEncoded
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
                pageSize: 50,
                batch: true,
                emptyMsg: 'No Record',
                schema: {
                    model: {
                        id: "Id",
                        fields: schemaFields
                    },
                    data: "Data",
                    total: "TotalRowCount"
                }
            });

            scope.mainGridOptions = {
                dataSource: dataSource,
                toolbar: kendo.template($("#templateHeader").html()),
                //excel: {
                //    fileName: modelName + ".xlsx",
                //    filterable: true
                //},
                pageable: {
                    refresh: true,
                    pageSizes: [10, 25, 50, 100, 1000],
                    buttonCount: 5
                },
                sortable: true,
                height: 500,
                columns: columns,
                editable: false,
                selectable: "multiple row",
                scrollable: { virtual: false },
                navigatable: false,
                resizable: true,
                reorderable: true,
                columnResize: changeColumnConfig,
                columnHide: changeColumnConfig,
                columnShow: changeColumnConfig,
                columnReorder: changeColumnPosition,
                columnMenu: attrs.showColumnMenu,
                dataBound: function (e) {
                    getGridConfig();
                },
                columnMenuInit: function (e) {
                    e.container.find('li.k-item.k-sort-asc[role="menuitem"]').remove();
                    e.container.find('li.k-item.k-sort-desc[role="menuitem"]').remove();
                    e.container.find('.k-columns-item .k-group').css({ 'width': '200px', 'max-height': '400px' });

                },
                change: function (e) {
                    e.preventDefault();
                    return;
                    var grid = e.sender;
                    var currentSelectedData = grid.dataItem(this.select());
                    var id = currentSelectedData.Id;
                    location.href = "#/" + modelName.toLowerCase() + "/update/" + id;
                }
            };

            scope.$on("ReloadGridWhenSearch", function (event, args) {
                var searchText = args.SearchText;
                searchText = "<SearchTerms>" + Encoder.htmlEncode(searchText) + "</SearchTerms>";
                searchText = Base64.encode('<AdvancedQueryParameters>' + searchText + '</AdvancedQueryParameters>');
                scope.SearchTextEncoded = searchText;
                dataSource.read();
            });

            scope.$on("ReloadGrid", function () {
                dataSource.read();
            });
        }
    };
}]);