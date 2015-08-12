/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../app/users/logout/users.logout.service.ts"/>
/// <reference path="../../../app/core/current-user.factory.ts"/>
/* tslint:disable:typedef */

describe('users.logout.service', () => {
    var userLogoutService: IUserLogoutService;
    var currentUser: ICurrentUserManager;
    var $state: any;
    beforeEach(angular.mock.module('app.users'));

    beforeEach(inject(function(_currentUser_: ICurrentUserManager,
            _userLogoutService_: IUserLogoutService, _$state_: any) {
        currentUser = _currentUser_;
        currentUser.RemoveUser = sinon.spy();
        userLogoutService = _userLogoutService_;
        $state = _$state_;
        $state.go = sinon.spy();
    }));
    it('Should remove user from session and local storage', function() {
        userLogoutService.Logout();
        chai.expect(currentUser.RemoveUser).has.been.called;
    });
    it('Should redirect to login', function() {
        userLogoutService.Logout();
        chai.expect($state.go).to.have.been.calledWith('login');
    });

});
