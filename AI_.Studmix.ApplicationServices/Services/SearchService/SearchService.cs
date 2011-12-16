using System.Collections.Generic;
using System.Linq;
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
            List<PropertyState> states = ConvertToPropertyStates(request.States);
            var response = new GetBoundedStatesResponse();

            var properties = UnitOfWork.GetRepository<Property>().Get();
            var foundedPackages = SearchPackages(request.States);
            foreach (var property in properties)
            {
                IEnumerable<PropertyState> filtredPropertyStates;
                var constrPropState = GetLimitingPropertyState(states, property);
                if (constrPropState == null)
                {
                    filtredPropertyStates = property.States;
                }
                else
                {
                    filtredPropertyStates = foundedPackages
                        .Where(p => p.PropertyStates.Contains(constrPropState))
                        .Aggregate(new List<PropertyState>().AsEnumerable(),
                                   (acc, elem) =>
                                   acc.Concat(elem.PropertyStates.Where(st => st.Property == property)));
                }

                AddToResponce(response, filtredPropertyStates);
            }

            return response;
        }

        public FindPackagesByPropertyStatesResponse FindPackagesByPropertyStates(
            FindPackagesByPropertyStatesRequest request)

        {
            IEnumerable<ContentPackage> contentPackages = SearchPackages(request.PropertyStates);
            var dictionary = contentPackages.ToDictionary(p => p.ID, p => p.Caption);
            return new FindPackagesByPropertyStatesResponse {Packages = dictionary};
        }

        #endregion

        private PropertyState GetLimitingPropertyState(IEnumerable<PropertyState> states, Property property)
        {
            var ordered = states.OrderByDescending(st => st.Property.Order);
            return ordered.FirstOrDefault(state => state.Property.Order < property.Order);
        }

        private List<PropertyState> ConvertToPropertyStates(IDictionary<int, string> states)
        {
            var response = new List<PropertyState>();
            var properties = UnitOfWork.GetRepository<Property>().Get();
            foreach (var state in states)
            {
                var property = properties.Where(p => p.ID == state.Key).Single();
                var propertyState = property.States.SingleOrDefault(s => s.Value == state.Value);
                if(propertyState!=null)
                response.Add(propertyState);
            }
            return response;
        }


        private void AddToResponce(GetBoundedStatesResponse response,
                                   IEnumerable<PropertyState> propertyStates)
        {
            var propertyStateInfos = propertyStates.Select(
                ps => new PropertyStateInfo(ps.Property.ID, ps.Value));
            response.States = response.States.Concat(propertyStateInfos).ToList();
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