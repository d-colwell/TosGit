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

        public string Name => _repository.Name;

    }
}
