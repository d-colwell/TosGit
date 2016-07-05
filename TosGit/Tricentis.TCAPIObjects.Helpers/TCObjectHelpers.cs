using System;
using System.Collections.Generic;
using System.Text;
using Tricentis.TCAPIObjects.Objects;

namespace Tricentis.TCAPIObjects.Objects
{
    public static class TCObjectHelpers
    {
        public static TCFolder GetFirstAncestor(this TCObject self, Func<TCFolder, bool> selector)
        {
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

        public static IEnumerable<T> FindAllDescendents<T>(this TCObject self, Func<TCObject, bool> selector, Func<TCObject, T> adapter)
            where T : TCObject
        {

        }
    }
}
