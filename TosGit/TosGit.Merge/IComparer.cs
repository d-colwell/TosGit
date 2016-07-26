using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosGit.Merge
{
    public interface IComparer
    {
        ComparisonResult Compare(object unmodified, object modified);
    }
}
