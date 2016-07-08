using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Stash.Entities;

namespace TosGit.Connectors.BitBucket
{
    class BitBucketRepository : IRepository
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
    }
}
