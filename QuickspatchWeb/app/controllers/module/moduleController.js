'use strict';
app.controller('moduleController', ['$rootScope', '$scope', 'common', 'messageLanguage', function ($rootScope, $scope, common, messageLanguage) {
    var controllerId = "moduleController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    activate();

    function activate() {
        //common.activateController(null, controllerId).then(function () { log(messageLanguage.listModule); });
    }

    $scope.$on("gridDataboundEvent", function(event, args) {
        var gridId = "#"+args.gridId;
        var dataSource = $(gridId).data("kendoGrid").dataSource;
        $( gridId + ">.k-grid-content tbody tr").each(function () {
            var $tr = $(this);
            var uid = $tr.attr("data-uid");
            var dataEntry;
            $.each(dataSource._data, function (index, item) {
                if (item.uid === uid) {
                    dataEntry = item;
                }
            });
            if (dataEntry != undefined && dataEntry.Id != undefined) {
                if (dataEntry.IsActive == false) {
                    $tr.addClass('gray');
                }
            }

        });
    });
    
    $scope.PopupViewModel = new PopupViewModel();

    $scope.SetupPermission = function (id) {
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width:  "800px",
            height: "400px",
            title: "Set permission for module",
            content: {
                url: "/ModuleDocumentTypeOperation/Update/" + id
            },
            animation: false
        });
        popup.open();
    };
}]);