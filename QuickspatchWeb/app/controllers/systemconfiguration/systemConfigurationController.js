'use strict';
app.controller('systemConfigurationController', ['$rootScope', '$scope', 'common', 'messageLanguage', function ($rootScope, $scope, common, messageLanguage) {
    var controllerId = "systemConfigurationController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);



    //grid
    var schemaFields = {
        Id: { editable: false },
    };
    $scope.PageSizeSelected = 20;
    $scope.DataSource = {};
    $scope.getDataSource = function () {
        $scope.DataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: "/SystemConfiguration/GetListSystemConfiguration",
                    type: "POST"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    } else if (operation == "read") {
                        $scope.PageSizeSelected = options.pageSize;
                        var result = {
                            pageSize: options.pageSize,
                            skip: options.skip,
                            take: options.take
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

    $scope.systemConfigurationGridOptions = {
        dataSource: $scope.getDataSource(),
        sortable: false,
        pageable: {
            refresh: true,
            pageSizes: [10, 20, 50, 100],
            buttonCount: 5
        },
        reorderable: true,
        resizable: true,
        height: $(window).height() - 121,

        columns: [
             {
                 field: "Name",
                 title: "Name",
                 width: "80px"

             },
            {
                field: "ValueNoFormat",
                title: "Value",
                width: "150px"

            },
            {
                field: "", width: "20px", template: kendo.template($("#withdrawButtons").html())
            }
        ],
    };

    //update
    $scope.Edit = function (id) {     
        var title = "Update System Configuration";      
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: "700px",
            height: "270px",
            title: title,
            content: {
                url: "/SystemConfiguration/Update/" + id
            },
            animation: false
        });
        popup.open();
    };

    $scope.refreshGrid = function() {
        $("#configuration-grid").data("kendoGrid").dataSource.read();
    }
}]);