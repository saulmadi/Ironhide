/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../layout.module.ts" />
/* tslint:disable */
(function() {
    'use strict';

    angular
        .module('app.layout')
        .directive('aaSidebar', aaSidebar);

    /* @ngInject */
    function aaSidebar () {
        // Opens and closes the sidebar menu.
        // Usage:
        //  <div ht-sidebar">
        //  <div ht-sidebar whenDoneAnimating="vm.sidebarReady()">
        // Creates:
        //  <div ht-sidebar class="sidebar">
        var directive = {
            bindToController: true,
            link : link,
            restrict : 'EA',
            scope : {
                whenDoneAnimating: '&?'
            }
        };
        return directive;

        function link(scope: any, element: any, attrs: any) {
            var $sidebarInner = element.find('.sidebar-inner');
            var $dropdownElement = element.find('.sidebar-dropdown a');
            element.addClass('sidebar');
            $dropdownElement.click(dropdown);

            function dropdown(e: any) {
                var dropClass = 'dropy';
                e.preventDefault();
                if (!$dropdownElement.hasClass(dropClass)) {
                    $sidebarInner.slideDown(350, scope.whenDoneAnimating);
                    $dropdownElement.addClass(dropClass);
                } else if ($dropdownElement.hasClass(dropClass)) {
                    $dropdownElement.removeClass(dropClass);
                    $sidebarInner.slideUp(350, scope.whenDoneAnimating);
                }
            }
        }
    }
})();
