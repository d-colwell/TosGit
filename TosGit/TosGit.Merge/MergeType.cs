using System.Collections.Generic;

namespace TosGit.Merge
{
    public class MergeType
    {
        public string TypeName { get; set; }
        public string FriendlyName { get; set; }
        public IList<MergeProperty> IncludedProperties { get; set; }
    }
}