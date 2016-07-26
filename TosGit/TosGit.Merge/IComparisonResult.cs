using System;
using System.Collections.Generic;

namespace TosGit.Merge
{
    public interface IComparisonResult
    {
        Type ComparedType { get; }
        string Description { get; }
        IList<Difference> Differences { get; set; }
        bool IsDifferent { get; set; }
    }
}