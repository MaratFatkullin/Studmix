using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Responses;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.Services.Abstractions
{
    public interface IMembershipService
    {
        CreateUserResponse CreateUser(CreateUserRequest request);
        ValidateUserResponce ValidateUser(ValidateUserRequest request);
    }
}