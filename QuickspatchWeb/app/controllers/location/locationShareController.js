'use strict';
app.controller('locationShareController', ['$rootScope', '$scope', 'uiGmapGoogleMapApi', 'common', 'masterfileService', '$timeout','$interval',
    function ($rootScope, $scope, uiGmapGoogleMapApi, common, masterfileService, $timeout, $interval) {
        $scope.controllerId = "locationShareController";

        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn($scope.controllerId, "Error");

        var geocoder;

        $scope.Location = new LocationViewModel();
       
        var intrval = $interval(function () {
            var startCtr = $('#open-hour').data('kendoTimePicker');
            var endCtr = $('#close-hour').data('kendoTimePicker');
            var initControlOk = true;
            initControlOk &= startCtr != undefined;
            initControlOk &= endCtr != undefined;
            if (initControlOk) {
                $timeout(function () {
                    $scope.$apply(function () {
                    
                    });
                });
                startCtr.value($scope.Location.OpenHour != null && $scope.Location.OpenHour != '' ? moment($scope.Location.OpenHour, 'HH:mm a').format('h:mm A') : $scope.Location.OpenHour);              
                endCtr.value($scope.Location.CloseHour != null && $scope.Location.CloseHour != '' ? moment($scope.Location.CloseHour, 'HH:mm a').format('h:mm A') : $scope.Location.CloseHour);
               
                $timeout(function () {
                    $scope.$apply(function () {
                       
                    });
                });

                $interval.cancel(intrval);
           }
        }, 200);

        function convertUtcTime() {
            //Thao         
            //TH tra ve 02:03 PM
            if ($scope.Location.OpenHour.indexOf('AM') > 0 || $scope.Location.OpenHour.indexOf('PM') > 0) {
                var tempStart = $scope.Location.OpenHour.split(":");
                var hourAddStart = parseInt(tempStart[0]);
                var secondAddStart = parseInt(tempStart[1].split(" ")[0]);

                if (tempStart[1].split(" ")[1] == 'PM' && hourAddStart != 12) {
                    hourAddStart += 12;
                }
                if (tempStart[1].split(" ")[1] == 'AM' && hourAddStart == 12) {
                    hourAddStart = 0;
                }

                $scope.Location.OpenHour = (new Date((new Date()).setHours(hourAddStart, secondAddStart))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.Location.OpenHour != '' && $scope.Location.OpenHour != undefined) {
                var a = new Date(Date.parse($scope.Location.OpenHour));
                $scope.Location.OpenHour = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();
            }

            if ($scope.Location.CloseHour.indexOf('AM') > 0 || $scope.Location.CloseHour.indexOf('PM') > 0) {
                var tempStart1 = $scope.Location.CloseHour.split(":");
                var hourAddStart1 = parseInt(tempStart1[0]);
                var secondAddStart1 = parseInt(tempStart1[1].split(" ")[0]);

                if (tempStart1[1].split(" ")[1] == 'PM' && hourAddStart1 != 12) {
                    hourAddStart1 += 12;
                }
                if (tempStart1[1].split(" ")[1] == 'AM' && hourAddStart1 == 12) {
                    hourAddStart1 = 0;
                }

                $scope.Location.CloseHour = (new Date((new Date()).setHours(hourAddStart1, secondAddStart1))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.Location.CloseHour != '' && $scope.Location.CloseHour != undefined) {
                var a = new Date(Date.parse($scope.Location.CloseHour));
                $scope.Location.CloseHour = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();
            }
        }

        $scope.$parent.Type = $scope.Location.Type;
        $scope.timeout = null;
        $scope.$watch("Location.Zip", function (newValue, oldValue) {
           clearTimeout($scope.timeout);
            $scope.timeout = setTimeout(function () {
                if (newValue != '' && newValue != null && newValue!=oldValue) {
                    $scope.GetCityAndSate();
                }
                else {
                    if ((newValue == '' || newValue == null) && newValue != oldValue) {
                        $scope.Location.City = "";
                        $scope.Location.StateOrProvinceOrRegion = "";
                        $scope.$apply();
                    }
                   
                    //$scope.Location.IdCountryOrRegion = 233;
                    //dropdownlistCountry.select(233);
                    //$scope.$apply();
                }
            }, 1000);

        });
        $scope.$watch("Location.IdCountryOrRegion", function (newValue, oldValue) {
            if (newValue != '' && newValue != null && newValue!=oldValue) {
                $scope.Location.Zip = "";
                $scope.Location.City = "";
                $scope.Location.StateOrProvinceOrRegion = "";
            }
            //else {
            //    $scope.Location.City = String.empty;
            //    $scope.Location.StateOrProvinceOrRegion = String.empty;
            //    $scope.Location.IdCountryOrRegion = 233;
            //    dropdownlistCountry.select(233);
            //}
          
        });

        $scope.GetCityAndSate = function () {
            if ($scope.Location.AutoGetCityState == true) {
                masterfileService.callWithUrl('/Location/GetLocationFromZip').perform({ zip: $scope.Location.Zip, idcountry: $scope.Location.IdCountryOrRegion != null ? $scope.Location.IdCountryOrRegion : "" }).$promise.then(function (result) {
                    if (result && result.results.length > 0 && result.results[0].address_components.length > 0) {
                        for (var i = 0; i < result.results[0].address_components.length; i++) {
                            if (result.results[0].address_components[i].types.length > 0) {
                                for (var j = 0; j < result.results[0].address_components[i].types.length; j++) {
                                    if (result.results[0].address_components[i].types[j].indexOf("locality") >= 0) {
                                        $scope.Location.City = result.results[0].address_components[i].long_name;
                                    }
                                    if (result.results[0].address_components[i].types[j].indexOf("area") >= 0) {
                                        $scope.Location.StateOrProvinceOrRegion = result.results[0].address_components[i].long_name;
                                    }
                                    //if (result.results[0].address_components[i].types[j].indexOf("country") >= 0) {
                                    //    if (dropdownlistCountry != undefined && $scope.Location.IdCountryOrRegion <= 0) {
                                    //        dropdownlistCountry.search(result.results[0].address_components[i].long_name);
                                    //        $scope.Location.IdCountryOrRegion = dropdownlistCountry.value();
                                    //    }
                                    //}
                                }
                            }
                        }
                    } else {
                        $scope.Location.City = "";
                        $scope.Location.StateOrProvinceOrRegion = "";
                        //dropdownlistCountry.select(233);
                        //$scope.Location.IdCountryOrRegion = 233;
                    }
                });
            }
            else {
                $scope.Location.City = "";
                $scope.Location.StateOrProvinceOrRegion = "";
                //dropdownlistCountry.select(233);
                //$scope.Location.IdCountryOrRegion = 233;
            }

        };

        $scope.$watch("Location.OpenHour", function (newValue, oldValue) {
            if ( newValue != oldValue) {
                EnableCreateFooterButton(true);
            }
            
        });
        $scope.$watch("Location.CloseHour", function (newValue, oldValue) {
            if (newValue != oldValue) {
                EnableCreateFooterButton(true);
            }
        });

        function sleep(milliseconds) {
            var start = new Date().getTime();
            for (var i = 0; i < 1e7; i++) {
                if ((new Date().getTime() - start) > milliseconds) {
                    break;
                }
            }
        }
        $scope.getShareViewData = function () {
            convertUtcTime();           
            return { SharedParameter: JSON.stringify($scope.Location) };
        };

        $scope.IsShowMap = false;
        $scope.HideMap = function () {
            $scope.IsShowMap = false;
        };

        var dropdownCountryOrRegion = $("#IdCountryOrRegion-dropdownlist").data("kendoDropDownList");

        $scope.setGoogleMapLocation = function () {
            $scope.Map = {
                Center:
                {
                    latitude: $scope.Location.Lat,
                    longitude: $scope.Location.Lng
                },
                Zoom: 12
            };

            $scope.Marker = {
                Id: $scope.controllerId,
                Coords: {
                    latitude: $scope.Location.Lat,
                    longitude: $scope.Location.Lng
                },
                Options: {
                    draggable: true,
                    labelContent: $scope.getFullAddress(),
                    labelAnchor: "25 55"
                },
                Events: {
                    dragend: function (marker, eventName, args) {
                        $scope.Location.Lat = marker.getPosition().lat();
                        $scope.Location.Lng = marker.getPosition().lng();
                        EnableCreateFooterButton(true);
                    }
                }
            };
        };

        $scope.validateAddress = function () {
            var mess = "FOLLOWING BUSINESS RULES HAVE FAILED:<br/>";
            var isError = false;
            if ($scope.Location.Address1 == null || $scope.Location.Address1 == "") {
                mess += "-The Address1 field is required<br/>";
                isError = true;
            }
            if ($scope.Location.City == null || $scope.Location.City == "") {
                mess += "-The City field is required<br/>";
                isError = true;
            }
            if ($scope.Location.StateOrProvinceOrRegion == null || $scope.Location.StateOrProvinceOrRegion == "") {
                mess += "-The State / Province / Region field is required<br/>";
                isError = true;
            }
            if ($scope.Location.Zip == null || $scope.Location.Zip == "") {
                mess += "-The Zip field is required<br/>";
                isError = true;
            }
            if ($scope.Location.IdCountryOrRegion == null || $scope.Location.IdCountryOrRegion <= 0) {
                mess += "-The Country / Region field is required<br/>";
                isError = true;
            }
            if (isError) {
                logError(mess);
                return false;
            }
            return true;
        };

        $scope.getFullAddress = function () {
            var fullAddress = $scope.Location.Address1
                + ($scope.Location.Address2 != null && $scope.Location.Address2 != '' ? ', ' + $scope.Location.Address2 : '')
                + ($scope.Location.City != null && $scope.Location.City != '' ? ', ' + $scope.Location.City : '')
                + ($scope.Location.StateOrProvinceOrRegion != null && $scope.Location.StateOrProvinceOrRegion != '' ? ', ' + $scope.Location.StateOrProvinceOrRegion : '')
                //+ ($scope.Location.Zip != null && $scope.Location.Zip != '' ? ', ' + $scope.Location.Zip : '')
                + (dropdownCountryOrRegion != undefined && dropdownCountryOrRegion.text() != '' && dropdownCountryOrRegion.text() != 'Select Country / Region' ? ', ' + dropdownCountryOrRegion.text() : '');
            return fullAddress;
        };

        $scope.CheckLocation = function () {

            if ($scope.validateAddress()) {
                if (!this.geocoder) this.geocoder = new google.maps.Geocoder();
                
                this.geocoder.geocode({ "address": $scope.getFullAddress() }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK && results.length > 0) {

                        var location = results[0].geometry.location;
                        $scope.Location.Lat = location.lat();
                        $scope.Location.Lng = location.lng();
                        $scope.setGoogleMapLocation();
                        $scope.IsShowMap = true;
                        $scope.$apply();

                    }
                });
            }

        };

        //$scope.setGoogleMapLocation();
        uiGmapGoogleMapApi.then(function (maps) {

        });

        $scope.$watchCollection('[Location.Address1, Location.City,Location.StateOrProvinceOrRegion,Location.Zip,Location.IdCountryOrRegion]', function (newValues) {
            if (newValues[0] != '' && newValues[0] != null &&
                newValues[1] != '' && newValues[1] != null &&
                newValues[2] != '' && newValues[2] != null &&
                newValues[3] != '' && newValues[3] != null &&
                newValues[4] != 0 && newValues[4] != '0') {
                $scope.CheckLocation();
            }

        });

    }]
);