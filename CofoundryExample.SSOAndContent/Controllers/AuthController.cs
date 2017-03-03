using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CofoundryExample.SSOAndContent
{
    [RoutePrefix("auth")]
    [Route("{action=login}")]
    public class AuthController : Controller
    {
        #region constructor

        private readonly IMemberSignInService _memberLoginService;
        private readonly IContentRepository _contentRepository;

        public AuthController(
            IMemberSignInService memberLoginService,
            IContentRepository contentRepository
            )
        {
            _memberLoginService = memberLoginService;
            _contentRepository = contentRepository;
        }

        #endregion

        #region actions

        public async Task<ActionResult> SignIn()
        {
            var vm = await GetSignInViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignMemberInCommand command)
        {
            if (_memberLoginService.IsAuthenticated(command))
            {
                await _memberLoginService.SignMemberInAsync(command);

                return Redirect("/");
            }

            var vm = await GetSignInViewModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            _memberLoginService.SignOut();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region private helpers

        private async Task<SignInViewModel> GetSignInViewModel()
        {
            var vm = new SignInViewModel();
            vm.Content = await _contentRepository.GetContentByKeyAsync("sign-in");

            return vm;
        }

        #endregion
    }
}