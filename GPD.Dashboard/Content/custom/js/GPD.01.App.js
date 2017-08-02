
(function () {
    'use strict';
    angular.module('GPD', ['ui.bootstrap', 'angular-loading-bar', 'ui.router', 'ngAnimate', 'toastr'])
    .config(['$stateProvider', '$urlRouterProvider','$locationProvider', 'cfpLoadingBarProvider',
        function ($stateProvider, $urlRouterProvider, $locationProvider, cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = false;
            $urlRouterProvider.otherwise('/dashboard');
            //$locationProvider.html5Mode(true);

            $stateProvider.state('GPD', {
                url: '',
                abstract: true,
                template: '<ui-view/>'
            }).state('GPD.Dashboard', {
                url: '/dashboard',
                templateUrl: __RootUrl + 'Home/Dashboard'
            }).state('GPD.Project', {
                url: '/project',
                templateUrl: __RootUrl + 'Home/Project'
            }).state('GPD.Report', {
                url: '/report',
                templateUrl: __RootUrl + 'Home/Report'
            }).state('GPD.Manage', {
                url: '/manage',
                templateUrl: __RootUrl + 'Home/Manage'
            }).state('GPD.Map', {
                url: '/map',
                templateUrl: __RootUrl + 'Home/Map'
            });

            //$urlRouterProvider.otherwise('/dashboard');
            //$stateProvider.state('GPD', {
            //    url: '',
            //    abstract: true,
            //    template: '<ui-view/>'                
            //}).state('GPD.Dashboard', {
            //    url: '/dashboard',
            //    controller: 'GPDDashboardCtrl',
            //    templateUrl: '/Home/Dashboard'
            //}).state('GPD.TopCategories', {
            //    url: '/top-categories',
            //    controller: 'GPDTopCategoriesController',
            //    templateUrl: '/Home/TopCategories'
            //});
        }]);
})();



