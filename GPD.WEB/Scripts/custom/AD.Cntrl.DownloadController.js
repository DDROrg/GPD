//=================================================
angular.module('Download', ['ngCookies']).controller('DownloadController', function ($scope, $http, $cookies, CommonServices) {

    $scope.data.errorDownloadLeadReport = "";
    $scope.data.leadsSources = [];
    $scope.data.selectedLeadsSources = "";
    $scope.data.selectedOpportunityTypes = "allop";

    var DownloadFile = function (url) {
        $cookies.remove('token', { path: '/' });
        var ifDownloadReport = $("iframe#ifDownloadFile");
        $(ifDownloadReport).attr("src", url);
        var tokenInterval = setInterval(function () {
            if ($cookies.get('token')) {
                clearInterval(tokenInterval);
                $scope.$apply(function () { $scope.data.downloadingReport = false; });
                $cookies.remove('token', { path: '/' });
            }
        }, 500);
    };

    var GetCommonParam = function () {
        var param = {};
        param.AccNo = GetSelectedAccount();
        param.pname = GetSelectedCompany();
        param.stdt = $scope.data.fromDate;
        param.enddt = $scope.data.toDate;
        param.token = $.now();
        return param;
    }

    var DownloadSummeryReport = function () {
        $scope.data.downloadingReport = true;
        var param = GetCommonParam();
        param.reportType = 'Dashboard_Summary_Report';
        var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
        DownloadFile(url);
    };

    var DownloadWebsiteActivityReport = function (id, reportType) {
        $scope.data.downloadingReport = true;
        var param = GetCommonParam();
        param.type = id;
        param.reportType = reportType == 'Content' ?
                            'Sweets_' + param.type + '_Content_Details_Report' :
                            'Sweets_' + param.type + '_Product_Details_Report';

        var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
        DownloadFile(url);

    };

    var DownloadProductLeadReport = function (userId) {
        $scope.data.downloadingReport = true;
        var param = GetCommonParam();
        param.leads = $scope.data.selectedLeadsSources;
        param.reportType = 'Sweets_Leads_Detail_Report';
        param.userid = userId;
        var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
        DownloadFile(url);
    };

    var DownloadProjectSummaryReport = function () {
        $scope.data.downloadingReport = true;
        var param = GetCommonParam();
        param.opportunity = $scope.data.selectedOpportunityTypes;
        param.reportType = 'Project_Summary_Report';
        var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
        DownloadFile(url);
    };

    var DownloadProjectDetailReport = function (drNo) {
        $scope.data.downloadingReport = true;
        var param = GetCommonParam();
        param.drNo = drNo;
        param.reportType = 'Project_Detail_Report';
        var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
        DownloadFile(url);
    };

    var DownloadNewsLetterReport = function () {
        $scope.data.downloadingReport = true;
        var param = GetCommonParam();
        param.reportType = 'Newsletter_Report';
        var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
        DownloadFile(url);
    };

    var DownloadLeadReport = function () {
        var items = [];
        $.each($scope.data.leadsSources, function (k, v) {
            if (v.SourceId != "All" && v.IsChecked == true) { items.push(v.SourceId); }
        });

        if (items.length > 0) {
            $scope.data.downloadingReport = true;
            var param = GetCommonParam();
            param.leads = items.join(",");
            param.reportType = 'Sweets_Leads_Report';
            var url = __RootUrl + "/DownloadReport.aspx?" + $.param(param);
            DownloadFile(url);
            $scope.$apply(function () { $scope.data.errorDownloadLeadReport = ""; });

            return true;
        }
        else {
            $scope.$apply(function () { $scope.data.errorDownloadLeadReport = "No Lead Type selected."; });
            return false;
        }
    };

    var OpenDownloadReportPopup = function () {
        var popup = $("div#divLeadsSources");
        $(popup).dialog({
            autoOpen: false,
            modal: true,
            width: 350,
            title: "Select Lead Type",
            dialogClass: "Popup",
            position: { my: 'top', at: 'top+100' },
            buttons: {
                "Download": function () {
                    if (DownloadLeadReport()) { $(this).dialog("close"); }
                },
                "Close": function () {
                    $(this).dialog("close");
                }
            }
        });

        $(popup).dialog("open");
    };

    var GetSelectedCompany = function () {
        return CommonServices.GetSelectedCompany($scope.data.selectedCompany, $scope.data.companies);
    };

    var GetSelectedAccount = function () {
        return CommonServices.GetSelectedAccount($scope.data.selectedAccount, $scope.data.accounts);
    };

    var GetSelectedLeadsSources = function () {
        return CommonServices.GetSelectedLeadsSources($scope.data.selectedLeadsSources, $scope.data.leadsSources);
    };

    var GetSelectedProductCategory = function () {
        return CommonServices.GetSelectedProductCategory($scope.data.productCategories);
    };

    var GetLeadsSources = function () {
        return CommonServices.GetLeadsSources(GetSelectedCompany(), $scope.data.fromDate, $scope.data.toDate)
        .then(function (payload) {
            $scope.data.leadsSources = payload;
            $scope.data.selectedLeadsSources = "All";
        });
    };

    $scope.OnDDLReportOpenClick = function ($event) {
        var ele = $($event.currentTarget);
        var ddl = $(ele).parents("div.dropdown__wrapper").find("div.dropdown__options");
        $(ddl).toggleClass("dropdown__options--open");
        $event.stopPropagation();
    };

    $scope.OnReportOpenClick = function (reportType, $event) {
        var ele = $($event.currentTarget);
        var ddl = $(ele).parents("div.dropdown__wrapper").find("div.dropdown__options");
        $(ddl).removeClass("dropdown__options--open");
        if (reportType == 'SummaryReport') { DownloadSummeryReport(); }
        else if (reportType == 'WebsiteActivityReport') {
            var id = ele.attr("data-id");
            var report = ele.attr("report-id");
            DownloadWebsiteActivityReport(id, report);
        }
        else if (reportType == 'ProductLeadReport') {
            var id = ele.attr("data-id");
            DownloadProductLeadReport(id);
        }
        else if (reportType == 'ProjectSummaryReport') {
            DownloadProjectSummaryReport();
        }
        else if (reportType == 'ProjectDetailReport') {
            var id = ele.attr("data-id");
            DownloadProjectDetailReport(id);
        }
        else if (reportType == 'NewsLetterReport') {
            DownloadNewsLetterReport();
        }
        else { OpenDownloadReportPopup(); }
        $event.stopPropagation();
    };

    $scope.OnLeadsSourcesCheckChange = function (d) {
        var tempLeadSource = $scope.data.leadsSources[d];
        if (tempLeadSource.SourceName == "All") {
            $.each($scope.data.leadsSources, function (k, v) { v.IsChecked = tempLeadSource.IsChecked; });
        } else {
            if (tempLeadSource.IsChecked == false) {
                var tempLeadSource = $($scope.data.leadsSources).filter(function (index) { return this.SourceId === "All"; });
                if (tempLeadSource.length > 0) { tempLeadSource[0].IsChecked = false; }
            }
        }
    };

    $scope.$on("Dashboard_FilterChanged", function (evt, data) {
        GetLeadsSources();
    });

    angular.element(document).ready(function () { });
});


