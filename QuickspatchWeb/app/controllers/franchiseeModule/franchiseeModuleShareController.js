'use strict';
app.controller('franchiseeModuleShareController', ['$scope', function ($scope) {
    $scope.controllerId = "franchiseeModuleShareController";

    $scope.IsGlobalAdmin = true;
    var dataSource = [];
    
    $scope.shareInit = function (franchiseeId) {
        $scope.urlToGetDataForFranchiseeModuleGrid = '/FranchiseeModule/GetDataForModuleGrid/?franchiseeId=' + franchiseeId;
        
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
            table: "#franchiseeModuleGrid",
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
            height: 235
        };
    };
    
    var schemaFields = {
        Id: { editable: false },
        ModuleName: { editable: false },
        IsActive: { type: "boolean", editable: true }
    };

    var columns = [
        { field: "ModuleName", title: "Name", attributes: { style: 'text-align:Left;' }, hidden: false },
        { field: "IsActive", title: "Active", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsActive){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' }
    ];
    
    $scope.FranchiseeModule = new FranchiseeModuleViewModel();

    $scope.getShareViewData = function() {
        return {
            SharedParameter: JSON.stringify($scope.FranchiseeModule),
            ModuleData: JSON.stringify(dataSource.data())
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
            $scope.FranchiseeModule.CheckAll = false;
            $("#franchiseeModuleGrid").parents('form').addClass('dirty');
            EnableCreateFooterButton(true);
        }
    }

    function getUnsaveDataInGrid() {
        return dataSource.GetUnsavedData();
    }

    $scope.CheckAll = function () {
        if ($scope.FranchiseeModule.CheckAll) {
            var checkedAllData = _.map(dataSource.data(), function(obj) {
                return { Id: obj.Id, ModuleName: obj.ModuleName, IsActive: true };
            });
            $("#franchiseeModuleGrid").data("kendoGrid").dataSource.data(checkedAllData);
        } else {
            $("#franchiseeModuleGrid").data("kendoGrid").dataSource.read();
        }
    };
}]);