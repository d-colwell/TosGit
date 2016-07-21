using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Stash.Entities;

namespace TosGit.Connectors.BitBucket
{
    class BitBucketBranch : IBranch
    {
        private readonly Branch _branch;

        public BitBucketBranch(Atlassian.Stash.Entities.Branch branch)
        {
            this._branch = branch;
        }

        public string Name
        {
            get
            {
                return _branch.DisplayId;
            }
        }
    }
}
