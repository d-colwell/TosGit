using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks.Project
{
    internal class LinkToRepositoryTask : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCProject);

        public override string Name => "Link To Repository";

        public override bool IsTaskPossible(TCObject obj) => true;

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            TCProject project = objectToExecuteOn as TCProject;
            var propertyDefinitions = project.ObjectPropertiesDefinitions.FirstOrDefault(x => x.Name == "XModule");
            if (propertyDefinitions == null)
                project.CreatePropertyDefinitionXModule().Name = "XModule";

            if (!objectToExecuteOn.GetPropertyNames().Any(p => p == Config.Instance.RepoProperty))
            {
                var prop = project.DefaultPropertiesDefinition.CreateProperty();
                prop.Name = Config.Instance.RepoProperty;
                prop.Value = string.Empty;
            }
            if (!objectToExecuteOn.GetPropertyNames().Any(p => p == Config.Instance.RepoUserProperty))
            {
                var prop = project.DefaultPropertiesDefinition.CreateProperty();
                prop.Name = Config.Instance.RepoUserProperty;
                prop.Value = string.Empty;
            }
            if (!objectToExecuteOn.GetPropertyNames().Any(p => p == Config.Instance.RepoPasswordProperty))
            {
                var prop = project.DefaultPropertiesDefinition.CreateProperty();
                prop.Visible = false;
                prop.Name = Config.Instance.RepoPasswordProperty;
                prop.Value = string.Empty;
            }
            if (!objectToExecuteOn.GetPropertyNames().Any(p => p == Config.Instance.RepoNameProperty))
            {
                var prop = project.DefaultPropertiesDefinition.CreateProperty();
                prop.Name = Config.Instance.RepoNameProperty;
                prop.Value = string.Empty;
            }


            SetRepoProperties(project, taskContext);

            var repoConnector = Container.Instance.GetRepositoryConnector(project.GetPropertyValue(Config.Instance.RepoProperty), project.GetPropertyValue(Config.Instance.RepoUserProperty), project.GetPropertyValue(Config.Instance.RepoPasswordProperty));
            if (!repoConnector.TestConnection())
            {
                taskContext.ShowWarningMessage("Could not connect", "Unable to connect to repository using credentials provided. Please try again.");
                return project;
            }
            var repositories = repoConnector.GetRepositories();

            string currentRepo = project.GetPropertyValue(Config.Instance.RepoNameProperty);
            currentRepo = taskContext.GetStringSelection("Which Repository do you want to connect to?", repositories.Select(x => x.Name).ToList(), currentRepo);
            project.SetAttibuteValue(Config.Instance.RepoNameProperty, currentRepo);

            return objectToExecuteOn;
        }


        private void SetRepoProperties(TCProject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            string repo, userName, password;

            repo = objectToExecuteOn.GetPropertyValue(Config.Instance.RepoProperty);
            userName = objectToExecuteOn.GetPropertyValue(Config.Instance.RepoUserProperty);
            password = objectToExecuteOn.GetPropertyValue(Config.Instance.RepoPasswordProperty);

            repo = taskContext.GetStringValue("Git Repo Location", false, repo);
            userName = taskContext.GetStringValue("Git Username", false, userName);
            password = taskContext.GetStringValue("Git Password", true, password);

            objectToExecuteOn.SetAttibuteValue(Config.Instance.RepoProperty, repo);
            objectToExecuteOn.SetAttibuteValue(Config.Instance.RepoUserProperty, userName);
            objectToExecuteOn.SetAttibuteValue(Config.Instance.RepoPasswordProperty, password);
        }

    }
}
