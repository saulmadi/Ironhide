/// <reference path="../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../typings/chai/chai.d.ts" />
/// <reference path="../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../app/core/current-user.factory.ts"/>
/* tslint:disable:typedef */

describe('app.module', () => {
    var $state: any;

    describe('When user has not been authenticated', () => {
        beforeEach(function() {
            angular.mock.module('app', function($provide: angular.auto.IProvideService) {
                $provide.value('$state', {
                    go: sinon.spy()
                });
                $provide.value('currentUser', {
                    GetUser: sinon.stub().returns(undefined)
                });
            });
            inject(function(_$state_: any) {
                $state = _$state_;
                });
        });

        it('Should route login on start', function() {
            chai.expect($state.go).to.have.been.calledWith('login');
        });

    });
    describe('When user has been authenticated', () => {

        beforeEach(function() {
            angular.mock.module('app', function($provide: angular.auto.IProvideService) {
                $provide.value('$state', {
                    go: sinon.spy()
                });
                $provide.value('currentUser', {
                    GetUser: sinon.stub().returns('userFaked')
                });
            });
            inject(function(_$state_: any) {
                $state = _$state_;
            });
        });

        it('Should route home on start', function() {
            chai.expect($state.go).to.have.been.calledWith('home');
        });
    });

});
