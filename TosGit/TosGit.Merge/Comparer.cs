using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Merge
{
    public class Comparer
    {
        private readonly MergeConfig config;

        public Comparer(MergeConfig config)
        {
            this.config = config;
        }

        public ComparisonResult Compare(TCObject unmodified, TCObject modified)
        {
            var type = unmodified.GetType();
            var mergeType = config.MergeTypes.FirstOrDefault(mt => mt.TypeName == type.Name);

            var comparResult = new ComparisonResult("Test Case", type);
            var properties = type.GetProperties()
                .Where(p => p.PropertyType.IsPrimitive ||
                            p.PropertyType == typeof(string) ||
                            p.PropertyType == typeof(DateTime) ||
                            p.PropertyType == typeof(decimal) ||
                            p.PropertyType.IsEnum)
                .Where(p => mergeType.IncludedProperties.Any(ip => ip.PropertyPath == p.Name));

            foreach (var property in properties)
            {
                var unmodValue = property.GetValue(unmodified);
                var modValue = property.GetValue(modified);
                if (!unmodValue.Equals(modValue))
                {
                    comparResult.Differences.Add(new Difference
                    {
                        Modified = modValue,
                        Unmodified = unmodValue,
                        Property = property.Name
                    });
                }
            }
            comparResult.IsDifferent = comparResult.Differences.Any();
            return comparResult;
        }
    }
}
