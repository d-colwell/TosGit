using System;
using System.Collections.Generic;

namespace TosGit.Merge
{
    public class ComparisonResult: IComparisonResult
    {
        public ComparisonResult(string description, Type comparedType)
        {
            ComparedType = comparedType;
            Description = description;
            Differences = new List<Difference>();
        }
        public string Description { get; private set; }
        public Type ComparedType { get; private set; }
        public bool IsDifferent { get; set; }
        public IList<Difference> Differences { get; set; }
    }
}