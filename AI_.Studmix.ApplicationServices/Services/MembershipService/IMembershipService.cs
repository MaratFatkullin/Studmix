using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.MembershipService
{
    public interface IMembershipService
    {
        CreateUserResponse CreateUser(CreateUserRequest request);
        ValidateUserResponce ValidateUser(ValidateUserRequest request);
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);
    }
}