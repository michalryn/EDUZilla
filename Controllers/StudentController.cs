using EDUZilla.Models;
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
        private readonly GradeService _gradeService; 

        public StudentController(StudentService studentService, ParentService parentService, GradeService gradeService)
        {
            _studentService = studentService;
            _parentService = parentService;
            _gradeService = gradeService;
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
                if (!result)
                {
                    return View();
                }

                return await AddParent(model.StudentId);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Student, Parent")]
        public async Task<IActionResult> StudentGrades(string? id)
        {
            string studentId = "";

            if (User.IsInRole("Student"))
            {
                string studentName = User.Identity.Name;

                if(studentName == null)
                {
                    return BadRequest();
                }

                studentId = await _studentService.GetStudentIdAsync(studentName);

                if(studentId == null)
                {
                    return NotFound();
                }

                var viewModel = await _gradeService.GetStudentGradesAsync(studentId);

                return View(viewModel);
            }

            if(User.IsInRole("Parent"))
            {
                if(id != null)
                {
                    var viewModel = await _gradeService.GetStudentGradesAsync(id);

                    return View(viewModel);
                }

                string parentName = User.Identity.Name;

                if (parentName == null)
                {
                    return BadRequest();
                }
                /*
                string parentId = await _parentService.GetParentIdAsync(parentName);

                if (parentId == null)
                {
                    return NotFound();
                }*/

                return RedirectToAction("ChildrenList");

            }

            return BadRequest();
        }

        [Authorize(Roles = "Parent")]
        [HttpGet]
        public async Task<IActionResult> ChildrenList()
        {
            var email = User.Identity?.Name;

            if(email == null)
            {
                return BadRequest();
            }

            var children = await _studentService.GetChildrenAsync(email);

            return View(children);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> ViewCourses()
        {
            string studentName = User.Identity.Name;

            if (studentName == null)
            {
                return BadRequest();
            }

            var studentId = await _studentService.GetStudentIdAsync(studentName);

            if (studentId == null)
            {
                return NotFound();
            }

            var courses = await _studentService.GetCourseListViewModel(studentId);

            return View(courses);

        }

    }
}
