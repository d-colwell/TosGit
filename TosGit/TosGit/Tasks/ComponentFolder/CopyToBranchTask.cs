using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks
{
    internal class MergeTask : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCObject);

        public override string Name => "Merge";

        public override bool IsTaskPossible(TCObject obj)
        {
            if (obj is TCProject || obj is TCFolder)
                return false;

            return true;
        }

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            return Execute(new List<TCObject>() { objectToExecuteOn }, taskContext);
        }

        public override TCObject Execute(List<TCObject> objs, TCAddOnTaskContext context)
        {
            var project = FindProject(objs.First());
            var propertyDefinitions = project.DefaultPropertiesDefinition;
            var branchesFolder = project.Items.First(i => i is TCComponentFolder && i.Name == Config.Instance.BranchFolderName) as TCComponentFolder;
            var branches = branchesFolder.Items.Where(i => i is TCComponentFolder && i.GetPropertyNames().Any(pn => pn == Config.Instance.BranchPropertyName));

            string toBranch = context.GetStringSelection("Select a branch", branches.Select(x => x.Name).ToList());

            TCComponentFolder branchDestinationFolder = branches.First(x => x.Name == toBranch) as TCComponentFolder;

            foreach (var item in objs)
            {
                var existingItems = branchDestinationFolder.Search(string.Format("=>SUBPARTS[(SourceItemID==\"{0}\")]", item.UniqueId));
                if(existingItems.Any())
                {
                    context.ShowWarningMessage("Duplicate Item", string.Format("Item {0} already exists in branch. This item was skipped", item.DisplayedName));
                    continue;
                }
                var targetFolder = CopyFolderStructure(branchDestinationFolder, item);
                TCObject pastedItem = null;
                item.Copy();
                targetFolder.Paste();
                pastedItem = targetFolder.Items.First(i => i.Name == item.DisplayedName);
                if (!pastedItem.GetPropertyNames().Any(p => p == Config.Instance.SourceItemProperty))
                {
                    pastedItem.DefaultPropertiesDefinition.CreateProperty().Name = Config.Instance.SourceItemProperty;
                }
                pastedItem.SetAttibuteValue(Config.Instance.SourceItemProperty, item.UniqueId);
            }

            var oldModuleIDs = new Dictionary<string, TCObject>();
            GetOldModuleIds(branchDestinationFolder, oldModuleIDs);
            RepointTestSteps(branchDestinationFolder, oldModuleIDs);
            return objs.FirstOrDefault();
        }



        private TCProject FindProject(TCObject tcObject)
        {
            TCFolder folder = tcObject.OwningObject.ParentFolder as TCFolder;
            while (folder.ParentFolder != null)
                folder = folder.ParentFolder as TCFolder;
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
                currentFolder = currentFolder.ParentFolder as TCFolder;
            }
            while (currentFolder != null && !(currentFolder is TCComponentFolder));

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
            if (root is TCComponentFolder)
            {
                var cf = root as TCComponentFolder;
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

        private void GetOldModuleIds(TCFolder folder, Dictionary<string,TCObject> reference)
        {
            var modulesFolders = folder.Items.Where(x => x is TCFolder && ((TCFolder)x).PossibleContent.Contains("Module"));
            foreach (var childFolder in modulesFolders)
            {
                GetOldModuleIds((TCFolder)childFolder,reference);
            }
            var modules = folder.Items.Where(x => x is Module || x is XModule);
            foreach (var module in modules)
            {
                reference.Add(module.GetPropertyValue(Config.Instance.SourceItemProperty), module);
            }
        }

        private void RepointTestSteps(TCFolder folder, Dictionary<string,TCObject> moduleReferences)
        {
            var testFolders = folder.Items.Where(x => x is TCFolder && ((TCFolder)x).PossibleContent.Contains("TestCase"));
            foreach (var childFolder in testFolders)
            {
                RepointTestSteps((TCFolder)childFolder, moduleReferences);
            }
            var tests = folder.Items.Where(x => x is TestCase);
            foreach (TestCase test in tests)
            {
                var xTestSteps = test.Items.Where(x => x is XTestStep).Cast<XTestStep>();
                foreach (var step in xTestSteps)
                {
                    if (moduleReferences.ContainsKey(step.Module.UniqueId))
                        step.AssignModuleToTestStep(moduleReferences[step.Module.UniqueId]);
                }
                var testSteps = test.Items.Where(x => x is TestStep).Cast<TestStep>();
                foreach (var step in testSteps)
                {
                    if (moduleReferences.ContainsKey(step.Module.UniqueId))
                        step.AssignModuleToTestStep(moduleReferences[step.Module.UniqueId]);
                }
            }

        }

    }
}
