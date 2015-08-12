/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/logout/users.logout.service.ts"/>
/// <reference path="../../../app/layout/aa-top-nav/aa-top-nav.controller.ts"/>

/* tslint:disable:typedef */
describe('aa-top-nav.controller', () => {

    var aaTopNavController: IAATopNav;
    var $rootScope: angular.IRootScopeService;
    var scope: IAATopNavScope;
    var userLogoutService: IUserLogoutService;
    var spyLogoutService: any;

    beforeEach(angular.mock.module('app.users'));
    beforeEach(angular.mock.module('app.layout'));
    beforeEach(inject(function($controller: angular.IControllerService,
        _$rootScope_: angular.IRootScopeService, _userLogoutService_: IUserLogoutService) {
        $rootScope = _$rootScope_;
        scope = <IAATopNavScope>$rootScope.$new();
        aaTopNavController = $controller('aa-TopNavController', {$scope: scope});
        userLogoutService = _userLogoutService_;
        spyLogoutService = sinon.spy(userLogoutService, 'Logout');
    }));

    it('Should be defined', function() {
        chai.expect(aaTopNavController).not.be.undefined;
    });
    it('Shoudl call logout service', function() {
        aaTopNavController.Logout();
        chai.expect(spyLogoutService).to.have.been.called;
    });

});
