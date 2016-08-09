using System;
using System.Collections.Generic;
using System.Linq;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks.Project
{
    internal class LinkToRepositoryTask : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCProject);

        public override string Name => Resources.LinkTaskName;

        public override bool IsTaskPossible(TCObject obj) => true;

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            TCProject project = objectToExecuteOn as TCProject;
            if (project != null)
            {
                var propertyDefinitions = project.ObjectPropertiesDefinitions.FirstOrDefault(x => x.Name == "XModule");
                if (propertyDefinitions == null)
                    project.CreatePropertyDefinitionXModule().Name = "XModule";
            }

            if (objectToExecuteOn.GetPropertyNames().All(p => p != Config.Instance.RepoProperty))
            {
                if (project != null)
                {
                    var prop = project.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.RepoProperty;
                    prop.Value = string.Empty;
                }
            }
            if (objectToExecuteOn.GetPropertyNames().All(p => p != Config.Instance.RepoUserProperty))
            {
                if (project != null)
                {
                    var prop = project.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.RepoUserProperty;
                    prop.Value = string.Empty;
                }
            }
            if (objectToExecuteOn.GetPropertyNames().All(p => p != Config.Instance.RepoPasswordProperty))
            {
                if (project != null)
                {
                    var prop = project.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.RepoPasswordProperty;
                    prop.Value = string.Empty;
                }
            }
            if (objectToExecuteOn.GetPropertyNames().All(p => p != Config.Instance.RepoNameProperty))
            {
                if (project != null)
                {
                    var prop = project.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.RepoNameProperty;
                    prop.Value = string.Empty;
                }
            }
            if (objectToExecuteOn.GetPropertyNames().All(p => p != Config.Instance.ProjectNameProperty))
            {
                if (project != null)
                {
                    var prop = project.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.ProjectNameProperty;
                    prop.Value = string.Empty;
                }
            }
            if (objectToExecuteOn.GetPropertyNames().All(p => p != Config.Instance.GitServerProperty))
            {
                if (project != null)
                {
                    var prop = project.DefaultPropertiesDefinition.CreateProperty();
                    prop.Name = Config.Instance.GitServerProperty;
                    prop.Value = string.Empty;
                }
            }
            SetRepoProperties(project, taskContext);

            if (project == null) return objectToExecuteOn;
            {
                var repoConnector = Container.Instance.GetRepositoryConnector(project.GetPropertyValue(Config.Instance.GitServerProperty),
                    project.GetPropertyValue(Config.Instance.RepoProperty), 
                    project.GetPropertyValue(Config.Instance.RepoUserProperty), 
                    project.GetPropertyValue(Config.Instance.RepoPasswordProperty));

                if (!repoConnector.TestConnection())
                {
                    taskContext.ShowWarningMessage("Could not connect", "Unable to connect to repository using credentials provided. Please try again.");
                    return project;
                }

                string projectName = project.GetPropertyValue(Config.Instance.ProjectNameProperty);

                var repositories = repoConnector.GetRepositories(projectName);

                string currentRepo = project.GetPropertyValue(Config.Instance.RepoNameProperty);
                currentRepo = taskContext.GetStringSelection("Which Repository do you want to connect to?", repositories.Select(x => x.Name).ToList(), currentRepo);
                project.SetAttibuteValue(Config.Instance.RepoNameProperty, currentRepo);
            }
            return objectToExecuteOn;
        }


        private void SetRepoProperties(TCProject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            var repo = objectToExecuteOn.GetPropertyValue(Config.Instance.RepoProperty);
            var userName = objectToExecuteOn.GetPropertyValue(Config.Instance.RepoUserProperty);
            var password = objectToExecuteOn.GetPropertyValue(Config.Instance.RepoPasswordProperty);
            var project = objectToExecuteOn.GetPropertyValue(Config.Instance.ProjectNameProperty);
            var gitServer = objectToExecuteOn.GetPropertyValue(Config.Instance.GitServerProperty);

            gitServer = taskContext.GetStringSelection("Git Server", new List<string> {"BitBucket","GitHub"}, gitServer);
            repo = taskContext.GetStringValue("Git Repo Location", false, repo);
            userName = taskContext.GetStringValue("Git Username", false, userName);
            password = taskContext.GetStringValue("Git Password", true, password);
            project = taskContext.GetStringValue("Git Project Name", false, project);

            objectToExecuteOn.SetAttibuteValue(Config.Instance.GitServerProperty, gitServer);
            objectToExecuteOn.SetAttibuteValue(Config.Instance.RepoProperty, repo);
            objectToExecuteOn.SetAttibuteValue(Config.Instance.RepoUserProperty, userName);
            objectToExecuteOn.SetAttibuteValue(Config.Instance.RepoPasswordProperty, password);
            objectToExecuteOn.SetAttibuteValue(Config.Instance.ProjectNameProperty, project);
        }
    }
}
