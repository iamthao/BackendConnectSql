(function() {
    'use strict';

    // Define the common module 
    // Contains services:
    //  - common
    //  - logger
    //  - spinner
    var commonModule = angular.module('common', []);

    // Must configure the common service and set its 
    // events via the commonConfigProvider
    commonModule.provider('commonConfig', function() {
        this.config = {
            // These are the properties we need to set
            //controllerActivateSuccessEvent: '',
            //spinnerToggleEvent: ''
            
        };

        this.$get = function() {
            return {
                config: this.config
            };
        };
    });

    commonModule.factory('common',
        ['$q', '$rootScope', '$timeout', 'commonConfig', 'logger', common]);

    function common($q, $rootScope, $timeout, commonConfig, logger) {
        var throttles = {};
        var service = {
            // common angular dependencies
            $broadcast: $broadcast,
            $q: $q,
            $timeout: $timeout,
            // generic
            activateController: activateController,
            createSearchThrottle: createSearchThrottle,
            debouncedThrottle: debouncedThrottle,
            isNumber: isNumber,
            logger: logger, // for accessibility
            textContains: textContains,
            showErrorModelState: showErrorModelState,
            formSaveDataEvent: formSaveDataEvent,
            formCancelDataEvent: formCancelDataEvent,
            showPopupKendo: showPopupKendo,
            randomNumber: randomNumber,
            getValueOfTime: getValueOfTime,
            getValueOfTimeAMPM:getValueOfTimeAMPM,
            generateCrontabString: generateCrontabString,
            getFirstDayInWeek: getFirstDayInWeek,
            getLastDayInWeek: getLastDayInWeek,
            getFirstDayInMonth: getFirstDayInMonth,
            getLastDayInMonth: getLastDayInMonth,
            bootboxConfirm: bootboxConfirm,
            getBoundsZoomLevel: getBoundsZoomLevel,
            setCookie: setCookie,
            getCookie: getCookie
        };

        return service;

        function formSaveDataEvent(controllerId) {
            var data = { controllerId: controllerId };
            $broadcast(commonConfig.config.controllerFormSaveDataEvent, data);
        }

        function formCancelDataEvent(controllerId) {
            var data = { controllerId: controllerId };
            $broadcast(commonConfig.config.controllerFormCancelSaveDataEvent, data);
        }

        function activateController(promises, controllerId) {
            return $q.all(promises).then(function(eventArgs) {
                var data = { controllerId: controllerId };
                $broadcast(commonConfig.config.controllerActivateSuccessEvent, data);
            });
        }

        function showErrorModelState(feedback) {
            toastr.clear();
            // Build the error message
            $.get('/ejsTemplate/errorHandlerTemplate.html', function(html) {
                var errorMessage = _.template(html, {
                    data: feedback
                });
                toastr.options.closeButton = true;
                toastr.options.timeOut = 5000;
                toastr.options.extendedTimeOut = 5000;
                if (errorMessage.indexOf('ErrorWarning') < 0)
                toastr.error(errorMessage);
                //toastr.options.closeButton = false;
                //toastr.options.timeOut = 4000;
                //toastr.options.extendedTimeOut = 1000;
            }, 'html');

        }

        function $broadcast() {
            return $rootScope.$broadcast.apply($rootScope, arguments);
        }

        function createSearchThrottle(viewmodel, list, filteredList, filter, delay) {
            // After a delay, search a viewmodel's list using 
            // a filter function, and return a filteredList.

            // custom delay or use default
            delay = +delay || 300;
            // if only vm and list parameters were passed, set others by naming convention 
            if (!filteredList) {
                // assuming list is named sessions, filteredList is filteredSessions
                filteredList = 'filtered' + list[0].toUpperCase() + list.substr(1).toLowerCase(); // string
                // filter function is named sessionFilter
                filter = list + 'Filter'; // function in string form
            }

            // create the filtering function we will call from here
            var filterFn = function() {
                // translates to ...
                // vm.filteredSessions 
                //      = vm.sessions.filter(function(item( { returns vm.sessionFilter (item) } );
                viewmodel[filteredList] = viewmodel[list].filter(function(item) {
                    return viewmodel[filter](item);
                });
            };

            return (function() {
                // Wrapped in outer IFFE so we can use closure 
                // over filterInputTimeout which references the timeout
                var filterInputTimeout;

                // return what becomes the 'applyFilter' function in the controller
                return function(searchNow) {
                    if (filterInputTimeout) {
                        $timeout.cancel(filterInputTimeout);
                        filterInputTimeout = null;
                    }
                    if (searchNow || !delay) {
                        filterFn();
                    } else {
                        filterInputTimeout = $timeout(filterFn, delay);
                    }
                };
            })();
        }

        function debouncedThrottle(key, callback, delay, immediate) {
            // Perform some action (callback) after a delay. 
            // Track the callback by key, so if the same callback 
            // is issued again, restart the delay.

            var defaultDelay = 1000;
            delay = delay || defaultDelay;
            if (throttles[key]) {
                $timeout.cancel(throttles[key]);
                throttles[key] = undefined;
            }
            if (immediate) {
                callback();
            } else {
                throttles[key] = $timeout(callback, delay);
            }
        }

        function isNumber(val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        }

        function textContains(text, searchText) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }

        function randomNumber() {
            return Math.floor((Math.random() * 1000000) + 1);
        }

        function bootboxConfirm(msg, callbackSuccess, callbackCancel) {
            var d = bootbox.confirm({
                message: msg,
                show: false,
                className: "bootbox-small",
                callback: function (result) {
                    if (result)
                        callbackSuccess();
                    else if (typeof (callbackCancel) == 'function')
                        callbackCancel();
                }
            });

            d.on("show.bs.modal", function () {
                $(".bootbox-small").css({ "padding-top": ($(window).height() - 200) / 2, "width": "100%","height":"100%" });
            });
            return d;
        }
        function showPopupKendo(width, height, title, content, isModal) {
            width = !isNumber(width) ? width + "px" : width;
            height = !isNumber(height) ? height + "px" : height;
            isModal = typeof isModal == "boolean" ? isModal : true;

            var elementId = "kendo-window-popup-" + randomNumber();

            $('#popupContainer').append('<div id="' + elementId + '"></div>');

            var window = $('#' + elementId);
            if (_.isUndefined(window.data("kendoWindow"))) {
                window.kendoWindow({
                    width: width,
                    height: height,
                    title: title,
                    content: content,
                    actions: [
                        "Close"
                    ],
                    visible: true,
                    draggable: false,
                    modal: isModal,
                    pinned: false,
                    resizable: false,
                    open: function() {
                        this.center();
                    },
                    close: function() {
                        this.destroy();
                    }
                });
            } else {
                window.data("kendoWindow").open();
            }
        }

        function getValueOfTime(newValue) {
            if (newValue.indexOf("_") > -1) {
                return "";
            } else {
                var value = newValue.split(":");
                if (value[0] >= 24) {
                    value[0] = 23;
                    value[1] = 59;
                }
                if (value[1] >= 60) {
                    value[1] = 59;
                }
                return value[0] + ":" + value[1];
            }
        }
        function getValueOfTimeAMPM(newValue) {
            if (newValue.indexOf("_") > -1) {
                return "";
            } else {
                var arr = newValue.split(":");
                if (arr.length == 2) {
                    var arr1 = arr[1].split(" ");
                    if (arr1.length == 2) {
                        if (((parseInt(arr[0]) == 12 && parseInt(arr1[0]) > 59) || (parseInt(arr[0]) > 12)) && arr1[1] == "PM") {
                            return "11:59 PM";
                        }
                        if (((parseInt(arr[0]) == 12 && parseInt(arr1[0]) > 59) || (parseInt(arr[0]) > 12)) && arr1[1] == "AM") {
                            return "11:59 AM";
                        }
                        return newValue;
                    } else {
                        return "";
                    }



                    //var tempSending = $scope.sendData.SendingTime.split(":");
                    //var hourAddSending = parseInt(tempSending[0]);
                    //var secondAddSending = parseInt(tempSending[1].split(" ")[0]);
                    //if (tempSending[1].split(" ")[1] == 'PM') {
                    //    hourAddSending += 12;
                    //}
                    //if (tempSending[1].split(" ")[1] == 'AM' && hourAddSending == 12) {
                    //    hourAddSending = 0;
                    //}


                    //if (value[0] >= 24) {
                    //    value[0] = 23;
                    //    value[1] = 59;
                    //}
                    //if (value[1] >= 60) {
                    //    value[1] = 59;
                    //}
                    //return value[0] + ":" + value[1];
                } else {
                    {
                        return "";
                    }
                }
            }
        }
        function generateCrontabString(frequency, daysOfWeek, daysOfMonth, hour, minute, second) {
            var result = '';
            if (frequency !== undefined && frequency !== '') {
                switch (frequency.toLowerCase()) {
                    case 'daily':
                        result = second + ' ' + minute + ' ' + hour + ' * * ?';
                        break;
                    case 'weekly':
                        result = second + ' ' + minute + ' ' + hour + ' ? * ' + daysOfWeek;
                        break;
                    case 'monthly':
                        result = second + ' ' + minute + ' ' + hour + ' ' + daysOfMonth + ' * ?';
                        break;
                    default:
                        result = '';
                }
            }
            return result;
        }
        function getFirstDayInWeek(curr) {
            var first = curr.getDate() - curr.getDay(); // First day is the day of the month - the day of the week
            var date = new Date(curr.setDate(first));
            return date;

        }
        function getLastDayInWeek(curr) {
            var first = curr.getDate() - curr.getDay(); // First day is the day of the month - the day of the week
            var last = first + 6; // last day is the first day + 6

            var date = new Date(curr.setDate(last));
            return date;

        }

        function getFirstDayInMonth(curr) {
            return  new Date(curr.getFullYear(), curr.getMonth(), 1);

        }
        function getLastDayInMonth(curr) {
            return new Date(curr.getFullYear(), curr.getMonth()+1, 0);

        }
        function getBoundsZoomLevel(bounds, map) {
            if (map.mapTypes.get(map.getMapTypeId()) == undefined || map.mapTypes.get(map.getMapTypeId()) == undefined) {
                return 11;
            }
            var MAX_ZOOM = map.mapTypes.get(map.getMapTypeId()).maxZoom || 21;
            var MIN_ZOOM = map.mapTypes.get(map.getMapTypeId()).minZoom || 0;

            var ne = map.getProjection().fromLatLngToPoint(bounds.getNorthEast());
            var sw = map.getProjection().fromLatLngToPoint(bounds.getSouthWest());

            var worldCoordWidth = Math.abs(ne.x - sw.x);
            var worldCoordHeight = Math.abs(ne.y - sw.y);

            //Fit padding in pixels 
            var FIT_PAD = 40;

            for (var zoom = MAX_ZOOM; zoom >= MIN_ZOOM; --zoom) {
                if (worldCoordWidth * (1 << zoom) + 2 * FIT_PAD < $(map.getDiv()).width() &&
                    worldCoordHeight * (1 << zoom) + 2 * FIT_PAD < $(map.getDiv()).height())
                    return zoom;
            }
            return 0;
        }

        function setCookie(c_name, value, exdays) {
            var exdate = new Date();
            exdate.setDate(exdate.getDate() + exdays);
            var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
            document.cookie = c_name + "=" + c_value + ";domain=;path=/";
        }
        function getCookie(c_name) {
            var i, x, y, ARRcookies = document.cookie.split(";");
            for (i = 0; i < ARRcookies.length; i++) {
                x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
                y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
                x = x.replace(/^\s+|\s+$/g, "");
                if (x == c_name) {
                    return unescape(y);
                }
            }
            return "";
        }
    }
})();