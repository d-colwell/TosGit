using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Stash;

namespace TosGit.Connectors.BitBucket
{
    class BitBucketRepositoryConnector : IRepositoryConnector
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
                //OMNOMNOMNOM tasty exceptions
            }
        }
        public IEnumerable<IBranch> GetRemoteBranches(string repository)
        {
            var repo = _client.Repositories.Get(repository).Result.Values.FirstOrDefault();
            var branches = _client.Branches.Get(repository, repo.Slug).Result.Values;
            return branches.Select(b => new BitBucketBranch(b));
        }

        public IEnumerable<IRepository> GetRepositories()
        {
            throw new NotImplementedException();
        }

        public bool TestConnection()
        {
            if (_client == null)
                return false;
            return true;
        }
    }
}
