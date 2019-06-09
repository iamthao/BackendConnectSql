'use strict';
app.factory('displayAlertExtended', ['$resource', function ($resource) {
    var displayAlertExtended = {
        getDisplayAlertExtended: getDisplayAlertExtended
    };
    return displayAlertExtended;
    

    ///////
    function getDisplayAlertExtended() {
        return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/Authentication/GetDisplayAlertExtended',
                            params: {
                            }
                        }
                    });
    }
    
}]);