using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TosGit.ObjectTracker
{
    public interface IObjectTracker
    {
        void Add(string sourceObject, string newObject, bool overrideExistingLinks = false);
        string GetNewObject(string sourceObject);
        string GetSourceObject(string newObject);
        bool HasSourceObject(string newObject);
        void Commit();
    }


    public class MyPageObject
    {
        public int NumberOfTabs { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
