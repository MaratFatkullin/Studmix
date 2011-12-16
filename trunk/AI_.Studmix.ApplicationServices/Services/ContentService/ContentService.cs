using System.Collections.ObjectModel;
using System.Linq;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.ApplicationServices.Services.Abstractions;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.ContentService.Responses;
using AI_.Studmix.ApplicationServices.Specifications;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Services
{
    public class ContentService : IContentService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected IFileRepository FileRepository { get; set; }

        public ContentService(IUnitOfWork unitOfWork, IFileRepository fileRepository)
        {
            UnitOfWork = unitOfWork;
            FileRepository = fileRepository;
        }

        #region IContentService Members

        public GetPropertiesResponse GetProperties()
        {
            var response = new GetPropertiesResponse();
            var properties = UnitOfWork.GetRepository<Property>().Get();
            response.Properties = properties.OrderBy(pr => pr.Order).ToDictionary(p => p.ID, p => p.Name);
            return response;
        }

        public void Store(StoreRequest request)
        {
            var userRepository = UnitOfWork.GetRepository<User>();
            User owner = userRepository.Get(new GetUserByUserName(request.OwnerUserName)).Single();

            //property states
            var propertyRepository = UnitOfWork.GetRepository<Property>();
            var propertyStates = new Collection<PropertyState>();
            foreach (var state in request.States)
            {
                var property = propertyRepository.GetByID(state.Key);
                var propertyState = property.GetState(state.Value);

                propertyStates.Add(propertyState);
            }

            //files
            var contentFileFactory = new ContentFileFactory();
            var contentFiles = new Collection<ContentFile>();
            foreach (var fileInfo in request.PreviewContentFiles)
            {
                var contentFile = contentFileFactory.CreateContentFile(fileInfo.FileName, true);
                contentFiles.Add(contentFile);
                FileRepository.Store(contentFile, fileInfo.Stream);
            }
            foreach (var fileInfo in request.ContentFiles)
            {
                var contentFile = contentFileFactory.CreateContentFile(fileInfo.FileName, false);
                contentFiles.Add(contentFile);
                FileRepository.Store(contentFile, fileInfo.Stream);
            }

            var factory = new ContentPackageFactory();
            var contentPackage = factory.CreateContentPackage(request.Caption,
                                                              request.Description,
                                                              owner,
                                                              request.Price,
                                                              propertyStates,
                                                              contentFiles);

            var repository = UnitOfWork.GetRepository<ContentPackage>();
            repository.Insert(contentPackage);

            UnitOfWork.Save();
        }

        #endregion
    }
}