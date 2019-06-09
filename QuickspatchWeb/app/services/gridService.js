'use strict';
app.factory('gridService', ['$resource', function ($resource) {
    var gridService = {
        gridConfig: function () {
            return $resource('', {},
                    {
                        get: {
                            method: 'GET',
                            url: '/GridConfig/Get'
                        }
                    });
        }
    };
    return gridService;
}]);