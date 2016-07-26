namespace TosGit
{
    public class Config
    {
        private static Config _instance;

        public static Config Instance => _instance ?? (_instance = new Config());

        public string RepoProperty => "Repository";
        public string RepoUserProperty => "RepoUser";
        public string RepoPasswordProperty => "RepoPassword";
        public string BranchFolderName => "Branches";
        public string BranchPropertyName => "OriginBranch";
        public string RepoNameProperty => "RepoName";
        public string SourceItemProperty => "SourceItemID";
        public string TrackingFileName => "object-map.csv";
        public string ProjectNameProperty => "ProjectName";
        public string GitServerProperty => "GitServer";
    }
}
