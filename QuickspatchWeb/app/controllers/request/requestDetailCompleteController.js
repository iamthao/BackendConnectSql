'use strict';
app.controller('requestDetailCompleteController', ['$rootScope', '$scope', 'common', 'messageLanguage', '$window', '$sce','$timeout', '$http',
    function ($rootScope, $scope, common, messageLanguage, $window, $sce, $timeout, $http) {
    var controllerId = "requestDetailCompleteController";
    var getLogFn = common.logger.getLogFn;
    var log = getLogFn(controllerId);
        //Completed = 30
        $scope.tabIndexDetailComplete = 1;
        $scope.isComplete = false;
        $scope.notes = '';
        $scope.picture = '';
        $scope.showPicture = false;
        $scope.selectTabDetailComplete = function(type) {
            $scope.tabIndexDetailComplete = type;
        }

        function activate() {
            $('#loading-request-detail').show();
            var requestItem = $('#popupWindow').data('RequestItem');
            $scope.isComplete = requestItem.StatusId == 30;
            //$scope.isComplete = true;
            if ($scope.isComplete) {
                loadPictureAndImage();
                $timeout(function () {
                    var paymentUrl = "/Report/PrintPdfFile?parameters=" + JSON.stringify({ QueryId: requestItem.Id }) + "&type=1";
                    $scope.currentProjectUrl = $sce.trustAsResourceUrl(paymentUrl);
                });
            } else {
                $('#loading-request-detail').hide();
            }

            function loadPictureAndImage() {
                $http.get('/Request/GetPictureAndNoteRequestComplete?requestId=' + requestItem.Id).then(function (data) {
                    $('#loading-request-detail').hide();
                    $scope.notes = data.data.Note.replace(/(?:\\r\\n|\\r|\\n)/g, '<br />');;
                    $scope.showPicture = data.data.Picture != null && data.data.Picture != '';
                    if ($scope.showPicture) {
                        $scope.picture = $sce.trustAsResourceUrl('data:image/png;base64,' + data.data.Picture);
                    }
                });
            }
        }
        activate();
    }]);