using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TosGit.Connectors;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks.Project
{
    internal class SyncWithRepository : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCProject);

        public override string Name => Resources.SyncTaskName;

        public override bool IsTaskPossible(TCObject obj)
        {
            return ((TCProject)obj).GetPropertyNames().Any(x => x == Config.Instance.RepoProperty);
        }

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            TCProject project = objectToExecuteOn as TCProject;
            string repoName = project.GetPropertyValue(Config.Instance.RepoNameProperty);
            string projectName = project.GetPropertyValue(Config.Instance.ProjectNameProperty);

            var repoConnector = Container.Instance.GetRepositoryConnector(
                                    project.GetPropertyValue(Config.Instance.GitServerProperty),
                                    project.GetPropertyValue(Config.Instance.RepoProperty), 
                                    project.GetPropertyValue(Config.Instance.RepoUserProperty), 
                                    project.GetPropertyValue(Config.Instance.RepoPasswordProperty));

            var branches = repoConnector.GetRemoteBranches(projectName,repoName).Where(x => x.Name != "master");

            var rootFolder = project.Items.FirstOrDefault(x => x.GetType() == typeof(TCComponentFolder) && x.Name == Config.Instance.BranchFolderName) as TCComponentFolder;
            if (rootFolder == null)
            {
                rootFolder = project.CreateComponentFolder() as TCComponentFolder;
                rootFolder.Name = Config.Instance.BranchFolderName;
            }
            foreach (var branch in branches)
            {
                CreateComponentFolder(rootFolder, branch);
            }
            //var branchTree = repoConnector.GetRemoteBranchTree();
            //CreateBranchFolderStructure(project, branchTree);
            return objectToExecuteOn;
        }

        public void CreateComponentFolder(TCComponentFolder parentFolder, IBranch branch)
        {
            TCFolder childFolder = null;
            if (!parentFolder.Items.Any(i => i.Name == branch.Name && i.GetType() == typeof(TCComponentFolder)))
            {
                childFolder = parentFolder.CreateFolder();
                childFolder.Name = branch.Name;
                if(!childFolder.GetPropertyNames().Any(pn => pn == Config.Instance.BranchPropertyName))
                {
                    var prop = childFolder.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.BranchPropertyName;
                }
                childFolder.SetAttibuteValue(Config.Instance.BranchPropertyName, branch.Name);
            }
            else
                childFolder = parentFolder.Items.First(i => i.Name == branch.Name && i.GetType() == typeof(TCComponentFolder)) as TCComponentFolder;
        }

    }
}
