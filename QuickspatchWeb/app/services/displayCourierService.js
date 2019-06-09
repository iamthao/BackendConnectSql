'use strict';
app.factory('displayCourierService', ['$resource', function ($resource) {
    var displayCourierService = {
        getDisplayLabel: getDisplayLabel
    };
    return displayCourierService;
    

    ///////
    function getDisplayLabel() {
        return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/Authentication/GetDisplayLabel',
                            params: {
                            }
                        }
                    });
    }
    
}]);