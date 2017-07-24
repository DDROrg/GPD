//============================================
var CommonServices = function ($http, $q, $log, BroadcastService) {
    var _GetChartData = function (fromDate, toDate) {        
        return $http.post(__RootUrl + "api/GetUserProfile?userEmail=" + encodeURI(__UserEmail));
    };

    this.LogedinUserProfile = { userId: "", firstName: "", lastName: "", email: "", partnerNames: [], selectedPartner: "" };

    this.SetDefaultData = function (myScope, myLocation) {
        myScope.data = {};
    };

    this.GetChartData = function (fromDate, toDate) {
        var deferred = $q.defer();
        var retVal = {};
        _GetChartData(fromDate, toDate)
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
.service('CommonServices', ['$http', '$q', '$log', 'BroadcastService', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); }]);
