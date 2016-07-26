using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit
{
    public static class Helpers
    {
        public static TCProject GetProject(this TCObject src)
        {
            if (src is TCProject)
                return src as TCProject;
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
            var branchParents = src.Search("=>SUPERPART:TCComponentFolder[OriginBranch != \"\"]");
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
            return parent.Search(string.Format("=>SUBPARTS[(UniqueId==\"{0}\")]", uniqueID)).FirstOrDefault();
        }
    }
}
