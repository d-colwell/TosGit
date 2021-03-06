﻿using System;

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

    }
}
