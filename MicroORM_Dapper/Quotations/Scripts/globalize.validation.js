// replace methods in jquery.validate.js ($.validator.methods) as necessary for localized validation:

$.validator.methods.number = function (value, element) {
    return this.optional(element) || !isNaN(Globalize.parseFloat(value));
}

$.validator.methods.date = function (value, element) {
    if (this.optional(element))
        return true;
    var result = Globalize.parseDate(value);
    return !isNaN(result) && (result != null);
}

$.validator.methods.min = function (value, element, param) {
    return this.optional(element) || Globalize.parseFloat(value) >= param;
}

$.validator.methods.max = function (value, element, param) {
    return this.optional(element) || Globalize.parseFloat(value) <= param;
}

$.validator.methods.range = function (value, element, param) {
    if (this.optional(element))
        return true;
    var result = Globalize.parseFloat(value);
    return (result >= param[0] && result <= param[1]);
}