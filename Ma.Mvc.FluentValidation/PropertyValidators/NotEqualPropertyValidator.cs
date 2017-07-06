using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ma.Mvc.FluentValidation.Models;

namespace Ma.Mvc.FluentValidation.PropertyValidators
{
    public class NotEqualPropertyValidator 
        : FluentValidationPropertyValidator
    {
        public NotEqualPropertyValidator(
            ModelMetadata metadata,
            ControllerContext controllerContext,
            PropertyRule rule,
            IPropertyValidator validator)
            : base(metadata, controllerContext, rule, validator)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules())
                yield break;

            string comparisonValuePlaceHolder = "ComparisonValue";

            // Convert validator
            NotEqualValidator notEqualValidator = (NotEqualValidator)Validator;

            // Initialize client rule
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ValidationType = "notequal";

            MessageFormatter formatter = new MessageFormatter()
                .AppendPropertyName(Metadata.DisplayName ?? Rule.PropertyName);
            if (notEqualValidator.MemberToCompare != null)
            {
                // Append comparision member to message
                formatter.AppendArgument(
                    comparisonValuePlaceHolder, 
                    notEqualValidator.MemberToCompare.GetDisplayName());

                // Append '*.' to the name of field for prefix merging
                rule.ValidationParameters["field"] = string.Format("*.{0}",
                    notEqualValidator.MemberToCompare.Name);
            }
            else if (notEqualValidator.ValueToCompare != null)
            {
                string valueToCompare = null;

                if(notEqualValidator.ValueToCompare is DateTime)
                {
                    // If value is of type DateTime convert it to DateTime
                    // and format as yyyy-MM-dd to be able to parse it at client side
                    // easily using jQuery.
                    DateTime dateValueToCompare = (DateTime)notEqualValidator.ValueToCompare;
                    valueToCompare = dateValueToCompare.ToString("yyyy-MM-dd");
                }
                else
                {
                    valueToCompare = notEqualValidator.ValueToCompare.ToString();
                }

                // Append comparision value to message
                formatter.AppendArgument(comparisonValuePlaceHolder, valueToCompare);
                // Set value to compare
                rule.ValidationParameters["value"] = valueToCompare;
            }

            // Set error message of rule
            rule.ErrorMessage = formatter.BuildMessage(Validator.ErrorMessageSource.GetString());

            yield return rule;
        }
    }
}
