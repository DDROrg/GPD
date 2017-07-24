
(function () {
    'use strict';
    angular.module('GPD', ['ui.bootstrap', 'angular-loading-bar', 'ui.router', 'ngAnimate', 'toastr'])
    .config(['$stateProvider', '$urlRouterProvider', 'cfpLoadingBarProvider',
        function ($stateProvider, $urlRouterProvider, cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = false;
            $urlRouterProvider.otherwise('/dashboard');

            $stateProvider.state('GPD', {
                url: '',
                abstract: true,
                template: '<ui-view/>'                
            }).state('GPD.dashboard', {
                url: '/dashboard',
                controller: 'GPDDashboardController',
                templateUrl: '/Home/Dashboard'
            }).state('GPD.TopCategories', {
                url: '/top-categories',
                controller: 'GPDTopCategoriesController',
                templateUrl: '/Home/TopCategories'
            });
        }]);
})();



