using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricentis.TCAPIObjects.Objects
{
    public static class Helpers
    {
        public static TCFolder GetFirstAncestor(this TCObject self, Func<TCFolder, bool> selector)
        {
            if (self.OwningObject == null || self.OwningObject.ParentFolder == null)
                return null;
            TCFolder parentFolder = self.OwningObject.ParentFolder as TCFolder;
            do
            {
                if (selector(parentFolder))
                {
                    return parentFolder;
                }
                parentFolder = parentFolder.ParentFolder as TCFolder;
            } while (parentFolder != null);
            return null;
        }

        public static TCProject GetProject(this TCObject self)
        {
            if (self is TCProject)
                return self as TCProject;
            if (self.OwningObject == null || self.OwningObject.ParentFolder == null)
                return null;
            TCFolder parentFolder = self.OwningObject.ParentFolder as TCFolder;
            do
            {
                if (parentFolder.Project != null)
                {
                    return parentFolder.Project as TCProject;
                }
                parentFolder = parentFolder.ParentFolder as TCFolder;
            } while (parentFolder != null);
            return null;
        }
    }
}
