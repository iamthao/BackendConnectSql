'use strict';
app.controller('moduleDocumentTypeOperationShareController', ['$scope', function ($scope) {
    $scope.controllerId = "moduleDocumentTypeOperationShareController";

    $scope.IsGlobalAdmin = true;
    var dataSource = [];
    
    $scope.shareInit = function (moduleId) {
        $scope.urlToGetDataForFranchiseeModuleGrid = '/ModuleDocumentTypeOperation/GetDataForModuleGrid/?moduleId=' + moduleId;
        
        dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: $scope.urlToGetDataForFranchiseeModuleGrid,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                },
                parameterMap: function (options, operation) {
                    if (operation == "read") {
                        // Limitation of nested json object; temporarily have to modify the json to
                        // pass in sort information correctly.
                        var result = {
                            pageSize: options.pageSize,
                            skip: options.skip,
                            take: options.take
                        };

                        return result;
                    }
                }
            },
            batch: true,
            emptyMsg: 'No Record',
            table: "#moduleGrid",
            change: changeValueInGrid,
            schema: {
                model: {
                    id: "Id",
                    fields: schemaFields
                },
                data: "Data",
                total: "TotalRowCount"
            }
        });
        
        $scope.mainGridOptions = {
            dataSource: dataSource,
            columns: columns,
            scrollable: { virtual: true },
            editable: $scope.IsGlobalAdmin,
            dataBound: onDataBound,
            height: 230
        };
    };
    
    var schemaFields = {
        Id: { editable: false },
        DocumentTypeName: { editable: false },
        IsView: { type: "boolean", editable: true },
        IsInsert: { type: "boolean", editable: true },
        IsUpdate: { type: "boolean", editable: true },
        IsDelete: { type: "boolean", editable: true },
        IsProcess: { type: "boolean", editable: true }
    };

    var columns = [
        { field: "DocumentTypeName", title: "Name", attributes: { style: 'text-align:Left;' }, hidden: false },
        { field: "IsView", title: "View", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsView){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsInsert", title: "Insert", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsInsert){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsUpdate", title: "Update", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsUpdate){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsDelete", title: "Delete", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsDelete){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsProcess", title: "Process", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsProcess){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' }
    ];
    
    $scope.Module = new ModuleViewModel();

    $scope.getShareViewData = function() {
        return {
            SharedParameter: JSON.stringify($scope.Module),
            ModuleOperationData: JSON.stringify(dataSource.data())
        };
    };
    
    function onDataBound(arg) {
        $(".k-grid td").each(function () {
            if ($(this).text() == "true") {
                $(this).css({ "color": "green" });
            }
        });
    }

    function changeValueInGrid(e) {
        if (e.action !== undefined) {
            $scope.Module.CheckAll = false;
            $("#moduleGrid").parents('form').addClass('dirty');
            EnableCreateFooterButton(true);
        }
    }

    function getUnsaveDataInGrid() {
        return dataSource.GetUnsavedData();
    }

    $scope.CheckAll = function () {
        if ($scope.Module.CheckAll) {
            var checkedAllData = _.map(dataSource.data(), function(obj) {
                return { Id: obj.Id, DocumentTypeName: obj.DocumentTypeName, IsView: true, IsInsert: true, IsUpdate: true, IsDelete: true, IsProcess: true };
            });
            $("#moduleGrid").data("kendoGrid").dataSource.data(checkedAllData);
        } else {
            $("#moduleGrid").data("kendoGrid").dataSource.read();
        }
    };
}]);