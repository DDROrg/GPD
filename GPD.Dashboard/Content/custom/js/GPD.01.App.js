
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
            }).state('GPD.Dashboard', {
                url: '/dashboard',
                templateUrl: '/Home/Dashboard'
            }).state('GPD.Project', {
                url: '/project',
                templateUrl: '/Home/Project'
            }).state('GPD.Report', {
                url: '/report',
                templateUrl: '/Home/Report'
            }).state('GPD.Manage', {
                url: '/manage',
                templateUrl: '/Home/Manage'
            }).state('GPD.Map', {
                url: '/map',
                templateUrl: '/Home/Map'
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



