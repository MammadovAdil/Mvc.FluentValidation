using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Ma.Mvc.FluentValidation.Models
{
    public static class Utilities
    {
        /// <summary>
        /// Get display name of member. If DisplayName attribute
        /// has been aplied get that name, otherwise get member name.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When member is null.
        /// </exception>
        /// <param name="member">Member to get display name.</param>
        /// <returns>Display name of member</returns>
        public static string GetDisplayName(this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            string displayName = member.Name;

            CustomAttributeData displayNameAttributeData =  member
                .CustomAttributes
                .Where(m => m.AttributeType == typeof(DisplayAttribute))
                .FirstOrDefault();

            if (displayNameAttributeData != null)
                displayName = displayNameAttributeData
                    .NamedArguments
                    .FirstOrDefault(a => a.MemberName == "Name")
                    .TypedValue
                    .Value
                    .ToString();

            return displayName;
        }
    }
}
