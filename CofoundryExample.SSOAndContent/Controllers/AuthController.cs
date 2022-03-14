using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CofoundryExample.SSOAndContent
{
    [Route("auth/[action]")]
    public class AuthController : Controller
    {
        private readonly MemberSignInService _memberSignInService;
        private readonly SimpleContentRepository _simpleContentRepository;

        public AuthController(
            MemberSignInService memberSignInService,
            SimpleContentRepository simpleContentRepository
            )
        {
            _memberSignInService = memberSignInService;
            _simpleContentRepository = simpleContentRepository;
        }

        [HttpGet]
        public async Task<ActionResult> SignIn()
        {
            var vm = await GetSignInViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignMemberInCommand command)
        {
            if (_memberSignInService.IsAuthenticated(command))
            {
                await _memberSignInService.SignMemberInAsync(command);

                return Redirect("/");
            }

            var vm = await GetSignInViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _memberSignInService.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        private async Task<SignInViewModel> GetSignInViewModel()
        {
            var vm = new SignInViewModel();
            vm.Content = await _simpleContentRepository.GetContentByKeyAsync("sign-in");

            return vm;
        }
    }
}