using EDUZilla.Services;
using EDUZilla.ViewModels.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class ClassController : Controller
    {
        private readonly ClassService _classService;

        public ClassController(ClassService classService)
        {
            _classService = classService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewClasses()
        {
            List<ClassListViewModel> list = await _classService.GetClassesListAsync();
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClass(string name)
        {
            if (ModelState.IsValid)
            {
                var exists = await _classService.CheckIfClassExists(name);

                //error
                if (exists)
                {
                    return RedirectToAction("ViewClasses");
                }

                var result = await _classService.AddClassAsync(name);

                //error
                if (!result)
                {
                    return RedirectToAction("ViewClasses");
                }


                return RedirectToAction("ViewClasses");
            }

            return RedirectToAction("ViewClasses");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditClass(int id)
        {

            EditClassViewModel model = await _classService.GetEditClassViewModelAsync(id);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditClass(EditClassForm form)
        {
            if(ModelState.IsValid)
            {
                var result = await _classService.EditClassAsync(form);

                return RedirectToAction("ViewClasses");
            }

            return await EditClass(form.ClassId);
        }
    }
}
