'use strict';
app.controller('franchiseeConfigurationController', ['$scope', 'masterfileService', '$state', 'common', 'config', 'messageLanguage', '$http',
            function ($scope, masterfileService, $state, common, config, messageLanguage, $http) {
                var events = config.events;
                $scope.controllerId = "franchiseeConfigurationController";
                var getLogFn = common.logger.getLogFn;
                var log = getLogFn($scope.controllerId);
                var logSuccess = getLogFn($scope.controllerId, "success");

                //Khai bao
                $("#loading-billing").show();
                $("#content-billing").hide();

                $scope.FranchiseeId = "";
                $scope.Logo = "";

                $scope.Name = "";
                $scope.PrimaryContactPhone = "";
                $scope.PrimaryContactEmail = "";
                $scope.PrimaryContactFax = "";
                $scope.PrimaryContactCellNumber = "";

                $scope.Address = "";

                $scope.StartActiveDate = "";
                $scope.EndActiveDate = "";
                $scope.LicenseKey = "";

                //location default
                $scope.LocationFromName = "";
                $scope.LocationFromAddress = "";
                $scope.LocationToName = "";
                $scope.LocationToAddress = "";

                function activate() {
                    $http.get("FranchiseeConfiguration/GetInfoFranchiseeIndex")
                     .then(function (result) {
                         //console.log(result.data);
                         $scope.FranchiseeId = result.data.FranchiseeId;
                         $scope.Logo = result.data.Logo;

                         $scope.Name = result.data.Name;
                         $scope.PrimaryContactPhone = result.data.PrimaryContactPhone;
                         $scope.PrimaryContactEmail = result.data.PrimaryContactEmail;
                         $scope.PrimaryContactFax = result.data.PrimaryContactFax;
                         $scope.PrimaryContactCellNumber = result.data.PrimaryContactCellNumber;

                         $scope.Address = result.data.Address;

                         $scope.StartActiveDate = result.data.StartActiveDate;
                         $scope.EndActiveDate = result.data.EndActiveDate;
                         $scope.LicenseKey = result.data.LicenseKey;


                         $("#loading-billing").hide();
                         $("#content-billing").show();
                     });
                }

                //$scope.getLocationDefault = function() {
                //    $http.get("FranchiseeConfiguration/GetLocationDefault")
                //    .then(function (result) {
                //        //console.log(result.data);
                //        $scope.LocationFromName = result.data.LocationFromName;
                //        $scope.LocationFromAddress = result.data.LocationFromAddress;
                //        $scope.LocationToName = result.data.LocationToName;
                //        $scope.LocationToAddress = result.data.LocationToAddress;
                //    });
                //}

                //$scope.getLocationDefault();
                activate();

                $scope.$on("ReloadConfig", function () {
                    $state.reload();
                });
                $scope.Edit = function (id) {
                    var popup = $("#popupWindow").data("kendoWindow");
                    popup.setOptions({
                        width: "600px",
                        height: "500px",
                        title: "Edit Information",
                        content: {
                            url: "/FranchiseeConfiguration/Update/" + id
                        },
                        animation: false
                    });
                    popup.open();
                };

                //add new contact
                $scope.addContact = function () {
                    var popup = $("#popupWindow").data("kendoWindow");
                    popup.setOptions({
                        width: "500px",
                        height: "250px",
                        title: "Add New Contact",
                        content: {
                            url: "/Contact/Create"
                        },
                        animation: false
                    });
                    popup.open();
                };

                //update contact                                                           
                $scope.editContact = function (id) {
                    //console.log(id);
                    var popup = $("#popupWindow").data("kendoWindow");
                    popup.setOptions({
                        width: "500px",
                        height: "250px",
                        title: "Update Contact",
                        content: {
                            url: "/Contact/Update/" + id
                        },
                        animation: false
                    });
                    popup.open();
                };

                //delete contact
                $scope.deleteContact = function (id) {
                    common.bootboxConfirm("Are you sure that you want to delete this record?", function () {
                        masterfileService.deleteById('Contact').perform({ id: id }).$promise.then(function (result) {
                            if (result.Error === undefined || result.Error === '') {
                                logSuccess('Delete contact successfully');
                                $("#contact-grid").data("kendoGrid").dataSource.read();
                            }
                        });
                    }, function () { }).modal('show');

                };

                //update location default
                $scope.updateLocation = function () {
                    var popup = $("#popupWindow").data("kendoWindow");
                    popup.setOptions({
                        width: 650,
                        height: 230,
                        title: "Update Location Default",
                        content: {
                            url: "/FranchiseeConfiguration/UpdateLocationDefault/"
                        }
                    });
                    popup.open();
                };


                //refreshgrid
                $scope.refreshGrid = function() {
                    $("#contact-grid").data("kendoGrid").dataSource.read();
                };

                //grid                                                                       
                var schemaFields = {
                    Id: { editable: false },
                    CreatedDateNoFormat: { editable: false, type: "date" },
                    TimeNoFormat: { editable: false, type: "date" }
                };

                $scope.PageSizeSelected = 10;
                $scope.DataSource = {};
                $scope.getDataSource = function () {
                    $scope.DataSource = new kendo.data.DataSource({
                        type: "json",
                        transport: {
                            read: {
                                url: "/FranchiseeConfiguration/GetListContact",
                                type: "POST"
                            },
                            parameterMap: function (options, operation) {
                                if (operation !== "read" && options.models) {
                                    return { models: kendo.stringify(options.models) };
                                } else if (operation == "read") {
                                    var searchString = $scope.SearchTextTransaction;
                                    $scope.PageSizeSelected = options.pageSize;
                                    var result = {
                                        pageSize: options.pageSize,
                                        skip: options.skip,
                                        take: options.take,
                                        SearchString: searchString,
                                        RequestId: $scope.requestId
                                    };

                                    if (options.sort) {
                                        for (var i = 0; i < options.sort.length; i++) {
                                            result["sort[" + i + "].field"] = options.sort[i].field;
                                            result["sort[" + i + "].dir"] = options.sort[i].dir;
                                        }
                                    }
                                    return result;
                                }
                            }
                        },
                        serverPaging: true,
                        serverSorting: true,
                        batch: true,
                        pageSize: $scope.PageSizeSelected,
                        schema: {
                            model: {
                                fields: {
                                    Id: 'Id',
                                    fields: schemaFields
                                }
                            },
                            data: "Data",
                            total: "TotalRowCount",
                        }
                    });
                    return $scope.DataSource;
                };

                $scope.listContactGridOptions = {
                    dataSource: $scope.getDataSource(),
                    //sortable: false,
                    //pageable: {
                    //    refresh: true,
                    //    pageSizes: [10, 20, 50, 100],
                    //    buttonCount: 5
                    //},
                    //reorderable: true,
                    //scrollable: true,
                    //resizable: true,
                    //height: 300,
                    scrollable: false,
                    columns: [
                        {
                            field: "Name",
                            title: "Name",
                            width: "200px"

                        },
                        {
                            field: "PhoneFormat",
                            title: "Phone",
                            width: "150px"
                        }
                        ,
                        {
                            field: "", width: "50px", template: kendo.template($("#commandButtons").html())
                        }

                    ],
                };
            }]);