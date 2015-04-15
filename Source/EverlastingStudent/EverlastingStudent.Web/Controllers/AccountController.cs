namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Models;
    using EverlastingStudent.Web.Models;
    using EverlastingStudent.Web.Providers;
    using EverlastingStudent.Web.Results;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;

    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            this.UserManager = userManager;
            this.AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this.userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(this.User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = this.User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            var logins = user.Logins
                .Select(linkedAccount => new UserLoginInfoViewModel
                    {
                        LoginProvider = linkedAccount.LoginProvider,
                        ProviderKey = linkedAccount.ProviderKey
                    })
                .ToList();

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = this.GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.UserManager.ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            return result.Succeeded ? this.Ok() : this.GetErrorResult(result);
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.UserManager.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);

            return result.Succeeded ? this.Ok() : this.GetErrorResult(result);
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var ticket = this.AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return this.BadRequest("External login failure.");
            }

            var externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return this.BadRequest("The external login is already associated with an account.");
            }

            var result = await this.UserManager.AddLoginAsync(this.User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            return result.Succeeded ? this.Ok() : this.GetErrorResult(result);
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await this.UserManager.RemovePasswordAsync(this.User.Identity.GetUserId());
            }
            else
            {
                result = await this.UserManager.RemoveLoginAsync(this.User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            return result.Succeeded ? this.Ok() : this.GetErrorResult(result);
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return this.Redirect(this.Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!this.User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var externalLogin = ExternalLoginData.FromIdentity(this.User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return this.InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var user = await this.UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                var oAuthIdentity = await user.GenerateUserIdentityAsync(this.UserManager,
                   OAuthDefaults.AuthenticationType);
                var cookieIdentity = await user.GenerateUserIdentityAsync(this.UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                this.Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                this.Authentication.SignIn(identity);
            }

            return this.Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var descriptions = this.Authentication.GetExternalAuthenticationTypes();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            var authenticationDescriptions = descriptions
                .Select(description => new ExternalLoginViewModel
                    {
                        Name = description.Caption,
                        Url = this.Url.Route("ExternalLogin", new
                            {
                                provider = description.AuthenticationType,
                                response_type = "token",
                                client_id = Startup.PublicClientId,
                                redirect_uri = new Uri(this.Request.RequestUri, returnUrl).AbsoluteUri,
                                state
                            }),
                        State = state
                    })
                .ToList();

            return authenticationDescriptions;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var coefficientKnowledgeGain = 1f;
            var coefficientEnergyGain = 1f;
            var coefficientExperienceGain = 1f;
            var coefficientMoneyGain = 1f;
            var coefficientEnergyLoss = 1f;
            var coefficientMoneyLoss = 1f;
            var chanceToSolveCoefficient = 1f;

            switch (model.Type)
            {
                case PlayerType.Freelancer:
                    coefficientMoneyGain = 1.1f;
                    break;
                case PlayerType.HyperActive:
                    coefficientEnergyLoss = 1.1f;
                    break;
                case PlayerType.Lucky:
                    chanceToSolveCoefficient = 1.1f;
                    break;
                case PlayerType.Nerd:
                    coefficientKnowledgeGain = 1.1f;
                    break;
            }

            var user = new Student()
            {
                UserName = model.Email,
                Email = model.Email,
                Experience = 10,
                Knowledge = 10,
                Energy = 100,
                Money = 100,
                ChanceToSolveCoefficient = chanceToSolveCoefficient,
                CoefficientEnergyGain = coefficientEnergyGain,
                CoefficientEnergyLoss = coefficientEnergyLoss,
                CoefficientExperienceGain = coefficientExperienceGain,
                CoefficientKnowledgeGain = coefficientKnowledgeGain,
                CoefficientMoneyGain = coefficientMoneyGain,
                CoefficientMoneyLoss = coefficientMoneyLoss,
                LastAction = DateTime.Now,
                PlayerType = model.Type

            };

            var result = await this.UserManager.CreateAsync(user, model.Password);

            return result.Succeeded ? this.Ok() : this.GetErrorResult(result);
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var info = await this.Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return this.InternalServerError();
            }

            var user = new Student() { UserName = model.Email, Email = model.Email };

            var result = await this.UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            result = await this.UserManager.AddLoginAsync(user.Id, info.Login);

            return !result.Succeeded ? this.Ok() : this.GetErrorResult(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                this.userManager.Dispose();
                this.userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return this.Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (result.Succeeded) return null;
            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("", error);
                }
            }

            if (this.ModelState.IsValid)
            {
                // No ModelState errors are available to send, so just return an empty BadRequest.
                return this.BadRequest();
            }

            return this.BadRequest(this.ModelState);
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; private set; }

            public string ProviderKey { get; private set; }

            private string UserName { get; set; }

            public IEnumerable<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, this.ProviderKey, null, this.LoginProvider));

                if (this.UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, this.UserName, null, this.LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
