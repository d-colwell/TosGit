using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace TosGit.Merge
{
    public abstract class ResolverBase<T>
    {
        private readonly T modified;
        private readonly T unmodified;

        public ResolverBase(T unmodified, T modified)
        {
            this.unmodified = unmodified;
            this.modified = modified;
        }
        public virtual void GenericResolver(string property, object value)
        {
            var type = typeof(T);
            var pInfo = type.GetProperty(property);
            pInfo.SetValue(unmodified, value);
        }
    }
}
