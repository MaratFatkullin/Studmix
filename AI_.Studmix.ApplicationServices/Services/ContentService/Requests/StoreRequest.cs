using System.Collections.Generic;
using System.IO;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Requests
{
    public class StoreRequest
    {
        /// <summary>
        ///   Словарь пары ID свойства - состояние.
        /// </summary>
        public IEnumerable<PropertyStateDto> States { get; set; }

        public IEnumerable<File> ContentFiles { get; set; }

        public IEnumerable<File> PreviewContentFiles { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string OwnerUserName { get; set; }


        public StoreRequest()
        {
            States = new List<PropertyStateDto>();
            ContentFiles = new List<File>();
            PreviewContentFiles = new List<File>();
        }

        #region Nested type: File

        public class File
        {
            public string FileName { get; set; }
            public Stream Stream { get; set; }

            public File(string key, Stream value)
            {
                FileName = key;
                Stream = value;
            }
        }

        #endregion
    }
}