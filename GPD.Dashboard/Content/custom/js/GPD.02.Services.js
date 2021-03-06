﻿//============================================
var CommonServices = function ($http, $httpParamSerializer, $q, $log, BroadcastService) {
    var _chartColor = ['#ff0000', '#ff6a00', '#ffd800', '#b6ff00', '#4cff00', '#5f798d', '#0094ff', '#0000ff'];

    this.LogedinUserProfile = { userId: "", firstName: "", lastName: "", email: "", partnerNames: [], selectedPartner: "", selectedMenu: "" };

    var _GetLogedinUserProfile = function () {
        return $http.post(__RootUrl + "api/GetUserProfile?userId=" + __UserId);
    };

    var _GetProjects = function (PartnarName, GlobalSearchParam, FromDate, ToDate, ProjectIdentifier, PageIndex, PageSize) {
        var data = {};
        data.searchTerm = GlobalSearchParam;
        data.pIdentifier = ProjectIdentifier;
        data.fromDate = FromDate;
        data.toDate = ToDate;
        return $http.get(__RootUrl + "api/" + PartnarName + "/Project/List/" + PageSize + "/" + PageIndex + "?" + $httpParamSerializer(data));
    };

    var _GetProjectChartData = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetProjectChartData?" + $httpParamSerializer(data));
    };

    var _GetTopProductChartData = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetTopProductChartData?" + $httpParamSerializer(data));
    };

    var _GetAppChartData = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetAppChartData?" + $httpParamSerializer(data));
    };

    var _GetTopCustomerChartData = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetTopCustomerChartData?" + $httpParamSerializer(data));
    };

    var _GetProjectCount = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetProjectCount?" + $httpParamSerializer(data));
    };

    var _GetUniqueUserCount = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetUniqueUserCount?" + $httpParamSerializer(data));
    };

    var _GetBPMCount = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetBPMCount?" + $httpParamSerializer(data));
    };

    var _GetPartnerCount = function () {
        return $http.post(__RootUrl + "api/GetPartnerCount");
    };

    var _GetPctProjectWithProductTAG = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetPctProjectWithProductTAG?" + $httpParamSerializer(data));
    };

    var _GetPctProjectWithManufacturer = function (partner) {
        var data = { partner: partner };
        return $http.post(__RootUrl + "api/GetPctProjectWithManufacturer?" + $httpParamSerializer(data));
    };
    
    this.SetDefaultData = function (myScope, myLocation) {
        myScope.data = {};
    };

    this.TransformChartDataD3 = function (d) {
        var tempData = {};
        tempData.tempPattern = [];
        tempData.tempXs = {};
        tempData.tempColumns = [];
        angular.forEach(d.lines, function (v, k) {
            var i = "x" + (k + 1);
            tempData.tempPattern.push(_chartColor[k]);
            tempData.tempXs[v.name] = i;
            v.dates.unshift(i);
            v.values.unshift(v.name)
            tempData.tempColumns.push(v.dates);
            tempData.tempColumns.push(v.values);
        });
        return tempData;
    };

    this.GetColor = function () {
        return ['#ff0000', '#ff6a00', '#ffd800', '#b6ff00', '#4cff00', '#5f798d', '#0094ff', '#0000ff'];
    };

    this.ChangePartner = function (partner) {
        this.LogedinUserProfile.selectedPartner = partner;
        BroadcastService.send('EVENT-ChangePartner', partner);
    };

    this.LogedinUserProfileLoaded = function () {
        BroadcastService.send('EVENT-LogedinUserProfileLoaded', '');
    };

    this.GetLogedinUserProfile = function () {
        var deferred = $q.defer();
        var retVal = {};
        _GetLogedinUserProfile()
        .then(function (payload) {
            this.LogedinUserProfile.userId = payload.data.userId;
            this.LogedinUserProfile.firstName = payload.data.firstName;
            this.LogedinUserProfile.lastName = payload.data.lastName;
            this.LogedinUserProfile.email = payload.data.email;
            this.LogedinUserProfile.partnerNames = payload.data.partnerNames;
            this.LogedinUserProfile.selectedPartner = payload.data.selectedPartner;
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetProjects = function (PartnarName, GlobalSearchParam, FromDate, ToDate, ProjectIdentifier, PageIndex, PageSize) {
        var deferred = $q.defer();
        var retVal = [];
        _GetProjects(PartnarName, GlobalSearchParam, FromDate, ToDate, ProjectIdentifier, PageIndex, PageSize)
        .then(function (payload) {
            retVal = payload.data;
            $.each(retVal.projects, function (k, v) {
                v.isExpanded = false;
                v.hasDetail = false;
                v.isSelected = false;
            });
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetProjectChartData = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetProjectChartData(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetTopProductChartData = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetTopProductChartData(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetProjectCount = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetProjectCount(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetUniqueUserCount = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetUniqueUserCount(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetBPMCount = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetBPMCount(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetPartnerCount = function () {
        var deferred = $q.defer();
        var retVal = {};
        _GetPartnerCount()
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetPctProjectWithProductTAG = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetPctProjectWithProductTAG(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetPctProjectWithManufacturer = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetPctProjectWithManufacturer(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
        
    this.GetAppChartData = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetAppChartData(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetTopCustomerChartData = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _GetTopCustomerChartData(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    //GetCategoriesChartData
    //GetUniqueUserCount
    //GetProjectCount

    return this;
};
//============================================
var BroadcastService = function ($rootScope, $log) {
    return {
        send: function (msg, data) {
            $rootScope.$broadcast(msg, data);
        }
    }
};
//============================================
angular.module('GPD')
.factory('BroadcastService', ['$rootScope', '$log', function ($rootScope, $log) { return BroadcastService($rootScope, $log); }])
.service('CommonServices', ['$http', '$httpParamSerializer', '$q', '$log', 'BroadcastService', function ($http, $httpParamSerializer, $q, $log, BroadcastService) { return CommonServices($http, $httpParamSerializer, $q, $log, BroadcastService); }]);
