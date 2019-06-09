'use strict';
app.controller('scheduleShareController', ['$rootScope', '$scope', 'common', '$timeout', '$interval',
    function ($rootScope, $scope, common, $timeout, $interval) {
        $scope.controllerId = "scheduleShareController";

        $scope.Schedule = new ScheduleViewModel();
        
        $scope.FrequencyWeek = {};
        $scope.FrequencyDaysOfMonthArray = [];

        initData();
        
        function initData() {
            if ($scope.Schedule.FrequencyMonthString != undefined && $scope.Schedule.FrequencyMonthString != '') {
                var monthArray = $scope.Schedule.FrequencyMonthString.split(",");
                _.each(monthArray, function (element, index) {
                    $scope.FrequencyDaysOfMonthArray.push(element);
                });
            }
            if ($scope.Schedule.FrequencyWeekString != undefined && $scope.Schedule.FrequencyWeekString != '') {
                var weekArray = $scope.Schedule.FrequencyWeekString.split(",");
                _.each(weekArray, function (element, index) {
                    $scope.FrequencyWeek[element] = true;
                });
            }
            
            switch ($scope.Schedule.FrequencySelected) {
                case "daily":
                    if ($("#frequency-weekly").is(":visible"))
                        $("#frequency-weekly").slideUp(400);
                    if ($("#frequency-monthly").is(":visible"))
                        $("#frequency-monthly").slideUp(400);
                    break;
                case "weekly":
                    if (!$("#frequency-weekly").is(":visible"))
                        $("#frequency-weekly").slideDown(400);
                    if ($("#frequency-monthly").is(":visible"))
                        $("#frequency-monthly").slideUp(400);
                    break;
                case "monthly":
                    if ($("#frequency-weekly").is(":visible"))
                        $("#frequency-weekly").slideUp(400);
                    if (!$("#frequency-monthly").is(":visible"))
                        $("#frequency-monthly").slideDown(400);
                    break;
            }
        };

        $scope.ChangeFrequencyWeek = function () {
            EnableCreateFooterButton(true);
        };
        
        $scope.$watch("Schedule.IsNoDurationEnd", function (newval, oldval) {
            if (newval != oldval) {
                var durationEnd = $("#DurationEnd").data("kendoDatePicker");
                if ($scope.Schedule.IsNoDurationEnd == true) {
                    $scope.Schedule.DurationEnd = '';
                    durationEnd.enable(false);
                } else {
                    durationEnd.enable();
                }
            }
        });

        $scope.$watch("Schedule.StartTime", function (newValue, oldValue) {
            if (newValue != oldValue) {
                EnableCreateFooterButton(true);
            }
        });
        $scope.$watch("Schedule.EndTime", function (newValue, oldValue) {
            if (newValue != oldValue) {
                EnableCreateFooterButton(true);
            }
        });
        $scope.getShareViewData = function () {
            //collect days of week
            var frequencyDaysOfWeekArray = [];
            _.each($scope.FrequencyWeek, function (element, index) {
                if (element) {
                    frequencyDaysOfWeekArray.push(index);
                }
            });

            //collect days of month
            var frequencyDaysOfMonthArray = [];
            _.each($scope.FrequencyDaysOfMonthArray, function (element) {
                if (element) {
                    frequencyDaysOfMonthArray.push(element);
                }
            });

            $scope.Frequency = common.generateCrontabString($scope.Schedule.FrequencySelected, frequencyDaysOfWeekArray.join(), frequencyDaysOfMonthArray.join(), 23, 59, 59);
            var currentDate = new Date();
            var endOfDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 23, 59, 59);
            var startTime = '';
            var endTime = '';
      
            convertUtcTime();
            startTime = $scope.Schedule.StartTime;
            endTime = $scope.Schedule.EndTime; 
            var param = {
                Id: $scope.Schedule.Id,
                Name: $scope.Schedule.RouteName,
                LocationFrom: $scope.Schedule.LocationFrom,
                LocationTo: $scope.Schedule.LocationTo,
                Frequency: $scope.Frequency,
                CourierId: $scope.$root.CourierId,
                StartTime: startTime,
                EndTime: endTime,
                DurationStart: $scope.Schedule.DurationStart,
                DurationEnd: $scope.Schedule.DurationEnd,
                IsNoDurationEnd: $scope.Schedule.IsNoDurationEnd,
                Description: $scope.Schedule.Description,
                Confirm: $scope.Schedule.Confirm
                //ExpiredTime: Math.round((endOfDate - currentDate) / 1000)
            };
           
            return { SharedParameter: JSON.stringify(param) };
        };
        $scope.GenerateArray = function (min, max) {
            var input = [];
            for (var i = min; i <= max; i++) input.push(i);
            return input;
        };
        
        $scope.DayOfWeekArray = [
            {
                ShortText: "MON",
                FullText: "Monday"
            },
            {
                ShortText: "TUE",
                FullText: "Tuesday"
            },
            {
                ShortText: "WED",
                FullText: "Wednesday"
            },
            {
                ShortText: "THU",
                FullText: "Thursday"
            },
            {
                ShortText: "FRI",
                FullText: "Friday"
            },
            {
                ShortText: "SAT",
                FullText: "Saturday"
            },
            {
                ShortText: "SUN",
                FullText: "Sunday"
            }
        ];
        
        
        $scope.changeSwitch = function (event) {
            EnableCreateFooterButton(true);
            var value = angular.element(event.target).text();
            var $el = $(angular.element(event.target));
            if ($el.hasClass("switch-on")) {
                $el.removeClass("switch-on");
                var index = $scope.FrequencyDaysOfMonthArray.indexOf(value);
                if (index > -1) {
                    $scope.FrequencyDaysOfMonthArray.splice(index, 1);
                }
            } else {
                $el.addClass("switch-on");
                $scope.FrequencyDaysOfMonthArray.push(value);
            }
        };

        function convertUtcTime() {
            //Thao         
            //TH tra ve 02:03 PM
            if ($scope.Schedule.StartTime.indexOf('AM') > 0 || $scope.Schedule.StartTime.indexOf('PM') > 0) {
                var tempStart = $scope.Schedule.StartTime.split(":");
                var hourAddStart = parseInt(tempStart[0]);
                var secondAddStart = parseInt(tempStart[1].split(" ")[0]);

                if (tempStart[1].split(" ")[1] == 'PM' && hourAddStart != 12) {
                    hourAddStart += 12;
                }
                if (tempStart[1].split(" ")[1] == 'AM' && hourAddStart == 12) {
                    hourAddStart = 0;
                }

                $scope.Schedule.StartTime = (new Date((new Date()).setHours(hourAddStart, secondAddStart))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.Schedule.StartTime != '' && $scope.Schedule.StartTime != undefined) {
                var a = new Date(Date.parse($scope.Schedule.StartTime));
                $scope.Schedule.StartTime = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();
            }

            if ($scope.Schedule.EndTime.indexOf('AM') > 0 || $scope.Schedule.EndTime.indexOf('PM') > 0) {
                var tempStart1 = $scope.Schedule.EndTime.split(":");
                var hourAddStart1 = parseInt(tempStart1[0]);
                var secondAddStart1 = parseInt(tempStart1[1].split(" ")[0]);

                if (tempStart1[1].split(" ")[1] == 'PM' && hourAddStart1 != 12) {
                    hourAddStart1 += 12;
                }
                if (tempStart1[1].split(" ")[1] == 'AM' && hourAddStart1 == 12) {
                    hourAddStart1 = 0;
                }

                $scope.Schedule.EndTime = (new Date((new Date()).setHours(hourAddStart1, secondAddStart1))).toUTCString();
            }
                //TH : tra ve 05/05/2016 14:03
            else if ($scope.Schedule.EndTime != '' && $scope.Schedule.EndTime != undefined) {
                var a = new Date(Date.parse($scope.Schedule.EndTime));
                $scope.Schedule.EndTime = (new Date((new Date()).setHours(a.getHours(), a.getMinutes()))).toUTCString();
            }
        }

        var intrval = $interval(function () {
            var startCtr = $('#start-hour').data('kendoTimePicker');
            var endCtr = $('#end-hour').data('kendoTimePicker');
            var initControlOk = true;
            initControlOk &= startCtr != undefined;
            initControlOk &= endCtr != undefined;
            if (initControlOk) {
                $timeout(function () {
                    $scope.$apply(function () {

                    });
                });
                startCtr.value($scope.Schedule.StartTime != null && $scope.Schedule.StartTime != '' ? moment($scope.Schedule.StartTime, 'HH:mm a').format('h:mm A') : $scope.Schedule.StartTime);
                endCtr.value($scope.Schedule.EndTime != null && $scope.Schedule.EndTime != '' ? moment($scope.Schedule.EndTime, 'HH:mm a').format('h:mm A') : $scope.Schedule.EndTime);

                $timeout(function () {
                    $scope.$apply(function () {
                        var durationEnd = $("#DurationEnd").data("kendoDatePicker");
                        if ($scope.Schedule.IsNoDurationEnd == true) {
                            $scope.Schedule.DurationEnd = '';
                            durationEnd.enable(false);
                        } else {
                            durationEnd.enable();
                        }
                    });
                });

                $interval.cancel(intrval);
            }
        }, 200);

        function checkdate(input) {
            var formats = ['MM/DD/YYYY'];
            return moment(input, formats).isValid();
        }
        $scope.$watch("Schedule.DurationStart", function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (!checkdate($scope.Schedule.DurationStart)) {
                    $scope.Schedule.DurationStart = "";
                }
            }
        });

        $scope.$watch("Schedule.DurationEnd", function (newValue, oldValue) {
            if (newValue != oldValue && $scope.Schedule.IsNoDurationEnd == false) {
                if (!checkdate($scope.Schedule.DurationEnd)) {
                    $scope.Schedule.DurationEnd = "";
                }
            }
        });
    }]
);