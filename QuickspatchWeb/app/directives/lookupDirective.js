'use strict';
app.directive('customLookup', ['$http', function($http) {
    return {
        restrict: "E",
        scope: {
            modelBinding: "=",
            showAddPopupLookup: "&addPopupLookup",
            showEditPopupLookup: "&editPopupLookup"
        },
        template: function(element, attrs) {
            return "<label class='tool-lookup' ng-show='showAddEdit'>" +
                "<span  ng-show ='!showEdit' title='Add' class ='k-icon k-i-plus' ng-click ='AddLookup()'></span>" +
                "<span  ng-show ='showEdit' title='Edit' class ='k-icon k-i-pencil' ng-click ='EditLookup(" + ")'></span>" +
                "</label>" +
                "<input id=\"" + attrs.idLookup + "\" class='k-input' /> ";
        },
        link: function(scope, element, attrs) {
            scope.showEdit = false;
            scope.showAddEdit = attrs.showAddEdit;
            scope.enableLookup = attrs.enableLookup;
            scope.customFilterConditionKey = '';
            scope.customFilterConditionValue = '';

            var modelUrl = "";
            var customParams = [];
            if (attrs.customParams !== null && attrs.customParams !== undefined && attrs.customParams !== "") {
                customParams = JSON.parse(attrs.customParams);
            }
            scope.hierarchyGroupName = attrs.hierarchyGroupName;
            if (scope.hierarchyGroupName == undefined) {
                scope.hierarchyGroupName = "";
            }
            var urlReadData = attrs.urlReadData;
            var modelName = attrs.modelName;
            if (urlReadData != undefined && urlReadData != "") {
                modelUrl = urlReadData;
            } else {
                modelUrl = "/" + modelName + "/GetLookup";
            }
            var openDropdown = false;
            var selectedText = "";
            var parentItems = new Array();
            var clientDataSource = JSON.parse(attrs.clientDataSource);

            if (clientDataSource == null || clientDataSource.length == 0) {
                clientDataSource = [{ KeyId: 0, DisplayName: "" }];
            }
            var dataSource = undefined;
            var container = element.find('#' + attrs.idLookup);
            var serverDataSource = new kendo.data.DataSource({
                serverFiltering: true,
                type: "json",
                schema: { model: { id: "KeyId", fields: { KeyId: {}, DisplayName: {} } } },
                transport: {
                    read: {
                        url: modelUrl,
                        dataType: "json",
                        type: "GET"
                    },
                    parameterMap: function(options, type) {
                        var value = '';

                        var includeCurrentRecord = false;
                        if (options.filter !== undefined && options.filter.filters != undefined && options.filter.filters[0] != undefined && options.filter.filters[0].value != '') {
                            value = options.filter.filters[0].value;
                        }
                        var combobox = container.data("kendoComboBox");
                        var currentId = parseInt(combobox.value())>0 ? parseInt(combobox.value()) : 0;
                        
                        
                        if (openDropdown)
                            includeCurrentRecord = true;

                        var result = {
                            Id: currentId,
                            IncludeCurrentRecord: includeCurrentRecord,
                            Query: value,
                            ParentItems: JSON.stringify(parentItems),
                            Take: 150,
                            CustomeFilterKey: scope.customFilterConditionKey,
                            CustomeFilterValue: scope.customFilterConditionValue
                        };
                        $.each(customParams, function(key, val) {
                            result[key] = val;
                        });
                        //console.log(result);
                        return result;
                    }
                    //,
                    //requestEnd: function (e) {
                    //    var response = e.response;
                    //    var type = e.type;
                    //    console.log(type); // displays "read"
                    //    console.log(response.length); // displays "77"
                    //}
                }
            });
            dataSource = new kendo.data.DataSource({
                serverFiltering: true,
                data: clientDataSource
            });
            var timeoutSearch = null;
            //console.log(attrs.idLookup);
            //console.log(container);
            container.kendoComboBox({
                highlightFirst: true,
                dataTextField: "DisplayName",
                dataValueField: "KeyId",
                filter: "contains",
                autoBind: true,
                minLength: 0,
                suggest: true,
                height: 100,
                dataSource: dataSource,
                enabled: attrs.enableLookup == 'true',
                // Needed to force the combo to refresh as transport.read.cache doesn't seem to work
                open: function(e) {
                    //flagOpenLookup = true;
                    var combo = container.data("kendoComboBox");
                    combo.setDataSource(serverDataSource);
                    if (this.text() == '') { // When user clears the textbox  
                        element.parents('form').removeClass("dirty");
                        element.parents('form').addClass('dirty');
                        EnableCreateFooterButton(true);
                        container.data("kendoComboBox").value("");
                        scope.modelBinding = 0;
                        selectedText = '';
                        scope.showEdit = false;
                        element.scope().$apply();
                    }
                    // change the datasource to server whenever client clicks on the dropdown. This is for performance improvement.
                },
                change: function () {
                    //console.log("change");
                    //console.log(this.value());
                    element.parents('form').removeClass("dirty");
                    element.parents('form').addClass('dirty');
                    EnableCreateFooterButton(true);
                    if (this.selectedIndex === -1) {
                        scope.modelBinding = 0;
                        selectedText = '';
                        //container.data("kendoComboBox").value("");
                        container.data("kendoComboBox").value(null);
                        scope.showEdit = false;
                    }
                    if (_.isUndefined(this.value()) || this.value() == "" || this.value() < 1) {
                        //console.log("1"+this.value());
                        scope.modelBinding = 0;
                        selectedText = '';
                        scope.showEdit = false;
                    } else {
                        //console.log("2"+this.value());
                        scope.modelBinding = parseInt(this.value());
                        selectedText = this.text();
                        scope.showEdit = true;
                    }
                    container.data("kendoComboBox").dataSource.filter([]);
                    // Apply change value for parent scope
                    element.scope().$apply();

                    //Nghiep edit 04/07/2015
                    //flagOpenLookup = false;
                },
                select: function () {
                    container.data("kendoComboBox").dataSource.filter([]);
                },
                filtering: function (e) {
                    container.data("kendoComboBox").open();
                },
                dataBound: function (e) {
                    scope.$root.$broadcast(modelName + "_DataBound", []);
                    if (attrs.showAddEdit) {
                        $("#lookup-container-" + attrs.idLookup).find(".k-dropdown-wrap.k-state-default").css({ "padding-right": "4.1em" });
                    }
                }
            });

            container.data('kendoComboBox').wrapper.find('[role=combobox].k-input').bind('select', function(e) {
                var val = $(e.target).val();
                //console.log(val, 'select');
            });
            var flagAutoOpen = false;
            container.data('kendoComboBox').wrapper.find('[role=combobox].k-input').bind('focus', function(e) {
                var val = $(e.target).val();
                flagAutoOpen = false;
                //console.log(val, 'focus');
            });

            container.data('kendoComboBox').wrapper.find('[role=combobox].k-input').bind('keyup', function(e) {
                var val = $(e.target).val();
                if (_.isUndefined(val) || val == "") {
                    //console.log(val, 'keyup');
                    scope.showEdit = false;
                    flagAutoOpen = true;
                }

            });


            if (clientDataSource.length > 0) {
                var containerValue = clientDataSource[0].KeyId;
                if (containerValue == 0 || containerValue == undefined) {
                    containerValue = '';
                    scope.showEdit = false;
                } else {
                    scope.showEdit = true;
                }
                container.data("kendoComboBox").value(containerValue);

            }


            scope.$watch("modelBinding", function(newValue, oldValue) {
                var notifiedData = { value: newValue, text: selectedText, clearChildren: false };
                if (scope.hierarchyGroupName == undefined) scope.hierarchyGroupName = "";

                if (scope.hierarchyGroupName != "") {
                    var eventName = modelName + "_" + scope.hierarchyGroupName;
                    scope.$root.$broadcast(eventName, notifiedData);
                }
                ////Nghiep 04/07/2015
                //else {
                //    scope.$root.$broadcast(modelName, notifiedData);
                //}


            });

            function updateSelection(keyId) {
                var kendoCombo = container.data("kendoComboBox");
                if (kendoCombo == null) {
                    return;
                }
                kendoCombo.value(keyId);
                selectedText = kendoCombo.text();
                scope.modelBinding = keyId;
                //element.scope().$apply();
            }

            var containerData = null;
            var deRegisterUpdateSelectionBaseOnChildEvent = null;
            if (scope.hierarchyGroupName != "" && container.data("kendoComboBox") != undefined) {
                containerData = container.data("kendoComboBox");
                var eventNameForUpdateSelectionBaseOnChild = modelName + "_" + scope.hierarchyGroupName + "_" + "UpdateSelectionBaseOnChild";
                deRegisterUpdateSelectionBaseOnChildEvent = scope.$root.$on(eventNameForUpdateSelectionBaseOnChild, function(event, filterItem) {

                    //console.log('on:' + eventNameForUpdateSelectionBaseOnChild);

                    if (filterItem.value == undefined || filterItem.value == "")
                        return; // don't need to populate parent

                    if (scope.modelBinding != 0 && scope.modelBinding != '' && scope.modelBinding != undefined)
                        return; // don't need to populate itself when it has the selected from 1st time binding

                    // Lookup parent item
                    //var url = "/" + modelName + "/GetLookupItem";
                    var url;
                    var urlGetLookupItem = attrs.urlGetLookupItem;
                    if (urlGetLookupItem != undefined && urlReadData != "") {
                        url = urlGetLookupItem;
                    } else {
                        url = "/" + modelName + "/GetLookupItem";
                    }

                    var data = { FilterItem: JSON.stringify(filterItem) };
                    $.each(customParams, function(key, val) {
                        data[key] = val;
                    });
                    $http.post(url, data).success(function(result) {
                        if (result != undefined) {
                            var kendoCombo = containerData;
                            var selectedId = kendoCombo.value();
                            if (selectedId == result.KeyId) return;
                            var currentData = kendoCombo.dataSource.data();
                            data = [{ KeyId: result.KeyId, DisplayName: result.DisplayName }];
                            dataSource = new kendo.data.DataSource({
                                serverFiltering: true,
                                data: data
                            });
                            kendoCombo.setDataSource(dataSource);

                            updateSelection(result.KeyId);
                        }
                    }).error(function(error) {
                    });

                });
            }

            var deRegisterUpdateParentFilterEvent = null;
            if (scope.hierarchyGroupName != "") {
                var eventNameForUpdateParentFilter = modelName + "_" + scope.hierarchyGroupName + "_" + "UpdateParentFilter";
                deRegisterUpdateParentFilterEvent = scope.$root.$on(eventNameForUpdateParentFilter, function(event, filterItem) {
                    var match = null;
                    //console.log(filterItem);
                    if (filterItem.searchName == undefined)
                        return; // don't need to populate parent
                    var searchValue = filterItem.searchValue;
                    var searchName = filterItem.searchName;
                    if (searchValue == "" || searchValue == undefined)
                        searchValue = 0;

                    for (var i = 0; i < parentItems.length; i++) {
                        if (parentItems[i].name == searchName) {
                            match = parentItems[i];
                        }
                    }
                    if (match == null)
                        parentItems.push({ name: searchName, value: searchValue });
                    else
                        match.value = searchValue;
                    //Nghiep 05/05/2015
                    if (searchValue == 0) {
                        containerData.text('');
                        scope.$root.$broadcast(modelName + '_ParentValueZero', 0);
                    }
                });
            }

            var deRegisterEvent = scope.$root.$on("deRegisterEvent", function() {
                if (typeof deRegisterUpdateSelectionBaseOnChildEvent == "function") {
                    deRegisterUpdateSelectionBaseOnChildEvent();
                }
                if (typeof deRegisterUpdateParentFilterEvent == "function") {
                    deRegisterUpdateParentFilterEvent();
                }
                deRegisterEvent();
            });
            
            element.data('kendoComboBox', container.data("kendoComboBox"));
            
            scope.$on(modelName + '_Change', function (event, vals) {
               
                var kendoCombobox = container.data("kendoComboBox");
                if (vals != null && vals.length > 0 && vals[0].KeyId != 0) {
                    //container.val(vals[0].KeyId);
                    scope.modelBinding = vals[0].KeyId;
                    kendoCombobox.setDataSource(vals);
                    kendoCombobox.value(vals[0].KeyId);
                    scope.showEdit = true;
                } else {
                    //console.log(vals, modelName);
                    kendoCombobox.setDataSource(serverDataSource);
                    if (vals[0].DisplayName == '' || vals[0].DisplayName == null) {
                        kendoCombobox.value(null);
                    } else {
                        kendoCombobox.value(0);
                    }
                    

                    scope.showEdit = false;
                    //if (flagAutoOpen) {
                    //    flagAutoOpen = false;
                    //    setTimeout(function() {
                    //        //element.data('kendoComboBox').open();
                    //    }, 500);
                    //}
                }
            });
            
            scope.$on(modelName + '_ChangeDataSource', function(event, val) {
                if (val != 0) {
                    container.val(val);
                    scope.modelBinding = val;
                    var kendoCombobox = container.data("kendoComboBox");
                    kendoCombobox.setDataSource(serverDataSource);
                    kendoCombobox.value(val);
                    if (val > 0) {
                        scope.showEdit = true;
                    } else {
                        scope.showEdit = false;
                    }
                } else {
                    scope.showEdit = false;
                }
            });
            scope.AddLookup = function() {
                if (attrs.addPopupLookup != undefined && attrs.addPopupLookup != '') {
                    scope.showAddPopupLookup();
                }
            };

            scope.EditLookup = function (id) {
                if (attrs.editPopupLookup != undefined && attrs.editPopupLookup != '') {
                    scope.showEditPopupLookup();
                }

            };
            
            scope.$on(modelName + '_RemoveItem', function (event, val) {
                for (var i = 0; i < serverDataSource.data().length; i++) {
                    var itemRemove = serverDataSource.data()[i];
                    if (itemRemove.KeyId == val.KeyId) {
                        serverDataSource.remove(itemRemove);
                        break;
                    }
                }
               
            });
            
            scope.$on(modelName + '_ChangeFilterCondition', function (event, val) {
                scope.customFilterConditionKey = val.Key;
                scope.customFilterConditionValue = val.Value;
            });
        }
    };
}]);