'use strict';
var app =
    angular.module('app', [
            'ui.router', // routing
            //'ngAnimate', // animations
            'ngSanitize', // sanitizes html bindings (ex: sidebar.js)
            'ngResource', // REST support
            // Custom modules
            'common', // common functions, logger, spinner
            'common.bootstrap', // bootstrap dialog wrapper functions
            'ui.bootstrap', // 3rd Party Modules
            'kendo.directives',// kendo ui in angular]
            'uiGmapgoogle-maps',
            'bm.bsTour', 
            'ngRoute',
            'angular-timeline',

    ], function ($locationProvider, $httpProvider) {

        var interceptor = [
            '$rootScope', '$q', 'common', function (scope, $q, common) {
                function success(response) {
                    var resultData = response.data;
                    if (resultData != undefined && resultData.Error != undefined && resultData.Error != "" ) {
                        common.showErrorModelState(new FeedbackViewModel(resultData.Status, resultData.Error, resultData.StackTrace, resultData.ModelStateErrors));
                        if (typeof EnableCreateFooterButton === 'function') {
                            EnableCreateFooterButton(true);
                        }
                    }
                    return response;
                }
                function error(response) {
                   
                    var getLogFn = common.logger.getLogFn;
                    var logError = getLogFn("", "error");
                    var status = response.status;
                    if (status == 403) {
                        location.reload();
                    } else if (status == 404) {
                        logError("404 not found");
                    } else if (status == 0) {
                    } else {
                        if (response != "") {
                            logError(response);
                        }
                        
                    } 
                    return $q.reject(response);
                }
                return function (promise) {
                    return promise.then(success, error);
                }
            }
        ];
        $httpProvider.responseInterceptors.push(interceptor);
    });

// Global variable
app.constant('GoogleApiJavaScriptKey', 'AIzaSyA6mmsPUCsMeNmhOkYkd0KVIlaSUySVJNc');

// Handle routing errors and success events
app.run(['$rootScope', '$templateCache', 'common', '$http', 'displayCourierService', 'displayAlertExtended', '$interval',
    function ($rootScope, $templateCache, common, $http, displayCourierService, displayAlertExtended, $interval) {
    // Include $route to kick start the router.
    $rootScope.$on('$viewContentLoaded', function () {
        $templateCache.removeAll();
    });
    $rootScope
        .$on('$stateChangeStart',
            function (event, toState, toParams, fromState, fromParams) {
                //console.log('$stateChangeStart');
            });

    $rootScope
        .$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                $rootScope.randomTimestamp = common.randomNumber();
                $rootScope.IsCamino = getCookie("IsCamino").toString();
                
                //console.log('$stateChangeSuccess');
            });
    //$rootScope.$on('$stateChangeSuccess',
    //    function(event, toState, toParams, fromState, fromParams) { $state.reload(); });
    
    //displayCourierService.getDisplayLabel().perform().$promise.then(function (data) {
        $rootScope.CourierDisplayName = "Mobile User";//data.data;
    //});

    displayAlertExtended.getDisplayAlertExtended().perform().$promise.then(function (data) {        
        $rootScope.IsShowAlertExtended = data.data;
        $rootScope.NumberExtended = data.totaldDay;
        //console.log(data.data)
        if (data.data == true) {
            $("#alert-extended").css({ display: "block" });
        }
        
    });
   
    
}]);

app.config(['$httpProvider', '$compileProvider', 'uiGmapGoogleMapApiProvider', 'GoogleApiJavaScriptKey',
    function ($httpProvider, $compileProvider, googleMapApi, googleApiJavaScriptKey) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|javascript):/);
        googleMapApi.configure({
            key: googleApiJavaScriptKey,
            v: '3.17',
            libraries: 'weather,geometry,visualization,places'
        });
    }]);
app.config(['$routeProvider', 'TourConfigProvider', function ($routeProvider, TourConfigProvider) {

    $routeProvider
        .when('/', {
            templateUrl: '/',
            controller: function ($scope) {
                $scope.viewName = 'dashboard';
            }
        })
        .when('/#/request', {
            templateUrl: '/#/request',
            controller: function ($scope) {
                $scope.viewName = 'request';
            }
        })
        .when('/#/user', {
            templateUrl: '/#/user',
            controller: function ($scope) {
                $scope.viewName = 'user';
            }
        })
        .when('/#/location', {
            templateUrl: '/#/location',
            controller: function ($scope) {
                $scope.viewName = 'location';
            }
        })
        .when('/#/courier', {
            templateUrl: '/#/courier',
            controller: function ($scope) {
                $scope.viewName = 'courier';
            }
        })
        .when('/#/schedule', {
            templateUrl: '/#/schedule',
            controller: function ($scope) {
                $scope.viewName = 'schedule';
            }
        })
        .when('/#/tracking', {
            templateUrl: '/#/tracking',
            controller: function ($scope) {
                $scope.viewName = 'tracking';
            }
        })
        .otherwise({
            redirectTo: '/'
        });

    //These are defaults
    TourConfigProvider.set('prefixOptions', false);
    TourConfigProvider.set('prefix', 'bsTour');

}]);

//Nghiep implement to remove interval;
var intervalObject = null;