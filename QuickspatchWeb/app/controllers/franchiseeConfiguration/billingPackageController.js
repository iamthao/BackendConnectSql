'use strict';
app.controller('billingPackageController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage', '$http', '$sce',
    function ($scope, masterfileService, $state, common, config, messageLanguage, $http, $sce) {
        var events = config.events;
        $scope.controllerId = "billingPackageController";
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn($scope.controllerId);
        var logError = getLogFn($scope.controllerId, "error");
        var logSuccess = getLogFn($scope.controllerId, "success");
    
        $scope.showMonthly = true;

        function activate() {
            $http.get("FranchiseeConfiguration/GetInfoPackageCurrent")
             .then(function (result) {
                // console.log(result.data.PackageId);
                 if (result.data.PackageId == 1) {
                     $scope.setPackageCurrent(result.data.PackageId);
                     $('#package-standard').removeClass('current');
                     $("#button-standard").html('Choose plan');

                     $('#package-prenium').removeClass('current');
                     $("#button-prenium").html('Choose plan');

                     $('#package-enterprise').removeClass('current');
                     $("#button-enterprise").html('Choose plan');

                     $('#package-enterprise-plus').removeClass('current');
                     $("#button-enterprise-plus").html('Choose plan');

                     $('#package-standard-annually').addClass('current'); //
                     $("#button-standard-annually").html('Selected');  //
                     $("#current-label-standard-annually").html('(Current)');//

                     $('#package-prenium-annually').removeClass('current');
                     $("#button-prenium-annually").html('Choose plan');

                     $('#package-enterprise-annually').removeClass('current');
                     $("#button-enterprise-annually").html('Choose plan');

                     $('#package-enterprise-plus-annually').removeClass('current');
                     $("#button-enterprise-plus-annually").html('Choose plan');
                 }
                 if (result.data.PackageId == 2) {
                     $scope.setPackageCurrent(result.data.PackageId);
                     $('#package-standard').addClass('current'); //
                     $("#button-standard").html('Selected');   //
                     $("#current-label-standard").html('(Current)');//

                     $('#package-prenium').removeClass('current');
                     $("#button-prenium").html('Choose plan');

                     $('#package-enterprise').removeClass('current');
                     $("#button-enterprise").html('Choose plan');

                     $('#package-enterprise-plus').removeClass('current');
                     $("#button-enterprise-plus").html('Choose plan');

                     $('#package-standard-annually').removeClass('current');
                     $("#button-standard-annually").html('Choose plan');

                     $('#package-prenium-annually').removeClass('current');
                     $("#button-prenium-annually").html('Choose plan');

                     $('#package-enterprise-annually').removeClass('current');
                     $("#button-enterprise-annually").html('Choose plan');

                     $('#package-enterprise-plus-annually').removeClass('current');
                     $("#button-enterprise-plus-annually").html('Choose plan');
                 }
                 if (result.data.PackageId == 3) {
                     $scope.setPackageCurrent(result.data.PackageId);
                     $('#package-standard').removeClass('current');
                     $("#button-standard").html('Choose plan');

                     $('#package-prenium').removeClass('current');
                     $("#button-prenium").html('Choose plan');

                     $('#package-enterprise').removeClass('current');
                     $("#button-enterprise").html('Choose plan');

                     $('#package-enterprise-plus').removeClass('current');
                     $("#button-enterprise-plus").html('Choose plan');

                     $('#package-standard-annually').removeClass('current');
                     $("#button-standard-annually").html('Choose plan');

                     $('#package-prenium-annually').addClass('current'); //
                     $("#button-prenium-annually").html('Selected');    //
                     $("#current-label-prenium-annually").html('(Current)');//

                     $('#package-enterprise-annually').removeClass('current');
                     $("#button-enterprise-annually").html('Choose plan');

                     $('#package-enterprise-plus-annually').removeClass('current');
                     $("#button-enterprise-plus-annually").html('Choose plan');
                 }

                 if (result.data.PackageId == 4) {
                     $scope.setPackageCurrent(result.data.PackageId);
                     $('#package-standard').removeClass('current');
                     $("#button-standard").html('Choose plan');

                     $('#package-prenium').addClass('current');//
                     $("#button-prenium").html('Selected');  //
                     $("#current-label-prenium").html('(Current)');//

                     $('#package-enterprise').removeClass('current');
                     $("#button-enterprise").html('Choose plan');

                     $('#package-enterprise-plus').removeClass('current');
                     $("#button-enterprise-plus").html('Choose plan');

                     $('#package-standard-annually').removeClass('current');
                     $("#button-standard-annually").html('Choose plan');

                     $('#package-prenium-annually').removeClass('current');
                     $("#button-prenium-annually").html('Choose plan');

                     $('#package-enterprise-annually').removeClass('current');
                     $("#button-enterprise-annually").html('Choose plan');

                     $('#package-enterprise-plus-annually').removeClass('current');
                     $("#button-enterprise-plus-annually").html('Choose plan');
                   
                 }
                if (result.data.PackageId == 5) {
                    $scope.setPackageCurrent(result.data.PackageId);

                    $('#package-standard').removeClass('current');
                    $("#button-standard").html('Choose plan');

                    $('#package-prenium').removeClass('current');
                    $("#button-prenium").html('Choose plan');

                    $('#package-enterprise').removeClass('current');
                    $("#button-enterprise").html('Choose plan');

                    $('#package-enterprise-plus').removeClass('current');
                    $("#button-enterprise-plus").html('Choose plan');

                    $('#package-standard-annually').removeClass('current');
                    $("#button-standard-annually").html('Choose plan');

                    $('#package-prenium-annually').removeClass('current');
                    $("#button-prenium-annually").html('Choose plan');

                    $('#package-enterprise-annually').addClass('current'); //
                    $("#button-enterprise-annually").html('Selected'); //
                    $("#current-label-enterprise-annually").html('(Current)');//

                    $('#package-enterprise-plus-annually').removeClass('current');
                    $("#button-enterprise-plus-annually").html('Choose plan');

                }
                if (result.data.PackageId == 6) {
                    $scope.setPackageCurrent(result.data.PackageId);
                    $('#package-standard').removeClass('current');
                    $("#button-standard").html('Choose plan');

                    $('#package-prenium').removeClass('current');
                    $("#button-prenium").html('Choose plan');

                    $('#package-enterprise').addClass('current'); //
                    $("#button-enterprise").html('Selected');    //
                    $("#current-label-enterprise").html('(Current)');//

                    $('#package-enterprise-plus').removeClass('current');
                    $("#button-enterprise-plus").html('Choose plan');

                    $('#package-standard-annually').removeClass('current');
                    $("#button-standard-annually").html('Choose plan');

                    $('#package-prenium-annually').removeClass('current');
                    $("#button-prenium-annually").html('Choose plan');

                    $('#package-enterprise-annually').removeClass('current');
                    $("#button-enterprise-annually").html('Choose plan');

                    $('#package-enterprise-plus-annually').removeClass('current');
                    $("#button-enterprise-plus-annually").html('Choose plan');
                }
                if (result.data.PackageId == 7) {
                    $scope.setPackageCurrent(result.data.PackageId);
                    $('#package-standard').removeClass('current');
                    $("#button-standard").html('Choose plan');

                    $('#package-prenium').removeClass('current');
                    $("#button-prenium").html('Choose plan');

                    $('#package-enterprise').removeClass('current');
                    $("#button-enterprise").html('Choose plan');

                    $('#package-enterprise-plus').removeClass('current');
                    $("#button-enterprise-plus").html('Choose plan');

                    $('#package-standard-annually').removeClass('current');
                    $("#button-standard-annually").html('Choose plan');

                    $('#package-prenium-annually').removeClass('current');
                    $("#button-prenium-annually").html('Choose plan');

                    $('#package-enterprise-annually').removeClass('current');
                    $("#button-enterprise-annually").html('Choose plan');

                    $('#package-enterprise-plus-annually').addClass('current');//
                    $("#button-enterprise-plus-annually").html('Selected');  //
                    $("#current-label-enterprise-plus-annually").html('(Current)');//
                }
                if (result.data.PackageId == 8) {
                    $scope.setPackageCurrent(result.data.PackageId);

                    $('#package-standard').removeClass('current');
                    $("#button-standard").html('Choose plan');

                    $('#package-prenium').removeClass('current');
                    $("#button-prenium").html('Choose plan');

                    $('#package-enterprise').removeClass('current');
                    $("#button-enterprise").html('Choose plan');

                    $('#package-enterprise-plus').addClass('current');//
                    $("#button-enterprise-plus").html('Selected'); //
                    $("#current-label-enterprise-plus").html('(Current)');//

                    $('#package-standard-annually').removeClass('current');
                    $("#button-standard-annually").html('Choose plan');

                    $('#package-prenium-annually').removeClass('current');
                    $("#button-prenium-annually").html('Choose plan');

                    $('#package-enterprise-annually').removeClass('current');
                    $("#button-enterprise-annually").html('Choose plan');

                    $('#package-enterprise-plus-annually').removeClass('current');
                    $("#button-enterprise-plus-annually").html('Choose plan');
                }
                if (result.data.PackageId == 0) {
                    //default annually
                     $scope.setPackageCurrent(1);
                     $('#package-standard').removeClass('current'); 
                     $("#button-standard").html('Choose plan');   

                     $('#package-prenium').removeClass('current');
                     $("#button-prenium").html('Choose plan');

                     $('#package-enterprise').removeClass('current');
                     $("#button-enterprise").html('Choose plan');

                     $('#package-enterprise-plus').removeClass('current');
                     $("#button-enterprise-plus").html('Choose plan');

                     $('#package-standard-annually').addClass('current');//
                     $("#button-standard-annually").html('Selected');//

                     $('#package-prenium-annually').removeClass('current');
                     $("#button-prenium-annually").html('Choose plan');

                     $('#package-enterprise-annually').removeClass('current');
                     $("#button-enterprise-annually").html('Choose plan');

                     $('#package-enterprise-plus-annually').removeClass('current');
                     $("#button-enterprise-plus-annually").html('Choose plan');
                }

                if (result.data.PackageId == 0 || result.data.PackageId == 1 || result.data.PackageId == 3 || result.data.PackageId == 5 || result.data.PackageId == 7) {
                    $scope.showMonthly = false;
                    $('#choose-monthly').removeClass('chosee-type-payment');
                    $('#choose-annually').addClass('chosee-type-payment'); //
                };
            });

            //set 
        }

        activate();
        //CSS  choose package
        $scope.selectStandard = function () {
            $scope.setPackageCurrent(2);

            $('#package-standard').addClass('current'); //
            $("#button-standard").html('Selected');   //

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').removeClass('current');
            $("#button-enterprise").html('Choose plan');

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').removeClass('current'); 
            $("#button-standard-annually").html('Choose plan');   

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan');

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };

        $scope.selectStandardAnnually = function () {
            $scope.setPackageCurrent(1);

            $('#package-standard').removeClass('current'); 
            $("#button-standard").html('Choose plan');

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').removeClass('current');
            $("#button-enterprise").html('Choose plan');

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').addClass('current'); //
            $("#button-standard-annually").html('Selected');  //

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan');

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };

        $scope.selectPreniumAnnually = function () {
            $scope.setPackageCurrent(3);

            $('#package-standard').removeClass('current'); 
            $("#button-standard").html('Choose plan');

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').removeClass('current');
            $("#button-enterprise").html('Choose plan');

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').removeClass('current'); 
            $("#button-standard-annually").html('Choose plan');    

            $('#package-prenium-annually').addClass('current'); //
            $("#button-prenium-annually").html('Selected');    //

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };

        $scope.selectPrenium = function () {
            $scope.setPackageCurrent(4);

            $('#package-standard').removeClass('current');
            $("#button-standard").html('Choose plan');

            $('#package-prenium').addClass('current');//
            $("#button-prenium").html('Selected');  //

            $('#package-enterprise').removeClass('current');
            $("#button-enterprise").html('Choose plan');

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').removeClass('current');
            $("#button-standard-annually").html('Choose plan');

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan');

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };

        $scope.selectEnterpriseAnnually = function () {
            $scope.setPackageCurrent(5);

            $('#package-standard').removeClass('current');
            $("#button-standard").html('Choose plan');

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').removeClass('current');
            $("#button-enterprise").html('Choose plan');

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').removeClass('current');
            $("#button-standard-annually").html('Choose plan');

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan'); 

            $('#package-enterprise-annually').addClass('current'); //
            $("#button-enterprise-annually").html('Selected'); //

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };

        $scope.selectEnterprise = function () {
            $scope.setPackageCurrent(6);

            $('#package-standard').removeClass('current');
            $("#button-standard").html('Choose plan');

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').addClass('current'); //
            $("#button-enterprise").html('Selected');    //

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').removeClass('current');
            $("#button-standard-annually").html('Choose plan');

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan');

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };

        $scope.selectEnterprisePlusAnnually = function () {
            $scope.setPackageCurrent(7);

            $('#package-standard').removeClass('current');
            $("#button-standard").html('Choose plan');

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').removeClass('current'); 
            $("#button-enterprise").html('Choose plan');    

            $('#package-enterprise-plus').removeClass('current');
            $("#button-enterprise-plus").html('Choose plan');

            $('#package-standard-annually').removeClass('current');
            $("#button-standard-annually").html('Choose plan');

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan');

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').addClass('current');//
            $("#button-enterprise-plus-annually").html('Selected');  //
        };

        $scope.selectEnterprisePlus = function () {
            $scope.setPackageCurrent(8);

            $('#package-standard').removeClass('current');
            $("#button-standard").html('Choose plan');

            $('#package-prenium').removeClass('current');
            $("#button-prenium").html('Choose plan');

            $('#package-enterprise').removeClass('current');
            $("#button-enterprise").html('Choose plan');

            $('#package-enterprise-plus').addClass('current');//
            $("#button-enterprise-plus").html('Selected'); //

            $('#package-standard-annually').removeClass('current');
            $("#button-standard-annually").html('Choose plan');

            $('#package-prenium-annually').removeClass('current');
            $("#button-prenium-annually").html('Choose plan');

            $('#package-enterprise-annually').removeClass('current');
            $("#button-enterprise-annually").html('Choose plan');

            $('#package-enterprise-plus-annually').removeClass('current');
            $("#button-enterprise-plus-annually").html('Choose plan');
        };
        //CSS choose type payment
       
        $scope.chooseMonthly = function () {
            $scope.showMonthly = true;
            $('#choose-monthly').addClass('chosee-type-payment'); //
            $('#choose-annually').removeClass('chosee-type-payment');
        };

        $scope.chooseAnnually = function () {
            $scope.showMonthly = false;
            $('#choose-monthly').removeClass('chosee-type-payment'); 
            $('#choose-annually').addClass('chosee-type-payment'); //
        };

        //1. billing index  2.Change package
        $scope.goBack = function () {
            $scope.setActionBilling(1);
        }

        var mess = "";
        $scope.paymentUrl = '';
        $scope.currentProjectUrl = '';
        $scope.submitPackage = function () {
            //console.log('package = ' + $scope.packageCurrentId);
            if ($scope.packageCurrentId == 0) {
                logError("Please choose one package.");
                return;
            }
            else {
                //kiem tra da co transaction
                $http.get("FranchiseeConfiguration/GetInfoPackageCurrent")
                     .then(function (result) {
                         //console.log(result.data.PackageId);
                         if (result.data.PackageId == $scope.packageCurrentId) {
                             logError(messageLanguage.sameCurrentPackage);
                             return;
                         }                       
                         else {
                             $http.get("FranchiseeConfiguration/CheckPackageNextApply", { params: { packageId: $scope.packageCurrentId } })
                                 .then(function (result1) {
                                     if (result1.data == 'false') {
                                         logError(messageLanguage.messPackageNextApply);
                                         return;
                                     } else {
                                        $http.get("FranchiseeConfiguration/CheckTransactionExist")
                                             .then(function (result2) {
                                                 if (result2.data == 'false') {
                                                     logError(messageLanguage.noExistTransaction);
                                                     return;
                                                 }
                                                 else {
                                                     $http.get("FranchiseeConfiguration/SubmitChangePackageEncodeData", { params: { packageIdParam: $scope.packageCurrentId, oldPackageIdParam: result.data.PackageId } })
                                                         .then(function (result3) {
                                                             //window.open(result.PaymentUrl + '?data=' + result.Data); //open in new tab
                                                             //window.location.href = result3.data.PaymentUrl + '?data=' + result3.data.Data;  //open in current tab
                                                             //console.log(result3);
                                                         if (result3.data.IsNewRequest == true) {
                                                             $('#modal-cpg-change-package').modal('show');
                                                             $scope.paymentUrl = result3.data.PaymentUrl + '?data=' + result3.data.Data;

                                                             $scope.currentProjectUrl = $sce.trustAsResourceUrl($scope.paymentUrl);
                                                         }
                                                         else {
                                                             logSuccess('Change plan successfully.');
                                                             $scope.setActionBilling(1);
                                                         }
                                                     });
                                                 }
                                             });
                                     }
                                 });

                         }
                     });

            }
        }

        $scope.showContactUs =function() {
            var popup = $("#popupWindow").data("kendoWindow");
            popup.setOptions({
                width: 650,
                height: 380,
                title: "Contact Us",
                content: {
                    url: "/FranchiseeConfiguration/ContactUs"
                }
            });
            popup.open();
        }
    }]);