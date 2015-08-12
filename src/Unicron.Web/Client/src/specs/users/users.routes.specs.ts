/// <reference path="../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../typings/chai/chai.d.ts" />
/// <reference path="../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/angularjs/angular-mocks.d.ts" />

describe('users.routes', () => {

    var views: any = {
        home: 'app/users/users.home.html',
        users: 'app/users/users.activate-deactivate.html',
        login: 'app/users/users.login.html'
    };
    var $state: any;
    var $templateCache: angular.ITemplateCacheService;
    var $rootScope: angular.IRootScopeService;
    beforeEach(angular.mock.module('app.users'));
    beforeEach(inject(function(_$state_: any,
        _$templateCache_: angular.ITemplateCacheService, _$rootScope_: angular.IRootScopeService) {
        $state = _$state_;
        $rootScope = _$rootScope_;
        $templateCache = _$templateCache_;
    }));
    beforeEach(function() {
        $templateCache.put(views.home, '');
        $templateCache.put(views.users, '');
        $templateCache.put(views.login, '');
    });
    it('Should map state home to url /home', function() {
        chai.expect($state.href('home', {})).to.equal('#/home');
    });
    it('Should map state users to url /users', function() {
        chai.expect($state.href('users', {})).to.equal('#/users');
    });
    it('Should map state login to url /login', function() {
        chai.expect($state.href('login', {})).to.equal('#/login');
    });
});
