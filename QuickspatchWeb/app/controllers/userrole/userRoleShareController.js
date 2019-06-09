'use strict';
app.controller('userRoleShareController', ['$scope', function ($scope) {
    $scope.controllerId = "userRoleShareController";

    $scope.IsGlobalAdmin = false;
    $scope.shareInit = function (isGlobalAdmin) {
        $scope.IsGlobalAdmin = isGlobalAdmin;        
        $scope.mainGridOptions = {
            dataSource: dataSource,
            columns: columns,
            scrollable: { virtual: true },
            editable: !($scope.IsGlobalAdmin),
            dataBound: onDataBound,
            height: 275
        };
    }

    var schemaFields = {
        Id: { editable: false },
        Name: { editable: false },
        IsView: { type: "boolean", editable: true },
        IsInsert: { type: "boolean", editable: true },
        IsUpdate: { type: "boolean", editable: true },
        IsDelete: { type: "boolean", editable: true },
        IsProcess: { type: "boolean", editable: true }
    };

    var columns = [
        { field: "Name", title: "Name", attributes: { style: 'text-align:Left;' }, hidden: false },
        { field: "IsView", title: "View", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsView){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsInsert", title: "Insert", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsInsert){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsUpdate", title: "Update", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsUpdate){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsDelete", title: "Delete", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsDelete){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' },
        { field: "IsProcess", title: "Process", attributes: { style: 'text-align:Left;' }, width: 100, hidden: false, template: '<span #if(IsProcess){# class="fa fa-check" #}else{#class="fa fa-times"#}#></span>' }
    ];

    var url = $scope.urlToGetDataForUserRoleGrid;
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: url,
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
        table: "#userRoleGrid",
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
 


    $scope.UserRole = new UserRoleViewModel();

    $scope.getShareViewData = function () {
        $scope.UserRole.UserRoleFunctionData = JSON.stringify(dataSource.data());
        return { SharedParameter: JSON.stringify($scope.UserRole) };
    }
    function onDataBound(arg) {
        $(".k-grid td").each(function () {
            if ($(this).text() == "true") {
                $(this).css({ "color": "green" });
            }
        });
    }

    function changeValueInGrid(e) {
        if (e.action !== undefined) {
            $scope.UserRole.CheckAll = false;
            $("#userRoleGrid").parents('form').addClass('dirty');
            EnableCreateFooterButton(true);
        }
    }

    function getUnsaveDataInGrid() {
        return dataSource.GetUnsavedData();
    }

    $scope.CheckAll = function () {
        if ($scope.UserRole.CheckAll) {
            var checkedAllData = _.map(dataSource.data(), function (obj) {
                return { Id: obj.Id, Name: obj.Name, IsView: true, IsInsert: true, IsUpdate: true, IsDelete: true, IsProcess: true };
            });
            $("#userRoleGrid").data("kendoGrid").dataSource.data(checkedAllData);
        } else {
            $("#userRoleGrid").data("kendoGrid").dataSource.read();
        }
    }
}]);