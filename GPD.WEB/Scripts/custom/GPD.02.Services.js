﻿//============================================
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
            var partnerDefImages = [];
            this.LogedinUserProfile.userId = payload.data.userId;
            this.LogedinUserProfile.firstName = payload.data.firstName;
            this.LogedinUserProfile.lastName = payload.data.lastName;
            this.LogedinUserProfile.email = payload.data.email;
            this.LogedinUserProfile.partnerNames = payload.data.partnerNames;
            this.LogedinUserProfile.selectedPartner = payload.data.selectedPartner;
            if (payload.data.roles != null) {
                $.each(payload.data.roles, function (k, v) {
                    partnerDefImages.push({ "name": v.partnerNames, "image": v.partnerImageUrl });
                });
            }
            this.LogedinUserProfile.partnerDefImages = partnerDefImages;
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
var ProjectServices = function ($http, $httpParamSerializer, $q, $log) {
    var _GetProjects = function (PartnarName, GlobalSearchParam, FromDate, ToDate, ProjectIdentifier, PageIndex, PageSize) {
        var data = {};
        data.searchTerm = GlobalSearchParam;
        data.pIdentifier = ProjectIdentifier;
        data.fromDate = FromDate;
        data.toDate = ToDate;
        return $http.get(__RootUrl + "api/" + PartnarName + "/Project/List/" + PageSize + "/" + PageIndex + "?" + $httpParamSerializer(data));
    };
    var _GetProjectDetail = function (id) {
        return $http.get(__RootUrl + "api/Project/" + id);
    };
    var _UpdateProject = function (projectId, project) {
        return $http.post(__RootUrl + "api/UpdateProject/" + projectId, project);
    };
    var _ProjectListActDact = function (projectIds, isActive) {
        return $http.post(__RootUrl + "api/ActivateProjectList?isActive=" + encodeURIComponent(isActive), projectIds);
    };
    var _ProjectListDelete = function (projectIds) {
        return $http.post(__RootUrl + "api/DeleteProjectList", projectIds);
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
    this.GetProjectDetail = function (id) {
        var deferred = $q.defer();
        var retVal = [];
        _GetProjectDetail(id)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.UpdateProject = function (projectId, project) {
        var deferred = $q.defer();
        var retVal = [];
        _UpdateProject(projectId, project)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.ProjectListActDact = function (projectIds, isActive) {
        var deferred = $q.defer();
        var retVal = {};
        _ProjectListActDact(projectIds, isActive)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.ProjectListDelete = function (projectIds) {
        var deferred = $q.defer();
        var retVal = {};
        _ProjectListDelete(projectIds)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    return this;
}
//============================================
var GpdManageServices = function ($http, $q, $log) {
    var _GetPartners = function () {
        return $http.post(__RootUrl + "api/GetPartners");
    };
    var _GetGroups = function () {
        return $http.post(__RootUrl + "api/GetGroups");
    };
    var _GetUsers = function (searchTerm) {
        return $http.post(__RootUrl + "api/GetUsers?searchTerm=" + encodeURI(searchTerm));
    };
    var _AddPartner = function (partner) {
        return $http.post(__RootUrl + "api/AddPartner", partner);
    };
    var _ActDactPartner = function (partnerId, isActive) {
        return $http.post(__RootUrl + "api/ActDactPartner?partnerId=" + encodeURI(partnerId) + "&isActive=" + encodeURI(isActive));
    };
    var _ActDactUser = function (userId, isActive) {
        return $http.post(__RootUrl + "api/ActDactUser?userId=" + encodeURI(userId) + "&isActive=" + encodeURI(isActive));
    };
    var _GetUserRoles = function (userId) {
        return $http.post(__RootUrl + "api/GetUserRoles?userId=" + encodeURI(userId));
    };
    var _DeleteUserRole = function (userId, partnerId, groupId) {
        return $http.post(__RootUrl + "api/DeleteUserRole?userId=" + encodeURI(userId) + "&partnerId=" + encodeURI(partnerId) + "&groupId=" + encodeURI(groupId));
    };
    var _AddUserRole = function (userId, partnerId, groupId) {
        return $http.post(__RootUrl + "api/AddUserRole?userId=" + encodeURI(userId) + "&partnerId=" + encodeURI(partnerId) + "&groupId=" + encodeURI(groupId));
    };
    var _RegisterUser = function (user) {
        var config = {
            headers: {
                'form-data-enewsletters-communication-flag': user.enewslettersCommunication,
                'form-data-email-communication-flag': user.emailCommunication
            }
        };
        return $http.post(__RootUrl + "api/RegisterUser", user, config);
    };

    var _UploadProfileImage = function (userId, file) {
        var config = {
            headers: {
                //"Content-Type": "multipart/form-data; boundary=gc0p4Jq0M2Yt08jU534c0p",
                //"Content-Type": "multipart/form-data",
                //"Content-Disposition": "form-data; name=profileImage"
                "Content-Type": undefined
            },
            transformRequest: angular.identity
        };
        return $http.post(__RootUrl + "api/UploadProfileImage?userId=" + userId, file, config);
    };

    var _GetCompanies = function (term) {
        return $http.post(__RootUrl + "api/GetCompanies?searchTerm=" + encodeURIComponent(term));
    };
    var _GetCountries = function (term) {
        return $http.get(__RootUrl + "Scripts/data/countries.json");
    };
    var _GetCompanyDetails = function (companyId) {
        return $http.post(__RootUrl + "api/GetCompanyDetails?countryId=" + encodeURIComponent(companyId));
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
    };
    this.GetGroups = function () {
        var deferred = $q.defer();
        var retVal = {};
        _GetGroups()
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.GetUsers = function (searchTerm) {
        var deferred = $q.defer();
        var retVal = {};
        _GetUsers(searchTerm)
        .then(function (payload) {
            retVal = payload.data;
            $.each(retVal, function (k, v) {
                v.isRoleExpanded = false;
                v.hasRole = false;
            });
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.AddPartner = function (partner) {
        var deferred = $q.defer();
        var retVal = {};
        _AddPartner(partner)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.ActDactPartner = function (partnerId, isActive) {
        var deferred = $q.defer();
        var retVal = {};
        _ActDactPartner(partnerId, isActive)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.ActDactUser = function (userId, isActive) {
        var deferred = $q.defer();
        var retVal = {};
        _ActDactUser(userId, isActive)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.GetUserRoles = function (userId) {
        var deferred = $q.defer();
        var retVal = {};
        _GetUserRoles(userId)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.DeleteUserRole = function (userId, partnerId, groupId) {
        var deferred = $q.defer();
        var retVal = {};
        _DeleteUserRole(userId, partnerId, groupId)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.AddUserRole = function (userId, partnerId, groupId) {
        var deferred = $q.defer();
        var retVal = {};
        _AddUserRole(userId, partnerId, groupId)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.RegisterUser = function (user) {
        var deferred = $q.defer();
        var retVal = {};
        _RegisterUser(user)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.UploadProfileImage = function (userId, file) {
        var deferred = $q.defer();
        var retVal = {};
        _UploadProfileImage(userId, file)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.GetCompanies = function (term) {
        var deferred = $q.defer();
        var retVal = {};
        _GetCompanies(term)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.GetCountries = function () {
        var deferred = $q.defer();
        var retVal = {};
        _GetCountries()
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    this.GetCompanyDetails = function (companyId) {
        var deferred = $q.defer();
        var retVal = {};
        _GetCompanyDetails(companyId)
        .then(function (payload) {
            retVal = payload.data;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    return this;
}
//============================================
angular.module('Project')
.factory('BroadcastService', ['$rootScope', '$log', function ($rootScope, $log) { return BroadcastService($rootScope, $log); }])
.service('CommonServices', ['$http', '$q', '$log', 'BroadcastService', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); }])
.service('ProjectServices', ['$http', '$httpParamSerializer', '$q', '$log', function ($http, $httpParamSerializer, $q, $log) { return ProjectServices($http, $httpParamSerializer, $q, $log); }]);

angular.module('ManageUser')
.factory('BroadcastService', ['$rootScope', '$log', function ($rootScope, $log) { return BroadcastService($rootScope, $log); }])
.service('CommonServices', ['$http', '$q', '$log', 'BroadcastService', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); }])
.service('GpdManageServices', ['$http', '$q', '$log', function ($http, $q, $log) { return GpdManageServices($http, $q, $log); }]);

angular.module('ManagePartner')
.factory('BroadcastService', ['$rootScope', '$log', function ($rootScope, $log) { return BroadcastService($rootScope, $log); }])
.service('CommonServices', ['$http', '$q', '$log', 'BroadcastService', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); }])
.service('GpdManageServices', ['$http', '$q', '$log', function ($http, $q, $log) { return GpdManageServices($http, $q, $log); }]);


angular.module('RegisterUser')
.factory('BroadcastService', ['$rootScope', '$log', function ($rootScope, $log) { return BroadcastService($rootScope, $log); }])
.service('CommonServices', ['$http', '$q', '$log', 'BroadcastService', function ($http, $q, $log, BroadcastService) { return CommonServices($http, $q, $log, BroadcastService); }])
.service('GpdManageServices', ['$http', '$q', '$log', function ($http, $q, $log) { return GpdManageServices($http, $q, $log); }]);