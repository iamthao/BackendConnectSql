'use strict';
app.controller('requestDetailRequestController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', '$http',
    function ($rootScope, $scope, common, messageLanguage, $window, $http) {
    var controllerId = "requestDetailRequestController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
    $scope.events = [];
    function activate() {
        $('#loading-request-detail').show();
        var requestItem = $('#popupWindow').data('RequestItem');
        $http.get('/Request/GetNotesDetail?requestId=' + requestItem.Id).then(function (data) {
            //process date to correct timezone
            var results = data.data.Data;
            //console.log(results);
            for (var i = 0; i < results.length; i++) {
                angular.forEach(results[i], function(value, key) {
                    if ("CreateTime" == key) {
                        var d = new Date(value + 'Z');
                        if (d.getFullYear() > 1970) {
                            results[i].CreateTime = d;
                        }
                    }
                    if ("Content" == key) {
                        var content = results[i].Content;
                        results[i].Content = content.replace(/(?:\\r\\n|\\r|\\n)/g, '<br />');
                    }
                });
            }
            
            $scope.events = results;
            $('#loading-request-detail').hide();
        });
    }
    activate();
}]);