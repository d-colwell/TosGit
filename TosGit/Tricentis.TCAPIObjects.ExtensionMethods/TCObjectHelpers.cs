using System;

namespace Tricentis.TCAPIObjects.Objects
{
    public static class Helpers
    {
        public static TCFolder GetFirstAncestor(this TCObject self, Func<TCFolder, bool> selector)
        {
            if (self.OwningObject?.ParentFolder == null)
                return null;
            TCFolder parentFolder = self.OwningObject.ParentFolder as TCFolder;
            do
            {
                if (selector(parentFolder))
                {
                    return parentFolder;
                }
                parentFolder = parentFolder?.ParentFolder as TCFolder;
            } while (parentFolder != null);
            return null;
        }

        public static TCProject GetProject(this TCObject self)
        {
            var project = self as TCProject;
            if (project != null)
                return project;
            if (self.OwningObject?.ParentFolder == null)
                return null;
            TCFolder parentFolder = self.OwningObject.ParentFolder as TCFolder;
            do
            {
                if (parentFolder?.Project != null)
                {
                    return parentFolder.Project as TCProject;
                }
                parentFolder = parentFolder?.ParentFolder as TCFolder;
            } while (parentFolder != null);
            return null;
        }
    }
}
