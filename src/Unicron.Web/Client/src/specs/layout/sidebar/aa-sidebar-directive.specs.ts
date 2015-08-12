/// <reference path="../../../../typings/mocha/mocha.d.ts" />
/// <reference path="../../../../typings/chai/chai.d.ts" />
/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../../../../typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../../../../typings/sinon/sinon.d.ts" />
/// <reference path="../../../../typings/sinon-chai/sinon-chai.d.ts" />
/// <reference path="../../../app/layout/sidebar/aa-sidebar-directive.ts"/>
/* tslint:disable:typedef */

describe('aaSidebar directive ', () => {
    var dropdownElement: any;
    var e1: any;
    var innerElement: any;
    var isOpenClass = 'dropy';
    var $compile: angular.ICompileService;
    var $rootScope: angular.IRootScopeService;

    beforeEach(angular.mock.module('app.layout'));
    beforeEach(inject(function(_$compile_: angular.ICompileService, _$rootScope_: angular.IRootScopeService) {
        $compile = _$compile_;
        $rootScope = _$rootScope_;
        e1 = angular.element('<aa-sidebar whenDoneAnimating="vm.sidebarReady(42)" > \
                <div class="sidebar-dropdown"><a href="">Menu</a></div> \
                <div class="sidebar-inner" style="display: none"></div> \
            </aa-sidebar>');
        dropdownElement = e1.find('.sidebar-dropdown a');
        innerElement = e1.find('.sidebar-inner');
        $compile(e1)($rootScope);
        $rootScope.$digest();
    }));
    describe('the isOpenClass', () => {
        it('is absent for a closed menu', function () {
            hasIsOpenClass(false);
        });
        it('is added to a closed menu after clicking', function () {
            clickIt();
            hasIsOpenClass(true);
        });
        it('is present for an open menu', function () {
            openDropdown();
            hasIsOpenClass(true);
        });
        it('is removed from a closed menu after clicking', function () {
            openDropdown();
            clickIt();
            hasIsOpenClass(false);
        });

    });
    describe('when animating w/ jQuery fx off', () => {
        var oldFxOff: any;
        beforeEach(function() {
            oldFxOff = $.fx.off;
            $.fx.off = true;
            e1.appendTo(document.body);
        });
        afterEach(function () {
            $.fx.off = this.oldFxOff;
            e1.remove();
        });
        it('dropdown is visible after opening a closed menu', function () {
            dropdownIsVisible(false); // hidden before click
            clickIt();
            dropdownIsVisible(true); // visible after click
        });
        it('dropdown is hidden after closing an open menu', function () {
            openDropdown();
            dropdownIsVisible(true); // visible before click
            clickIt();
            dropdownIsVisible(false); // hidden after click
        });

    });
    function openDropdown() {
        dropdownElement.addClass(isOpenClass);
        innerElement.css('display', 'block');
    }
    function hasIsOpenClass(isTrue: boolean): void {
        var hasClass = dropdownElement.hasClass(isOpenClass);
        chai.expect(hasClass).equal(!!isTrue,
            'dropdown has the "is open" class is ' + hasClass);
    }
    function clickIt(): void {
        dropdownElement.trigger('click');
    }
    function dropdownIsVisible(isTrue) {
        var display = innerElement.css('display');
        chai.expect(display).to.equal(isTrue ? 'block' : 'none',
            'innerElement display value is ' + display);
    }

});
