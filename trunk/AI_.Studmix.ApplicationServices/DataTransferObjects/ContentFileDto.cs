using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class ContentFileDto
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsPreview { get; set; }
        public bool IsImage { get; set; }

        public ContentFileDto(int id, string filename, bool isPreview, bool isImage)
        {
            ID = id;
            Name = filename;
            IsPreview = isPreview;
            IsImage = isImage;
        }
    }
}