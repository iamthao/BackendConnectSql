'use strict';
app.controller('MapRequestController', ['$q','$rootScope', '$scope', 'common', 'uiGmapGoogleMapApi', 'messageLanguage', '$window', 'masterfileService', '$timeout',
    function ($q,$rootScope, $scope, common, uiGmapGoogleMapApi, messageLanguage, $window, masterfileService, $timeout) {
        $scope.controllerId = 'MapRequestController';
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        activate();

        function activate() {
            // common.activateController(null, controllerId).then(function () { log(messageLanguage.listdashboard); });
            $timeout(function () {
                $scope.Map.show = true;
                $("#create-request-courier-map div.angular-google-map-container").css({ height: '300px' });
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
        $scope.showMarkerStart = true;
        $scope.showMarkerEnd = true;

        function getTextMiles(i) {
            return Math.round(i * 0.000621371192) + " miles";
        }
        function getTextHour(i) {
            var h = parseInt(i / (60 * 60));
            var m = (h * 60) - parseInt(i / 60);
            if (h > 0) {
                return parseInt(h) + " hour " + Math.abs(parseInt(m)) + " minutes ";
            } else {
                return Math.abs(parseInt(m)) + " minutes";
            }
        }
        

        $scope.distanceAndDurationCurrent = {
            distance: 0,
            duration: 0,
            distanceMiles: '',
            durationHour: ''
        };
        $scope.getDistance = function(latlngA, latlngB) {
            var deferred = $q.defer();
            var directionsService = new google.maps.DirectionsService();
            var request = {
                origin: latlngA.latitude + ',' + latlngA.longitude,
                destination: latlngB.latitude + ',' + latlngB.longitude,
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                var condition = status == google.maps.DirectionsStatus.OK;
                condition &= response.routes.length > 0;
                condition &= response.routes[0].legs != undefined && response.routes[0].legs != null;
                condition &= response.routes[0].legs.length > 0;
                condition &= response.routes[0].legs[0].distance != undefined && response.routes[0].legs[0].distance != null;
                condition &= response.routes[0].legs[0].duration != undefined && response.routes[0].legs[0].duration != null;
                if ( condition) {
                    $scope.distanceAndDurationCurrent.distance = response.routes[0].legs[0].distance.value;
                    $scope.distanceAndDurationCurrent.duration = response.routes[0].legs[0].duration.value;
                    $scope.distanceAndDurationCurrent.distanceMiles = getTextMiles(response.routes[0].legs[0].distance.value);
                    $scope.distanceAndDurationCurrent.durationHour = getTextHour(response.routes[0].legs[0].duration.value);
                    deferred.resolve($scope.distanceAndDurationCurrent);
                }
            });
            return deferred.promise;
        }
        $scope.showWindowEnd = false;
        $scope.setGoogleMapLocation = function () {
            $scope.index++;
            $scope.Map = {
                control: {},
                Center: $scope.initCenter,
                Zoom: $scope.initZoom,
                Polys: $scope.Polys,
                show: true,
            };
            
            $scope.showMarkerStart = $scope.MarkerStartPoint.latitude != undefined;
            
            $scope.MarkerStartDirection = {
                Id: $scope.controllerId + '_StartDirection_' + $scope.index,
                Coords: $scope.MarkerStartPoint,
                Options: {
                    draggable: false,
                    icon: 'FileUpload/googleMapIcons/start-end/icon-start-google.png',
                }
            };
            $scope.showMarkerEnd = $scope.MarkerEndPoint.latitude != undefined;
            $scope.MarkerEndDirection = {
                Id: $scope.controllerId + '_EndDirection_' + $scope.index,
                Coords: $scope.MarkerEndPoint,
                Options: {
                    draggable: false,
                    icon: 'FileUpload/googleMapIcons/start-end/icon-end-google.png',
                }
            };
            if ($scope.showMarkerEnd && $scope.showMarkerStart) {
                $scope.getDistance($scope.MarkerStartPoint, $scope.MarkerEndPoint).then(function(result) {
                    $scope.showWindowEnd = true;
                });
            }
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
        $scope.$parent.$watch(function() {
            return { LocationFrom: $scope.$parent.data.LocationFrom, LocationTo: $scope.$parent.data.LocationTo }
        }, function (nval, oval) {
            if (nval.LocationFrom != oval.LocationFrom || nval.LocationTo != oval.LocationTo) {
                var fromId = nval.LocationFrom;
                var toId = nval.LocationTo;
                getPoint(fromId, toId);
            }
        }, true);

        function getPoint(fromId, toId) {
            if (fromId != 0 || toId != 0) {
                var url = '/Request/GetListTrackingDataFromTo';
                masterfileService.callWithUrl(url)
                    .perform({ fromId: fromId, toId: toId })
                    .$promise.then(function (data) {
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
            }
            if (data.ToLocation != null) {
                lat = data.ToLocation.Lat;
                lng = data.ToLocation.Lng;
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
            $scope.index++;
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

        $scope.currentEndWindowOptions = {
            visible: false,
            
        };

        $scope.currentEndOnClick = function () {
            if ($scope.showMarkerEnd && $scope.showMarkerStart) {
                $scope.windowOptions.visible = !$scope.windowOptions.visible;
            } else {
                $scope.windowOptions.visible = false;
            }
              
        };

        $scope.currentEndCloseClick = function () {
            $scope.windowOptions.visible = false;
        };
        $scope.windowcss = 'window-text-color-yellow';
        $scope.title = "Window Title!";

        $scope.PolyOtherRequests = [];


        $scope.getDistanceForPoly = function (latlngA, latlngB) {
            var deferred = $q.defer();
            var directionsService = new google.maps.DirectionsService();
            var request = {
                origin: latlngA.latitude + ',' + latlngA.longitude,
                destination: latlngB.latitude + ',' + latlngB.longitude,
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                var condition = status == google.maps.DirectionsStatus.OK;
                if (condition) {
                    var paths = [];
                    _.each(response.routes[0].overview_path, function (obj) {
                        var path = {
                            "latitude": obj.lat(),
                            "longitude": obj.lng()
                        };
                        paths.push(path);
                    });
                    var objPoint = getPointOtherPoly(paths);
                    deferred.resolve(objPoint);
                } else {
                    deferred.resolve(null);
                }
            });
            return deferred.promise;
        }

        function getPointOtherPoly(paths) {
            //draw direction from google
            $scope.index++;
            var point = {
                "id": $scope.index,
                "path": paths,
                "stroke": {
                    "color": "#56647F",
                    "weight": 3
                },
                "editable": false,
                "draggable": false,
                "geodesic": true,
                "visible": true
            };
            return point;
        }


        //$scope.$parent.$watch(function () {
        //    return $scope.$parent.data.CourierId;
        //}, function (nval, oval) {
        //    if (nval > 0) {
        //        $timeout(function () {
        //            var dataGrid = $('#requests-courier-grid').data('kendoGrid').dataSource.data();
        //            var indexProcess = 0;
        //            for (var i = 0; i < dataGrid.length; i++) {
        //                indexProcess ++;
        //                if (dataGrid[i].RequestId != $scope.$parent.data.Id && dataGrid[i].LocationFromLng != null) {
        //                    var objFrom = { latitude: dataGrid[i].LocationFromLat, longitude: dataGrid[i].LocationFromLng };
        //                    var objTo = { latitude: dataGrid[i].LocationToLat, longitude: dataGrid[i].LocationToLng };
        //                    $scope.getDistanceForPoly(objFrom, objTo).then(function (result) {
        //                        $scope.Polys.push(result);
        //                        $scope.setGoogleMapLocation();
        //                    });
        //                }
        //            }
                    
        //        }, 200);
        //    }
        //}, true);
    }]);