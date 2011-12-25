using System.Collections.ObjectModel;
using System.Linq;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.DataTransferObjects.Mapper;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.ContentService.Responses;
using AI_.Studmix.ApplicationServices.Specifications;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Services.ContentService
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
            var properties = UnitOfWork.GetRepository<Property>().Get();
            var propertiesDictionary = properties.OrderBy(pr => pr.Order).ToDictionary(p => p.ID, p => p.Name);
            return new GetPropertiesResponse(propertiesDictionary);
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

            var factory = new ContentPackageFactory();
            var contentPackage = factory.CreateContentPackage(request.Caption,
                                                              request.Description,
                                                              owner,
                                                              request.Price,
                                                              propertyStates);

            //files
            foreach (var fileInfo in request.PreviewContentFiles)
            {
                var contentFile = contentPackage.AddFile(fileInfo.FileName, true);
                FileRepository.Store(contentFile, fileInfo.Stream);
            }
            foreach (var fileInfo in request.ContentFiles)
            {
                var contentFile = contentPackage.AddFile(fileInfo.FileName, false);
                FileRepository.Store(contentFile, fileInfo.Stream);
            }


            var repository = UnitOfWork.GetRepository<ContentPackage>();
            repository.Insert(contentPackage);

            UnitOfWork.Save();
        }

        public GetPackageByIDResponse GetPackageByID(GetPackageByIDRequest request)
        {
            var repository = UnitOfWork.GetRepository<ContentPackage>();
            var contentPackage = repository.GetByID(request.ID);

            if (contentPackage == null)
                return new GetPackageByIDResponse(null);

            var contentPackageDto = DtoMapper.Map(contentPackage);
            return new GetPackageByIDResponse(contentPackageDto);
        }

        #endregion
    }
}