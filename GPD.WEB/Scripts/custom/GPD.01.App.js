﻿
(function () {
    'use strict';
    angular.module('Project', ['ui.bootstrap', 'angular-loading-bar', 'ui.router', 'ngAnimate', 'cp.ngConfirm', 'toastr'])
    .config(['$stateProvider', '$urlRouterProvider', 'cfpLoadingBarProvider',
        function ($stateProvider, $urlRouterProvider, cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = false;
            $urlRouterProvider.otherwise('/list');

            $stateProvider.state('project', {
                url: '',
                abstract: true,
                template: '<ui-view/>'                
            }).state('project.list', {
                url: '/list',
                templateUrl: __RootUrl + 'Home/ProjectList',
                params: { cachedData: null }
            }).state('project.edit', {
                url: '/edit/{id}',
                templateUrl: __RootUrl + 'Home/ProjectEdit',
                params: { id: null, project: null, cachedData: null }
                //,
                //resolve: {
                //    project: ['$stateParams', function ($stateParams) {
                //        return $stateParams.project;
                //    }]
                //}
            });
        }]);

    angular.module('ManageUser', ['ui.bootstrap', 'angular-loading-bar', 'ngAnimate', 'toastr'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
    angular.module('ManagePartner', ['ui.bootstrap', 'angular-loading-bar', 'ngAnimate', 'toastr'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
    angular.module('RegisterUser', ['ui.bootstrap', 'angular-loading-bar', 'ngAnimate', 'toastr'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
})();



