//============================================
var CommonServices = function ($http, $httpParamSerializer, $q, $log, BroadcastService) {
    var _GetLogedinUserProfile = function () {
        return $http.post(__RootUrl + "api/GetUserProfile?userEmail=" + encodeURI(__UserEmail));
    };

    var _GetProjectChartData = function (partner, fromDate, toDate) {
        var data = { partner: partner, fromDate: fromDate, toDate: toDate };
        $log.log(data);
        return $http.post(__RootUrl + "api/GetProjectChartData?" + $httpParamSerializer(data));
    };

    this.LogedinUserProfile = { userId: "", firstName: "", lastName: "", email: "", partnerNames: [], selectedPartner: "" };

    this.SetDefaultData = function (myScope, myLocation) {
        myScope.data = {};
    };

    this.ChangePartner = function (partner) {
        this.LogedinUserProfile.selectedPartner = partner;
        BroadcastService.send('EVENT-ChangePartner', partner);
    };

    this.LogedinUserProfileLoaded = function () {
        BroadcastService.send('EVENT-LogedinUserProfileLoaded', '');
    }

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
    }

    this.GetProjectChartData = function (partner, fromDate, toDate) {
        var deferred = $q.defer();
        var retVal = {};
        _GetProjectChartData(partner, fromDate, toDate)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    }
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
