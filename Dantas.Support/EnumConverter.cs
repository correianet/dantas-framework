using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Support
{
    /// <summary>
    /// Convert enum to string representation, using attribute and resource file (optionally).
    /// </summary>
    public static class EnumConverter
    {
        /// <summary>
        /// Parse the enum value to string.
        /// </summary>
        /// <param name="enumValue">Enum value.</param>
        /// <returns>String representation for enum value.</returns>
        public static string ToString(object enumValue)
        {
            var result = String.Empty;

            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString()).Single();
            var enumAttribute = (EnumTextAttribute)memInfo.GetCustomAttributes(typeof(EnumTextAttribute), false).SingleOrDefault();

            if (enumAttribute == null)
            {
                enumAttribute = (EnumTextAttribute)type.GetCustomAttributes(typeof(EnumTextAttribute), false).SingleOrDefault();
                if (enumAttribute == null)
                    return enumValue.ToString();
            }

            if (enumAttribute.Text == null)
            {
                var resourceName = enumAttribute.ResourceName;
                if (String.IsNullOrWhiteSpace(enumAttribute.ResourceName))
                    resourceName = enumValue.GetType().Name + enumValue.ToString();

                PropertyInfo resource = enumAttribute.ResourceType.GetProperty(resourceName);

                if (resource != null)
                {
                    result = resource.GetValue(enumAttribute, null).ToString();
                }
            }
            else
                result = enumAttribute.Text;

            return result;
        }
    }
}
