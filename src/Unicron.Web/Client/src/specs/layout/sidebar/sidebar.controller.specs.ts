/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../app/layout/sidebar/sidebar.controller.ts"/>
/// <reference path="../../../app/core/current-user.factory.ts"/>

/* tslint:disable:typedef */
var expect: any = chai.expect;

describe('Sidebar', () => {
    var controller: ISidebar;
    var scope: ISidebarScope;
    var $state: angular.ui.IStateService;
    var $location: angular.ILocationService;
    var routeHelper: any;
    var $rootScope: angular.IRootScopeService;
    var user: ICurrentUser = {
        name: 'userTest',
        email: 'user@user',
        token: 'token',
        expires: 'expires',
        claims: ['test']
    };
    var stubGetUser: any;
    var currentUser: ICurrentUserManager;
    beforeEach(angular.mock.module('app.layout'));

    beforeEach(inject(function($controller: angular.IControllerService,
                _$rootScope_: angular.IRootScopeService,
                _$state_: angular.ui.IStateService,
                _$location_: angular.ILocationService,
                _routeHelper_: any, _$httpBackend_: angular.IHttpBackendService, _currentUser_: ICurrentUserManager) {
                    scope = <ISidebarScope>_$rootScope_.$new();
                    $rootScope = _$rootScope_;
                    routeHelper = _routeHelper_;
                    routeHelper.configureStates(getMockStates(), '/');
                    currentUser = _currentUser_;
                    stubGetUser = sinon.stub(currentUser, 'GetUser');
                    stubGetUser.returns(user);
                    controller = $controller('Sidebar',
                                {
                                    $scope: scope, $state: _$state_,
                                    routeHelper: _routeHelper_, currentUser: currentUser
                                });
                    $state = _$state_;
                    $location = _$location_;

                    $rootScope.$apply();

            }
        )
    );

    afterEach(function() {
       stubGetUser.restore();
    });
    beforeEach(inject(function($templateCache) {
        $templateCache.put('app/users/users/users.html', '');
        $templateCache.put('app/users/login/users.login.html', '');
    }));
    it('Should have isCurrent() for /login to return `current`', function() {
       $location.path('/login');
       $rootScope.$apply();
       expect(controller.isCurrent($state.current)).to.equal('current');
    });
   it('Should have isCurrent() for non route not return `current`', function() {
            $location.path('/invalid');
            $rootScope.$apply();
            expect(controller.isCurrent({title: 'invalid'})).not.to.equal('current');
    });
    it('Should showSideBar() if route is configured for that', function() {
        $location.path('/users');
        $rootScope.$apply();
        chai.expect(controller.showSideBar()).to.be.true;
    });
    it('Should not showSideBar() if route is configured for that', function() {
        $location.path('/login');
        $rootScope.$apply();
        chai.expect(controller.showSideBar()).to.be.false;
    });
    it('Should return user allowed routes', function() {
        var isUserLogged = controller.isUserLogged();
        var userRoutes = controller.navRoutes;
        chai.expect(isUserLogged).to.be.true;
        chai.expect(userRoutes.length).to.be.equal(1);
        chai.expect(userRoutes[0].name).to.be.equal('test');
    });
    it('Should return true isUserLogged if the user is logged', function() {
        var isUserLogged = controller.isUserLogged();
        chai.expect(isUserLogged).to.be.true;
    });
    it('Should return false isUserLogged if the user is not logged', function() {
        stubGetUser.returns(undefined);
        var isUserLogged = controller.isUserLogged();
        chai.expect(isUserLogged).to.be.false;
    });

     function getMockStates(): any {
        return [
            {
                state: 'users',
                config: {
                    url: '/users',
                    templateUrl: 'app/users/users/users.html',
                    controller: 'users.controller',
                    controllerAs: 'vm',
                    title: 'users',
                    settings: {
                            nav: 1,
                            content: '<i></i> Dashboard',
                            notShowInMenu: false,
                            notShowSideBar: false,
                            claim: 'noTest'
                        }
                }
            },
            {
                state: 'test',
                config: {
                    url: '/home',
                    templateUrl: 'app/users/home/users.home.html',
                    controller: 'users.home.controller',
                    controllerAs: 'vm',
                    title: 'Home',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-home"></i> Home',
                        claim: 'test'
                    }
                }
            },
            {
                state: 'login',
                config: {
                    url: '/login',
                    templateUrl: 'app/users/login/users.login.html',
                    controller: 'users.login.controller',
                    controllerAs: 'vm',
                    title: 'users',
                    settings: {
                        nav: 1,
                        content: '<i></i> Dashboard',
                        notShowInMenu: true,
                        notShowSideBar: true,
                        isPublic: true
                    }
                }
            }
        ];
    }
});
