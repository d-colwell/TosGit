using Octokit;

namespace TosGit.Connectors.GitHub
{
    public class GitHubBranch : IBranch
    {
        private readonly Branch _branch;

        public GitHubBranch(Octokit.Branch branch)
        {
            this._branch = branch;
        }
        // Changed from a GET into a single function call to the Branch Name.
        public string Name => _branch.Name;
    }
}
