angular.module('Dashboard')
.controller('DashboardController', function ($scope, $http, $location, $q, CommonServices, DashboardServices) {
    CommonServices.SetDefaultData($scope, $location);
    $scope.data.showProjects = __IsProjectsApplicableForUser;

    $scope.data.webSiteActivityUsageLogLoading = true;
    $scope.data.promotionsUsageLogLoading = false;
    $scope.data.projectOpportunitiesLoading = false;
    $scope.data.chartLoading = false;
    $scope.data.downloadingReport = false;

    $scope.data.webSiteActivityUsageLogDetails = { "100": "-", "101": "-", "102": "-", "103": "-", "104": "-" };
    $scope.data.promotionsUsageLogsDetails = { "423": "-", "425": "-" };
    $scope.data.projectOpportunitiesDetails = { "totalCount": "-", "count": "-", "brandCount": "-", "notSelectedCount": "-" };
    $scope.data.selectedChart = { "cType": "websiteactivity", "cName": "Impressions", "color": "#117899" };
    $scope.data.errorFilterSelection = "";

    $scope.data.tooltip = {
        IMPRESSIONS: "Click here to update the graph with Impressions data. An Impression is counted each time an image of a company’s product is shown on Sweets.",
        VIEWS: "Click here to update the graph with Views data. A View is counted each time a user views a company’s webpage or document on Sweets.",
        CLICKS: "Click here to update the graph with Clicks data. A Click is counted every time a user clicks on a link or navigation item associated with a company’s content on Sweets.",
        VISITS: "Click here to update the graph with Visits data. A Visit is counted every time a user accesses Sweets.",
        LEADS: "Click here to update the graph with Leads data. A Lead is counted every time an authenticated user views or downloads a company’s content on Sweets.",
        PROJECTS: "Click here to update the graph with Projects data. These are Projects with a Published date within the specified time period.",
        HOMEPAGE_SPOTLIGHT: " Click here to update the graph with Spotlight data on the graph. Spotlights are an accumulation of how many impressions the customer received for any product that was displayed in the featured products area of the Sweets.com homepage for the time period specified.",
        NEWSLETTER: " Click here to update the graph with Newsletters data on the graph. Newsletters are a total of all open rates that the customer received for every newsletter that included their product information for the time period specified."
    };

    var _chartObj;

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

    var GetSelectedAccount = function () {
        return CommonServices.GetSelectedAccount($scope.data.selectedAccount, $scope.data.accounts);
    };

    var GetSelectedCompany = function () {
        return CommonServices.GetSelectedCompany($scope.data.selectedCompany, $scope.data.companies);
    };

    var GetWebSiteActivityUsageLogDetails = function () {
        $scope.data.webSiteActivityUsageLogLoading = true;
        return DashboardServices.GetWebSiteActivityUsageLogDetails(GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.webSiteActivityUsageLogDetails = payload;
            $scope.data.webSiteActivityUsageLogLoading = false;
        });
    };

    var GetPromotionsUsageLogsDetails = function () {
        $scope.data.promotionsUsageLogLoading = true;
        return DashboardServices.GetPromotionsUsageLogsDetails(GetSelectedAccount(), GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.promotionsUsageLogsDetails = payload;
            $scope.data.promotionsUsageLogLoading = false;
        });
    };

    var GetProjectOpportunitiesDetails = function () {
        $scope.data.projectOpportunitiesLoading = true;
        return DashboardServices.GetProjectOpportunitiesDetails(GetSelectedAccount(), GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.projectOpportunitiesDetails = payload;
            $scope.data.projectOpportunitiesLoading = false;
        });
    };

    var GetChartData = function () {
        $scope.data.chartLoading = true;
        DestroyChartData();
        return DashboardServices.GetChartData(GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate, $scope.data.selectedChart.cType, $scope.data.selectedChart.cName)
        .then(function (payload) {
            RenderChartData(payload, $scope.data.selectedChart.cName, $scope.data.selectedChart.color);
            $scope.data.chartLoading = false;
        });
    }

    var GetActivityData = function () {
        $scope.$broadcast("Dashboard_FilterChanged", "");
        GetWebSiteActivityUsageLogDetails();
        GetPromotionsUsageLogsDetails();
        GetProjectOpportunitiesDetails();
        GetChartData();
        if ($scope.data.showAccountSearch) {
            $location.search("searchedAccNo", $scope.data.searchedAccountNo);
        }
        $location.search("selAccNo", $scope.data.selectedAccount);
        $location.search("selProNo", $scope.data.selectedCompany);
        $location.search("stdt", $scope.data.fromDate);
        $location.search("enddt", $scope.data.toDate);
    };

    var DestroyChartData = function () {
        if (_chartObj) { _chartObj = _chartObj.destroy(); }
    }

    var RenderChartData = function (d, name, color) {
        d.dates.unshift("x");
        d.values.unshift(name);

        _chartObj = c3.generate({
            bindto: "#c3Chart",
            grid: {
                x: { show: true },
                y: { show: true }
            },
            color: { pattern: [color] },
            data: {
                x: 'x',
                columns: [
                    d.dates,
                    d.values
                ],
                type: 'spline'
            },
            point: { r: 2 },
            axis: {
                x: {
                    //type: 'category',
                    type: 'timeseries',
                    tick: { format: "%d-%b" } // %b - month name, %d - date, %m - month, %y - year
                }
            },
            legend: { show: false }
        });
    }

    $scope.OnLoadChartClick = function (cType, cName, $event) {
        //unselect all check box
        $.each($("div.bb"), function (k, v) {
            $(v).removeClass("selected");
        });

        //select check box            
        var ele = $($event.currentTarget);
        $(ele).addClass("selected");

        $scope.data.selectedChart.cType = cType;
        $scope.data.selectedChart.cName = cName;
        $scope.data.selectedChart.color = $(ele).css("background-color");

        GetChartData();
    };

    $scope.$on("AccountSelect_OnAccountChange", function (evt, data) {
        GetCompanies();
        GetActivityData();
    });

    $scope.$on("AccountSelect_OnCompanyChange", function (evt, data) {
        GetActivityData();
    });

    $scope.$on("AccountSearch_ClearAccount", function (evt, data) {
        GetAccount().then(function () { GetActivityData(); });
    });

    $scope.$on("AccountSearch_SearchAccount", function (evt, data) {
        GetAccount().then(function () { GetActivityData(); });
    });

    $scope.$on("DateSelect_DateChanged", function (evt, data) {
        GetActivityData();
    });

    $scope.NavigateLink = function (type, $event) {
        var url = type;
        var p = {
            "selAccNo": $scope.data.selectedAccount,
            "selProNo": $scope.data.selectedCompany,
            "stdt": $scope.data.fromDate,
            "enddt": $scope.data.toDate
        }
        if ($scope.data.showAccountSearch) {
            p.searchedAccNo = $scope.data.searchedAccountNo;
        }

        if (type == "project") {
            url = __RootUrl + "/Projects/ProjectSummary2.aspx";
        }
        else if (type == "Views") {
            url = __RootUrl + "/WebsiteActivity/WebsiteActivity2.aspx";
            p.webActType = "VIEWS";
            p.reportType = "Content";
        }
        else if (type == "Clicks") {
            url = __RootUrl + "/WebsiteActivity/WebsiteActivity2.aspx";
            p.webActType = "CLICKS";
            p.reportType = "Content";
        }
        else if (type == "Leads") {
            url = __RootUrl + "/Requests/RequestDetail2.aspx";
            p.webActType = "REQUESTS";
            p.statId = "All";
        }
        else if (type == "promotions") {
            url = __RootUrl + "/Promotions/NewsLetter2.aspx";
            p.type = type;
            p.id = "newsletter";
        }

        return url + "#?" + $.param(p);
    };

    $scope.IsLoading = function () {
        return $scope.data.webSiteActivityUsageLogLoading
            || $scope.data.promotionsUsageLogLoading
            || $scope.data.projectOpportunitiesLoading
            || $scope.data.chartLoading
            || $scope.data.downloadingReport;
    };

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
        GetAccount().then(function () { GetActivityData(); });
    });
});

