using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Specifications
{
    public class GetUserByUserName : Specification<User>
    {
        public GetUserByUserName(string username)
        {
            Filter = u => u.UserName == username;
        }
    }
}