using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CofoundryExample.SSOAndContent
{
    [Route("auth/[action]")]
    public class AuthController : Controller
    {
        #region constructor

        private readonly MemberSignInService _memberLoginService;
        private readonly ContentRepository _contentRepository;

        public AuthController(
            MemberSignInService memberLoginService,
            ContentRepository contentRepository
            )
        {
            _memberLoginService = memberLoginService;
            _contentRepository = contentRepository;
        }

        #endregion

        #region actions

        [HttpGet]
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
        public async Task<IActionResult> SignOut()
        {
            await _memberLoginService.SignOutAsync();
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