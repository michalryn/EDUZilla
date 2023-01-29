using EDUZilla.Services;
using EDUZilla.ViewModels.Grade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class GradeController : Controller
    {
        private readonly GradeService _gradeService;

        public GradeController(GradeService gradeService)
        {
            _gradeService = gradeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ClassesGrades(int courseId, int classId)
        {
            var viewModel = await _gradeService.GetClassGradeViewModelAsync(classId, courseId);

            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> AddGrade(string studentId, int courseId, int classId)
        {

            return View(new { studentId = studentId, courseId = courseId, classId = classId});
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> AddGrade(AddGradeForm form)
        {
            form.CreatedDate = DateTime.Now;

            var result = await _gradeService.AddGradeAsync(form);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("ClassesGrades", new { courseId = form.CourseId, classId = form.ClassId });
        }
    }
}
