'use strict';
app.controller('gridController', ['$rootScope', '$scope', '$http', 'common', 'masterfileService', function ($rootScope, $scope, $http, common, masterfileService) {
    var controllerId = "gridController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    $scope.mainGridOptions = {};
    $scope.changeColumnConfigEvent = {};
    $scope.changeColumnPositionEvent = {};
    $scope.userId = 0;
    $scope.documentTypeId = 0;
    $scope.gridInternalName = '';
    $scope.modelName = '';
    $scope.columns = [];
    $scope.viewColumnData = [];
    $scope.idGrid = 0;
    $scope.gridConfigViewModel = {};

    $scope.changeColumnConfig = function () {
        $scope.gridConfigViewModel.changeColumnConfig($scope.changeColumnConfigEvent.column);
        saveGridConfig();
    };

    $scope.changeColumnPosition = function () {
        var gridId = "#" + $scope.modelName;
        var grid = $(gridId).data("kendoGrid");
        var direction = 1;
        var start = $scope.changeColumnPositionEvent.newIndex;
        var end = $scope.changeColumnPositionEvent.oldIndex;
        if ($scope.changeColumnPositionEvent.oldIndex < $scope.changeColumnPositionEvent.newIndex) {
            direction = -1;
            start = $scope.changeColumnPositionEvent.oldIndex;
            end = $scope.changeColumnPositionEvent.newIndex;
        }
        //re-order the column in range of effect
        for (var j = start; j <= end; j++) {
            var column = setColumnIndex(grid.columns[j], j + 1 * direction);
            $scope.gridConfigViewModel.changeColumnOrder(column);
        }
        //update order of the changed column
        var changedColumn = setColumnIndex($scope.changeColumnPositionEvent.column, $scope.changeColumnPositionEvent.newIndex);
        $scope.gridConfigViewModel.changeColumnOrder(changedColumn);
        saveGridConfig();
    };

    function setColumnIndex(column, index) {
        column.index = index;
        return column;
    }

    function saveGridConfig() {
        var gridConfig = angular.toJson($scope.gridConfigViewModel);
        //Save grid's config
        $http.post('/GridConfig/Save', gridConfig)
            .success(function (result) {
                if (result.Error === undefined || result.Error === '') {
                    $scope.gridConfigViewModel["Id"] = result.Data.Id;
                }
            }).error(function (error) {
            });

    }

    $scope.getGridConfig = function () {
        var data = { Id: $scope.idGrid, ViewColumns: $scope.viewColumnData };
        var widthOfCommandColumn = 90;
        var widthOfCheckboxColumn = 30;
        var gridId = '#' + $scope.modelName;
        if (_.isNull(data.ViewColumns) || _.isUndefined(data.ViewColumns)) {
            $scope.columns.forEach(function (column) {
                $scope.gridConfigViewModel.addColumn(column);
            });
            return;
        }
        $scope.gridConfigViewModel.Id = data.Id;
        $scope.gridConfigViewModel.importColumnConfigs(data.ViewColumns);

        //Processing Grid Columns Config
        var totalWidth = 0;
        $scope.columns.forEach(function (column) {

            if (_.isString(column.width))
                column.width = parseInt(column.width.replace("px", ""));

            var columnConfig = $scope.gridConfigViewModel.findViewColumn(column);
            //Get column config
            if (!_.isUndefined(columnConfig) && !_.isNull(columnConfig)) {
                column.hidden = columnConfig.HideColumn;

                if (columnConfig.ColumnWidth > 0) {
                    if (!_.isUndefined(column.field) && column.field.trim() == "IsChecked") {
                        column.width = widthOfCheckboxColumn;
                    } else if (!_.isUndefined(column.command)) {
                        column.width = widthOfCommandColumn;
                    } else {
                        column.width = columnConfig.ColumnWidth;
                    }
                }
            } else {

                column.hidden = false;
                $scope.gridConfigViewModel.addColumn(column);
            }

            if (_.isUndefined(column.hidden) || !column.hidden) {
                totalWidth += column.width;
            }
        });

        //sort column order
        $scope.columns = _.sortBy($scope.columns, function (column) { return $scope.gridConfigViewModel.findViewColumnIndex(column); });

        //work around extend the last column to fit with grid width if total columns width less than grid width
        //prevent auto ajust column
        var gridWidth = $(gridId).width() - 22; //remove width of vertical scrollbar
        if (totalWidth < gridWidth) {
            var visibleColumns = _.where($scope.columns, { hidden: false });
            var lastColumn = _.last(visibleColumns);
            if (!_.isUndefined(lastColumn.command)) {
                lastColumn.width = widthOfCommandColumn;
                // Caculate width the pre column
                var prePosition = _.size(visibleColumns) - 2;
                var preColumn = visibleColumns[prePosition];
                if (prePosition != 0) {
                    preColumn.width += (gridWidth - totalWidth);
                }
            } else {
                lastColumn.width += (gridWidth - totalWidth);
            }

        }

        var mandatoryFields = $scope.gridConfigViewModel.getMandatoryColumns();
        mandatoryFields.forEach(function (field) {
            $(gridId + ">.k-grid-header table>thead").find("[data-field='" + field.Name + "']>.k-header-column-menu").remove();
        });
        // Prevent drag and drop for some mandatory field
        //$("th:nth(0)", gridId).data("kendoDropTarget").destroy();
        $(gridId).data("kendoDraggable").bind("dragstart", function (e) {

            mandatoryFields.forEach(function (field) {
                if (_.isUndefined(e.currentTarget.attr('data-field')) || e.currentTarget.attr('data-field') === "" || e.currentTarget.attr('data-field') === field.Name) {

                    e.preventDefault();
                    return;
                }
            });
        });
    };

    $scope.Delete = function (id) {
        common.bootboxConfirm("Are you sure that you want to delete this record?", function () {
            masterfileService.deleteById($scope.modelName).perform({ id: id }).$promise.then(function (result) {
                if (result.Error === undefined || result.Error === '') {
                    var logSuccess = getLogFn(controllerId, "success");
                    if ($scope.$parent.deleteMessage != undefined) {
                        logSuccess($scope.$parent.deleteMessage);
                    } else {
                        logSuccess('Delete ' + $scope.modelName.toLowerCase() + ' successfully');
                    }
                    $("#" + $scope.modelName).data("kendoGrid").dataSource.read();
                }
            });
        }, function () { }).modal('show');
       
    };
    

    $scope.PopupViewModel = new PopupViewModel();

    $scope.Add = function () {
        var title = "Create " + $scope.modelName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); });
        if ($scope.modelName == 'Courier') {
            title = 'Create ' + $rootScope.CourierDisplayName;
        }
        var height = $scope.PopupViewModel.PopupHeight;
        if ($scope.modelName == 'Location') {
            height = $(window).height() - 100;
        }

        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: $(window).width() > $scope.PopupViewModel.PopupWidth ? $scope.PopupViewModel.PopupWidth + "px" : $(window).width() + "px",
            height: height + "px",
            title: title,
            content: {
                url: "/" + $scope.modelName + "/Create"
            },
            animation: false
        });
        popup.open();
    };

    $scope.Edit = function(id) {
        var title = "Update " + $scope.modelName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); });
        if ($scope.modelName == 'Courier') {
            title = 'Update ' + $rootScope.CourierDisplayName;
        }
        var height = $scope.PopupViewModel.PopupHeight;
        if ($scope.modelName == 'Location') {
            height = $(window).height() - 100;
        }
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: $(window).width() > $scope.PopupViewModel.PopupWidth ? $scope.PopupViewModel.PopupWidth + "px" : $(window).width() + "px",
            height: height + "px",
            title: title,
            content: {
                url: "/" + $scope.modelName + "/Update/" + id
            },
            animation: false
        });
        popup.open();
    };



    $scope.ResetPassword = function(id) {
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: $(window).width() > 500 ?  "500px" : $(window).width() + "px",
            height: "180px",
            title: "Change Password " + $scope.modelName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); }),
            content: {
                url: "/" + $scope.modelName + "/ResetPassword/" + id
            },
            animation: false
        });
        popup.open();
    };
    $scope.ExportToExcel = function() {
        var gridId = "#" + $scope.modelName;
        var grid = $(gridId).data("kendoGrid");
        var gridColumnsConfig = _.filter(grid.columns, function(obj) {
            return obj.field != "Command" && typeof obj.template != "function";
        });

        var gridColumns = _.map(gridColumnsConfig, function(obj) {
            return { Field: obj.field, Title: obj.title };
        });

        var sort = grid.dataSource.sort();

        var searchString = $scope.SearchText;
        searchString = "<SearchTerms>" + Encoder.htmlEncode(searchString) + "</SearchTerms>";
        searchString = Base64.encode('<AdvancedQueryParameters>' + searchString + '</AdvancedQueryParameters>');

        var total = grid.dataSource.total();
        var selected = $("#dropdown-export input[type='radio']:checked");
        if (selected.length > 0) {
            if(parseInt(selected.val())>0) {
                total = parseInt(selected.val());
            }
        }


        var queryInfo = {
            SearchString: searchString,
            Sort: sort,
            Take: total
        };

        var exportUrl = "/" + $scope.modelName + "/" + "ExportExcel";
        var downloadExcelUrl = "/" + $scope.modelName + "/" + "DownloadExcelFile";

        $http.post(exportUrl, { queryInfo: queryInfo, gridColumns: gridColumns }).success(function (result) {
            $.fileDownload(downloadExcelUrl, {
                httpMethod: "POST",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                data: { 'fileName': result.FileNameResult }
            });
        }).error(function (result) {
        });
    };
    
    
    //$scope.$on("ReloadGridWhenSearch", function (event, args) {
    //    var searchText = args.SearchText;
    //    searchText = "<SearchTerms>" + Encoder.htmlEncode(searchText) + "</SearchTerms>";
    //    searchText = Base64.encode('<AdvancedQueryParameters>' + searchText + '</AdvancedQueryParameters>');
    //    scope.SearchTextEncoded = searchText;
    //    dataSource.read();
       
    //});

    //scope.$on("ReloadGrid", function () {
    //    dataSource.read();

    $scope.$on("ReloadGrid", function () {
        //dataSource.read();        
        $("#" + $scope.modelName).data("kendoGrid").dataSource.read();
    });
    
    $scope.Search = function($event) {
        if ($event != null) {
            var currentKey = $event.which || $event.charCode;
            if (currentKey === 13) {
                $scope.$broadcast("ReloadGridWhenSearch", { SearchText: $scope.SearchText });
            }
        } else {
            $scope.$broadcast("ReloadGridWhenSearch", { SearchText: $scope.SearchText });
        }
    };

}]);