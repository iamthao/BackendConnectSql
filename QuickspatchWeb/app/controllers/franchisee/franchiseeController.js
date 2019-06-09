'use strict';
app.controller('franchiseeController', ['$rootScope', '$scope', 'common', 'messageLanguage', 'masterfileService',
    function ($rootScope, $scope, common, messageLanguage, masterfileService) {
    var controllerId = "franchiseeController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    activate();

    function activate() {
       // common.activateController(null, controllerId).then(function () { log(messageLanguage.listFranchisee); });
    }

    $scope.$on("gridDataboundEvent", function(event, args) {
        var gridId = "#" + args.gridId;
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
    
    $scope.EditModule = function(id) {
        var popup = $("#popupWindow").data("kendoWindow");
        popup.setOptions({
            width: $scope.PopupViewModel.PopupWidth + "px",
            height: $scope.PopupViewModel.PopupHeight + "px",
            title: "Update franchisee module",
            content: {
                url: "/FranchiseeModule/Update/" + id
            },
            animation: false
        });
        popup.open();
    };
    
    $scope.Deactivate = function (id) {
        common.bootboxConfirm("Are you sure that you want to deactivate this franchisee?", function () {
            masterfileService.callWithUrl('/FranchiseeTenant/DeactivateFranchisee').perform({ id: id }).$promise.then(function (data) {
                if (data.Error === undefined || data.Error === '') {
                    $scope.$root.$broadcast("ReloadGrid");
                    var logSuccess = getLogFn(controllerId, "success");
                    logSuccess('Deactivate franchisee successfully');
                }
            });
        }, function () { }).modal('show');
    };
        
    $scope.Activate = function (id) {
        common.bootboxConfirm("Are you sure that you want to activate this franchisee?", function () {
            masterfileService.callWithUrl('/FranchiseeTenant/ActivateFranchisee').perform({ id: id }).$promise.then(function (data) {
                if (data.Error === undefined || data.Error === '') {
                    $scope.$root.$broadcast("ReloadGrid");
                    var logSuccess = getLogFn(controllerId, "success");
                    logSuccess('Activate franchisee successfully');
                }
            });
        }, function () { }).modal('show');
    };
}]);