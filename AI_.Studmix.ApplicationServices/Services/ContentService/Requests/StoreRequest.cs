using System;
using System.Collections.Generic;
using System.IO;

namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.ContentService.Requests
{
    public class StoreRequest
    {
        /// <summary>
        ///   Словарь пары ID свойства - состояние.
        /// </summary>
        public IEnumerable<StateInfo> States { get; set; }

        public IEnumerable<ContentFileInfo> ContentFiles { get; set; }

        public IEnumerable<ContentFileInfo> PreviewContentFiles { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string OwnerUserName { get; set; }


        public StoreRequest()
        {
            States = new List<StateInfo>();
            ContentFiles = new List<ContentFileInfo>();
            PreviewContentFiles = new List<ContentFileInfo>();
        }
    }


    public class StateInfo
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public StateInfo(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }


    public class ContentFileInfo
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }

        public ContentFileInfo(string key, Stream value)
        {
            FileName = key;
            Stream = value;
        }
    }
}