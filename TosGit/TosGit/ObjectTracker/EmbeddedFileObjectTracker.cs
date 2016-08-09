using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.ObjectTracker
{
    class EmbeddedFileObjectTracker : IObjectTracker
    {
        private readonly string _filePath;
        private readonly TCComponentFolder _folder;
        private readonly IDictionary<string, string> _sourceObjects;
        private readonly IDictionary<string, string> _newObjects;
        public EmbeddedFileObjectTracker(TCComponentFolder componentFolder)
        {
            this._folder = componentFolder;
            this._sourceObjects = new Dictionary<string, string>();
            this._newObjects = new Dictionary<string, string>();

            this._filePath = Path.Combine(Path.GetTempPath(), Config.Instance.TrackingFileName);
            if (componentFolder.AttachedFiles.All(x => x.Name != Config.Instance.TrackingFileName))
            {
                using (var fStream = File.CreateText(_filePath))
                {

                }
                componentFolder.AttachFile(_filePath, "Embedded");
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
            if (_sourceObjects.ContainsKey(sourceObject))
            {
                if (_sourceObjects[sourceObject] == newObject)
                    return; //avoid creating duplicates
                else if (overrideExistingLinks)
                {
                    _sourceObjects.Remove(sourceObject);
                    if (_newObjects.ContainsKey(newObject))
                        _newObjects.Remove(newObject);
                }
                else
                    throw new InvalidOperationException(
                        $"Source object {sourceObject} is already linked to object {newObject}");
            }
            _sourceObjects.Add(new KeyValuePair<string, string>(sourceObject, newObject));
            _newObjects.Add(newObject, sourceObject);
        }

        public string GetNewObject(string sourceObject)
        {
            return _sourceObjects[sourceObject];
        }

        public string GetSourceObject(string newObject)
        {
            return _newObjects[newObject];
        }

        public bool HasSourceObject(string newObject)
        {
            return _sourceObjects.ContainsKey(newObject);
        }

        public void Commit()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
            using (var fStream = File.CreateText(_filePath))
            {
                foreach (var record in _sourceObjects)
                {
                    fStream.Write($"{record.Key},{record.Value}{Environment.NewLine}");
                }
            }
            foreach (var attachedFile in _folder.AttachedFiles.Where(x => x.Name == Config.Instance.TrackingFileName))
            {
                var continueOnWarning = MsgBoxResult_OkCancel.Ok;
                var deleteConfirm = MsgBoxResult_YesNo.Yes;
                attachedFile.Delete(continueOnWarning, deleteConfirm);
            }
            _folder.AttachFile(_filePath, "Embedded");
        }
    }
}
