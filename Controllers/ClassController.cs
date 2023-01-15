using EDUZilla.Services;
using EDUZilla.ViewModels.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            if (form.AddStudent != null || form.DeleteStudent != null)
            {
                //error handle
                var result = await _classService.EditClassAsync(form);

                return RedirectToAction("ViewClasses");
            }

            return await EditClass(form.ClassId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditTutor(int id)
        {
            EditTutorViewModel? model = await _classService.GetEditTutorViewModelAsync(id);

            //error handle
            if (model == null)
            {
                return RedirectToAction("ViewClasses");
            }

            model.AvailableTeachers.Insert(0, new SelectListItem() { Value = "", Text = "Wybierz nauczyciela", Selected = true });

            return View("EditTutor", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditTutor(int id, string? teacher)
        {
            if (teacher == null)
            {
                return RedirectToAction("ViewClasses");
            }

            var result = await _classService.EditTutorAsync(id, teacher);

            //handle error
            if (!result)
            {
                return RedirectToAction("ViewClasses");
            }

            return await EditTutor(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            var result = await _classService.DeleteTutorAsync(id);

            //handle error
            if (!result)
            {
                return RedirectToAction("ViewClasses");
            }

            return await EditTutor(id);
        }
    }
}
