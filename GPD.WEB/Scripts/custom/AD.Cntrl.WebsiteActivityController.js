angular.module('WebsiteActivity')
.controller('WebsiteActivityController', function ($scope, $http, $location, $q, CommonServices, WebsiteActivityServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.websiteActivityLoading = false;

    $scope.data.reportTypes = [
        { id: "Content", text: "Content Details" },
        { id: "Product", text: "Product Details" }
    ];
    {
        var tempSel = $($scope.data.reportTypes).filter(function (index) { return this.id === $scope.data.selectedReportType; });
        if (tempSel.length == 0) { $scope.data.selectedReportType = "Content"; }
    }

    $scope.data.reports = [];

    if ($scope.data.selectedReportType == "Content") {
        $scope.data.sort = {
            column: 'seq',
            descending: false
        };
    } else {
        $scope.data.sort = {
            column: 'count',
            descending: true
        };
    }    

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

    var GetWebsiteActivityData = function () {
        $scope.data.websiteActivityLoading = true;
        return WebsiteActivityServices.GetWebsiteActivityData($scope.data.websiteActivityType, GetSelectedCompany(), $scope.data.selectedReportType, $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.reports = payload.reports;
            $scope.data.websiteActivityLoading = false;
        });
    };

    var GetSelectedAccount = function () {
        return CommonServices.GetSelectedAccount($scope.data.selectedAccount, $scope.data.accounts);
    };

    var GetSelectedCompany = function () {
        return CommonServices.GetSelectedCompany($scope.data.selectedCompany, $scope.data.companies);
    };

    $scope.OnReportTypeChange = function () {
        GetWebsiteActivityData();
    };

    $scope.OnProductLinkClick = function () {
        $scope.data.selectedReportType = "Product";
        $scope.data.sort = {
            column: 'count',
            descending: true
        };
        GetWebsiteActivityData();
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

    $scope.IsLoading = function () {
        return $scope.data.websiteActivityLoading
            || $scope.data.downloadingReport;
    };

    $scope.GetWebsiteActivityType = function () {
        if ($scope.data.websiteActivityType == "CLICKS") {
            return "Clicks";
        }
        else if ($scope.data.websiteActivityType == "VIEWS") {
            return "Views";
        }
        else {
            return $scope.data.websiteActivityType;
        }
    };

    $scope.GetHeading = function () {
        var retVal = "";
        if ($scope.data.selectedReportType == "Content") {
            retVal = "WEBSITE ACTIVITY - " + $scope.data.websiteActivityType;
        } else {
            retVal = "PRODUCT ACTIVITY - " + $scope.data.websiteActivityType;
        }
        return retVal;
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

    $scope.$on("AccountSelect_OnAccountChange", function (evt, data) {
        GetCompanies();
        GetWebsiteActivityData();
    });

    $scope.$on("AccountSelect_OnCompanyChange", function (evt, data) {
        GetWebsiteActivityData();
    });

    $scope.$on("AccountSearch_ClearAccount", function (evt, data) {
        GetAccount().then(function () { GetWebsiteActivityData(); });
    });

    $scope.$on("AccountSearch_SearchAccount", function (evt, data) {
        GetAccount().then(function () { GetWebsiteActivityData(); });
    });

    $scope.$on("DateSelect_DateChanged", function (evt, data) {
        GetWebsiteActivityData();
    });

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
        GetAccount().then(function () { GetWebsiteActivityData(); });
    });

});

