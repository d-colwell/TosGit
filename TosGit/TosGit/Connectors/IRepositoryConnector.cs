using System.Collections.Generic;

namespace TosGit.Connectors
{
    public interface IRepositoryConnector
    {
        IEnumerable<IBranch> GetRemoteBranches(string project, string repository);
        IEnumerable<IRepository> GetRepositories(string project);
        bool TestConnection();
    }
}