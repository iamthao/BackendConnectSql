'use strict';
app.controller('courierOnlineController', ['$rootScope', '$scope', 'common', 'uiGmapGoogleMapApi', 'messageLanguage', '$window', 'masterfileService','$timeout',
    function ($rootScope, $scope, common, uiGmapGoogleMapApi, messageLanguage, $window, masterfileService, $timeout) {
        $scope.controllerId = "courierOnlineController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            $(".angular-google-map-container").css({ height: ($(window).height() - 178) / 2 });
            // common.activateController(null, controllerId).then(function () { log(messageLanguage.listdashboard); });
        }
        $scope.initCenter = {
            latitude: 29.764143,
            longitude: -95.362839
        };

        $scope.initZoom = 14;

        $scope.Marker = [];
        $scope.index = 1;
        $scope.isViewAll = false;
        var map = null;
        //$scope.Map = {
        //    Center: $scope.initCenter,
        //    Zoom: $scope.initZoom
        //};

        $scope.setGoogleMapLocation = function () {
            $scope.index++;
            $scope.Map = {
                control: {},
                Center: $scope.initCenter,
                Zoom: $scope.initZoom,
            };
            $scope.Marker = [];
            //$scope.Marker = [{
            //    Id: $scope.index,
            //    Coords: $scope.initCenter,
            //    Options: {
            //        draggable: false,
            //        labelContent: '',
            //    }
            //}];
        };

        $scope.$on("Dashboard_Courier_Changed", function (event, val) {
            if (val.clickCourier) {
                $scope.isViewAll = false;
            }
            if ($scope.isViewAll == false) {
                if ((val.lat == 0 && val.lng == 0) || (val.lat == undefined && val.lng == undefined)) {
                    $scope.Marker = [];
                } else {
                    if (val.avatar == null || val.avatar == '') {
                        val.avatar = "/content/quickspatch/img/icon-courier.png";
                    }
                    var avatar = new google.maps.MarkerImage(
                        "/content/quickspatch/img/icon-courier.png", //url
                        new google.maps.Size(32, 39), //size
                        new google.maps.Point(0, 0), //origin
                        new google.maps.Point(32, 19), //anchor 
                        new google.maps.Size(32, 39)
                    );
                    $scope.index++;
                    $scope.Map.Center = {
                        latitude: val.lat,
                        longitude: val.lng
                    };
                    $scope.Map.Zoom = 16;
                    $scope.Marker = [
                        {
                            Id: $scope.index,
                            Coords: {
                                latitude: val.lat,
                                longitude: val.lng
                            },
                            Options: {
                                draggable: false,
                                //labelContent: val.fullName,
                                icon: avatar
                            },
                            Click: function() {
                                $scope.Show = true;
                            },
                            FullName: val.fullName,
                            RequestNo: val.requestNo,
                            CurrentVelocity: val.currentVelocity,
                            Avatar: val.avatar,
                            Show: true
                        }
                    ];
                }
            } else {
                $scope.loadAll();
            }

            //$scope.$apply();

        });

        $scope.timeout = null;
        $scope.RefreshMapCOurierOnline = function () {
            $timeout.cancel($scope.timeout);
            $scope.loadAll();
        };


        $scope.loadAll = function () {
            $scope.isViewAll = true;
            $scope.Marker = [];

            masterfileService.callWithUrl('/Courier/GetAllCourierOnlineLocation').perform().$promise.then(function (data) {
                if (data.Error === undefined || data.Error === '') {
                    if (data.TotalRowCount == 0) {
                        $scope.Marker = [];
                    } else {
                        var arr = [];
                        _.each(data.Data, function (obj, index) {
                            if (obj.AvatarImage == null || obj.AvatarImage == '') {
                                obj.AvatarImage = "/content/quickspatch/img/icon-courier.png";
                            }
                            var avatar = new google.maps.MarkerImage(
                                   "/content/quickspatch/img/icon-courier.png", //url
                               new google.maps.Size(32, 39), //size
                               new google.maps.Point(0, 0), //origin
                               new google.maps.Point(32, 19), //anchor 
                               new google.maps.Size(32, 39)
                            );
                            if (obj.Lat != 0 && obj.Lng != 0) {
                                var marker = {
                                    Id: $scope.index,
                                    Coords: {
                                        latitude: obj.Lat,
                                        longitude: obj.Lng
                                    },
                                    Options: {
                                        draggable: false,
                                        //labelContent: obj.FullName,
                                        icon: avatar
                                    },
                                    Click: function () {
                                        $scope.Show = true;
                                    },
                                    FullName: obj.FullName,
                                    RequestNo: obj.CurrentRequestNo,
                                    CurrentVelocity: obj.CurrentVelocity,
                                    Avatar: obj.AvatarImage,
                                    Show: false
                                };
                                arr.push(marker);
                            }
                            

                            $scope.index++;
                        });
                        $scope.Marker = arr;

                        var bounds = new google.maps.LatLngBounds();
                        for (var i = 0; i < $scope.Marker.length; i++) {
                            var latlng = new google.maps.LatLng($scope.Marker[i].Coords.latitude, $scope.Marker[i].Coords.longitude);
                            bounds.extend(latlng);
                        }
                        $scope.Map.Bounds = bounds;
                        $scope.Map.Center = {
                            latitude: bounds.getCenter().lat(),
                            longitude: bounds.getCenter().lng()
                        };

                        if (map == null) {
                            map = $scope.Map.control.getGMap();
                        }

                        if ($scope.Marker.length == 1) {
                            $scope.Map.Zoom = 15;
                        } else {
                            $scope.Map.Zoom = common.getBoundsZoomLevel(bounds, map);
                        }

                    }

                    $scope.$emit("LoadCourierOnlineDone");
                }
                $scope.timeout = $timeout(function () { $scope.RefreshMapCOurierOnline(); }, 15000);
            });
        };

        uiGmapGoogleMapApi.then(function (maps) {
            maps.visualRefresh = true;
            $scope.setGoogleMapLocation();
        });
    }]);