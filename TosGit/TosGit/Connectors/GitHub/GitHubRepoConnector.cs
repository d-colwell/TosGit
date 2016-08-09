using System;
using System.Collections.Generic;
using System.Linq;
using Octokit;

namespace TosGit.Connectors.GitHub
{
    public class GitHubRepoConnector : IRepositoryConnector
    {
        public GitHubClient Client { get; }

        public GitHubRepoConnector(string repoUrl, string username, string password)
        {
            var uri = new Uri(repoUrl);
            this.Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ToscaConnector"), uri);
            var credentials = new Octokit.Credentials(username, password);
            Client.Credentials = credentials;
        }

        public bool TestConnection()
        {
            try
            {
                // IF statement did not give optimized code at this level. Converted to single return of the Array for both TRUE or FALSE
                return GetRepositories().Any();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<IRepository> GetRepositories(string project = "")
        {
            var repositories = Client.Repository.GetAllForCurrent().Result;
            return repositories.Select(x => new GitHubRepository(x));
        }

        public IEnumerable<IBranch> GetRemoteBranches(string project, string repository)
        {
            var branches = Client.Repository.GetAllBranches(Client.Credentials.Login, repository).Result;
            return branches.Select(b => new GitHubBranch(b));
        }

    }
}
