using System.Collections.Generic;
using System.IO;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.ContentService.Requests;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.FileRepository
{
    public interface IFileRepository
    {
        Stream GetFileStream(ContentFile contentFile);
        void Store(ContentFile file, Stream stream);
    }
}