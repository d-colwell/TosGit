using System;

namespace TosGit.MergeUI.CustomComparers
{
    public abstract class ComparerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected ComparerBase(Type[] applicableTypes, string nameToDisplay)
        {
            ApplicableTypes = applicableTypes;
            NameToDisplay = nameToDisplay;
        }

        public virtual Type[] ApplicableTypes { get; }
        /// <summary>
        /// The text that should be used when describing the types this comparer utilise
        /// </summary>
        public virtual string NameToDisplay { get; }

    }
}
