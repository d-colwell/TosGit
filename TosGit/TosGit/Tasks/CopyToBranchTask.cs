using System;
using System.Collections.Generic;
using System.Linq;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks
{
    internal class RemoveFromBranch : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCObject);

        public override string Name => Resources.CopyToBranchTaskName;

        public override bool IsTaskPossible(TCObject obj)
        {
            return !(obj is TCProject) && !(obj is TCFolder);
        }

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            return Execute(new List<TCObject>() { objectToExecuteOn }, taskContext);
        }

        public override TCObject Execute(List<TCObject> objs, TCAddOnTaskContext context)
        {
            var project = FindProject(objs.First());
            var propertyDefinitions = project.DefaultPropertiesDefinition;
            var branchesFolder = project.Items.FirstOrDefault(i => i is TCComponentFolder && i.Name == Config.Instance.BranchFolderName) as TCComponentFolder;
            if(branchesFolder == null)
            {
                context.ShowErrorMessage("No branch folder found", "Please ensure that there are branches in your project");
                return objs.First();
            }
            var branches = branchesFolder.Items.Where(i => i is TCComponentFolder && i.GetPropertyNames().Any(pn => pn == Config.Instance.BranchPropertyName));

            var ownedItems = branches as OwnedItem[] ?? branches.ToArray();
            string toBranch = context.GetStringSelection("Select a branch", ownedItems.Select(x => x.Name).ToList());

            TCComponentFolder branchDestinationFolder = ownedItems.First(x => x.Name == toBranch) as TCComponentFolder;
            var objectTracker = Container.Instance.GetObjectTracker(branchDestinationFolder);

            foreach (var item in objs)
            {
                if (branchDestinationFolder != null)
                {
                    var existingItems = branchDestinationFolder.Search(string.Format("=>SUBPARTS[(SourceItemID==\"{0}\")]", item.UniqueId));
                    if (existingItems.Any())
                    {
                        context.ShowWarningMessage("Duplicate Item", string.Format("Item {0} already exists in branch. This item was skipped", item.DisplayedName));
                        continue;
                    }
                }
                var targetFolder = CopyFolderStructure(branchDestinationFolder, item);
                TCObject pastedItem = null;
                item.Copy();
                targetFolder.Paste();
                pastedItem = targetFolder.Items.First(i => i.Name == item.DisplayedName);
                objectTracker.Add(item.UniqueId, pastedItem.UniqueId);

                var subItems = item.Search("=>SUBPARTS:TCObject").ToArray();
                var copiedSubItems = pastedItem.Search("=>SUBPARTS:TCObject").ToArray();
                for (int i = 0; i < subItems.Length; i++)
                {
                    objectTracker.Add(subItems[i].UniqueId, copiedSubItems[i].UniqueId);
                }

                if (pastedItem.GetPropertyNames().All(p => p != Config.Instance.SourceItemProperty))
                {
                    pastedItem.DefaultPropertiesDefinition.CreateProperty().Name = Config.Instance.SourceItemProperty;
                }
                pastedItem.SetAttibuteValue(Config.Instance.SourceItemProperty, item.UniqueId);
            }

            var oldModuleIDs = new Dictionary<string, TCObject>();
            GetOldModuleIds(branchDestinationFolder, oldModuleIDs);
            RepointTestSteps(branchDestinationFolder, oldModuleIDs);
            objectTracker.Commit();
            return objs.FirstOrDefault();
        }



        private TCProject FindProject(TCObject tcObject)
        {
            TCFolder folder = tcObject.OwningObject.ParentFolder as TCFolder;
            if (folder != null)
            {
                while (folder != null && folder.ParentFolder != null)
                    folder = folder.ParentFolder as TCFolder;
            }

            return folder.Project as TCProject;
        }

        /// <summary>
        /// Copies the folder structure and returns the lowest leaf folder
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private TCFolder CopyFolderStructure(TCComponentFolder branch, TCObject source)
        {
            Stack<TCFolder> structure = new Stack<TCFolder>();
            TCFolder currentFolder = source.OwningObject.ParentFolder as TCFolder;
            do
            {
                structure.Push(currentFolder);
                currentFolder = currentFolder?.ParentFolder as TCFolder;
            } while (currentFolder != null && !(currentFolder is TCComponentFolder));

            TCFolder newRoot = null;
            while (structure.Count > 0)
            {
                TCFolder fldr = structure.Pop();
                newRoot = CreateFolderIfNotExists(newRoot ?? branch, fldr);
            }
            return newRoot;
        }

        private TCFolder CreateFolderIfNotExists(TCFolder root, TCFolder folder)
        {
            TCFolder resultFolder = root.Items.FirstOrDefault(i => i is TCFolder && i.Name == folder.Name) as TCFolder;
            if (resultFolder != null)
                return resultFolder;
            var componentFolder = root as TCComponentFolder;
            if (componentFolder != null)
            {
                var cf = componentFolder;
                if (folder.PossibleContent.Contains("TestCase"))
                    resultFolder = cf.CreateTestCasesFolder();
                else if (folder.PossibleContent.Contains("Module"))
                    resultFolder = cf.CreateModulesFolder();
                else
                    throw new NotImplementedException("This is only implemented for tests and modules at the moment");
            }
            else
                resultFolder = root.CreateFolder();
            resultFolder.Name = folder.Name;
            return resultFolder;
        }

        private void GetOldModuleIds(TCFolder folder, Dictionary<string, TCObject> reference)
        {
            var modulesFolders = folder.Items.Where(x => x is TCFolder && ((TCFolder)x).PossibleContent.Contains("Module"));
            foreach (var childFolder in modulesFolders)
            {
                GetOldModuleIds((TCFolder)childFolder, reference);
            }
            var modules = folder.Items.Where(x => x is Module || x is XModule);
            foreach (var module in modules)
            {
                reference.Add(module.GetPropertyValue(Config.Instance.SourceItemProperty), module);
            }
        }

        private void RepointTestSteps(TCFolder folder, Dictionary<string, TCObject> moduleReferences)
        {
            var testFolders = folder.Items.Where(x => x is TCFolder && ((TCFolder)x).PossibleContent.Contains("TestCase"));
            var xTestSteps = folder.Search("=>SUBPARTS:TestCase=>SUBPARTS:XTestStep").Cast<XTestStep>();
            var testSteps = folder.Search("=>SUBPARTS:TestCase=>SUBPARTS:TestStep").Cast<TestStep>();

            foreach (var step in xTestSteps)
            {
                if (moduleReferences.ContainsKey(step.Module.UniqueId))
                    step.AssignModuleToTestStep(moduleReferences[step.Module.UniqueId]);
            }
            foreach (var step in testSteps)
            {
                if (moduleReferences.ContainsKey(step.Module.UniqueId))
                    step.AssignModuleToTestStep(moduleReferences[step.Module.UniqueId]);
            }
        }

    }
}
