using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.ObjectTracker
{
    class EmbeddedFileObjectTracker : IObjectTracker
    {
        private readonly string filePath;
        private readonly TCComponentFolder folder;
        private IDictionary<string, string> sourceObjects;
        private IDictionary<string, string> newObjects;
        public EmbeddedFileObjectTracker(TCComponentFolder componentFolder)
        {
            this.folder = componentFolder;
            this.sourceObjects = new Dictionary<string, string>();
            this.newObjects = new Dictionary<string, string>();

            this.filePath = Path.Combine(Path.GetTempPath(), Config.Instance.TrackingFileName);
            if (!componentFolder.AttachedFiles.Any(x => x.Name == Config.Instance.TrackingFileName))
            {
                using (var fStream = File.CreateText(filePath))
                {

                }
                componentFolder.AttachFile(filePath, "Embedded");
            }
            else
            {
                var attachedFile = componentFolder.AttachedFiles.First(x => x.Name == Config.Instance.TrackingFileName);
                string content = System.Text.Encoding.UTF8.GetString(attachedFile.EmbeddedContent.Data);
                var records = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var record in records)
                {
                    string[] splitRecord = record.Split(',');
                    Add(splitRecord[0], splitRecord[1], true);
                }
            }
        }
        public void Add(string sourceObject, string newObject, bool overrideExistingLinks = false)
        {
            if (sourceObjects.ContainsKey(sourceObject))
            {
                if (sourceObjects[sourceObject] == newObject)
                    return; //avoid creating duplicates
                else if (overrideExistingLinks)
                {
                    sourceObjects.Remove(sourceObject);
                    if (newObjects.ContainsKey(newObject))
                        newObjects.Remove(newObject);
                }
                else
                    throw new InvalidOperationException(string.Format("Source object {0} is already linked to object {1}", sourceObject, newObject));
            }
            sourceObjects.Add(new KeyValuePair<string, string>(sourceObject, newObject));
            newObjects.Add(newObject, sourceObject);
        }

        public string GetNewObject(string sourceObject)
        {
            return sourceObjects[sourceObject];
        }

        public string GetSourceObject(string newObject)
        {
            return newObjects[newObject];
        }

        public bool HasSourceObject(string newObject)
        {
            return sourceObjects.ContainsKey(newObject);
        }

        public void Commit()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            using (var fStream = File.CreateText(filePath))
            {
                foreach (var record in sourceObjects)
                {
                    fStream.Write(string.Format("{0},{1}{2}", record.Key, record.Value, Environment.NewLine));
                }
            }
            foreach (var attachedFile in folder.AttachedFiles.Where(x => x.Name == Config.Instance.TrackingFileName))
            {
                var continueOnWarning = MsgBoxResult_OkCancel.Ok;
                var deleteConfirm = MsgBoxResult_YesNo.Yes;
                attachedFile.Delete(continueOnWarning, deleteConfirm);
            }
            folder.AttachFile(filePath, "Embedded");
        }
    }
}
