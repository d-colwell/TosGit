using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.ObjectTracker
{
    internal class EmbeddedFileObjectTracker : IObjectTracker
    {
        private readonly string _filePath;
        private readonly TCComponentFolder _folder;
        private IList<KeyValuePair<string, string>> _objectMap;

        public EmbeddedFileObjectTracker(TCComponentFolder componentFolder)
        {
            this._folder = componentFolder;
            _objectMap = new List<KeyValuePair<string, string>>();
            this._filePath = Path.Combine(Path.GetTempPath(), Config.Instance.TrackingFileName);
            if (componentFolder != null && componentFolder.AttachedFiles.All(x => x.Name != Config.Instance.TrackingFileName))
            {
                using (var fStream = File.CreateText(_filePath))
                {

                }
                componentFolder.AttachFile(_filePath,"Embedded");
            }
            else
            {
                if (componentFolder != null)
                {
                    var attachedFile = componentFolder.AttachedFiles.First(x => x.Name == Config.Instance.TrackingFileName);
                    string content = System.Text.Encoding.UTF8.GetString(attachedFile.EmbeddedContent.Data);
                    var records = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var record in records)
                    {
                        string[] splitRecord = record.Split(',');
                        _objectMap.Add(new KeyValuePair<string, string>(splitRecord[0], splitRecord[1]));
                    }
                }
            }
        }
        public void Add(string sourceObject, string newObject)
        {
            _objectMap.Add(new KeyValuePair<string, string>(sourceObject, newObject));
        }

        public string GetNewObject(string sourceObject)
        {
            return _objectMap.First(x => x.Key == sourceObject).Value;
        }

        public string GetSourceObject(string newObject)
        {
            return _objectMap.First(x => x.Value == newObject).Key;
        }

        public void Commit()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
            using (var fStream = File.CreateText(_filePath))
            {
                foreach (var record in _objectMap)
                {
                    fStream.Write($"{record.Key},{record.Value}{Environment.NewLine}");
                }
            }
            foreach(var attachedFile in _folder.AttachedFiles.Where(x => x.Name == Config.Instance.TrackingFileName))
            {
                var continueOnWarning = MsgBoxResult_OkCancel.Ok;
                var deleteConfirm = MsgBoxResult_YesNo.Yes;
                attachedFile.Delete(continueOnWarning, deleteConfirm);
            }
            _folder.AttachFile(_filePath, "Embedded");
        }
    }
}
