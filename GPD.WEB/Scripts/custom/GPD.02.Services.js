//============================================
var CommonServices = function ($http, $q, BroadcastService) {

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
var BroadcastService = function ($rootScope) {
    return {
        send: function (msg, data) {
            $rootScope.$broadcast(msg, data);
        }
    }
};
//============================================
var ProjectServices = function ($http, $q) {
    var _GetProjects = function (PartnarName, GlobalSearchParam, PageIndex, PageSize) {
        return $http.get(__RootUrl + "api/" + PartnarName + "/Project/List/" + PageSize + "/" + PageIndex + "/" + encodeURI(GlobalSearchParam) + "/");
    };

    var _GetProjectDetail = function (PartnarName, id) {
        return $http.get(__RootUrl + "api/" + PartnarName + "/Project/" + id);
    };

    this.GetProjects = function (PartnarName, GlobalSearchParam, PageIndex, PageSize) {
        GlobalSearchParam = GlobalSearchParam == "" ? " " : GlobalSearchParam;
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
angular.module('Project')
.factory('BroadcastService', function ($rootScope) { return BroadcastService($rootScope); })
.service('CommonServices', function ($http, $q, BroadcastService) { return CommonServices($http, $q, BroadcastService); })
.service('ProjectServices', function ($http, $q) { return ProjectServices($http, $q); });