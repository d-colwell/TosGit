using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricentis.TCAPIObjects.Objects
{
    public static class TCFolderHelpers
    {
        /// <summary>
        /// Searches the subtrees recursively and finds all the descendent items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="selector"></param>
        /// <param name="adapter"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetDescendents<T>(this TCFolder self, Func<OwnedItem,bool> selector, Func<OwnedItem, T> adapter)
            where T:OwnedItem
        {
            var items = self.Items.Where(selector);

            foreach(var item in self.Items.Where(i => i is TCFolder))
            {
                items = items.Concat(GetDescendents(item as TCFolder, selector, adapter));    
            }
            return items.Select(adapter);
        }
    }
}
