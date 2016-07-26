using System;
using System.Collections.Generic;
using System.Linq;
using Atlassian.Stash;

namespace TosGit.Connectors.BitBucket
{
    internal class BitBucketRepositoryConnector : IRepositoryConnector
    {
        private readonly StashClient _client;

        public BitBucketRepositoryConnector(string url, string username, string password)
        {
            try
            {
                _client = new Atlassian.Stash.StashClient(url, username, password);
            }
            catch (Exception)
            {
                // OMNOMNOMNOM tasty exceptions
                // Added Tasty Exceptions of Not Implemented
                throw new NotImplementedException();
            }
        }
        public IEnumerable<IBranch> GetRemoteBranches(string project ,string repository )
        {
            var repo = _client.Repositories.Get(project).Result.Values.FirstOrDefault(x => x.Name == repository);
            if (repo != null)
            {
                var branches = _client.Branches.Get(project, repo.Slug).Result.Values;
                return branches.Select(b => new BitBucketBranch(b));
            }
            return null;
        }

        public IEnumerable<IRepository> GetRepositories(string project)
        {
            return _client.Repositories.Get(project).Result.Values.Select(x => new BitBucketRepository(x));

        }

        public bool TestConnection()
        {
            if (_client == null)
                return false;
            return true;
        }
    }
}
