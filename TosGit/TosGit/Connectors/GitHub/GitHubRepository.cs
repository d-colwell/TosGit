using System;
using Octokit;

namespace TosGit.Connectors.GitHub
{
    public class GitHubRepository : IRepository
    {
        private readonly Repository _repository;

        public GitHubRepository(Octokit.Repository repo)
        {
            this._repository = repo;
        }
        // Changed from a GET into a single function call to the Repo Name.
        public string Name => _repository.Name;

    }
}
