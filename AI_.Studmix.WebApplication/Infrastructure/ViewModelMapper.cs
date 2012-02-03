using System.Collections;
using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Responses;
using AI_.Studmix.WebApplication.ViewModels.Account;
using AI_.Studmix.WebApplication.ViewModels.Content;
using AutoMapper;
using System.Linq;

namespace AI_.Studmix.WebApplication.Infrastructure
{
    internal static class ViewModelMapper
    {
        static ViewModelMapper()
        {
            Mapper.CreateMap<KeyValuePair<int, string>, PropertyStateDto>()
                .ConstructUsing(m => new PropertyStateDto(m.Key, m.Value));

            Mapper.CreateMap<ViewAccountViewModel, UpdateUserRequest>()
                .ConstructUsing(delegate(ViewAccountViewModel m)
                                {
                                    var request = new UpdateUserRequest {User = m.User};
                                    request.User.States = MapSequence<PropertyStateDto>(m.States);
                                    return request;
                                });

            Mapper.CreateMap<GetUserResponse, Dictionary<int,string>>()
            .ConstructUsing(
                    delegate(GetUserResponse r)
                    {
                        var states = new Dictionary<int, string>();
                        foreach (var propertyDto in r.Properties)
                        {
                            var stateDto = r.User.States.SingleOrDefault(ps => ps.Key == propertyDto.ID);
                            states[propertyDto.ID] = stateDto != null ? stateDto.Value : null;
                        }
                        return states;
                    });

            Mapper.CreateMap<GetUserResponse, ViewAccountViewModel>()
                .ForMember(f=>f.States,option=>option.MapFrom(r=>r));

            Mapper.CreateMap<GetUserResponse,SearchViewModel>()
                .ForMember(f=>f.States,option=>option.MapFrom(r=>r));
        }

        public static TDestionation Map<TDestionation>(object source)
        {
            return Mapper.Map<TDestionation>(source);
        }

        public static IEnumerable<TDestionation> MapSequence<TDestionation>(IEnumerable<object> source)
        {
            return Mapper.Map<IEnumerable<TDestionation>>(source);
        }

        public static IEnumerable<TDestionation> MapSequence<TDestionation>(IEnumerable source)
        {
            return Mapper.Map<IEnumerable<TDestionation>>(source);
        }

    }
}