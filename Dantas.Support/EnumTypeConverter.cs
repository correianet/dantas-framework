using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Support
{
    /// <summary>
    /// AutoMapper Converter for enum types.
    /// </summary>
    public class EnumTypeConverter : AutoMapper.ITypeConverter<Enum, string>
    {
        #region ITypeConverter<Enum,string> Members

        /// <summary>
        /// Convert a enum using automapper context
        /// </summary>
        /// <param name="context">AutoMapper context used by framework</param>
        /// <returns>String that represents the enum value</returns>
        public string Convert(AutoMapper.ResolutionContext context)
        {
            if (context.SourceValue != null)
                return EnumConverter.ToString(context.SourceValue);
            else
                return String.Empty;
        }

        #endregion
    }
}
