using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
