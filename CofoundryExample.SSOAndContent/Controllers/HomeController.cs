using Cofoundry.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CofoundryExample.SSOAndContent
{
    public class HomeController : Controller
    {
        private readonly IContentRepository _contentRepository;

        public HomeController(
            IContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        [Route]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var vm = new HomeViewModel();
            vm.Content = await _contentRepository.GetContentByKeyAsync("home");

            return View(vm);
        }
    }
}