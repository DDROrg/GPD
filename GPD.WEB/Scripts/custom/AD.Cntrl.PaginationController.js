//PaginationController =================================
angular.module('Pagination').controller('PaginationController', function ($scope, $location) {
    var maxButtonCount = 5;
    
    $scope.$on("BeforePaginationRendered", function (evt, data) {
        $scope.data.Pagination.buttons = [];
        $scope.data.Pagination.maxPageIndex = Math.ceil($scope.data.Pagination.totalRecord / $scope.data.Pagination.selectedPageSize);

        var firstButtonIndex = Math.ceil($scope.data.Pagination.currentPageIndex / maxButtonCount) * maxButtonCount - maxButtonCount + 1;
        var lastButtonIndex = Math.ceil($scope.data.Pagination.currentPageIndex / maxButtonCount) * maxButtonCount;
        if (lastButtonIndex > $scope.data.Pagination.maxPageIndex) { lastButtonIndex = $scope.data.Pagination.maxPageIndex; }

        for (var i = firstButtonIndex; i <= lastButtonIndex; i++) {
            $scope.data.Pagination.buttons.push(i);
        }
    });

    $scope.OnPageIndexClick = function (v) {
        $scope.data.Pagination.currentPageIndex = v;
        $scope.$emit("Pagination_OnChange", "");
    };

    $scope.OnPageSizeChange = function () {
        $scope.data.Pagination.currentPageIndex = 1;
        $scope.$emit("Pagination_OnChange", "");
    };

    $scope.OnPreviousPageIndexClick = function () {
        var st = $scope.data.Pagination.buttons[0] - maxButtonCount;
        if (st >= 1) {
            $scope.data.Pagination.currentPageIndex = st;
        }
        else {
            $scope.data.Pagination.currentPageIndex = 1;
        }
        $scope.$emit("Pagination_OnChange", "");
    };

    $scope.OnNextPageIndexClick = function () {
        var st = $scope.data.Pagination.buttons[0] + maxButtonCount;
        if (st <= $scope.data.Pagination.maxPageIndex) {
            $scope.data.Pagination.currentPageIndex = st;
        }
        else {
            $scope.data.Pagination.currentPageIndex = $scope.data.Pagination.maxPageIndex;
        }
        $scope.$emit("Pagination_OnChange", "");
    };

    $scope.IsPreviousVisible = function () {
        return $.inArray(1, $scope.data.Pagination.buttons) == -1 ? true : false;
    };

    $scope.IsNextVisible = function () {
        return $.inArray($scope.data.Pagination.maxPageIndex, $scope.data.Pagination.buttons) == -1 ? true : false;
    };

    $scope.GetPaginationText = function () {
        var retVal = "Showing: 0 result";
        if ($scope.data.Pagination.totalRecord > 0 && $scope.data.Pagination.totalRecord <= $scope.data.Pagination.selectedPageSize) {
            retVal = "Showing: " + $scope.data.Pagination.totalRecord + " of " + $scope.data.Pagination.totalRecord + " result(s)";
        } else if ($scope.data.Pagination.totalRecord > 0) {
            var resultStart = ($scope.data.Pagination.currentPageIndex - 1) * $scope.data.Pagination.selectedPageSize + 1;
            var resultEnd = $scope.data.Pagination.currentPageIndex * $scope.data.Pagination.selectedPageSize;
            if (resultEnd > $scope.data.Pagination.totalRecord) { resultEnd = $scope.data.Pagination.totalRecord; }
            retVal = "Showing: " + resultStart + " - " + resultEnd + " of " + $scope.data.Pagination.totalRecord + " result(s)";
        }
        return retVal;
    };

    $scope.GetButtonCss = function (v) {
        return $scope.data.Pagination.currentPageIndex == v ? "inactive" : "active";
    };

    angular.element(document).ready(function () { });
});