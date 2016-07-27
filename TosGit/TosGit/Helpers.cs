using System.Linq;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit
{
    public static class Helpers
    {
        public static TCProject GetProject(this TCObject src)
        {
            var project = src as TCProject;
            if (project != null)
                return project;
            else
                return src.Search("=>SUPERPART:TCProject").First() as TCProject;
        }

        /// <summary>
        /// Finds the parent of this object which is a branch component folder.
        /// </summary>
        /// <param name="src"></param>
        /// <returns>Branch Folder, or Null if none found</returns>
        public static TCComponentFolder GetParentBranch(this TCObject src)
        {
            var branchParents = src.Search($"=>SUPERPART:TCComponentFolder[OriginBranch != \"\"]");
            if (!branchParents.Any())
                return null;
            else
                return branchParents.First() as TCComponentFolder;
        } 

        /// <summary>
        /// Finds the child item by its unique ID, or returns null if it doesnt exist.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="uniqueID"></param>
        /// <returns></returns>
        public static TCObject FindChildByID(this TCObject parent, string uniqueID)
        {
            return parent.Search($"=>SUBPARTS[(UniqueId==\"{uniqueID}\")]").FirstOrDefault();
        }
    }
}
