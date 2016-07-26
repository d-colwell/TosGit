using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TosGit.Connectors;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit
{
    public  class Container
    {
        private static Container instance = null;
        public static Container Instance
        {
            get
            {
                if (instance == null)
                    instance = new Container();
                return instance;
            }
        }

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
