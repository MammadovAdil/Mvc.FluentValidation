using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System.Web.Mvc;
using Ma.Mvc.FluentValidation.Models;

namespace Ma.Mvc.FluentValidation.PropertyValidators
{
    public class GreaterThanOrEqualPropertyValidator
        : FluentValidationPropertyValidator
    {
        public GreaterThanOrEqualPropertyValidator(
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

            string comparisionValuePlaceHolder = "ComparisonValue";

            // Convert validator
            GreaterThanOrEqualValidator greaterThanOrEqualValidator =
                (GreaterThanOrEqualValidator)Validator;

            // Initializ client rule
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ValidationType = "greaterthanorequal";

            MessageFormatter formatter = new MessageFormatter()
                .AppendPropertyName(Metadata.DisplayName ?? Rule.PropertyName);
            if (greaterThanOrEqualValidator.MemberToCompare != null)
            {
                // If member has been selected to compare create tule for field.

                // Append comparision member to message
                formatter.AppendArgument(
                    comparisionValuePlaceHolder,
                    greaterThanOrEqualValidator.MemberToCompare.GetDisplayName());

                // Append '*.' to the name of field for prefix merging
                rule.ValidationParameters["field"] = string.Format("*.{0}",
                    greaterThanOrEqualValidator.MemberToCompare.Name);                    
            }
            else if(greaterThanOrEqualValidator.ValueToCompare != null)
            {
                // If value has been set to compare create rule for value
                string valueToCompare = null;

                if(greaterThanOrEqualValidator.ValueToCompare is DateTime)
                {
                    // If value is of type DateTime convert it to DateTime
                    // and format as yyyy-MM-dd to be able to parse it at client side
                    // easily using jQuery.
                    DateTime dateValueToCompare = (DateTime)greaterThanOrEqualValidator.ValueToCompare;
                    valueToCompare = dateValueToCompare.ToString("yyyy-MM-dd");
                }
                else
                {
                    valueToCompare = greaterThanOrEqualValidator.ValueToCompare.ToString();
                }

                // Append comparision value to message
                formatter.AppendArgument(comparisionValuePlaceHolder, valueToCompare);
                // Set value to compare
                rule.ValidationParameters["value"] = valueToCompare;
            }

            // Set error message or rule
            rule.ErrorMessage = formatter.BuildMessage(Validator.ErrorMessageSource.GetString());

            yield return rule;
        }
    }
}