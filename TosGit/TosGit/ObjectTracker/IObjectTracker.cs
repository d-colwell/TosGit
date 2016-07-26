namespace TosGit.ObjectTracker
{
    public interface IObjectTracker
    {
        void Add(string sourceObject, string newObject);
        string GetNewObject(string sourceObject);
        string GetSourceObject(string newObject);
        void Commit();
    }
}
