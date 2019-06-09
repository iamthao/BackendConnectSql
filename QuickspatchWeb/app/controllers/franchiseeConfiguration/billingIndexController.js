'use strict';
app.controller('billingIndexController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage', '$http', '$timeout', '$sce',
    function ($scope, masterfileService, $state, common, config, messageLanguage, $http, $timeout, $sce) {
        var events = config.events;
        $scope.controllerId = "billingIndexController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        var logError = getLogFn($scope.controllerId, "error");
        var logSuccess = getLogFn($scope.controllerId, "success");

        $("#loading-billing").show();
        $("#content-billing").hide();


        //Khai bao
        $scope.currentPackage = "";
        $scope.accountStatus = "";
        $scope.nextBillingDate = "";
        $scope.accountOwner = "";
        $scope.accountName = "";
        $scope.urlInfo = "";

        $scope.name = "";
        $scope.phone = "";
        $scope.address = "";
        $scope.city = "";
        $scope.state = "";
        $scope.zip = "";

        $scope.isTrial = true;
        $scope.setIsCloseAccount(false);
        function activate() {
            $http.get("FranchiseeConfiguration/GetInfoBillingIndex")
                .then(function (result) {
                    //console.log(result.data.AccountName);
                    $scope.isTrial = result.data.IsTrial;

                    $scope.currentPackage = result.data.CurrentPlan;
                    $scope.accountStatus = result.data.AccountStatus;
                    $scope.nextBillingDate = result.data.NextBillingDate;
                    $scope.accountOwner = result.data.AccountOwner;
                    $scope.accountName = result.data.AccountName;
                    $scope.urlInfo = result.data.Url;

                    $scope.name = result.data.NamePaymentInfo;
                    $scope.phone = result.data.Phone;
                    $scope.address = result.data.Address;
                    $scope.city = result.data.City;
                    $scope.state = result.data.State;
                    $scope.zip = result.data.Zip;

                    $scope.setIsCloseAccount(result.data.IsClosingAccount);

                    $("#loading-billing").hide();
                    $("#content-billing").show();
            });
        }

        activate();

        $scope.refresh = function() {
            activate();
        }

        //1. billing index  2.Change package
        $scope.activate = function () {
            $scope.setActionBilling(2);
        }

        $scope.upgrade = function () {

            if ($scope.isCloseAccount == true) {
                logError(messageLanguage.messReopenAcountPackage);
                return;
            }
            $scope.setActionBilling(2);
        }

        $scope.closeAccount = function () {
            $scope.setActionBilling(3);
        }
        $scope.isShowCpg = false;
        $scope.paymentUrl = '';
        $scope.changeBillingInfo = function () {
            if ($scope.isTrial) {
                logError(messageLanguage.messReopenAcount);
            }
            else if ($scope.isCloseAccount == true) {
                logError(messageLanguage.messReopenAcount);
            }
            else {
                $http.get("FranchiseeConfiguration/ChangePaymentInfoApi")
                  .then(function (result) {
                      //console.log(result.data.Data);
                      $('#modal-cpg').modal('show');
                      $timeout(function () {
                          
                          $scope.paymentUrl = result.data.PaymentUrl + '?data=' + result.data.Data;
                          $scope.currentProjectUrl = $sce.trustAsResourceUrl($scope.paymentUrl);
                          //console.log(result);
                          //$scope.currentProjectUrl = '/LicenceExtension/Close';
                      });
                      
                      $scope.isShowCpg = true;
                    
                  });
            }
        }

        $scope.cancelCloseAccount = function () {
            $http.get("FranchiseeConfiguration/CheckCurrentRequestCancel")
                .then(function (result) {
                    if (result.data == 'true') {
                        logError(messageLanguage.closeAccountRequesrCanceled);
                    } else {
                        common.bootboxConfirm("Are you sure that you want to reopen account?", function () {
                            $http.post('/FranchiseeConfiguration/CancelAccount').then(function (rep) {
                                if (rep.data == 'true') {
                                    logSuccess(messageLanguage.cancelSuccess);
                                    $scope.setIsCloseAccount(!$scope.isCloseAccount);
                                } else {
                                    logError(messageLanguage.closeAccountError);
                                }
                            });;
                        }, function () { }).modal('show');
                    }
            });
            
        }
    }]);