using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosGit
{
    public class GitRepoConnector
    {
        Octokit.GitHubClient client;
        public GitRepoConnector(string repoURL, string username, string password)
        {
            var uri = new Uri(repoURL);
            this.client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ToscaConnector"),uri);
            var credentials = new Octokit.Credentials(username, password);
            client.Credentials = credentials;
        }

        public bool TestConnection()
        {
            try
            {
                if(GetRepositories().Any())
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Octokit.Repository> GetRepositories()
        {
            var repositories = client.Repository.GetAllForCurrent().Result;
            return repositories;
        }

        public IEnumerable<Octokit.Branch> GetRemoteBranches(string repository)
        {
            var repositories = client.Repository.GetAllBranches(client.Credentials.Login, repository).Result;
            return repositories;
            //using (var repo = new Repository(repoPath))
            //{
            //    var remote = repo.Network.Remotes.First();
            //    var references = repo.Network.ListReferences(remote);
            //    return references.Where(x=>x.CanonicalName.StartsWith("refs/heads")).Select(x => new BranchDefinition { Name = x.CanonicalName.Replace("refs/heads/", ""), Reference = x.CanonicalName });
            //    //var branches = repo.Branches.Where(x => x.IsRemote);
            //    //var origin = branches.First(x => x.CanonicalName == "origin/master");
            //    //var rootNode = GetChildrenRecursive(origin, branches);
            //    //return new GitBranchTree { Origin = rootNode };
            //}
        }

        public IEnumerable<Octokit.PullRequest> GetPullRequests(string repository)
        {
            var pullRequests = client.PullRequest.GetAllForRepository(client.Credentials.Login, repository);
            return pullRequests.Result;
        }

        //public IEnumerable<Octokit.PullRequest> GetPullRequests()
        //{

        //    Octokit.ApiConnection connection = new Octokit.ApiConnection();
        //    Octokit.PullRequestsClient client = new Octokit.PullRequestsClient()
        //}
    }
}
