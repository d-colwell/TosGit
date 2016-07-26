using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosGit.Merge.CustomComparers
{
    public abstract class ComparerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ComparerBase()
        {
        }
        public virtual Type[] ApplicableTypes { get; }

    }
}
