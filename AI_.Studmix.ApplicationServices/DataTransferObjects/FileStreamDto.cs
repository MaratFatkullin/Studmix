using System.IO;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class FileStreamDto
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
        public bool IsImage { get; set; }

        public FileStreamDto(string filename, Stream stream, bool isImage)
        {
            FileName = filename;
            Stream = stream;
            IsImage = isImage;
        }
    }
}