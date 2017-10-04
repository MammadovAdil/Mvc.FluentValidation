using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System.Web.Mvc;
using Ma.Mvc.FluentValidation.Models;

namespace Ma.Mvc.FluentValidation.PropertyValidators
{
    public class LessThanOrEqualPropertyValidator 
        : FluentValidationPropertyValidator
    {
        public LessThanOrEqualPropertyValidator(
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
            LessThanOrEqualValidator lessThanOrEqualValidator = (LessThanOrEqualValidator)Validator;

            // Initialize client rule
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ValidationType = "lessthanorequalto";

            MessageFormatter formatter = new MessageFormatter()
                .AppendPropertyName(Metadata.DisplayName ?? Rule.PropertyName);
            if(lessThanOrEqualValidator.MemberToCompare != null)
            {                
                // Append comparision member to message
                formatter.AppendArgument(
                    comparisionValuePlaceHolder, 
                    lessThanOrEqualValidator.MemberToCompare.GetDisplayName());

                // Append '*.' to the name of field for prefix merging
                rule.ValidationParameters["field"] = string.Format("*.{0}",
                    lessThanOrEqualValidator.MemberToCompare.Name);
            }
            else if(lessThanOrEqualValidator.ValueToCompare != null)
            {
                string valueToCompare = null;

                if(lessThanOrEqualValidator.ValueToCompare is DateTime)
                {
                    // If value is of type DateTime convert it to DateTime
                    // and format as yyyy-MM-dd to be able to parse it at client side
                    // easily using jQuery.
                    DateTime dateValueToCompare = (DateTime)lessThanOrEqualValidator.ValueToCompare;
                    valueToCompare = dateValueToCompare.ToString("yyyy-MM-dd");
                }
                else
                {
                    valueToCompare = lessThanOrEqualValidator.ValueToCompare.ToString();
                }

                // Append comparision value to message
                formatter.AppendArgument(comparisionValuePlaceHolder, valueToCompare);
                // Set value to compare
                rule.ValidationParameters["value"] = valueToCompare;
            }

            // Set error message of rule
            rule.ErrorMessage = formatter.BuildMessage(Validator.ErrorMessageSource.GetString(null));

            yield return rule;
        }
    }
}
