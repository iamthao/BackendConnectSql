(function () {
    'use strict';

    var controllerId = 'sidebar';
    angular.module('app').controller(controllerId,
        ['$rootScope', '$scope', '$state', 'config', 'routes', '$sce', sidebar]);

    function sidebar($rootScope, $scope, $state, config, routes, $sce) {
        var vm = this;

        $scope.listMenu = {};
        vm.isCurrent = isCurrent;
        vm.isVisible = isVisible;
        vm.classDropdownMenu = classDropdownMenu;

        $scope.init = function(listMenu) {
            $scope.listMenu = JSON.parse(listMenu);
        };
        
        activate();
        $scope.createRoute = function (route) {
            if (route.url == undefined || route.url == "") {
                return $sce.trustAsHtml("Javascript:void(0);");
            } else {
                return "#" + route.url;
            }
        }
        function activate() { getNavRoutes(); }

        function getNavRoutes() {
            vm.navRoutes = routes.filter(function (r) {
                return r.config.settings && r.config.settings.nav;
            }).sort(function (r1, r2) {
                return r1.config.settings.nav - r2.config.settings.nav;
            });
        }

        function isCurrent(route,len) {
            if (!route.config.title || !$state.current || !$state.current.views || !$state.current.views["main"] || !$state.current.views["main"].title) {
                return '';
            }
            var menuId = route.config.settings.nav;
            $rootScope.Title = $state.current.views["main"].title == "Couriers" ? $rootScope.CourierDisplayName : $state.current.views["main"].title;
            return $state.current.views["main"].settings.nav === menuId ? 'active' + (len > 0 ? " dropdown" : "") : (len > 0 ? "dropdown" : "");
        }

        function classDropdownMenu(numberChild) {
            if (numberChild > 0) {
                return "material dropdown-toggle";
            }
            return "material";
        }

        function isVisible(route) {
            //user
            var isShowUser = $scope.listMenu.CanViewUserSetup;
            if (route.config.title == "User Management") {
                return isShowUser;
            }
            
            //user role
            var isShowUserRole = $scope.listMenu.CanViewUserRoleSetup;

            if (route.config.title == "Role Management") {
                return isShowUserRole;
            }
            
            //module
            var isShowModuleManage = $scope.listMenu.CanViewModuleSetup;

            if (route.config.title == "Module Setup") {
                return isShowModuleManage;
            }
            
            //franchisee
            var isShowFranchiseeManage = $scope.listMenu.CanViewFranchiseeSetup;

            if (route.config.title == "Franchisee Setup") {
                return isShowFranchiseeManage;
            }
            
            //user
            var isShowDashboard = $scope.listMenu.CanViewDashboard;
            if (route.config.title == "Dashboard") {
                return isShowDashboard;
            }
            
            //request
            var isShowRequest = $scope.listMenu.CanViewRequest;

            if (route.config.title == "Request") {
                return isShowRequest;
            }
            
            //schedule
            var isShowSchedule = $scope.listMenu.CanViewSchedule;

            if (route.config.title == "Schedule") {
                return isShowSchedule;
            }
            
            //tracking
            var isShowCustomer = $scope.listMenu.CanViewTracking;

            if (route.config.title == "Tracking") {
                return isShowCustomer;
            }
            
            //location
            var isShowLocation = $scope.listMenu.CanViewLocation;

            if (route.config.title == "Locations") {
                return isShowLocation;
            }

            //Courier
            var isShowCourier = $scope.listMenu.CanViewCourier;

            if (route.config.title == "Mobile Users" || route.config.title == "Report" || route.config.title == "Template" || route.config.title == "System Configuration") {
                return isShowCourier;
            }

            //Administration
            var isShowAdmin = $scope.listMenu.CanViewUserSetup || $scope.listMenu.CanViewUserRoleSetup
                                || $scope.listMenu.CanViewModuleSetup || $scope.listMenu.CanViewFranchiseeSetup
            || $scope.listMenu.CanViewCourier || $scope.listMenu.CanViewLocation;

            if (route.config.title == "Administration") {
                return isShowAdmin;
            }

            return true;
        }
    };
})();
