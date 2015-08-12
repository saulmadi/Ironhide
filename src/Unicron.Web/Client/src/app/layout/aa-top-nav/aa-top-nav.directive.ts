/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../layout.module.ts" />

'use strict';
class AATopNav implements angular.IDirective {
      bindToController: boolean = true;
      controllerAs: string = 'vm';
      controller: any = AATopNavController;
      restrict: string = 'EA';
      scope: any = {
                title:  '='
            };
     templateUrl: string = 'app/layout/aa-top-nav/aa-top-nav.html';
}
appLayout.directive('aaTopNav', [() => new AATopNav()]);
