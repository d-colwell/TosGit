using System;
using System.Collections.Generic;
using System.Linq;

namespace Tricentis.TCAPIObjects.Objects
{
    public static class TestCaseHelpers
    {
        public static IEnumerable<T> GetItemsRecursive<T>(this TestCase self, Func<TestCaseItem,bool> selector, Func<TestCaseItem,T> adapter)
            where T:TestCaseItem
        {
            var folders = self.Items.Where(i => i is TestStepFolder);
            var items = self.Items.Where(selector);
            foreach (var testCaseItem in folders)
            {
                var folder = (TestStepFolder) testCaseItem;
                items = items.Concat(GetItemsRecursive(folder, selector));
            }
            return items.Select(adapter);
        }

        private static IEnumerable<TestCaseItem> GetItemsRecursive(TestStepFolder folder, Func<TestCaseItem, bool> selector)
        {
            var items = folder.Items.Where(selector);
            foreach (var testCaseItem in folder.Items.Where(i => i is TestStepFolder))
            {
                var subFolder = (TestStepFolder) testCaseItem;
                items = items.Concat(GetItemsRecursive(subFolder, selector));
            }
            return items;
        }
    }
}
