/// <reference path="../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../typings/chai/chai.d.ts" />
/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../typings/toastr/toastr.d.ts" />
/// <reference path="../../../typings/moment/moment.d.ts"/>
/// <reference path="../../../typings/lodash/lodash.d.ts"/>
/// <reference path="../../app/core/core.constants.ts"/>

describe('core.constants', () => {
    beforeEach(angular.mock.module('app.core'));

    it('Should toastr be defined', inject(function(toastr: Toastr) {
        chai.expect(toastr).to.be.not.undefined;
    }));
    it('Should moment be defined', inject(function(_moment_: moment.Moment) {
        chai.expect(_moment_).to.be.not.undefined;
    }));
    it('Should lodash be defined', inject(function(_: _.LoDashStatic) {
        chai.expect(_).to.be.not.undefined;
    }));

});
