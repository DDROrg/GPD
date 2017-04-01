//============================================
var CommonServices = function ($http, $q) {
    this.SetDefaultData = function (myScope, myLocation) {
        myScope.data = {};
    }

    return this;
};

//============================================
var ProjectServices = function ($http, $q) {
    //var _GetProjects = function () {
    //    var postData = new Object();
    //    return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetLeadEmailSettingsForUser", JSON.stringify(postData))
    //    .success(function (data, status, headers, config) { })
    //    .error(function (data, status, header, config) { });
    //};

    this.GetProjects = function () {
        var deferred = $q.defer();
        var retVal = [
            {
                "id": "C5B3004E-D3D7-45BB-96EB-232D8F1723A1",
                "author": "James Jackson",
                "building-name": "",
                "client": "North Development Group",
                "filename": "Roswell Math and Science - 2016.rvt",
                "identifiers": [],
                "items": [],
                "location": {
                    "address1": "820 Ebenezer",
                    "city": "Rd",
                    "state": "",
                    "zip": ""
                },
                "name": "Roswell Math and Science Charter School",
                "number": "3222121",
                "organization-description": "",
                "organization-name": "Global Product Data, LLC.",
                "session": {
                    "type": "ApplicationSession",
                    "application": {
                        "build": "20150714_1515(x64)",
                        "name": "Autodesk Revit 2016",
                        "plugin-build": "2.3.0.383",
                        "plugin-source": "SAVEPOSTDATA",
                        "type": "REVIT",
                        "version": "2016"
                    },
                    "platform": "windows"
                },
                "status": "Schematic Design - Demo Project"
            },
            {
                "id": "97B7AB44-D997-427B-9A42-37A119949503",
                "author": "Debabrata Dalapati",
                "building-name": "",
                "client": "North Development Group",
                "filename": "Roswell Math and Science - 2016.rvt",
                "identifiers": [],
                "items": [],
                "location": {
                    "address1": "820 Ebenezer",
                    "city": "Rd",
                    "state": "",
                    "zip": ""
                },
                "name": "Roswell Math and Science Charter School",
                "number": "3222122",
                "organization-description": "",
                "organization-name": "Global Product Data, LLC.",
                "session": {
                    "type": "ApplicationSession",
                    "application": {
                        "build": "20150714_1515(x64)",
                        "name": "Autodesk Revit 2016",
                        "plugin-build": "2.3.0.383",
                        "plugin-source": "SAVEPOSTDATA",
                        "type": "REVIT",
                        "version": "2016"
                    },
                    "platform": "windows"
                },
                "status": "Schematic Design - Demo Project"
            }
        ];
        //_GetProjects()
        //.then(function (payload) {
        //    retVal = payload;
        //    deferred.resolve(retVal);
        //});
        deferred.resolve(retVal);
        return deferred.promise;
    };
    return this;
}

//============================================
angular.module('Project')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('ProjectServices', function ($http, $q) { return ProjectServices($http, $q); });
