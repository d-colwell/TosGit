using System.Collections.Generic;

namespace TosGit.Connectors
{
    public interface IRepositoryConnector
    {
        IEnumerable<IBranch> GetRemoteBranches(string repository);
        IEnumerable<IRepository> GetRepositories();
        bool TestConnection();
    }
}