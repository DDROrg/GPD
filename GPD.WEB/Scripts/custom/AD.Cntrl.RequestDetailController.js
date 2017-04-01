angular.module('RequestDetail')
.controller('RequestDetailController', function ($scope, $http, $location, $q, CommonServices, RequestDetailServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.requestDetailLoading = false;

    $scope.data.leadsSources = [];
    $scope.data.selectedLeadsSources = "";

    $scope.data.reports = [];
    
    $scope.data.sort = {
        column: 'total',
        descending: true
    };    

    var GetAccount = function () {
        return CommonServices.GetAccount($scope.data.searchedAccountNo, $scope.data.selectedAccount)
        .then(function (payload) {
            $scope.data.accounts = payload.accounts;
            $scope.data.selectedAccount = payload.selectedAccount;
            $scope.data.allCompanies = payload.allCompanies;
            GetCompanies();
        });
    };

    var GetCompanies = function () {
        if ($scope.data.selectedAccount == "All") { $scope.data.selectedCompany = "All"; }
        var tempData = CommonServices.GetCompanies($scope.data.allCompanies, $scope.data.selectedAccount, $scope.data.selectedCompany);
        $scope.data.companies = tempData.companies;
        $scope.data.selectedCompany = tempData.selectedCompany;
    };

    var GetLeadsSources = function () {
        return CommonServices.GetLeadsSources(GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.leadsSources = payload;
            if ($scope.data.leadsSources.length == 2) {
                $scope.data.selectedLeadsSources = $scope.data.leadsSources[1].SourceId;
            } else {
                var tempSel = $($scope.data.leadsSources).filter(function (index) { return this.SourceId === $scope.data.selectedLeadsSources; });
                if (tempSel.length == 0) { $scope.data.selectedLeadsSources = "All"; }
            }
        });
    };

    var GetRequestDetailData = function (resetPage) {
        $scope.data.requestDetailLoading = true;
        if (resetPage == true) { $scope.data.Pagination.currentPageIndex = 1;}
        return RequestDetailServices.GetRequestDetailData($scope.data.websiteActivityType, GetSelectedCompany(), GetSelectedLeadSources(), $scope.data.fromDate, $scope.data.toDate, $scope.data.Pagination.selectedPageSize, $scope.data.Pagination.currentPageIndex)
        .then(function (payload) {
            $scope.data.Pagination.totalRecord = payload.totalCount;
            $scope.data.reports = payload.reports;
            $scope.$broadcast("BeforePaginationRendered", "");
            $scope.data.requestDetailLoading = false;
        });
    };

    var GetSelectedAccount = function () {
        return CommonServices.GetSelectedAccount($scope.data.selectedAccount, $scope.data.accounts);
    };

    var GetSelectedCompany = function () {
        return CommonServices.GetSelectedCompany($scope.data.selectedCompany, $scope.data.companies);
    };

    var GetSelectedLeadSources = function () {        
        return CommonServices.GetSelectedLeadsSources($scope.data.selectedLeadsSources, $scope.data.leadsSources);
    };

    $scope.OnChangeSorting = function (column) {
        if ($scope.data.sort.column == column) {
            $scope.data.sort.descending = !$scope.data.sort.descending;
        } else {
            $scope.data.sort.column = column;
            $scope.data.sort.descending = true;
        }
    };

    $scope.ColumnSortClass = function (column) {
        var retVal = "fa fa-sort";
        if ($scope.data.sort && $scope.data.sort.column && column == $scope.data.sort.column) {
            if ($scope.data.sort.descending) { retVal = "fa fa-sort-down"; }
            else { retVal = "fa fa-sort-up"; }
        }
        return retVal;
    };

    $scope.GetProductDetailLink = function (userId) {
        var url = __RootUrl + "/Requests/ProductDetail2.aspx";
        var p = {
            "selAccNo": $scope.data.selectedAccount,
            "selProNo": $scope.data.selectedCompany,
            "stdt": $scope.data.fromDate,
            "enddt": $scope.data.toDate,
            "userId": userId
        }
        if ($scope.data.showAccountSearch) {
            p.searchedAccNo = $scope.data.searchedAccountNo;
        }
        return url + "#?" + $.param(p);
    };

    $scope.SetBreadCrumbValue = function () {
        var url = __RootUrl + "/LeadsDashboard.aspx";
        var p = {
            "selAccNo": $scope.data.selectedAccount,
            "selProNo": $scope.data.selectedCompany,
            "stdt": $scope.data.fromDate,
            "enddt": $scope.data.toDate
        }
        if ($scope.data.showAccountSearch) {
            p.searchedAccNo = $scope.data.searchedAccountNo;
        }
        return url + "#?" + $.param(p);
    };

    $scope.OnSourceTypeChange = function () { GetRequestDetailData(true); };

    $scope.OnReportTypeChange = function () { GetRequestDetailData(true); };

    $scope.IsLoading = function () {
        return $scope.data.requestDetailLoading
                || $scope.data.downloadingReport;
    };

    $scope.$on("AccountSelect_OnAccountChange", function (evt, data) {
        GetCompanies();
        GetLeadsSources().then(function () { GetRequestDetailData(true); });
    });

    $scope.$on("AccountSelect_OnCompanyChange", function (evt, data) {
        GetLeadsSources().then(function () { GetRequestDetailData(true); });
    });

    $scope.$on("AccountSearch_ClearAccount", function (evt, data) {
        GetAccount().then(function () {
            GetLeadsSources().then(function () { GetRequestDetailData(true); });
        });
    });

    $scope.$on("AccountSearch_SearchAccount", function (evt, data) {
        GetAccount().then(function () {
            GetLeadsSources().then(function () { GetRequestDetailData(true); });
        });
    });

    $scope.$on("DateSelect_DateChanged", function (evt, data) {
        GetLeadsSources().then(function () { GetRequestDetailData(true); });
    });

    $scope.$on("Pagination_OnChange", function (evt, data) { GetRequestDetailData(false); });

    $scope.$watch(
    function () {
        var retval = 700;
        try { retval = $("div.body").height(); } catch (e) { }
        return retval > 700 ? retval : 700;
    },
    function (newValue, oldValue) {
        if (newValue != oldValue) {
            try {
                window.parent.resizeIframe(newValue);
            } catch (e) { }
        }
    });

    angular.element(document).ready(function () {
        CommonServices.SetDefaultDate($scope);
        CommonServices.SetDefaultPagination($scope);
        GetAccount().then(function () {
            GetLeadsSources().then(function () { GetRequestDetailData(true); });
        });
    });
});

