var CommonServices = function ($http, $q) {
    var _ValidateAccountNumber = function (accountNo) {
        var postData = {};
        postData.accountNo = accountNo;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/ValidateAccountNumber", JSON.stringify(postData))
            .success(function (data, status, headers, config) { })
            .error(function (data, status, header, config) { });
    };

    var _GetAccounts = function (term) {
        var postData = {};
        postData.term = term;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetAccounts", JSON.stringify(postData))
            .success(function (data, status, headers, config) { })
            .error(function (data, status, header, config) { });
    };

    var _GetAccount = function (searchedAccountNo) {
        var postData = {};
        postData.accountNo = searchedAccountNo;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetUserAccountInfo", JSON.stringify(postData))
            .success(function (data, status, headers, config) { })
            .error(function (data, status, header, config) { });
    };

    var _GetLeadsSources = function (lstCompanies, startDate, endDate) {
        var postData = {};
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetLeadsSources", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetSelectedAccount = function (selectedAccount, accounts) {
        var tempSelAcc = "";
        if (selectedAccount == "All") {
            var items = new Array();
            $.each(accounts, function (k, v) {
                if (v.AccountNo != "All" && $.inArray(v.AccountNo, items) == -1) { items.push(v.AccountNo); }
            });
            tempSelAcc = items.join(",");
        } else {
            tempSelAcc = selectedAccount;
        }
        return tempSelAcc;
    };

    this.GetSelectedCompany = function (selectedCompany, companies) {
        var tempSelCom = "";
        if (selectedCompany == "All") {
            var items = new Array();
            $.each(companies, function (k, v) {
                if (v.CompanyId != "All" && $.inArray(v.CompanyId, items) == -1) { items.push(v.CompanyId); }
            });
            tempSelCom = items.join(",");
        } else {
            tempSelCom = selectedCompany;
        }
        return tempSelCom;
    };

    this.GetSelectedLeadsSources = function (selectedLeadSources, leadSources) {
        var tempLeadSource = "";
        if (selectedLeadSources == "All") {
            var items = new Array();
            $.each(leadSources, function (k, v) {
                if (v.SourceId != "All" && $.inArray(v.SourceId, items) == -1) { items.push(v.SourceId); }
            });
            tempLeadSource = items.join(",");
        } else {
            tempLeadSource = selectedLeadSources;
        }
        return tempLeadSource;
    };

    this.GetSelectedProductCategory = function (productCategories) {
        var items = [];
        $.each(productCategories, function (k, v) {
            if (v.value != "All" && v.IsChecked == true) { items.push(v.value); }
        });
        return items.join("|");
    };

    this.ValidateAccountNumber = function (accountNo) {
        var deferred = $q.defer();
        var retVal = { isValid: false };
        _ValidateAccountNumber(accountNo)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetAccounts = function (term) {
        var deferred = $q.defer();
        var retVal = [];
        _GetAccounts(term)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetAccount = function (searchedAccountNo, selectedAccount) {
        var deferred = $q.defer();
        var retVal = {};
        retVal.accounts = [{ AccountNo: "All", AccountName: "All" }];
        retVal.selectedAccount = selectedAccount;
        retVal.allCompanies = [];

        _GetAccount(searchedAccountNo)
		.then(function (payload) {
		    $.each(payload.data.d.AccountList, function (k, v) {
		        retVal.accounts.push({ AccountNo: v.AccountNo, AccountName: v.AccountName });
		    });
		    if (retVal.accounts.length == 2) {
		        retVal.selectedAccount = retVal.accounts[1].AccountNo;
		    } else {
		        var tempSel = $(retVal.accounts).filter(function (index) { return this.AccountNo === retVal.selectedAccount; });
		        if (tempSel.length == 0) { retVal.selectedAccount = "All"; }
		    }
		    retVal.allCompanies = payload.data.d.CompanyList;
		    deferred.resolve(retVal);
		});
        return deferred.promise;
    };

    this.GetCompanies = function (allCompanies, selectedAccount, selectedCompany) {
        var retVal = {};
        retVal.companies = [{ AccountNo: "All", CompanyId: "All", CompanyName: "All" }];
        retVal.selectedCompany = selectedCompany;
        $.each(allCompanies, function (k, v) {
            if (selectedAccount == "All") {
                retVal.companies.push({ AccountNo: v.AccountNo, CompanyId: v.CompanyId, CompanyName: v.CompanyName });
            }
            else if (v.AccountNo == selectedAccount) {
                retVal.companies.push({ AccountNo: v.AccountNo, CompanyId: v.CompanyId, CompanyName: v.CompanyName });
            }
        });

        if (retVal.companies.length == 2) {
            retVal.selectedCompany = retVal.companies[1].CompanyId;
        } else {
            var tempSel = $(retVal.companies).filter(function (index) { return this.CompanyId === retVal.selectedCompany; });
            if (tempSel.length == 0) { retVal.selectedCompany = "All"; }
        }
        return retVal;
    };

    this.GetLeadsSources = function (lstCompanies, startDate, endDate) {
        var deferred = $q.defer();
        var retVal = [{ SourceId: "All", SourceName: "All", IsChecked: true }];
        _GetLeadsSources(lstCompanies, startDate, endDate)
        .then(function (payload) {
            $.each(payload.data.d, function (k, v) {
                retVal.push({ SourceId: v.SourceId, SourceName: v.SourceName, IsChecked: true });
            });
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.SetDefaultData = function (myScope, myLocation) {
        myScope.data = {};
        myScope.data.showAccountSearch = __IsUserAccountSearchEligible;
        myScope.data.DateSelect = {};
        myScope.data.DateSelect.hasError = false;
        myScope.data.errorFilterSelection = "";

        var qp = myLocation.search();
        myScope.data.selectedAccount = qp.selAccNo ? qp.selAccNo : "";
        myScope.data.selectedCompany = qp.selProNo ? qp.selProNo : "";
        myScope.data.searchedAccountNo = myScope.data.showAccountSearch && qp.searchedAccNo ? qp.searchedAccNo : "";
        myScope.data.selectedOpportunityTypes = qp.OppId ? qp.OppId : "allop";
        myScope.data.selectedReportType = qp.reportType ? qp.reportType : "";
        myScope.data.websiteActivityType = qp.webActType ? qp.webActType : "";
        myScope.data.userId = qp.userId ? qp.userId : "";
        //myScope.data.selectedLeadsSources = qp.statId ? qp.statId : "";
        myScope.data.fromDate = qp.stdt ? qp.stdt : "";
        myScope.data.toDate = qp.enddt ? qp.enddt : "";
        myScope.data.drNumber = qp.drNo ? qp.drNo : "";

        myScope.data.accounts = [];
        myScope.data.allCompanies = [];
        myScope.data.companies = [];

        myScope.data.ac = {};
        myScope.data.ac.accounts = [];
        myScope.data.ac.isVisible = false;
    }

    this.SetDefaultDate = function (myScope) {
        var startDt = myScope.data.fromDate;
        var endDate = myScope.data.toDate;

        try {
            if (startDt != "" && endDate != "") {
                if (endDate != "") {
                    var curDate = new Date();
                    curDate.setDate(curDate.getDate() - 1);
                    var tmpDate = $.datepicker.parseDate("mm/dd/yy", endDate);
                    if (tmpDate > curDate) {
                        endDate = $.datepicker.formatDate('mm/dd/yy', curDate);
                    }
                }

                if (startDt != "") {
                    var tmpStDate = $.datepicker.parseDate("mm/dd/yy", startDt);
                    var tmpEndDate = $.datepicker.parseDate("mm/dd/yy", endDate);
                    if (tmpStDate > tmpEndDate) {
                        startDt = $.datepicker.formatDate('mm/dd/yy', tmpEndDate);
                    } else {
                        tmpEndDate.setMonth(tmpEndDate.getMonth() - 3);
                        if (tmpStDate < tmpEndDate) {
                            startDt = $.datepicker.formatDate('mm/dd/yy', tmpEndDate);
                        }
                    }
                }
            }
        } catch (e) {
            startDt = "";
            endDate = "";
        }

        if (startDt == "" || endDate == "") {
            var curDate = new Date();
            curDate.setDate(curDate.getDate() - 1);
            endDate = $.datepicker.formatDate('mm/dd/yy', curDate);
            curDate.setMonth(curDate.getMonth() - 3);
            startDt = $.datepicker.formatDate('mm/dd/yy', curDate);
        }

        myScope.data.fromDate = startDt;
        myScope.data.toDate = endDate;
    };

    this.SetDefaultPagination = function (myScope) {
        //Pagination default data
        myScope.data.Pagination = {};
        myScope.data.Pagination.pageSize = [
            { id: "10" },
            { id: "20" },
            { id: "50" },
            { id: "100" }
        ];
        myScope.data.Pagination.selectedPageSize = "100";
        myScope.data.Pagination.currentPageIndex = 1;
        myScope.data.Pagination.totalRecord = 0;

        myScope.data.Pagination.buttons = [];
        myScope.data.Pagination.maxPageIndex = 0;
    };

    return this;
};
//============================================
var DashboardServices = function ($http, $q) {

    var _GetWebSiteActivityUsageLogDetails = function (lstCompanies, startDate, endDate) {
        var postData = {};
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetWebSiteActivityUsageLogDetails", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    var _GetPromotionsUsageLogsDetails = function (accNo, lstCompanies, startDate, endDate) {
        var postData = {};
        postData.accNo = accNo;
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetPromotionsUsageLogsDetails", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    var _GetProjectOpportunitiesDetails = function (accNo, lstCompanies, startDate, endDate) {
        var postData = {};
        postData.accNo = accNo;
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetProjectOpportunities", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    var _GetChartData = function (lstCompanies, startDate, endDate, chartType, chartName) {
        var postData = new Object();
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;
        postData.chartType = chartType;
        postData.chartName = chartName;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetChartData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    }

    this.GetWebSiteActivityUsageLogDetails = function (lstCompanies, startDate, endDate) {
        var deferred = $q.defer();
        var retVal = { "100": "-", "101": "-", "102": "-", "103": "-", "104": "-" };
        _GetWebSiteActivityUsageLogDetails(lstCompanies, startDate, endDate)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetPromotionsUsageLogsDetails = function (accNo, lstCompanies, startDate, endDate) {
        var deferred = $q.defer();
        var retVal = { "423": "-", "425": "-" };
        _GetPromotionsUsageLogsDetails(accNo, lstCompanies, startDate, endDate)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetProjectOpportunitiesDetails = function (accNo, lstCompanies, startDate, endDate) {
        var deferred = $q.defer();
        var retVal = { "totalCount": "-", "count": "-", "brandCount": "-", "notSelectedCount": "-" };
        _GetProjectOpportunitiesDetails(accNo, lstCompanies, startDate, endDate)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    this.GetChartData = function (lstCompanies, startDate, endDate, chartType, chartName) {
        var deferred = $q.defer();
        var retVal = { dates: [], values: [] };
        _GetChartData(lstCompanies, startDate, endDate, chartType, chartName)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    }

    return this;
};

//============================================
var ReportSettingsServices = function ($http, $q) {
    var _GetLeadEmailSettingsForUser = function () {
        var postData = new Object();
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetLeadEmailSettingsForUser", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetLeadEmailSettingsForUser = function () {
        var deferred = $q.defer();
        var retVal = { "MonthlyAlertInd": false, "SweetsAccNumbers": [], "AltEmail": "", "errorMessage": "" };
        _GetLeadEmailSettingsForUser()
        .then(function (payload) {
            retVal.SweetsAccNumbers = payload.data.d.SweetsAccNumbers;
            retVal.MonthlyAlertInd = payload.data.d.MonthlyAlertInd == 'M';
            var tempAltEmail = [];

            $.each(payload.data.d.SweetsAccNumbers, function (k, v) { v.IsChecked = v.IsEmdAvail === "Y" ? true : false; });
            $.each(payload.data.d.MonthlyAlertAltEmail, function (k, v) { tempAltEmail.push(v) });
            retVal.AltEmail = tempAltEmail.join(',');
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };
    return this;
}

//============================================
var ProjectSummaryServices = function ($http, $q) {
    var _GetProjectSummaryData = function (lstCompanies, startDate, endDate, selectedOpportunity, selectedProductCategory, pageSize, pageNbr, includeProdCtg) {
        var postData = {};
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;
        postData.selectedOpportunity = selectedOpportunity;
        postData.selectedProductCategory = selectedProductCategory;
        postData.pageSize = pageSize;
        postData.pageNbr = pageNbr;
        postData.includeProdCtg = includeProdCtg;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetProjectSummaryData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetProjectSummaryData = function (lstCompanies, startDate, endDate, selectedOpportunity, selectedProductCategory, pageSize, pageNbr, includeProdCtg) {
        var deferred = $q.defer();
        var retVal = "";
        _GetProjectSummaryData(lstCompanies, startDate, endDate, selectedOpportunity, selectedProductCategory, pageSize, pageNbr, includeProdCtg)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    return this;
};

//============================================
var ProjectDetailServices = function ($http, $q) {
    var _GetProjectDetailData = function (drNumber, selectedProductCategory, includeProdCtg) {
        var postData = {};
        postData.dodgeDrNbr = drNumber;
        postData.selectedProductCategory = selectedProductCategory;
        postData.includeProdCtg = includeProdCtg;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetProjectDetailData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetProjectDetailData = function (drNumber, selectedProductCategory, includeProdCtg) {
        var deferred = $q.defer();
        var retVal = "";
        _GetProjectDetailData(drNumber, selectedProductCategory, includeProdCtg)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    return this;
};

//============================================
var WebsiteActivityServices = function ($http, $q) {
    var _GetWebsiteActivityData = function (webActType, lstCompanies, reportType, startDate, endDate) {
        var postData = {};
        postData.webActType = webActType;
        postData.lstCompanies = lstCompanies;
        postData.reportType = reportType;
        postData.startDate = startDate;
        postData.endDate = endDate;
        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetWebsiteActivityData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetWebsiteActivityData = function (webActType, lstCompanies, reportType, startDate, endDate) {
        var deferred = $q.defer();
        var retVal = "";
        _GetWebsiteActivityData(webActType, lstCompanies, reportType, startDate, endDate)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    return this;
};

//============================================
var RequestDetailServices = function ($http, $q) {
    var _GetRequestDetailData = function (groupDefName, lstCompanies, statLookUpId, startDate, endDate, pageSize, pageNbr) {
        var postData = {};
        postData.groupDefName = groupDefName;
        postData.lstCompanies = lstCompanies;
        postData.statLookUpId = statLookUpId;
        postData.startDate = startDate;
        postData.endDate = endDate;
        postData.pageSize = pageSize;
        postData.pageNbr = pageNbr;

        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetRequestDetailData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetRequestDetailData = function (groupDefName, lstCompanies, statLookUpId, startDate, endDate, pageSize, pageNbr) {
        var deferred = $q.defer();
        var retVal = "";
        _GetRequestDetailData(groupDefName, lstCompanies, statLookUpId, startDate, endDate, pageSize, pageNbr)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    return this;
};

//============================================
var ProductDetailServices = function ($http, $q) {
    var _GetProductDetailData = function (lstCompanies, userId, statLookUpId, startDate, endDate, pageSize, pageNbr) {
        var postData = {};
        postData.lstCompanies = lstCompanies;
        postData.userId = userId;
        postData.statLookUpId = statLookUpId;
        postData.startDate = startDate;
        postData.endDate = endDate;
        postData.pageSize = pageSize;
        postData.pageNbr = pageNbr;

        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetProductDetailData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetProductDetailData = function (lstCompanies, userId, statLookUpId, startDate, endDate, pageSize, pageNbr) {
        var deferred = $q.defer();
        var retVal = "";
        _GetProductDetailData(lstCompanies, userId, statLookUpId, startDate, endDate, pageSize, pageNbr)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    return this;
};

//============================================
var NewsLetterActivityServices = function ($http, $q) {
    var _GetNewsLetterActivityData = function (lstCompanies, startDate, endDate) {
        var postData = {};
        postData.lstCompanies = lstCompanies;
        postData.startDate = startDate;
        postData.endDate = endDate;

        return $http.post(__RootUrl + "/WebMethods/MarketingLeadsWebMethods.aspx/GetNewsLetterActivityData", JSON.stringify(postData))
        .success(function (data, status, headers, config) { })
        .error(function (data, status, header, config) { });
    };

    this.GetNewsLetterActivityData = function (lstCompanies, startDate, endDate) {
        var deferred = $q.defer();
        var retVal = "";
        _GetNewsLetterActivityData(lstCompanies, startDate, endDate)
        .then(function (payload) {
            retVal = payload.data.d;
            deferred.resolve(retVal);
        });
        return deferred.promise;
    };

    return this;
};

//============================================
angular.module('Dashboard')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('DashboardServices', function ($http, $q) { return DashboardServices($http, $q); });

angular.module('Download')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('ProjectSummaryServices', function ($http, $q) { return ProjectSummaryServices($http, $q); });

angular.module('ReportSettings')
.service('ReportSettingsServices', function ($http, $q) { return ReportSettingsServices($http, $q); });

angular.module('ProjectSummary')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('ProjectSummaryServices', function ($http, $q) { return ProjectSummaryServices($http, $q); });

angular.module('ProjectDetail')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('ProjectSummaryServices', function ($http, $q) { return ProjectSummaryServices($http, $q); })
.service('ProjectDetailServices', function ($http, $q) { return ProjectDetailServices($http, $q); });

angular.module('WebsiteActivity')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('WebsiteActivityServices', function ($http, $q) { return WebsiteActivityServices($http, $q); });

angular.module('RequestDetail')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('RequestDetailServices', function ($http, $q) { return RequestDetailServices($http, $q); });

angular.module('ProductDetail')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('ProductDetailServices', function ($http, $q) { return ProductDetailServices($http, $q); });

angular.module('NewsLetterActivity')
.service('CommonServices', function ($http, $q) { return CommonServices($http, $q); })
.service('NewsLetterActivityServices', function ($http, $q) { return NewsLetterActivityServices($http, $q); });

