using EDUZilla.Services;
using EDUZilla.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly ParentService _parentService;

        public StudentController(StudentService studentService, ParentService parentService)
        {
            _studentService = studentService;
            _parentService = parentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddParent(string id)
        {
            StudentAddParentViewModel student = await _studentService.GetStudentAddParentAsync(id);
            student.Parents = await _parentService.GetParentsListAsync();

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddParent(AddParentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _studentService.AddParentAsync(model.StudentId, model.ParentId);
                if(!result)
                {
                    return View();
                }

                return await AddParent(model.StudentId);
            }
            return View();
        }
    }
}
