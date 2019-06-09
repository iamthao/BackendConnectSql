'use strict';
app.controller('mapCreateRequestController', ['$rootScope', '$scope', 'common', 'uiGmapGoogleMapApi', 'messageLanguage', '$window', 'masterfileService', '$timeout',
    function ($rootScope, $scope, common, uiGmapGoogleMapApi, messageLanguage, $window, masterfileService, $timeout) {
        $scope.controllerId = "mapCreateRequestController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            // common.activateController(null, controllerId).then(function () { log(messageLanguage.listdashboard); });
            
            $timeout(function () {
                $scope.Map.show = false;
            }, 500);

        }
        $scope.initCenter = {
            latitude: 29.764143,
            longitude: -95.362839
        };

        $scope.initZoom = 12;

        $scope.Marker = [];
        $scope.index = 1;
        $scope.isViewAll = false;
        var map = null;
        $scope.MarkerStartDirection = {};
        $scope.MarkerStartPoint = {};
        $scope.MarkerEndDirection = {};
        $scope.MarkerEndPoint = {};

        $scope.$parent.$watch('ShowFormRequest', function(nval, oval) {
                $timeout(function () {
                    $scope.Map.show = nval;
                    $timeout(function () {
                        $("#create-request-courier-map div.angular-google-map-container").css({ height: '320px' });
                    });
                }, 500);
        });

        function getDefault() {
            $scope.index++;
            return [{
                "id": $scope.index,
                "path": [
                  {
                      "latitude": 29.764143,
                      "longitude": -95.362839
                  }
                ],
                "stroke": {
                    "color": "#6060FB",
                    "weight": 3
                },
                "editable": false,
                "draggable": false,
                "geodesic": true,
                "visible": true
            }];
        }

        $scope.Polys = getDefault();
        
        $scope.setGoogleMapLocation = function () {
            $scope.index++;
            $scope.Map = {
                control: {},
                Center: $scope.initCenter,
                Zoom: $scope.initZoom,
                Polys: $scope.Polys,
                show: true,
            };
            $scope.MarkerStartDirection = {
                Id: $scope.controllerId + '_StartDirection',
                Coords: $scope.MarkerStartPoint,
                Options: {
                    draggable: false,
                    icon: 'FileUpload/googleMapIcons/start-end/icon-start-google.png',
                }
            };

            $scope.MarkerEndDirection = {
                Id: $scope.controllerId + '_EndDirection',
                Coords: $scope.MarkerEndPoint,
                Options: {
                    draggable: false,
                    icon: 'FileUpload/googleMapIcons/start-end/icon-end-google.png',
                }
            };
           
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
                            Click: function () {
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

                        if ($scope.Marker.length == 1) {
                            $scope.Map.Zoom = 15;
                        } else {
                            $scope.Map.Zoom = common.getBoundsZoomLevel(bounds, map);
                        }

                    }

                    $scope.$emit("LoadCourierOnlineDone");
                }
            });
        };
        //
        $scope.$parent.$watchCollection('Request', function (nval, oval) {
            var fromId = nval.LocationFrom;
            var toId = nval.LocationTo;
            getPoint(fromId, toId);
        });

        function getPoint(fromId, toId) {
            if (fromId != 0 || toId != 0) {
                var url = '/Request/GetListTrackingDataFromTo';
                masterfileService.callWithUrl(url)
                    .perform({ fromId: fromId, toId: toId })
                    .$promise.then(function (data) {
                    //console.log(data);
                        markFromToGoogle(data);
                        drawDataFromGoogle(data);
                        $scope.setGoogleMapLocation();
                });
            } else {
                $scope.initCenter = {
                    latitude: 29.764143,
                    longitude: -95.362839
                };
                $scope.Polys = getDefault();
                $scope.MarkerStartPoint = {};
                $scope.MarkerEndPoint = {};

                $scope.setGoogleMapLocation();
            }
        }
        function markFromToGoogle(data) {
            var lat = 29.764143;
            var lng = -95.362839;
            if (data.FromLocation != null) {
                lat = data.FromLocation.Lat;
                lng = data.FromLocation.Lng;
            } else {
                if (data.ToLocation != null) {
                    lat = data.ToLocation.Lat;
                    lng = data.ToLocation.Lng;
                }
            }
            $scope.initCenter = {
                latitude: lat,
                longitude: lng
            };
            if (data.FromLocation != null) {
                $scope.MarkerStartPoint = {
                    latitude: data.FromLocation.Lat,
                    longitude: data.FromLocation.Lng
                };
            } else {
                $scope.MarkerStartPoint = {};
            }
            if (data.ToLocation != null) {
                $scope.MarkerEndPoint = {
                    latitude: data.ToLocation.Lat,
                    longitude: data.ToLocation.Lng
                };
            } else {
                $scope.MarkerEndPoint = {};
            }
        }
        function drawDataFromGoogle(data) {
            //draw direction from google
            $scope.index ++;
            var arr = [];
            var point = {
                "id": $scope.index,
                "path": [
                ],
                "stroke": {
                    "color": "#3F51B5",
                    "weight": 3
                },
                "editable": false,
                "draggable": false,
                "geodesic": true,
                "visible": true
            };
            _.each(data.Direction, function (obj) {
                var path = {
                    "latitude": obj.Latitude,
                    "longitude": obj.Longitude
                };
                point.path.push(path);
            });
            arr.push(point);
            $scope.Polys = arr;
            if (point.path.length > 0) {
                var indexMiddleLoation = parseInt(point.path.length / 2);
                $scope.initCenter = {
                    latitude: point.path[indexMiddleLoation].latitude,
                    longitude: point.path[indexMiddleLoation].longitude
                };
            }
        }
        
        uiGmapGoogleMapApi.then(function (maps) {
            maps.visualRefresh = true;
            $scope.setGoogleMapLocation();
        });
    }]);