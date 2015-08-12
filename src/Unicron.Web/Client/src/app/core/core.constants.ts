
/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../typings/toastr/toastr.d.ts" />
/// <reference path="../../../typings/moment/moment.d.ts"/>
/// <reference path="../../../typings/lodash/lodash.d.ts"/>
/// <reference path="core.module.ts" />
'use strict';
angular.module('app.core')
.constant('toastr', toastr);
angular.module('app.core')
.constant('moment', moment);
angular.module('app.core')
.constant('_', _);

