'use strict';
app.service('requestDataService', ['$q', '$http', function ($q, $http) {
    function RequestData() {
        var self = this;
        self.data = {
            LocationVersion: '',
            Locations: [],
            CourierVersion: '',
            Couriers: []
        };

        self.getVersion = function() {
            var deferred = $q.defer();
            $http.get('/TableVersion/GetVersionRequest').then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        self.getDataLocation = function () {
            var locationVersion = "";
            var deferred = $q.defer();
            self.getVersion().then(function(data) {
                for (var i = 0; i < data.data.length; i++) {
                    if (data.data[i].TableId === 1) {
                        locationVersion = data.data[i].Version;
                    }
                }
                
                if (self.data.LocationVersion !== locationVersion) {
                    $http.get('/Location/GetDataLocation').then(function (result) {
                        self.data.LocationVersion = locationVersion;
                        self.data.Locations = result.data;
                        deferred.resolve(self.data.Locations);
                    });
                } else {
                    deferred.resolve(self.data.Locations);
                }
            });
           
            return deferred.promise;
        }

        self.getDataCourier = function () {
            var courierVersion = "";
            var deferred = $q.defer();
            self.getVersion().then(function (data) {

                for (var i = 0; i < data.data.length; i++) {
                    if (data.data[i].TableId === 2) {
                        courierVersion = data.data[i].Version;
                    }
                }
                if (self.data.CourierVersion !== courierVersion) {
                    
                    $http.get('/Courier/GetDataCourier').then(function(result) {
                        self.data.CourierVersion = courierVersion;
                        self.data.Couriers = result.data; //_.union([{KeyId: 0, DisplayName: ''}], result.data);
                        deferred.resolve(self.data.Couriers);
                    });
                } else {
                    deferred.resolve(self.data.Couriers);
                }
            });
            return deferred.promise;
        }

        self.getLocationById = function(id) {
            var obj = _.findWhere(self.data.Locations, { KeyId: id });
            return obj;
        }

        //Thao
        self.getDataLocationNew = function () {
            var locationVersion = "";
            var deferred = $q.defer();
           
                    $http.get('/Location/GetDataLocation').then(function (result) {
                        self.data.LocationVersion = locationVersion;
                        self.data.Locations = result.data;
                        deferred.resolve(self.data.Locations);
                    });               
            return deferred.promise;
        }
        //self.getData = function () {
        //    var deferred = $q.defer();
        //    self.getVersion().then(function (result) {
        //        var locationVersion = "";
        //        var courierVersion = "";
        //        for (var i = 0; i < result.data.length; i++) {
        //            if (result.data[i].TableId == 1) {
        //                locationVersion = result.data[i].Version;
        //            }
        //            if (result.data[i].TableId == 2) {
        //                courierVersion = result.data[i].Version;
                        
        //            }
        //        }
        //        if (self.data.LocationVersion != locationVersion) {
        //            $http.get('/Location/GetDataLocation').then(function (locations) {
        //                self.data.Locations = locations.data;
        //                self.data.LocationVersion = locationVersion;
                        
        //                if (self.data.CourierVersion != courierVersion) {
        //                    $http.get('/Courier/GetDataCourier').then(function (courier) {
        //                        self.data.CourierVersion = courierVersion;
        //                        self.data.Couriers = courier.data;
        //                        deferred.resolve(self.data);
        //                    });
        //                }
        //            });
        //        }
        //    });
        //    return deferred.promise;
        //}
    }
    return new RequestData();
}]);