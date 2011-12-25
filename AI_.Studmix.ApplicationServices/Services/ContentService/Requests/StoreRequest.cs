using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Requests
{
    public class StoreRequest
    {
        /// <summary>
        ///   Словарь пары ID свойства - состояние.
        /// </summary>
        public IEnumerable<PropertyStateDto> States { get; set; }

        public IEnumerable<FileStreamDto> ContentFiles { get; set; }

        public IEnumerable<FileStreamDto> PreviewContentFiles { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string OwnerUserName { get; set; }


        public StoreRequest()
        {
            States = new List<PropertyStateDto>();
            ContentFiles = new List<FileStreamDto>();
            PreviewContentFiles = new List<FileStreamDto>();
        }
    }
}