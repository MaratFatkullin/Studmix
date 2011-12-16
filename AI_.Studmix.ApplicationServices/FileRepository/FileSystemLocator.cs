using System;
using System.IO;
using System.Linq;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Infrastructure.FileSystem
{
    public class FileSystemLocator : IFileSystemLocator
    {
        private const string DEFAULT_FOLDER_NAME = "-";

        #region IFileSystemLocator Members

        public string GetLocation(ContentFile file)
        {
            var path = GetPath(file);
            var extension = Path.GetExtension(file.Name);
            var fileName = Path.GetFileNameWithoutExtension(file.Name);

            return Path.Combine(path, string.Format("{0}-{1}{2}", fileName, file.GlobalID, extension));
        }

        private string GetPath(ContentFile file)
        {
            string path = string.Empty;
            var propertyStates = file.ContentPackage.PropertyStates;

            if (propertyStates.Count() == 0)
                return path;

            var maxOrder = propertyStates.Max(ps => ps.Property.Order);
            for (int i = 1; i <= maxOrder; i++)
            {
                var state = propertyStates.SingleOrDefault(ps => ps.Property.Order == i);
                var folderName = state == null
                                     ? DEFAULT_FOLDER_NAME
                                     : string.Format("{0}_{1}", state.Property.ID, state.Index);

                path = Path.Combine(path, folderName);
            }
            return path;
        }

        #endregion
    }
}