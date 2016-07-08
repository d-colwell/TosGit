using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosGit
{
    public class Config
    {
        private static Config _instance;

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Config();
                return _instance;
            }
        }

        public string RepoProperty => "Repository";
        public string RepoUserProperty => "RepoUser";
        public string RepoPasswordProperty => "RepoPassword";
        public string BranchFolderName => "Branches";
        public string BranchPropertyName => "OriginBranch";
        public string RepoNameProperty => "RepoName";
        public string SourceItemProperty => "SourceItemID";
        public string TrackingFileName => "object-map.csv";
    }
}
