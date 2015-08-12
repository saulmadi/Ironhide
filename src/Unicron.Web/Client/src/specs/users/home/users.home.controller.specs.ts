/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/home/users.home.controller.ts"/>
/* tslint:disable:typedef */
describe('users.home.controller', () => {

    var homeController: IHome;
    var $rootScope: angular.IRootScopeService;
    var scope: IHomeScope;

    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function($controller: angular.IControllerService, _$rootScope_: angular.IRootScopeService) {
        $rootScope = _$rootScope_;
        scope = <IHomeScope>$rootScope.$new();
        homeController = $controller('users.home.controller', {$scope: scope});
    }));

    it('Should be defined', function() {
       chai.expect(homeController).to.be.not.undefined;
    });
});
