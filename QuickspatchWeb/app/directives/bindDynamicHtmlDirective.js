app.directive('bindDynamicHtml', ['$compile', function ($compile) {
    return function (scope, element, attrs) {
        //console.log("A");
        scope.$watch(
          function (scope) {
              return scope.$eval(attrs.bindDynamicHtml);
          },
          function (value) {
              //console.log("B");
              element.html(value);
              $compile(element.contents())(scope);
          }
      );
    };
}]);