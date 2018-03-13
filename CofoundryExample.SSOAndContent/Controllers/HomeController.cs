using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofoundry.Web.Admin;

namespace CofoundryExample.SSOAndContent
{
    public class HomeController : Controller
    {
        private readonly ContentRepository _contentRepository;

        public HomeController(
            ContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        [Route("")]
        [AuthorizeUserArea(MemberUserArea.AreaCode)]
        public async Task<ActionResult> Index()
        {
            var vm = new HomeViewModel();
            vm.Content = await _contentRepository.GetContentByKeyAsync("home");

            return View(vm);
        }
    }
}