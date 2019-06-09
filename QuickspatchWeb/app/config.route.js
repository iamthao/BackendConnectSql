(function () {
    'use strict';
    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getLeftMenu());
    // Configure the routes and route resolvers
    app.config(['$stateProvider', '$urlRouterProvider', routeConfigurator]);

    function routeConfigurator($stateProvider, $urlRouterProvider) {
        $stateProvider.state('dashboard', {
            url: "/dashboard",
            views: {
                "main": {
                    templateUrl: '/Dashboard/Index',
                    title: 'Dashboard',
                    settings: {
                        nav: 1
                    }
                }
            }
        }).state('user', {
            url: "/user",
            views: {
                "main": {
                    templateUrl: '/User/Index',
                    title: 'Manage User',
                    settings: {
                        nav: 6,
                    }
                }
            }
        }).state('module', {
            url: "/module",
            views: {
                "main": {
                    templateUrl: '/Module/Index',
                    title: 'Manage Module',
                    settings: {
                        nav: 6,
                    }
                }
            }
        }).state('franchisee', {
            url: "/franchisee",
            views: {
                "main": {
                    templateUrl: '/FranchiseeTenant/Index',
                    title: 'Manage Franchisee',
                    settings: {
                        nav: 6,
                    }
                }
            }
        }).state('franchiseeModule', {
            url: "/franchiseeModule?id",
            views: {
                "main": {
                    templateUrl: function ($stateParams) {
                        return '/FranchiseeModule/Update/' + $stateParams.id;
                    },
                    title: 'Manage Franchisee Module',
                    settings: {
                        nav: 6,
                    }
                }
            }
        }).state('userrole', {
            url: "/role",
            views: {
                "main": {
                    templateUrl: '/UserRole/Index',
                    title: 'Manage Role',
                    settings: {
                        nav: 6,
                    }
                }
            }
        }).state('request', {
            url: "/request",
            views: {
                "main": {
                    templateUrl: '/Request/Index',
                    title: 'Request',
                    settings: {
                        nav: 2,
                    }
                }
            }
        }).state('schedule', {
            url: "/schedule",
            views: {
                "main": {
                    templateUrl: '/Schedule/Index',
                    title: 'Schedule',
                    settings: {
                        nav: 3,
                    }
                }
            }
        }).state('tracking', {
            url: "/tracking",
            views: {
                "main": {
                    templateUrl: '/Tracking/Index',
                    title: 'Tracking',
                    settings: {
                        nav: 4,
                    }
                }
            }
        }).state('location', {
            url: "/location",
            views: {
                "main": {
                    templateUrl: '/Location/Index',
                    title: 'Locations',
                    settings: {
                        nav: 6,
                    }
                }
            }
        }).state('mobileusers', {
            url: "/mobileusers",
            views: {
                "main": {
                    templateUrl: '/Courier/Index',
                    title: 'Mobile Users',
                    settings: {
                        nav: 6,
                    },
                }
            }
        }).state('template', {
            url: "/template",
            views: {
                "main": {
                    templateUrl: '/Template/Index',
                    title: 'Template',
                    settings: {
                        nav: 6,
                    },
                }
            }
        }).state('systemconfiguration', {
            url: "/systemconfiguration",
            views: {
                "main": {
                    templateUrl: '/SystemConfiguration/Index',
                    title: 'System Configuration',
                    settings: {
                        nav: 6,
                    },
                }
            }
        }).state('profile', {
            url: "/profile",
            views: {
                "main": {
                    templateUrl: '/User/UserProfile',
                    title: 'User Profile',
                    settings: {
                        nav: 7,
                    }
                }
            }
            //}).state('franchiseeconfiguration', {
            //    url: "/franchiseeconfiguration",
            //    views: {
            //        "main": {
            //            templateUrl: '/FranchiseeConfiguration/index',
            //            title: 'Franchisee Configuration',
            //            settings: {
            //                nav: 8,
            //            }
            //        }
            //    }
        }).state('franchiseeconfiguration', {
            url: "/franchiseeconfiguration?tabIndex",
            views: {
                "main": {
                    templateUrl: function ($stateParams) {
                        var link = '/FranchiseeConfiguration/Index';
                        if ($stateParams.tabIndex == null || $stateParams.tabIndex == undefined) {
                            return link + "?tabIndex=" + $stateParams.tabIndex;
                        }
                        else {
                            return link + "?tabIndex=" + $stateParams.tabIndex;
                        }
                    },
                    title: 'Franchisee Configuration',
                    settings: {
                        nav: 8,
                    }
                }
            }
        }).state('welcome', {
            url: "/welcome",
            views: {
                "main": {
                    templateUrl: '/welcome/index',
                    title: 'Wellcom to QuickSpatch',
                    settings: {
                        nav: 9,
                    }
                }
            }
        }).state('report', {
            url: "/report",
            views: {
                "main": {
                    templateUrl: '/report/index',
                    title: 'Report',
                    settings: {
                        nav: 5,
                    }
                }
            }
        });

        //$urlRouterProvider.otherwise("/dashboard");
        // use this to change default state based on type of site & user
        $urlRouterProvider.otherwise(function ($injector, $location) {
            var $rootScope = $injector.get('$rootScope');
            var $state = $injector.get('$state');
            if ($rootScope.IsQuickTour == 'true') {
                location.href = "/welcome";
                //$state.go("wellcome", { location: false });
            } else {
                if (getCookie("IsCamino").toString() == 'true') {
                    $state.go("franchisee", { location: false });
                } else {
                    $state.go("dashboard", { location: false });
                }
            }
        });
    }

    // Define the routes 

    function getLeftMenu() {
        return [{
            url: '/dashboard',
            config: {
                title: 'Dashboard',
                settings: {
                    nav: 1,
                    content: 'Dashboard'
                }
            }
        },
            {
                url: '/request',
                config: {
                    title: 'Request',
                    settings: {
                        nav: 2,
                        content: 'Request'
                    }
                }
            },
            {
                url: '/schedule',
                config: {
                    title: 'Schedule',
                    settings: {
                        nav: 3,
                        content: 'Schedule'
                    }
                }
            },
            {
                url: '/tracking',
                config: {
                    title: 'Tracking',
                    settings: {
                        nav: 4,
                        content: 'Tracking'
                    }
                }
            },
            {
                url: '',
                config: {
                    title: 'Administration',
                    settings: {
                        nav: 6,
                        content: 'Administration <span class="caret"></span>',
                        children: [{
                            url: '/franchisee',
                            config: {
                                title: 'Franchisee Setup',
                                settings: {
                                    nav: 6,
                                    content: 'Franchisee Setup',
                                }
                            }
                        },
                        {
                            url: '/module',
                            config: {
                                title: 'Module Setup',
                                settings: {
                                    nav: 6,
                                    content: 'Module Setup',
                                }
                            }
                        },
                        {
                            url: '/role',
                            config: {
                                title: 'Role Management',
                                settings: {
                                    nav: 6,
                                    content: 'Role Management',
                                }
                            }
                        },
                        {
                            url: '/user',
                            config: {
                                title: 'User Management',
                                settings: {
                                    nav: 6,
                                    content: 'Web User Management',
                                }
                            }
                        },
                        {

                            url: '/mobileusers',
                            config: {
                                title: 'Mobile Users',
                                settings: {
                                    nav: 6,
                                    content: 'Mobile User Management',
                                }
                            }
                        },
                        {
                            url: '/location',
                            config: {
                                title: 'Locations',
                                settings: {
                                    nav: 6,
                                    content: 'Locations',
                                }
                            }
                        },
                        //{
                        //    url: '/template',
                        //    config: {
                        //        title: 'Template',
                        //        settings: {
                        //            nav: 6,
                        //            content: 'Template',
                        //        }
                        //    }
                        //},
                        {
                            url: '/systemconfiguration',
                            config: {
                                title: 'System Configuration',
                                settings: {
                                    nav: 6,
                                    content: 'System Configuration',
                                }
                            }
                        }
                        ]
                    }
                }
            },
            {
                url: '/report',
                config: {
                    title: 'Report',
                    settings: {
                        nav: 5,
                        content: 'Report'
                    }
                }

            }];
    }
})();