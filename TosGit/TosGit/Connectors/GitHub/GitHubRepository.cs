using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        string IRepository.Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
