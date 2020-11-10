using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Support
{
    /// <summary>
    /// Attribute for usage when enum type needed be converted to a custom string. Optionally can use with resource file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumTextAttribute : Attribute
    {
        /// <summary>
        /// Create default instance.
        /// </summary>
        public EnumTextAttribute() { }

        /// <summary>
        /// Create a instance with text.
        /// </summary>
        /// <param name="text">Text for representing enum value.</param>
        public EnumTextAttribute(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Resource file type.
        /// </summary>
        public Type ResourceType { get; set; }

        /// <summary>
        /// Resource key name.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Text configured on initialization.
        /// </summary>
        public string Text { get; private set; }
    }
}
