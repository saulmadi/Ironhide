/// <reference path="../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../typings/chai/chai.d.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../app/core/current-user.factory.ts"/>

/* tslint:disable:typedef */

describe('current-user factory', () => {
    var spyLocalStorage: any;
    var spyDeleteLocalStorage: any;
    var spyDeleteSessionStorage: any;
    var spySessionStorage: any;
    var stubSessionStorage: any;
    var stubLocalStorage: any;
    var currentUserManager: ICurrentUserManager;
    var userMock: ICurrentUser = {
        email : 'email',
        name : 'name',
        token : 'token',
        expires : getExpireDate(),
        claims: ['home', 'testing']
    };
    function getExpireDate(): string {
        'use strict';
        var date = new Date();
        date.setDate(date.getDate() + 1);
        return JSON.stringify(date);
    }
    beforeEach(function() {
       spyLocalStorage = sinon.spy(localStorage, 'setItem');
       spySessionStorage = sinon.spy(sessionStorage, 'setItem');
       stubLocalStorage = sinon.stub(localStorage, 'getItem');
       stubSessionStorage = sinon.stub(sessionStorage, 'getItem');
       spyDeleteLocalStorage = sinon.spy(localStorage, 'removeItem');
       spyDeleteSessionStorage = sinon.spy(sessionStorage, 'removeItem');
    });
    beforeEach(angular.mock.module('app.core'));
    beforeEach(inject(function(_currentUser_) {
        currentUserManager = _currentUser_;
    }));
    afterEach(function() {
        spyLocalStorage.restore();
        spySessionStorage.restore();
        stubLocalStorage.restore();
        stubSessionStorage.restore();
        spyDeleteLocalStorage.restore();
        spyDeleteSessionStorage.restore();
    });
    it('should save the user on local storage', () => {
        var testDate = new Date();
        var claims: string[] = ['home'];
        currentUserManager.SetUserLocal('email', 'name', 'token', testDate, claims);
        chai.expect(spyLocalStorage).to.have.been.called;
    });
    it('should save the user on session storage', () => {
        var testDate = new Date();
        var claims: string[] = ['home'];
        currentUserManager.SetUserOnSession('email', 'name', 'token', testDate, claims);
        chai.expect(spySessionStorage).to.have.been.called;
    });
    it('should get the user from the local storage', () => {
        stubLocalStorage.returns(JSON.stringify(userMock));
        var userSaved: ICurrentUser = currentUserManager.GetUser();
        chai.expect(userSaved).to.be.eqls(userMock);
    });
     it('should get the user from the session storage', () => {
        stubLocalStorage.returns(undefined);
        stubSessionStorage.returns(JSON.stringify(userMock));
        var userSaved: ICurrentUser = currentUserManager.GetUser();
        chai.expect(userSaved).to.be.eql(userSaved);
    });
     it('should return undefined when user does not exists', () => {
        stubLocalStorage.returns(undefined);
        stubSessionStorage.returns(undefined);
         var userSaved: ICurrentUser = currentUserManager.GetUser();
        chai.expect(userSaved).to.be.undefined;
    });
     it('should return undefined when user have been expired', () => {
        var dateExpired = new Date();
        dateExpired.setDate(dateExpired.getDate() - 1);
        userMock.expires = JSON.stringify(dateExpired);
        stubLocalStorage.returns(JSON.stringify(userMock));
        var userSaved: ICurrentUser = currentUserManager.GetUser();
        chai.expect(userSaved).to.be.undefined;
    });
    it('should delete user when have been expired', () => {
        var dateExpired = new Date();
        dateExpired.setDate(dateExpired.getDate() - 1);
        userMock.expires = JSON.stringify(dateExpired);
        stubLocalStorage.returns(JSON.stringify(userMock));
        var userSaved: ICurrentUser = currentUserManager.GetUser();
        chai.expect(userSaved).to.be.undefined;
        chai.expect(spyDeleteLocalStorage).to.have.been.called;
        chai.expect(spyDeleteSessionStorage).to.have.been.called;
    });
});
