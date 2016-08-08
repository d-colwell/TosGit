using System;
using System.Linq;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Merge
{
    public class Comparer
    {
        private readonly MergeConfig _config;

        public Comparer(MergeConfig config)
        {
            this._config = config;
        }

        public ComparisonResult Compare(TCObject unmodified, TCObject modified)
        {
            var type = unmodified.GetType();
            var mergeType = _config.MergeTypes.FirstOrDefault(mt => mt.TypeName == type.Name);

            var comparResult = new ComparisonResult("Test Case", type);
            var properties = type.GetProperties()
                .Where(p => p.PropertyType.IsPrimitive ||
                            p.PropertyType == typeof(string) ||
                            p.PropertyType == typeof(DateTime) ||
                            p.PropertyType == typeof(decimal) ||
                            p.PropertyType.IsEnum)
                .Where(p => { return mergeType?.IncludedProperties.Any(ip => ip.PropertyPath == p.Name) ?? false; });

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
