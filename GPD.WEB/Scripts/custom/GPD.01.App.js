
(function () {
    'use strict';
    angular.module('Project', ['ui.bootstrap', 'angular-loading-bar'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
})();



