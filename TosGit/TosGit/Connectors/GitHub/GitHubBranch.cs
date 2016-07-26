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
        public string Name
        {
            get
            {
                return _branch.Name;
            }
        }
    }
}
