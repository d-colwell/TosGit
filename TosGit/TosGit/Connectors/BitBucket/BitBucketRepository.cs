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
        public string Name
        {
            get
            {
                return _repository.Name;
            }
        }

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
