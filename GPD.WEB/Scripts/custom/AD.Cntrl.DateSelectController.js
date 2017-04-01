//DateSelectController =================================
angular.module('DateSelect').controller('DateSelectController', function ($scope) {
    
    var BindDatePicker = function () {
        var minSelDate = $.datepicker.parseDate("mm/dd/yy", "03/01/2015");
        var minToDate = $.datepicker.parseDate("mm/dd/yy", $scope.data.fromDate);
        var txtDateTo = $("input#txtDateTo");
        var txtDateFrom = $("input#txtDateFrom");

        $(txtDateFrom).datepicker({
            minDate: minSelDate,
            maxDate: new Date(),
            dateFormat: "mm/dd/yy",
            changeMonth: true,
            changeYear: true,
            beforeShow: function (elm, obj) {
                $(txtDateFrom).parents("span.input-wrapper").removeClass("error");
                $(txtDateTo).parents("span.input-wrapper").removeClass("error");
                $scope.$apply(function () {
                    $scope.data.DateSelect.hasError = false;
                    $scope.data.errorFilterSelection = "";
                });
                $(".ui-datepicker").css('font-size', 12);
                $(document.body).delegate('select.ui-datepicker-year', 'mousedown', function () {
                    (function (sel) {
                        var el = $(sel);
                        var ops = $(el).children().get();
                        if (ops.length > 0 && $(ops).first().val() < $(ops).last().val()) {
                            $(el).empty();
                            $(el).html(ops.reverse().slice(0, 3));
                        }
                    })(this);
                });
            },
            onClose: function (selectedDate, instance) {
                if (selectedDate != '') {
                    $(txtDateTo).datepicker("option", "minDate", selectedDate);
                    var dateFrom = $.datepicker.parseDate(instance.settings.dateFormat, selectedDate, instance.settings);
                    var tempDateTo = new Date(dateFrom.valueOf());
                    tempDateTo.setMonth(tempDateTo.getMonth() + 3);
                    var dateTo = $.datepicker.parseDate("mm/dd/yy", $(txtDateTo).val());

                    if (dateFrom > dateTo || dateTo > tempDateTo) {
                        $(txtDateFrom).parents("span.input-wrapper").addClass("error");
                        $scope.$apply(function () {
                            $scope.data.DateSelect.hasError = true;
                            $scope.data.errorFilterSelection = "You can generate a maximum of 3 months of data to review.";
                        });
                    } else if ($scope.data.fromDate != selectedDate || $scope.data.toDate != $(txtDateTo).val()) {
                        $scope.data.fromDate = selectedDate;
                        $scope.data.toDate = $(txtDateTo).val();
                        $scope.$emit("DateSelect_DateChanged", "");
                    }
                }
            }
        });

        $(txtDateTo).datepicker({
            minDate: minToDate,
            maxDate: new Date(),
            dateFormat: "mm/dd/yy",
            changeMonth: true,
            changeYear: true,
            beforeShow: function (elm, obj) {
                $(txtDateFrom).parents("span.input-wrapper").removeClass("error");
                $(txtDateTo).parents("span.input-wrapper").removeClass("error");
                $scope.$apply(function () {
                    $scope.data.DateSelect.hasError = false;
                    $scope.data.errorFilterSelection = "";
                });
                $(".ui-datepicker").css('font-size', 12);
                $(document.body).delegate('select.ui-datepicker-year', 'mousedown', function () {
                    (function (sel) {
                        var el = $(sel);
                        var ops = $(el).children().get();
                        if (ops.length > 0 && $(ops).first().val() < $(ops).last().val()) {
                            $(el).empty();
                            $(el).html(ops.reverse().slice(0, 3));
                        }
                    })(this);
                });
            },
            onClose: function (selectedDate, instance) {
                if (selectedDate != '') {
                    var dateFrom = $.datepicker.parseDate("mm/dd/yy", $(txtDateFrom).val());
                    var tempDateTo = new Date(dateFrom.valueOf());
                    tempDateTo.setMonth(tempDateTo.getMonth() + 3);
                    var dateTo = $.datepicker.parseDate(instance.settings.dateFormat, selectedDate, instance.settings);
                    if (dateFrom > dateTo || dateTo > tempDateTo) {
                        $(txtDateTo).parents("span.input-wrapper").addClass("error");
                        $scope.$apply(function () {
                            $scope.data.DateSelect.hasError = true;
                            $scope.data.errorFilterSelection = "You can generate a maximum of 3 months of data to review.";
                        });
                    } else if ($scope.data.fromDate != $(txtDateFrom).val() || $scope.data.toDate != selectedDate) {
                        $scope.data.fromDate = $(txtDateFrom).val();
                        $scope.data.toDate = selectedDate;
                        $scope.$emit("DateSelect_DateChanged", "");
                    }
                }
            }
        });    
    };

    angular.element(document).ready(function () {
        BindDatePicker();
    });
});