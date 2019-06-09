'use strict';
app.directive('popup', ['$window', function () {
    return {
        restrict: "E",
        link: function (scope, element, attrs) {
            var optionsBind = attrs.kOptionsBind;
            //console.log(optionsBind);
            scope[optionsBind] = {
                width: 500,
                height: 200,
                title: "",
                content: {
                    url: ""
                },
                actions: [
                    "Close"
                ],
                visible: false,
                draggable: false,
                modal: true,
                pinned: false,
                resizable: false,
                open: function () {
                    this.refresh().center();
                },
                close: function () {
                    scope.$root.$broadcast("closePopupRegisterEvent_" + optionsBind);
                    this.content("");
                }
            };
        }
    }
}]);

app.directive('compileHtml', ["$compile", "$parse", function ($compile, $parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $compile(element.contents())(scope);
        }
    }
}]);