//============================================
var CommonServices = function ($http, $httpParamSerializer, $q, $log, BroadcastService) {
    var _chartColor = ['#ff0000', '#ff6a00', '#ffd800', '#b6ff00', '#4cff00', '#5f798d', '#0094ff', '#0000ff'];

    var _GetLogedinUserProfile = function () {
        return $http.post(__RootUrl + "api/GetUserProfile?userEmail=" + encodeURI(__UserEmail));
    };

    var _GetProjectChartData = function (partner, fromDate, toDate) {
        var data = { partner: partner, fromDate: fromDate, toDate: toDate };
        return $http.post(__RootUrl + "api/GetProjectChartData?" + $httpParamSerializer(data));
    };

    var _GetCategoriesChartData = function (partner, fromDate, toDate) {
        var data = { partner: partner, fromDate: fromDate, toDate: toDate };
        return $http.post(__RootUrl + "api/GetCategoriesChartData?" + $httpParamSerializer(data));
    };

    var _GetProjectCount = function (partner, fromDate, toDate) {
        var data = { partner: partner, fromDate: fromDate, toDate: toDate };
        return $http.post(__RootUrl + "api/GetProjectCount?" + $httpParamSerializer(data));
    };

    var _GetUniqueUserCount = function (partner) {
        var data = { partner: partner};
        return $http.post(__RootUrl + "api/GetUniqueUserCount?" + $httpParamSerializer(data));
    };
    
    this.LogedinUserProfile = { userId: "", firstName: "", lastName: "", email: "", partnerNames: [], selectedPartner: "" };

    this.SetDefaultData = function (myScope, myLocation) {
        myScope.data = {};
    };

    this.TransformChartData = function (d) {
        var tempData = {};
        tempData.tempPattern = [];
        tempData.tempXs = {};
        tempData.tempColumns = [];

        angular.forEach(d, function (v, k) {
            var i = "x" + (k + 1);
            //tempPattern.push(v.color);
            tempData.tempPattern.push(_chartColor[k]);
            tempData.tempXs[v.name] = i;
            v.dates.unshift(i);
            v.values.unshift(v.name)
            tempData.tempColumns.push(v.dates);
            tempData.tempColumns.push(v.values);
        });
        return tempData;
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

    this.GetProjectChartData = function (partner, fromDate, toDate) {
        var deferred = $q.defer();
        var retVal = {};
        _GetProjectChartData(partner, fromDate, toDate)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetCategoriesChartData = function (partner, fromDate, toDate) {
        var deferred = $q.defer();
        var retVal = {};
        _GetCategoriesChartData(partner, fromDate, toDate)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetProjectCount = function (partner, fromDate, toDate) {
        var deferred = $q.defer();
        var retVal = {};
        _GetProjectCount(partner, fromDate, toDate)
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
