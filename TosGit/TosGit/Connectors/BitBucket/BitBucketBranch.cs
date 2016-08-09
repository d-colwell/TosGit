using Atlassian.Stash.Entities;

namespace TosGit.Connectors.BitBucket
{
    internal class BitBucketBranch : IBranch
    {
        private readonly Branch _branch;

        public BitBucketBranch(Atlassian.Stash.Entities.Branch branch)
        {
            this._branch = branch;
        }
        // Changed from a GET into a single function call to the Branch ID.
        public string Name => _branch.DisplayId;
    }
}
