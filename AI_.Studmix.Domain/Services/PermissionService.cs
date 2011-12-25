using System.Linq;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Services
{
    public class PermissionService : IPermissionService
    {
        public bool UserHasPermissions(User user, ContentPackage package)
        {
            if (package.Owner == user)
                return true;

            if (user.IsInRole("admin"))
                return true;

            return UserHasOrder(user, package);
        }

        protected bool UserHasOrder(User user, ContentPackage package)
        {
            return user.Orders.Any(o => o.ContentPackage == package);
        }
    }
}