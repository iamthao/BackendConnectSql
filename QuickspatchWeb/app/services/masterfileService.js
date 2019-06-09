'use strict';
app.factory('masterfileService', ['$resource', function ($resource) {
    var masterfileService = {
        create: function (model) {
            return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/' + model + '/Create',
                            params: {
                            }
                        }
                    });
        },
        update: function (model) {
            return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/' + model + '/Update',
                            params: {
                            }
                        }
                    });
        },
        deleteById: function (model) {
            return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/' + model + '/Delete',
                            params: {
                            }
                        }
                    });
        },
        deleteMultiByIds: function (model) {
            return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/' + model + '/DeleteMulti',
                            params: {
                            }
                        }
                    });
        },
        callWithUrl: function (url) {
            return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: url,
                            params: {
                            }
                        }
                    });
        },
    };
    return masterfileService;
}]);