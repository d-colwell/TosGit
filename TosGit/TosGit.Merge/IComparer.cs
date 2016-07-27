namespace TosGit.Merge
{
    public interface IComparer
    {
        ComparisonResult Compare(object unmodified, object modified);
    }
}
