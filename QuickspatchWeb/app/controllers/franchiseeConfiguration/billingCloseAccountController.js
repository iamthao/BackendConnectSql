'use strict';
app.controller('billingCloseAccountController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage','$http',
    function ($scope, masterfileService, $state, common, config, messageLanguage, $http) {
        var events = config.events;
        $scope.controllerId = "billingCloseAccountController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);

        $("#loading-billing").show();
        $("#content-close-account").hide();

        function CloseAccountViewModel() {
            var self = this;
            self.Password = '';
            self.Question = 'I am leaving because ...';
            self.Description = '';
        }

        $scope.data = new CloseAccountViewModel();

        $scope.EndDate = "";
        $scope.DateChangeMind = "";
        
        function active() {
            $http.get("FranchiseeConfiguration/GetInfoCloseAccount")
                .then(function (result) {
                    //console.log(result);
                    $scope.EndDate = result.data.EndDate;
                    $scope.DateChangeMind = result.data.DateChangeMind;

                    $("#loading-billing").hide();
                    $("#content-close-account").show();
            });
        };

        active();
        var logSuccess = getLogFn($scope.controllerId, "success");
        var logError = getLogFn($scope.controllerId, "error");

        //1. billing index  2.Change package 3. Close account
        $scope.goBack = function () {
            
            $scope.setActionBilling(1);
        }
       

        $scope.close = function () {;
            if ($scope.data.Password == "" || $scope.data.Password == undefined) {
                logError(messageLanguage.rePasswordCloseAccountRequired);
            } else {
                $http.post('/FranchiseeConfiguration/CloseAccount', { Password: $scope.data.Password, Question: $scope.data.Question, Description: $scope.data.Description }).then(function(rep) {
                    if (rep.data == 'true') {
                        logSuccess(messageLanguage.closeAcountSuccess);
                        $scope.setIsCloseAccount(!$scope.isCloseAccount);
                        $scope.goBack();
                    }
                    else if (rep.data == '"errorPassword"') {
                        logError(messageLanguage.incorrectPasswordCloseAccount);
                    }
                    else {
                        logError(messageLanguage.closeAccountError);
                    }
                });
            }
        }
    }]);