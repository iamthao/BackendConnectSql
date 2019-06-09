'use strict';
app.factory('userProfileService', ['$resource', function ($resource) {
    var userProfileService = {
        changePassword: function () {
            return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/UserProfile/ChangePassword',
                            params: {
                            }
                        }
                    });
        }
    };
    return userProfileService;
}]);
