using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks
{
    internal class CopyToBranchTask : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCObject);

        public override string Name => Resources.RemoveFromBranchTaskName;

        public override bool IsTaskPossible(TCObject obj)
        {
            if (obj is TCProject || obj is TCComponentFolder)
                return false;
            var branchFolder = obj.GetFirstAncestor(fld => fld is TCComponentFolder && fld.GetPropertyNames().Contains(Config.Instance.BranchPropertyName));
            return branchFolder != null;
        }

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            string originalID = null;
            try
            {
                originalID = objectToExecuteOn.GetPropertyValue(Config.Instance.SourceItemProperty);
            }
            catch (Exception)
            {
                originalID = null;
            }
            if (originalID != null && (objectToExecuteOn is Module || objectToExecuteOn is XModule))
            {
                var project = objectToExecuteOn.GetProject();
                var referencedModule = project.Search(string.Format("=>SUBPARTS[(UniqueId==\"{0}\")]", originalID)).FirstOrDefault();
                var testCaseItems = objectToExecuteOn.Search("->AllReferences:TestCaseItem").Cast<TestCaseItem>();
                foreach (TestCaseItem item in testCaseItems)
                {
                    if (item is XTestStep)
                        ((XTestStep)item).AssignModuleToTestStep(referencedModule);
                    else if (item is TestStep)
                        ((TestStep)item).AssignModuleToTestStep(referencedModule);
                }
                /*
                var branchFolder = objectToExecuteOn.GetFirstAncestor(fldr => fldr is TCComponentFolder && fldr.GetPropertyNames().Contains(Config.Instance.BranchPropertyName));
                var tests = branchFolder.GetDescendents(f => f is TestCase, f => (TestCase)f);
                foreach (TestCase test in tests)
                {
                    var stepsWhichReferenceModule = test.GetItemsRecursive(i => i is TestStep, s => (TestStep)s).Where(x => x.Module != null && x.Module.UniqueId == objectToExecuteOn.UniqueId);
                    var xStepsWhichReferenceModule = test.GetItemsRecursive(i => i is XTestStep, s => (XTestStep)s).Where(x => x.Module != null && x.Module.UniqueId == objectToExecuteOn.UniqueId);

                    if (stepsWhichReferenceModule.Any() || xStepsWhichReferenceModule.Any())
                    {
                        var project = objectToExecuteOn.GetProject();
                        var modulesFolder = project.Items.First(x => x is TCFolder && ((TCFolder)x).PossibleContent.Contains("Module")) as TCFolder;
                        var branchesFolder = project.Items.First(x => x is TCComponentFolder && x.Name == Config.Instance.BranchFolderName) as TCComponentFolder;
                        var referencedItem = branchesFolder.GetDescendents(x => x.UniqueId == originalID, x => x).FirstOrDefault();
                        if(referencedItem != null)
                        {

                        }
                    }
                }
                */
            }
            MsgBoxResult_OkCancel continueOnWarning = MsgBoxResult_OkCancel.Ok;
            MsgBoxResult_YesNo deleteSelectedObject = MsgBoxResult_YesNo.Yes;
            objectToExecuteOn.Delete(continueOnWarning, deleteSelectedObject);

            return null;
        }

    }
}
