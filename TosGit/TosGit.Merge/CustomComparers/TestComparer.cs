using System.Linq;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Merge.CustomComparers
{
    public class TestComparer : IComparer
    {
        public ComparisonResult Compare(object u, object m)
        {
            var unmodified = u as TestCase;
            var modified = m as TestCase;
            var compResult = new ComparisonResult("Test Case", typeof(TestCase));

            if (modified != null && (unmodified != null && unmodified.Description != modified.Description))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.Description, Unmodified = unmodified.Description, Property = "Description" });
            if (modified != null && (unmodified != null && unmodified.Name != modified.Name))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.Name, Unmodified = unmodified.Name, Property = "Name" });
            if (modified != null && (unmodified != null && unmodified.IsBusinessTestCase != modified.IsBusinessTestCase))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.IsBusinessTestCase, Unmodified = unmodified.IsBusinessTestCase, Property = "Is Business Test Case" });
            if (modified != null && (unmodified != null && unmodified.IsOsvItem != modified.IsOsvItem))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.IsOsvItem, Unmodified = unmodified.IsOsvItem, Property = "IS OSV Item" });
            if (modified != null && (unmodified != null && unmodified.IsTemplate != modified.IsTemplate))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.IsTemplate, Unmodified = unmodified.IsTemplate, Property = "Is Template" });
            if (modified != null && (unmodified != null && unmodified.Pausable != modified.Pausable))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.Pausable, Unmodified = unmodified.Pausable, Property = "Pausable" });
            if (modified != null && (unmodified != null && unmodified.TestCaseWorkState != modified.TestCaseWorkState))
                compResult.Differences.Add(new Merge.Difference { Modified = modified.TestCaseWorkState, Unmodified = unmodified.TestCaseWorkState, Property = "Work State" });


            if (modified != null)
                foreach (var item in modified.GetPropertyNames().Where(x=>
                {
                    return unmodified != null && unmodified.GetPropertyNames().Contains(x);
                })) //Matched properties
                {
                    if(unmodified != null && unmodified.GetPropertyValue(item) != modified.GetPropertyValue(item))
                    {
                        compResult.Differences.Add(new Merge.Difference
                        {
                            Modified = modified.GetPropertyValue(item),
                            Property = "Properties/" + item,
                            Unmodified = unmodified.GetPropertyValue(item)
                        });
                    }
                }

            compResult.IsDifferent = compResult.Differences.Count > 0;
            return compResult;
        }
    }
}
