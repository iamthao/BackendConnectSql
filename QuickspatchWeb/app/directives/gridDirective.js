'use strict';
app.directive('pantherGrid', [function () {
    return {
        restrict: "E",
        link: function (scope, element, attrs) {
            // Get attribute and assign for grid
            var customHeaderTemplate = "#templateHeader";
            if (attrs.customHeaderTemplate != null && attrs.customHeaderTemplate != undefined && attrs.customHeaderTemplate != "") {
                customHeaderTemplate = "#" + attrs.customHeaderTemplate;
            }

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
            scope.idGrid = attrs.gridId;
            scope.userId = attrs.userId;
            scope.modelName = modelName;
            scope.documentTypeId = attrs.documentTypeId;
            scope.gridInternalName = attrs.gridInternalName;
            scope.gridConfigViewModel = new GridConfigViewModel(0, scope.userId, scope.documentTypeId, scope.gridInternalName);
            var columnDataStr = attrs.viewColumnsData;
            scope.viewColumnData = JSON.parse(columnDataStr);

            var getUrl = "/" + modelName + "/" + "GetDataForGrid";
            var changeColumnConfig = function(e) {
                scope.changeColumnConfigEvent = e;
                scope.$apply("changeColumnConfig()");
            };

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
                schema: {
                    model: {
                        id: "Id",
                        fields: schemaFields
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

            scope.mainGridOptions = {
                dataSource: dataSource,
                toolbar: kendo.template($(customHeaderTemplate).html()),
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
                height: $(window).height() -121,
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
                //columnMenu: attrs.showColumnMenu,
                dataBound: function (e) {
                    getGridConfig();
                    //if (dataSource.total() == 0) {
                    //    $(".k-pager-wrap").hide();
                    //}
                    scope.$emit("gridDataboundEvent", { gridId: scope.modelName });
                    $('#dropdown-export a').on('click', function (event) {
                        event.stopPropagation();
                    });
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