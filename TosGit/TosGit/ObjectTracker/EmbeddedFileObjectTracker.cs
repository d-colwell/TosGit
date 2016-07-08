using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.ObjectTracker
{
    class EmbeddedFileObjectTracker : IObjectTracker
    {
        private readonly string filePath;
        private readonly TCComponentFolder folder;
        private IList<KeyValuePair<string, string>> objectMap;

        public EmbeddedFileObjectTracker(TCComponentFolder componentFolder)
        {
            this.folder = componentFolder;
            objectMap = new List<KeyValuePair<string, string>>();
            this.filePath = Path.Combine(Path.GetTempPath(), Config.Instance.TrackingFileName);
            if (!componentFolder.AttachedFiles.Any(x => x.Name == Config.Instance.TrackingFileName))
            {
                using (var fStream = File.CreateText(filePath))
                {

                }
                componentFolder.AttachFile(filePath,"Embedded");
            }
            else
            {
                var attachedFile = componentFolder.AttachedFiles.First(x => x.Name == Config.Instance.TrackingFileName);
                string content = System.Text.Encoding.UTF8.GetString(attachedFile.EmbeddedContent.Data);
                var records = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var record in records)
                {
                    string[] splitRecord = record.Split(',');
                    objectMap.Add(new KeyValuePair<string, string>(splitRecord[0], splitRecord[1]));
                }
            }
        }
        public void Add(string sourceObject, string newObject)
        {
            objectMap.Add(new KeyValuePair<string, string>(sourceObject, newObject));
        }

        public string GetNewObject(string sourceObject)
        {
            return objectMap.First(x => x.Key == sourceObject).Value;
        }

        public string GetSourceObject(string newObject)
        {
            return objectMap.First(x => x.Value == newObject).Key;
        }

        public void Commit()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            using (var fStream = File.CreateText(filePath))
            {
                foreach (var record in objectMap)
                {
                    fStream.Write(string.Format("{0},{1}{2}", record.Key, record.Value, Environment.NewLine));
                }
            }
            foreach(var attachedFile in folder.AttachedFiles.Where(x => x.Name == Config.Instance.TrackingFileName))
            {
                var continueOnWarning = MsgBoxResult_OkCancel.Ok;
                var deleteConfirm = MsgBoxResult_YesNo.Yes;
                attachedFile.Delete(continueOnWarning, deleteConfirm);
            }
            folder.AttachFile(filePath, "Embedded");
        }
    }
}
