'use strict';
app.controller('courierController', ['$rootScope', '$scope', 'common', 'messageLanguage', function ($rootScope, $scope, common, messageLanguage) {
    var controllerId = "courierController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    activate();

    function activate() {
       // common.activateController(null, controllerId).then(function () { log('Show list ' + $rootScope.CourierDisplayName); });
    }


    $scope.CleanImei = function (id) {
        common.bootboxConfirm("Are you sure that you want to Force Sign Out this " + $rootScope.CourierDisplayName + "?", function () {
            $.ajax({
                method: "POST",
                url: "/Courier/CleanImei",
                data: { id: id }
            })
               .done(function (result) {
                   if (result == true) {
                       var logSuccess = getLogFn(controllerId, "success");
                       logSuccess('Force Sign Out successfully');
                       $scope.$root.$broadcast("ReloadGrid");
                   }
               });
        }, function () { }).modal('show');

    };

    $scope.$on("gridDataboundEvent", function (event, args) {
        var gridId = "#" + args.gridId;
        var dataSource = $(gridId).data("kendoGrid").dataSource;
        $(gridId + ">.k-grid-content tbody tr").each(function () {
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
}]);