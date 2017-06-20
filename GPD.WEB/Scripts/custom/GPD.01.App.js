
(function () {
    'use strict';
    angular.module('Project', ['ui.bootstrap', 'angular-loading-bar','ngAnimate', 'toastr'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
    angular.module('ManageUser', ['ui.bootstrap', 'angular-loading-bar', 'ngAnimate', 'toastr'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
    angular.module('ManagePartner', ['ui.bootstrap', 'angular-loading-bar', 'ngAnimate', 'toastr'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
})();



