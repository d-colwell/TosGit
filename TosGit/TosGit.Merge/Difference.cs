using System.Collections.Generic;

namespace TosGit.Merge
{
    public class Difference
    {
        public string Property { get; set; }
        public object Unmodified { get; set; }
        public object Modified { get; set; }
    }
}