using Cafe.Domain.ViewModel.Profile;
using Cafe.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public IActionResult Save(ProfileViewModel model)
        {
            ModelState.Remove("IdP");
            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                var response = _profileService.Save(model);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return View("Detail");
                }
            }
            return View("Detail");
        }

        public IActionResult Detail()
        {
            var userName = User.Identity.Name;
            var response = _profileService.GetProfile(userName);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View();
        }
    }
}
