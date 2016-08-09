using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TosGit.Merge;
using TosGit.Merge.CustomComparers;
using TosGit.ObjectTracker;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks
{
    class JunkCompareTestsTask : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TCComponentFolder);

        public override string Name => "Merge";
        public override bool IsTaskPossible(TCObject obj)
        {
            var folder = obj as TCComponentFolder;
            if (folder != null && !folder.GetPropertyNames().Contains(Config.Instance.BranchPropertyName))
                return false;
            return true;
        }
        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            TCComponentFolder folder = objectToExecuteOn as TCComponentFolder;
            if (folder != null)
            {
                var otherBranches = folder.Search(
                    $"=>SUPERPART=>SUBPARTS:TCComponentFolder[{Config.Instance.BranchPropertyName} != \"\"]").Cast<TCComponentFolder>().Except(new TCComponentFolder[] { folder });
                var tcComponentFolders = otherBranches as TCComponentFolder[] ?? otherBranches.ToArray();
                var allBranches = tcComponentFolders.Select(b => b.Name).Concat(new string[] { Config.Instance.RootBranchName });
                var selectedBranch = taskContext.GetStringSelection("Select branch to merge to", allBranches.ToList());
                OwnedItem target;

                if (selectedBranch == Config.Instance.RootBranchName)
                    target = folder.GetProject();
                else if (!string.IsNullOrEmpty(selectedBranch))
                    target = tcComponentFolders.First(x => x.Name == selectedBranch);
                else //they cancelled the input
                    return objectToExecuteOn;

                IObjectTracker objectTracker = Container.Instance.GetObjectTracker(folder);

                //Get the branched test cases
                var branchItems = folder.Search("=>SUBPARTS:OwnedItem").Cast<OwnedItem>();
            
                foreach (var branchItem in branchItems)
                {
                    if(!objectTracker.HasSourceObject(branchItem.UniqueId))
                    {
                        continue;
                    }
                    var sourceItemID = objectTracker.GetSourceObject(branchItem.UniqueId);
                    if (sourceItemID == null) throw new ArgumentNullException(nameof(sourceItemID));
                    var sourceTest = target.FindChildByID(sourceItemID);
                }
            }

            return objectToExecuteOn;
        }

        public override TCObject Execute(List<TCObject> objs, TCAddOnTaskContext context)
        {
            return Execute(objs.First(), context);
        }
    }
}
