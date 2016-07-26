namespace TosGit.MergeUI.Resolvers
{
    public abstract class ResolverBase<T>
    {
        private readonly T _unmodified;

        protected ResolverBase(T unmodified, T modified)
        {
            this._unmodified = unmodified;
            this.Modified = modified;
        }

        public T Modified { get; }

        public virtual void GenericResolver(string property, object value)
        {
            var type = typeof(T);
            var pInfo = type.GetProperty(property);
            pInfo.SetValue(_unmodified, value);
        }
    }
}
