using System.Collections.Generic;
using System.Linq;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.DataTransferObjects.Mapper;
using AI_.Studmix.ApplicationServices.Services.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Services.SearchService.Responses;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Services.SearchService
{
    public class SearchService : ISearchService
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public SearchService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #region ISearchService Members

        public GetBoundedStatesResponse GetBoundedStates(GetBoundedStatesRequest request)
        {
            var targetProperty = UnitOfWork.GetRepository<Property>().GetByID(request.PropertyID);
            var propertyStates = ConvertToPropertyStates(request.States);

            var filtredStates = propertyStates
                .Where(s => s.Property.Order < targetProperty.Order)
                .ToDictionary(s => s.Property.ID, s => s.Value);
            var foundedPackages = SearchPackages(filtredStates);

            var boundedStates = targetProperty.States
                .Where(st => foundedPackages.Any(p => p.PropertyStates.Contains(st)))
                .Select(st => st.Value).ToList();

            return new GetBoundedStatesResponse(boundedStates);
        }

        public FindPackagesByPropertyStatesResponse FindPackagesByPropertyStates(
            FindPackagesByPropertyStatesRequest request)

        {
            IEnumerable<ContentPackage> contentPackages = SearchPackages(request.PropertyStates);
            var packageDtos = DtoMapper.MapSequence<ContentPackageDto>(contentPackages);
            return new FindPackagesByPropertyStatesResponse {Packages = packageDtos};
        }

        #endregion

        private IEnumerable<PropertyState> ConvertToPropertyStates(IDictionary<int, string> states)
        {
            var response = new List<PropertyState>();
            var properties = UnitOfWork.GetRepository<Property>().Get();
            foreach (var state in states)
            {
                var property = properties.Where(p => p.ID == state.Key).Single();
                var propertyState = property.States.SingleOrDefault(s => s.Value == state.Value);
                if (propertyState != null)
                    response.Add(propertyState);
            }
            return response;
        }


        private IEnumerable<ContentPackage> SearchPackages(IDictionary<int, string> propertyStateInfos)
        {
            IEnumerable<ContentPackage> contentPackages = UnitOfWork.GetRepository<ContentPackage>().Get();

            foreach (var propertyStateInfo in propertyStateInfos)
            {
                if (string.IsNullOrEmpty(propertyStateInfo.Value))
                    continue;

                contentPackages = contentPackages
                    .Where(p => p.PropertyStates.Any(ps => ps.Property.ID == propertyStateInfo.Key &&
                                                           ps.Value == propertyStateInfo.Value))
                    .ToList();
            }
            return contentPackages;
        }
    }
}