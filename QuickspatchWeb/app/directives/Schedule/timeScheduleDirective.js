'use strict';
app.directive('timeSchedule', ['$q', '$http', '$timeout', function ($q, $http, $timeout) {
    return {
        restrict: "E",
        scope: true,
        template: function (element, attrs) {
            return '<input style="width:100%" type="text" class="date-picker-100" placeholder="7:30 PM" id="' + attrs["timeId"] + '">';
        },
        link: function ($scope, $element, $attrs) {

            function setValue(ele) {
               
                var val = '', result = '', bind='';
                var dateobj = null;
                if (ele.val() != '') {
                    dateobj = moment(ele.val(), 'HH:mm', true);
                    if (!dateobj.isValid()) {
                        dateobj = moment(ele.val(), 'h:mm', true);
                        if (!dateobj.isValid()) {
                            dateobj = moment(ele.val(), 'h:m A', true);
                            if (!dateobj.isValid()) {
                                dateobj = moment(ele.val(), 'h:mA', true);
                                if (!dateobj.isValid()) {
                                    dateobj = moment(ele.val(), 'hh:mA', true);
                                }
                            }
                        }
                    }

                    val = dateobj.format('h:mm A');
                    result = val;
                    if (dateobj.isValid()) {
                        bind = dateobj.format('MM/DD/YYYY HH:mm');
                    } else {
                        result = "Invalid time";
                    }
                }

                ele.val(result);
                $timeout(function () {
                    $scope.$apply(function () {
                        if ($attrs["bindObj"] == 'Schedule') {
                            if ($attrs["bindVal"] == 'StartTime') {
                                $scope.$parent.Schedule[$attrs["bindVal"]] = bind;
                            }
                            if ($attrs["bindVal"] == 'EndTime') {
                                $scope.$parent.Schedule[$attrs["bindVal"]] = bind;
                            }
                        }                       
                    });
                });
            }

            
           

            $("#" + $attrs["timeId"]).kendoTimePicker({
                change: function (e) {
                    setValue($(e.sender.element[0]));
                }
            });
            $("#" + $attrs["timeId"]).bind('blur', function (e) {
                setValue($(this));
            });
            $("#" + $attrs["timeId"]).focusin(function (e) {
                $(this).select();
            });         
        }
    }
}]);