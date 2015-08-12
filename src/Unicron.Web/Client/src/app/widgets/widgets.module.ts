
/// <reference path="../../../typings/angularjs/angular.d.ts" />


interface IAppWidgets extends angular.IModule { }

// Create the module and define its dependencies.
var appWidget: IAppWidgets = angular.module('app.widgets', [
    // Angular modules
    // Custom modules
    // 3rd Party Modules
]);
// Execute bootstrapping code and any dependencies.
appWidget.run([ () => {
}]);
