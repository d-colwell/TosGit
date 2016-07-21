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
        public IEnumerable<IBranch> GetRemoteBranches(string project ,string repository )
        {
            var repo = _client.Repositories.Get(project).Result.Values.Where(x=>x.Name == repository).FirstOrDefault();
            var branches = _client.Branches.Get(project, repo.Slug).Result.Values;
            return branches.Select(b => new BitBucketBranch(b));
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
