//============================================
var CommonServices = function ($http, $q, $log, BroadcastService) {
    var _GetLogedinUserProfile = function () {
        return $http.post(__RootUrl + "api/GetUserProfile?userEmail=" + encodeURI(__UserEmail));
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
var ProjectServices = function ($http, $q, $log) {
    var _GetProjects = function (PartnarName, GlobalSearchParam, PageIndex, PageSize) {
        GlobalSearchParam = GlobalSearchParam == "" ? encodeURIComponent(" ") : encodeURIComponent(GlobalSearchParam);
        return $http.get(__RootUrl + "api/" + PartnarName + "/Project/List/" + PageSize + "/" + PageIndex + "/" + GlobalSearchParam + "/");
    };

    var _GetProjectDetail = function (PartnarName, id) {
        return $http.get(__RootUrl + "api/" + PartnarName + "/Project/" + id);
    };

    this.GetProjects = function (PartnarName, GlobalSearchParam, PageIndex, PageSize) {
        var deferred = $q.defer();
        var retVal = [];
        _GetProjects(PartnarName, GlobalSearchParam, PageIndex, PageSize)
        .then(function (payload) {
            retVal = payload.data;
            $.each(retVal.projects, function (k, v) {
                v.isExpanded = false;
                v.hasDetail = false;
            });
            deferred.resolve(retVal);
        });

        return deferred.promise;
    };

    this.GetProjectDetail = function (PartnarName, id) {
        var deferred = $q.defer();
        var retVal = [];
        _GetProjectDetail(PartnarName, id)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    }

    return this;
}
//============================================
var GpdManageServices = function ($http, $q, $log) {
    var _GetPartners = function () {
        return $http.post(__RootUrl + "api/GetPartners");
    };
    var _GetUsers = function (searchTerm) {
        return $http.post(__RootUrl + "api/GetUsers?searchTerm=" + encodeURI(searchTerm));
    };

    this.GetPartners = function () {
        var deferred = $q.defer();
        var retVal = {};
        _GetPartners()
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    }

    this.GetUsers = function (searchTerm) {
        var deferred = $q.defer();
        var retVal = {};
        _GetUsers(searchTerm)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    }

    return this;
}
//============================================
angular.module('Project')
.factory('BroadcastService', function ($rootScope, $log) { return BroadcastService($rootScope, $log); })
.service('CommonServices', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); })
.service('ProjectServices', function ($http, $q, $log) { return ProjectServices($http, $q, $log); });

angular.module('ManageUser')
.factory('BroadcastService', function ($rootScope, $log) { return BroadcastService($rootScope, $log); })
.service('CommonServices', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); })
.service('GpdManageServices', function ($http, $q, $log) { return GpdManageServices($http, $q, $log); });

angular.module('ManagePartner')
.factory('BroadcastService', function ($rootScope, $log) { return BroadcastService($rootScope, $log); })
.service('CommonServices', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); })
.service('GpdManageServices', function ($http, $q, $log) { return GpdManageServices($http, $q, $log); });