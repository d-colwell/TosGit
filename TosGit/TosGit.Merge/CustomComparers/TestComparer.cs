using System;
using System.Collections.Generic;
using System.Linq;
using TosGit.MergeUI.CustomComparers;
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

            if (unmodified.Description != modified.Description)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.Description, Unmodified = unmodified.Description, Property = "Description" });
            if (unmodified.Name != modified.Name)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.Name, Unmodified = unmodified.Name, Property = "Name" });
            if (unmodified.IsBusinessTestCase != modified.IsBusinessTestCase)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.IsBusinessTestCase, Unmodified = unmodified.IsBusinessTestCase, Property = "Is Business Test Case" });
            if (unmodified.IsOsvItem != modified.IsOsvItem)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.IsOsvItem, Unmodified = unmodified.IsOsvItem, Property = "IS OSV Item" });
            if (unmodified.IsTemplate != modified.IsTemplate)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.IsTemplate, Unmodified = unmodified.IsTemplate, Property = "Is Template" });
            if (unmodified.Pausable != modified.Pausable)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.Pausable, Unmodified = unmodified.Pausable, Property = "Pausable" });
            if (unmodified.TestCaseWorkState != modified.TestCaseWorkState)
                compResult.Differences.Add(new Merge.Difference { Modified = modified.TestCaseWorkState, Unmodified = unmodified.TestCaseWorkState, Property = "Work State" });


            foreach (var item in modified.GetPropertyNames().Where(x=> unmodified.GetPropertyNames().Contains(x))) //Matched properties
            {
                if(unmodified.GetPropertyValue(item) != modified.GetPropertyValue(item))
                {
                    compResult.Differences.Add(new Merge.Difference
                    {
                        Modified = modified.GetPropertyValue(item),
                        Property = "Properties/" + item,
                        Unmodified = unmodified.GetPropertyValue(item)
                    });
                }
            }

            if (compResult.Differences.Count > 0)
                compResult.IsDifferent = true;
            else
                compResult.IsDifferent = false;
            return compResult;
        }
    }
}
