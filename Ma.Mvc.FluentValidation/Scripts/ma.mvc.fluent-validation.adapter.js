(function ($) {

    /*
     * 
     * 
     * 
     * 
     * 
     * Not equal comparer
     * 
     * 
     * 
     * 
     * 
     * 
     */
    $.validator.addMethod("notequal", function (value, element, param) {
        var comparedValues = getComparedValues(value, param);

        // If compared values was not found do not validate
        if (!comparedValues)
            return true;

        return this.optional(element) || comparedValues.currentValue != comparedValues.comparedValue;
    }, "This has to be different...");

    $.validator.unobtrusive.adapters.add("notequal", ["field", "value"], function (options) {
        // Get rule parametrs
        var ruleParams = getRuleParametersFromOptions(options);

        // Set rule parameters
        options.rules["notequal"] = ruleParams;

        if (options.message)
            options.messages["notequal"] = options.message;
    });


    /*
     * 
     * 
     * 
     * 
     * 
     * Less than or equal to comparer
     * 
     * 
     * 
     * 
     * 
     * 
     */
    $.validator.addMethod("lessthanorequalto", function (value, element, param) {
        var comparedValues = getComparedValues(value, param);

        // If compared values was not found do not validate
        if (!comparedValues)
            return true;

        return this.optional(element) || comparedValues.currentValue <= comparedValues.comparedValue;
    }, "Must be less than or equal to...");

    $.validator.unobtrusive.adapters.add("lessthanorequalto", ["field", "value"], function (options) {
        // Get rule parameters
        var ruleParams = getRuleParametersFromOptions(options);

        // Set rule parameters
        options.rules["lessthanorequalto"] = ruleParams;

        if (options.message)
            options.messages["lessthanorequalto"] = options.message;
    });



    /*
     * 
     * 
     * 
     * 
     * 
     * Greater than comparer
     * 
     * 
     * 
     * 
     * 
     * 
     */
    $.validator.addMethod("greaterthan", function (value, element, param) {
        var comparedValues = getComparedValues(value, param);

        // If compared values was not found do not validate
        if (!comparedValues)
            return true;

        return this.optional(element) || comparedValues.currentValue > comparedValues.comparedValue;
    }, "Must be greater than....");

    $.validator.unobtrusive.adapters.add("greaterthan", ["field", "value"], function (options) {
        // Get rule parameters
        var ruleParams = getRuleParametersFromOptions(options);

        // Set rule parameters
        options.rules["greaterthan"] = ruleParams;

        if (options.message)
            options.messages["greaterthan"] = options.message;
    });

    /*
     * 
     * 
     * 
     * 
     * 
     * Greater than or equal comparer
     * 
     * 
     * 
     * 
     * 
     * 
     */
    $.validator.addMethod("greaterthanorequal", function (value, element, param) {
        var comparedValues = getComparedValues(value, param);

        // If compared values was not found do not validate
        if (!comparedValues)
            return true;

        return this.optional(element) || comparedValues.currentValue >= comparedValues.comparedValue;
    }, "Must be greater than or equal....");

    $.validator.unobtrusive.adapters.add("greaterthanorequal", ["field", "value"], function (options) {
        // Get rule parameters
        var ruleParams = getRuleParametersFromOptions(options);

        // Set rule parameters
        options.rules["greaterthanorequal"] = ruleParams;

        if (options.message)
            options.messages["greaterthanorequal"] = options.message;
    });


    /*
     * 
     * 
     * 
     * 
     * 
     * 
     * Common functions
     * 
     * 
     * 
     * 
     * 
     * 
     */

    // Get rule parameters from options
    function getRuleParametersFromOptions(options) {
        var ruleParams = new Object();

        if (options.params.field) {
            // Get full name of field
            var elementName = $(options.element).attr("name");
            var prefix = getModelPrefix(elementName);
            var field = options.params.field;
            var fullFieldName = appendModelPrefix(field, prefix);
            var element = $(options.form).find(":input[name='" + fullFieldName + "']").first();

            // Set values of ruleParams
            ruleParams.type = "field";
            ruleParams.value = element;
        }
        else if (options.params.value) {
            // Set values of ruleParams
            ruleParams.type = "value";
            ruleParams.value = options.params.value;
        }

        return ruleParams;
    }

    // Get and parse current and compared value
    function getComparedValues(value, param) {
        var comparedValue;
        var currentValue = parseValue(value);

        if (param.type === "value") {
            // If param type is value then get its value
            comparedValue = parseValue(param.value);
        }
        else if (param.type === "field") {
            // If param type is field then get value of the field
            var comparedElement = $(param.value);

            // If field was found get its value, otherwise return undefined
            if (comparedElement.length)
                comparedValue = parseValue(comparedElement.val());
            else
                return undefined;
        }

        // Get type of compared values
        var comparedType = jQuery.type(currentValue);

        // If compared types are date then
        // compare them using .getTime() method.
        if (comparedType === "date") {
            currentValue = currentValue.getTime();
            comparedValue = comparedValue.getTime();
        }

        var comparedValues = new Object();
        comparedValues.currentValue = currentValue;
        comparedValues.comparedValue = comparedValue;

        return comparedValues;
    }

    // Parse value
    function parseValue(value) {
        // Replace possible comma delimiters
        value = replaceComma(value);

        // Check if value is number or not
        var isNotNumber = isNaN(value);

        // If it is number try to parse it to float
        var result;
        if (!isNotNumber) {
            result = parseFloat(value);
            return result;
        }
        else {
            // If it is not a number try to parse it to date
            result = parseDate(value);
            return result;
        }
    }


    // Get prefix of models
    function getModelPrefix(fieldName) {
        return fieldName.substr(0, fieldName.lastIndexOf(".") + 1);
    }

    // Add prefix of model to name of field
    function appendModelPrefix(value, prefix) {
        if (value.indexOf("*.") === 0) {
            value = value.replace("*.", prefix);
        }
        return value;
    }

    // check and parse date
    function parseDate(date) {
        var m = date.match(/^(\d{4})-(\d{1,2})-(\d{1,2})$/);
        return m ? new Date(parseInt(m[1]), parseInt(m[2]) - 1, parseInt(m[3])) : null;
    }

    // Parse number wich has comma delimiter 
    function replaceComma(number) {
        return number.replace(",", ".");
    }
})(jQuery);