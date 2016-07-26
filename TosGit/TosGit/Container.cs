using System;
using TosGit.Connectors;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit
{
    public  class Container
    {
        private static Container _instance = null;
        public static Container Instance => _instance ?? (_instance = new Container());

        public IRepositoryConnector GetRepositoryConnector(string gitServer, string url, string username, string password)
        {
            //Decision which repository to use here
            
            if (gitServer == "GitHub")
                return new Connectors.GitHub.GitHubRepoConnector(url, username, password);
            else if (gitServer == "BitBucket")
                return new Connectors.BitBucket.BitBucketRepositoryConnector(url, username, password);
            else
                throw new NotImplementedException();
        }

        public ObjectTracker.IObjectTracker GetObjectTracker(TCComponentFolder branchFolder)
        {
            return new ObjectTracker.EmbeddedFileObjectTracker(branchFolder);
        }
    }
}
