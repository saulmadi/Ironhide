/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../widgets.module.ts" />

var compareTo: any = function(): angular.IDirective {
return {
        require: 'ngModel',
        scope: {
                otherModelValue: '=compareTo'
        },
        link: function(scope: any, element: any, attributes: any, ngModel: any): any {

            ngModel.$validators.compareTo = function(modelValue: any): any {
                    return modelValue === scope.otherModelValue;
            };

            scope.$watch('otherModelValue', function(): any {
                ngModel.$validate();
            });
        }
    };
};

appWidget.directive('compareTo', compareTo);
