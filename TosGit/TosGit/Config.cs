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

        public string RootBranchName => "Trunk";

        public Merge.MergeConfig MergeConfig => new Merge.MergeConfig
        {
            MergeTypes = new Merge.MergeType[]
            {
                new Merge.MergeType {FriendlyName = "Test Case", TypeName = "TestCase", IncludedProperties =
                    {
                        new Merge.MergeProperty {Name = "Name", PropertyPath="Name" },
                        new Merge.MergeProperty {Name = "Synchronization Policy", PropertyPath="SynchronizationPolicy" },
                        new Merge.MergeProperty {Name = "Owning Group Name", PropertyPath="OwningGroupName" },
                        new Merge.MergeProperty {Name = "Viewing Group Name", PropertyPath="Name" },
                        new Merge.MergeProperty {Name = "Work State", PropertyPath="TestCaseWorkState" }
                    }
                }
            }
        };

        public string ProjectNameProperty => "ProjectName";
        public string GitServerProperty => "GitServer";
    }
}
