using System.Web.Mvc;
using System.Web.Security;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.WebApplication.Infrastructure.Authentication;
using AI_.Studmix.WebApplication.ViewModels.Account;

namespace AI_.Studmix.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        protected IMembershipService MembershipService { get; set; }
        protected IAuthenticationProvider AuthenticationProvider { get; set; }

        public AccountController(IMembershipService membershipService,
                                 IAuthenticationProvider authenticationProvider)
        {
            MembershipService = membershipService;
            AuthenticationProvider = authenticationProvider;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var request = new ValidateUserRequest(viewModel.UserName, viewModel.Password);
                var responce = MembershipService.ValidateUser(request);
                if (responce.IsValid)
                {
                    AuthenticationProvider.LogOn(viewModel.UserName, viewModel.RememberMe);
                    if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            AuthenticationProvider.LogOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ViewResult Register()
        {
            var viewModel = new RegisterViewModel();
            viewModel.MinRequiredPasswordLength = MembershipService.Configuration.MinRequiredPasswordLength;
            return View(viewModel);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user

                var request = new CreateUserRequest(viewModel.UserName,
                                                    viewModel.Password,
                                                    viewModel.Email,
                                                    null,
                                                    null,
                                                    null,
                                                    true);
                var response = MembershipService.CreateUser(request);

                if (response.Status == MembershipCreateStatus.Success)
                {
                    AuthenticationProvider.LogOn(response.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(response.Status));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var request = new ChangePasswordRequest(User.Identity.Name,
                                                        viewModel.OldPassword,
                                                        viewModel.NewPassword);
                var response = MembershipService.ChangePassword(request);

                if (response.ChangePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("",
                                             "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        [HttpGet]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        public ViewResult ViewAccount()
        {
            var request = new GetUserRequest(User.Identity.Name);
            var response = MembershipService.GetUser(request);
            var viewModel = new ViewAccountViewModel {User = response.User};
            return View(viewModel);
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return
                        "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return
                        "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}