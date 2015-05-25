(function ($, Globalize) {
    if (Globalize) {
        var language = window.language || $("html").attr("lang") || $("html").data("lang") || navigator.language || navigator.browserLanguage;
        var culture = Globalize.culture(language);

        $.fn.datepicker.dates[culture.name] = {
            days: culture.calendar.days.names,
            daysShort: culture.calendar.days.namesAbbr,
            daysMin: culture.calendar.days.namesShort,
            months: culture.calendar.months.names,
            monthsShort: culture.calendar.months.namesAbbr,
            today: Globalize.localize("today") || "Today",
            format: "d",
            rtl: !!culture.isRTL,
            weekStart: culture.calendar.firstDay || 0
        }

        $.extend($.fn.datepicker.defaults, {
            language: culture.name
        });

        $.extend($.fn.datepicker.DPGlobal, {
            parseFormat: function (format) {
                return format;
            },
            formatDate: function (date, format) {
                return Globalize.format(date, format);
            },
            parseDate: function (date, format) {
                return Globalize.parseDate(date, format) || new Date();
            }
        });
    }
}(window.jQuery, window.Globalize))