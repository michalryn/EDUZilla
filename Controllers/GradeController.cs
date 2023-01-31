using EDUZilla.Services;
using EDUZilla.ViewModels.Class;
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
        public async Task<IActionResult> ClassGradesSummary(int courseId, int classId, DateTime? startDate, DateTime? endDate)
        {
            ClassGradesSummaryVM viewModel = null;

            if(startDate == null || endDate == null)
            {
                DateTime fromDate = new DateTime(2022, 9, 1);
                DateTime toDate = DateTime.Today.AddDays(1);

                viewModel = await _gradeService.GetClassGradesSummary(classId, courseId, fromDate, toDate);
                return View(viewModel);
            }

            viewModel = await _gradeService.GetClassGradesSummary(classId, courseId, (DateTime)startDate, (DateTime)endDate);

            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> ClassGradesSummary(int courseId, int classId, DateTime startDate, DateTime endDate)
        {
            ClassGradesSummaryVM viewModel = null;

            if (startDate == null || endDate == null)
            {
                DateTime fromDate = new DateTime(2022, 9, 1);
                DateTime toDate = DateTime.Today.AddDays(1);

                viewModel = await _gradeService.GetClassGradesSummary(classId, courseId, fromDate, toDate);
                return View(viewModel);
            }

            viewModel = await _gradeService.GetClassGradesSummary(classId, courseId, (DateTime)startDate, (DateTime)endDate);

            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult AddGrade(string studentId, int courseId, int classId)
        {

            return View(new { studentId = studentId, courseId = courseId, classId = classId });
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

        public async Task<IActionResult> GradeDetails(int id, int? courseId, int? classId)
        {
            GradeViewModel viewModel = await _gradeService.GetGradeDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(new { Grade = viewModel, CourseId = courseId, ClassId = classId });
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> DeleteGrade(int id, int courseId, int classId)
        {
            var result = await _gradeService.DeleteGradeAsync(id);

            if(!result)
            {
                return BadRequest();
            }

            return RedirectToAction("ClassesGrades", new { courseId = courseId, classId = classId });
        }

        public async Task<IActionResult> GetStudentGradesSummary(string id)
        {
            DateTime dateTime = DateTime.Now;

            var summary = await _gradeService.GetStudentGradesSummary(id, dateTime.Subtract(new TimeSpan(365, 1, 1, 1)), dateTime);

            return View();
        }
    }
}
