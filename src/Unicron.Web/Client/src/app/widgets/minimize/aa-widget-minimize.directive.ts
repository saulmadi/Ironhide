/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../widgets.module.ts" />
var minimize: any = function(): angular.IDirective {
    var directive: any = {
        link: link,
        template: '<i class="fa fa-chevron-up"></i>',
        restrict: 'A'
    };
    return directive;
    function link(scope: any, element: any, attrs: any): any {

        attrs.$set('href', '#');
        attrs.$set('wminimize');
        element.click(minimize);

        function minimize(e: any): any {
            e.preventDefault();
            var $wcontent: any = element.parent().parent().next('.widget-content');
            var iElement: any = element.children('i');
            if ($wcontent.is(':visible')) {
                iElement.removeClass('fa fa-chevron-up');
                iElement.addClass('fa fa-chevron-down');
            } else {
                iElement.removeClass('fa fa-chevron-down');
                iElement.addClass('fa fa-chevron-up');
            }
            $wcontent.toggle(500);
        }
    }
};

appWidget.directive('aaWidgetMinimize', minimize);
