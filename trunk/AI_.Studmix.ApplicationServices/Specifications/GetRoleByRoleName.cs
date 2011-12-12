using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Specifications
{
    public class GetRoleByRoleName : Specification<Role>
    {
        public GetRoleByRoleName(string rolename)
        {
            Filter = p => (p.RoleName == rolename.ToLower());
        }
    }
}