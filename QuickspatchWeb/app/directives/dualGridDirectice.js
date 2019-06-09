'use strict';
app.directive('dualCustomGrid', ['$http','common', function ($http,common) {
    return {
        restrict: "E",
        require: 'ngModel',
        templateUrl: function (element, attrs) {
            return '/app/views/dualCustomGridTemplate.html?t='+common.randomNumber();
        },
        scope: {
            allUrl: '=gridAllUrl',
            selectedUrl: '=gridSelectedUrl',
            gridColumns: '=gridDualColumns',
            gridSchema: '=gridDualSchema',
            DualGridSelectedText: '@dualGridSelectedText',
            DualGridAllText: '@dualGridAllText',
            listSelectedId: '=ngModel',
            height:'@height'
        },
        link: function (scope, element, attrs) {
            if (scope.height == undefined || scope.height == "") {
                scope.height = 360;
            }
            scope.fullHeight = parseInt(scope.height) + 40;
            scope.listSelectedId = [];
            scope.DualGridSearchString = "";
            scope.DualGridSelectedSearchString = "";
            // Add column checkbox
            var gridSelectColumn = [];
            var gridAllColumn = [{ field: "IsChecked", title: "&nbsp;", attributes: { style: 'text-align:center;' }, width: "20px", sortable: false, filterable: false, menu: false, template: "<input type='checkbox'/>" }];

            _.each(scope.gridColumns, function (item) {
                gridAllColumn.push(item);
                gridSelectColumn.push(item);
            });
            var gridAllSchema = scope.gridSchema;
            gridAllSchema.model.fields["IsChecked"] = false;
            var gridSelectSchema = scope.gridSchema;

            var mainGridAllDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: scope.allUrl,
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation == "read") {

                            // Limitation of nested json object; temporarily have to modify the json to
                            // pass in sort information correctly.
                            var searchText = "<SearchTerms>" + Encoder.htmlEncode(scope.DualGridSearchString) + "</SearchTerms>";
                            searchText = Base64.encode('<AdvancedQueryParameters>' + searchText + '</AdvancedQueryParameters>');
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                SearchString: searchText
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
                schema: scope.gridSchema
            });
            // add checcbox column
            var mainGridSelectedDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: scope.selectedUrl,
                        type: "GET"
                    },
                },
                serverPaging: false,
                serverSorting: false,
                pageSize: 50,
                batch: true,
                emptyMsg: 'No Record',
                schema: scope.gridSchema,
                change: function (e) {
                    if (scope.listSelectedId.length <= 0) {
                        setDataForSelectedItem();
                    }
                }
            });
            scope.dualGridAll = {
                dataSource: mainGridAllDataSource,
                pageable: false,
                sortable: true,
                height: scope.height,
                columns: scope.gridColumns,
                editable: false,
                scrollable: { virtual: true },
                navigatable: false,
                resizable: true,
                reorderable: true,
                selectable: "multiple row",
            };

            scope.dualGridSelected = {
                dataSource: mainGridSelectedDataSource,
                pageable: false,
                sortable: true,
                height: scope.height,
                columns: scope.gridColumns,
                editable: false,
                scrollable: { virtual: true },
                navigatable: false,
                resizable: true,
                reorderable: true,
                selectable: "multiple row",
            };
            
            scope.DualGridKeyPressEnterSearch = function (keyEvent) {
                if (keyEvent.keyCode == 13) {
                    $("#grid-dual-all").data('kendoGrid').dataSource.read();
                }
                return true;
            };
            scope.DualGridClickSearchString = function() {
                $("#grid-dual-all").data('kendoGrid').dataSource.read();
            };

            function searchInDualGridSelected() {
                if (scope.DualGridSelectedSearchString == "") {
                    $("#grid-dual-selected").data("kendoGrid").dataSource.filter({
                        logic: "or",
                        filters: []
                    });
                } else {
                    // Get list filter
                    var filters = [];
                    _.each(scope.gridColumns, function(item) {
                        var itemAdd = {operator: "contains",value   : scope.DualGridSelectedSearchString};
                        itemAdd.field = item.field;
                        filters.push(itemAdd);
                    });
                    $("#grid-dual-selected").data("kendoGrid").dataSource.filter({
                        logic: "or",
                        filters: filters
                    });
                }
            }
            scope.DualGridSelectedKeyPressEnterSearch = function (keyEvent) {
                if (keyEvent.keyCode == 13) {
                    searchInDualGridSelected();
                }
                return true;
            };
            scope.DualGridSelectedClickSearchString = function () {
                searchInDualGridSelected();
            };

            scope.DualGridMoveSelected = function () {
                $('a[data-function="FOOTER_ACTION_SAVE"]').removeAttr('disabled');
                $('a[data-function="FOOTER_ACTION_SAVE"]').removeClass('k-state-disabled');
                // Get all items selected 
                var gridAll = $("#grid-dual-all").data("kendoGrid");
                var gridSelected = $("#grid-dual-selected").data("kendoGrid");
                var rows = gridAll.select();
                rows.each(function (index, row) {
                    var selectedItem = gridAll.dataItem(row);
                    // Check in the selected grid have selected item or not, if yes, add to grid
                    
                    var allSelectedData = gridSelected._data;
                    var checkExistsSelectedDataWithId = _.findWhere(allSelectedData, { Id: selectedItem.Id });
                    if (checkExistsSelectedDataWithId == undefined || checkExistsSelectedDataWithId.length==0) {
                        var dataSourceForGridSelected = gridSelected.dataSource;
                        dataSourceForGridSelected.add(selectedItem).dirty = true;
                    }
                    // selectedItem has EntityVersionId and the rest of your model
                });
                gridSelected.refresh();
                gridAll.clearSelection();
                setDataForSelectedItem();
            };
            scope.DualGridRemoveSelected = function () {
                $('a[data-function="FOOTER_ACTION_SAVE"]').removeAttr('disabled');
                $('a[data-function="FOOTER_ACTION_SAVE"]').removeClass('k-state-disabled');
                // Get all items selected 
                var gridSelected = $("#grid-dual-selected").data("kendoGrid");
                var rows = gridSelected.select();
                
                rows.each(function (index, row) {
                    gridSelected.removeRow(row);
                });
                gridSelected.refresh();
                setDataForSelectedItem();
            };

            function setDataForSelectedItem() {
                scope.listSelectedId = [];
                var gridSelectedData = $("#grid-dual-selected").data("kendoGrid").dataSource._data;
                _.each(gridSelectedData, function (item) {
                    scope.listSelectedId.push(item.Id);
                });
            }

            scope.DualGridRemoveAllSelected = function () {
                $('a[data-function="FOOTER_ACTION_SAVE"]').removeAttr('disabled');
                $('a[data-function="FOOTER_ACTION_SAVE"]').removeClass('k-state-disabled');
                // Get all items selected 
                var gridSelected = $("#grid-dual-selected").data("kendoGrid");
                gridSelected.dataSource.data([]);
                gridSelected.refresh();
                setDataForSelectedItem();
            };
        }
    }
}]);