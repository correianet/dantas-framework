using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Implements this for get validate at repositories operations.
    /// </summary>
    public interface IAutoValidable
    {
        /// <summary>
        /// Validate object.
        /// </summary>
        /// <param name="ruleSet">Ruleset name.</param>
        void Validate(string ruleSet=null);
    }
}
