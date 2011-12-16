using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.Abstractions
{
    public interface IMembershipService
    {
        CreateUserResponse CreateUser(CreateUserRequest request);
        ValidateUserResponce ValidateUser(ValidateUserRequest request);
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);
    }
}