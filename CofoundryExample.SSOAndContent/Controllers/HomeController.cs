using Cofoundry.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CofoundryExample.SSOAndContent
{
    [AuthorizeUserArea(MemberUserArea.Code)]
    public class HomeController : Controller
    {
        private readonly SimpleContentRepository _simpleContentRepository;

        public HomeController(
            SimpleContentRepository simpleContentRepository
            )
        {
            _simpleContentRepository = simpleContentRepository;
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var vm = new HomeViewModel();
            vm.Content = await _simpleContentRepository.GetContentByKeyAsync("home");

            return View(vm);
        }
    }
}