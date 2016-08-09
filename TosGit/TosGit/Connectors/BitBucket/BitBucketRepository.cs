using System;
using Atlassian.Stash.Entities;

namespace TosGit.Connectors.BitBucket
{
    internal class BitBucketRepository : IRepository
    {
        private readonly Repository _repository;

        public BitBucketRepository(Atlassian.Stash.Entities.Repository repository)
        {
            this._repository = repository;
        }
        // Changed from a GET into a single function call to the Repo Name.
        public string Name => _repository.Name;
    }
}
