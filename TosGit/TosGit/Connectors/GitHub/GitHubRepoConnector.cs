using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using TosGit.Connectors;

namespace TosGit.Connectors.GitHub
{
    public class GitHubRepoConnector : IRepositoryConnector
    {
        Octokit.GitHubClient client;
        public GitHubRepoConnector(string repoURL, string username, string password)
        {
            var uri = new Uri(repoURL);
            this.client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ToscaConnector"), uri);
            var credentials = new Octokit.Credentials(username, password);
            client.Credentials = credentials;
        }

        public bool TestConnection()
        {
            try
            {
                if (GetRepositories().Any())
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<IRepository> GetRepositories()
        {
            var repositories = client.Repository.GetAllForCurrent().Result;
            return repositories.Select(x => new GitHubRepository(x));
        }

        public IEnumerable<IBranch> GetRemoteBranches(string repository)
        {
            var branches = client.Repository.GetAllBranches(client.Credentials.Login, repository).Result;
            return branches.Select(b => new GitHubBranch(b));
        }

    }
}
