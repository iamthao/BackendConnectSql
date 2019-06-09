'use strict';
app.controller('requestDetailTrackingController', ['$rootScope', '$scope', 'uiGmapGoogleMapApi', 'common', 'masterfileService', '$interval', 'messageLanguage', '$window', function ($rootScope, $scope, uiGmapGoogleMapApi, common, masterfileService, $interval, messageLanguage, $window) {
    var controllerId = "requestDetailTrackingController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);

    function TrackingViewModel() {
        var self = this;
        self.Datepicker = '03/23/2016';
        self.Courier = 2;
    }
    activate();
    var index = 2;
    var bounds = null;

    function activate() {
        $("#tracking-map .angular-google-map-container").css({ height: $(window).height() - 300 });
        
    }

    $scope.Tracking = new TrackingViewModel();
    $scope.Tracking.Datepicker = kendo.toString(new Date(), "MM/dd/yyyy");
    $scope.IsRequestFinished = false;
    $scope.IdRequestSelected = 0;
    $scope.iconMarkEndPoint = 'FileUpload/googleMapIcons/start-end/icon-end.png';
    $scope.iconMarkEndPointRunning = 'FileUpload/googleMapIcons/start-end/mylocation.gif';

    var map = null;
    $scope.initCenter = {
        latitude: 29.764143,
        longitude: -95.362839
    };

    $scope.initZoom = 14;

    $scope.markStart = {
        latitude: 0,
        longitude: 0
    };

    $scope.markEnd = {
        latitude: 0,
        longitude: 0
    };
    $scope.Marker = [];
    $scope.TimeTrackingEndPoint = "N/A";
    $scope.VelocityEndPoint = 0;
    $scope.TimeTrackingStartPoint = "N/A";
    $scope.VelocityStartPoint = 0;
    $scope.Polys = [{
        "id": 1,
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

    
    $scope.SelectCourier = function (id) {
        $scope.IdRequestSelected = id;
        getMapData(id, true);
        if (intervalObject != null) {
            $interval.cancel(intervalObject);
        }
        intervalObject = $interval(function () {
            getMapData(id, false);
            $scope.BindTrackingList();
        }, 10000);
    };
    function initData() {
        var requestItem = $('#popupWindow').data('RequestItem');
        $scope.Tracking.Courier = requestItem.CourierId;
        $scope.Tracking.Datepicker = requestItem.Time;
        $scope.SelectCourier(requestItem.Id);
    }

    initData();

    var dereg = $rootScope.$on('$stateChangeSuccess', function () {
        if (intervalObject != null) {
            $interval.cancel(intervalObject);
        }
        dereg();
    });

    //$scope.$watch("Tracking.Datepicker", function (newValue, oldValue) {
    //    if (newValue !== undefined && newValue != '' && newValue != oldValue) {
    //        $scope.CourierName = $('#Courier').data('kendoComboBox').text();
    //        $scope.Tracking.Courier = 0;
    //        $scope.$broadcast("Courier_Change", [{ KeyId: 0, DisplayName: "Select All" }]);
    //        $scope.RequestList = [];
    //        $scope.BindTrackingList();
    //    }

    //    $scope.$broadcast("Courier_ChangeFilterCondition", { Key: 'TrackingDate', Value: newValue });
    //});

    //$scope.$watch("Tracking.Courier", function (newValue, oldValue) {
    //    if (newValue >= 0) {
    //        if (newValue != '' || newValue == 0) {
    //            $scope.CourierName = $('#Courier').data('kendoComboBox').text();
    //            $scope.BindTrackingList();
    //        } else {
    //            $scope.CourierName = '';
    //            $scope.RequestList = [];
    //        }
    //        $(".courier-tracking input").css({ "color": "#000", "font-style": "normal" });
    //    }
    //});

    function getMapData(id, firstTime) {
        if ($scope.Tracking.Datepicker != '' && $scope.Tracking.Datepicker != undefined && $scope.Tracking.Courier != undefined && id != undefined) {
            var url = '/Tracking/GetListTrackingData';
            masterfileService.callWithUrl(url).perform({ courierId: $scope.Tracking.Courier, filterDateTime: $scope.Tracking.Datepicker, requestId: id }).$promise.then(function (data) {
                if (data != null && (data.Error === undefined || data.Error === '')) {

                    drawDataFromGoogle(data);

                    drawDataFromMobile(data);

                    bounds = resetPointAndMarker();

                    $scope.Map.Bounds = bounds;

                    if (map == null) {
                        map = $scope.Map.control.getGMap();
                    }

                    if (firstTime === true) {
                        $scope.initCenter = {
                            latitude: bounds.getCenter().lat(),
                            longitude: bounds.getCenter().lng()
                        };
                        $scope.initZoom = common.getBoundsZoomLevel(bounds, map);
                    }
                    else {
                        $scope.initZoom = map.getZoom();
                    }

                    $scope.setGoogleMapLocation();
                }
                $('#loading-request-detail').hide();
            });
        }
    }

    function drawDataFromGoogle(data) {
        //draw direction from google
        var arr = [];
        var point = {
            "id": index,
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
        index++;

        if (data.Direction != undefined && data.Direction.length > 0) {
            $scope.markStartPointDirection =
            {
                latitude: data.Direction[0].Latitude,
                longitude: data.Direction[0].Longitude
            };
            $scope.markEndPointDirection =
            {
                latitude: data.Direction[data.Direction.length - 1].Latitude,
                longitude: data.Direction[data.Direction.length - 1].Longitude
            };
        } else {
            $scope.markStartPointDirection =
            {
                latitude: data.DataDirectionPoints[0].Latitude,
                longitude: data.DataDirectionPoints[0].Longitude
            };
            $scope.markEndPointDirection =
            {
                latitude: data.DataDirectionPoints[1].Latitude,
                longitude: data.DataDirectionPoints[1].Longitude
            };
        }
    }

    function drawDataFromMobile(data) {
        //draw real data from mobile
        var arrayMarkerPoint = [];
        _.each(data.Data, function (obj) {
            _.each(obj.LocationList, function (p, idx) {
                var iconImage = 'FileUpload/googleMapIcons/line/line.png';
                var isShow = false;
                if (idx == obj.LocationList.length - 1 && obj.LocationList.length > 0) {
                    iconImage = 'FileUpload/googleMapIcons/dec/' + obj.IconDirectionName;
                    isShow = true;
                }
                arrayMarkerPoint.push(
                    {
                        Id: index,
                        Coords: {
                            latitude: p.Latitude,
                            longitude: p.Longitude
                        },
                        Options: {
                            draggable: false,
                            icon: iconImage,
                        },
                        ShowWindow: isShow,
                        Distance: obj.Distance,
                        TimeTracking: obj.TimeTracking,
                        Velocity: obj.Velocity,
                        IsShowPoint: !(p.Latitude == 0 && p.Longitude == 0)
                    }
                );
                index++;
            });
        });

        $scope.Marker = arrayMarkerPoint;

        if (data.Data != null && data.Data.length > 0 && data.Data[0].LocationList != null && data.Data[0].LocationList.length > 0) {
            var lengths = data.Data.length - 1;
            var length = data.Data[lengths].LocationList.length - 1;
            $scope.markStartPoint =
            {
                latitude: data.Data[0].LocationList[0].Latitude,
                longitude: data.Data[0].LocationList[0].Longitude
            };
            $scope.markEndPoint =
            {
                latitude: data.Data[lengths].LocationList[length].Latitude,
                longitude: data.Data[lengths].LocationList[length].Longitude
            };


            $scope.TimeTrackingEndPoint = data.Data[lengths].TimeTracking;
            $scope.VelocityEndPoint = data.Data[lengths].Velocity;

            $scope.TimeTrackingStartPoint = data.Data[0].TimeTracking;
            $scope.VelocityStartPoint = data.Data[0].Velocity;
            $scope.IsRequestFinished = data.Data[lengths].IsActiveRequest;
        } else {
            $scope.markStartPoint =
            {
                latitude: 0,
                longitude: 0
            };
            $scope.markEndPoint =
            {
                latitude: 0,
                longitude: 0
            };
            $scope.TimeTrackingEndPoint = "N/A";
            $scope.VelocityEndPoint = 0;
            $scope.TimeTrackingStartPoint = "N/A";
            $scope.VelocityStartPoint = 0;
        }

    }

    function resetPointAndMarker() {
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < $scope.Marker.length; i++) {
            if ($scope.Marker[i].Coords.latitude != 0 && $scope.Marker[i].Coords.longitude != 0) {
                var latlng = new google.maps.LatLng($scope.Marker[i].Coords.latitude, $scope.Marker[i].Coords.longitude);
                bounds.extend(latlng);
            }
        }
        if ($scope.markStartPointDirection != undefined) {
            bounds.extend(new google.maps.LatLng($scope.markStartPointDirection.latitude, $scope.markStartPointDirection.longitude));
        }
        if ($scope.markEndPointDirection != undefined) {
            bounds.extend(new google.maps.LatLng($scope.markEndPointDirection.latitude, $scope.markEndPointDirection.longitude));
        }


        return bounds;
    }

    $scope.BindTrackingList = function () {
        if ($scope.Tracking.Datepicker != '' && $scope.Tracking.Datepicker != undefined && $scope.Tracking.Courier != undefined) {
            var url = '/Tracking/GetListTrackingData';
            masterfileService.callWithUrl(url).perform({ courierId: $scope.Tracking.Courier, filterDateTime: $scope.Tracking.Datepicker }).$promise.then(function (data) {
                if (data.Error === undefined || data.Error === '') {
                    var uniqueData = _.uniq(data.Data, function (d) {
                        return d.RequestId;
                    });
                    $scope.RequestList = uniqueData;
                }
                
            });
        }
    };

    $scope.setGoogleMapLocation = function () {
        $scope.Map = {
            control: {},
            Center: $scope.initCenter,
            Zoom: $scope.initZoom,
            Polys: $scope.Polys,
        };

        if ($scope.markStartPoint != undefined && $scope.markStartPoint.latitude == 0 && $scope.markStartPoint.longitude == 0) {
            $scope.MarkerStart = [{
                Id: $scope.controllerId + '_Start',
                Coords: $scope.markStartPoint,
                Options: {
                    draggable: false,
                    icon: 'FileUpload/googleMapIcons/start-end/icon-start.png',
                },
                IsShow: false,
                TimeTracking: $scope.TimeTrackingStartPoint,
                Velocity: $scope.VelocityStartPoint
            }];
        } else {
            $scope.MarkerStart = [{
                Id: $scope.controllerId + '_Start',
                Coords: $scope.markStartPoint,
                Options: {
                    draggable: false,
                    icon: 'FileUpload/googleMapIcons/start-end/icon-start.png',
                },
                IsShow: true,
                TimeTracking: $scope.TimeTrackingStartPoint,
                Velocity: $scope.VelocityStartPoint
            }];
        }


        if ($scope.markEndPoint != undefined && $scope.markEndPoint.latitude == 0 && $scope.markEndPoint.longitude == 0) {
            $scope.MarkerEnd = [{
                Id: $scope.controllerId + '_End',
                Coords: $scope.markEndPoint,
                Options: {
                    draggable: false,
                    optimized: false,
                    icon: $scope.IsRequestFinished == false ? $scope.iconMarkEndPoint : $scope.iconMarkEndPointRunning,
                },
                IsShow: false,
                TimeTracking: $scope.TimeTrackingEndPoint,
                Velocity: $scope.VelocityEndPoint
            }];
        } else {
            $scope.MarkerEnd = [{
                Id: $scope.controllerId + '_End',
                Coords: $scope.markEndPoint,
                Options: {
                    draggable: false,
                    optimized: false,
                    icon: $scope.IsRequestFinished == false ? $scope.iconMarkEndPoint : $scope.iconMarkEndPointRunning
                },
                IsShow: true,
                TimeTracking: $scope.TimeTrackingEndPoint,
                Velocity: $scope.VelocityEndPoint
            }];
        }
        $scope.MarkerStartDirection = {
            Id: $scope.controllerId + '_StartDirection',
            Coords: $scope.markStartPointDirection,
            Options: {
                draggable: false,
                icon: 'FileUpload/googleMapIcons/start-end/icon-start-google.png',
            }
        };
        $scope.MarkerEndDirection = {
            Id: $scope.controllerId + '_EndDirection',
            Coords: $scope.markEndPointDirection,
            Options: {
                draggable: false,
                icon: 'FileUpload/googleMapIcons/start-end/icon-end-google.png',
            }
        };

    };

    uiGmapGoogleMapApi.then(function (maps) {
        maps.visualRefresh = true;
        $scope.setGoogleMapLocation();
    });
    
}]);