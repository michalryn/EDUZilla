using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDUZilla.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var course = await _courseService.GetCourseViewModelAsync(id);

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _courseService.GetEditCourseViewModel(id);

            //error handle
            if(course == null)
            {
                return View();
            }

            course.AvailableTeachers?.Insert(0, new SelectListItem() { Value = "", Text = "Wybierz nauczyciela", Selected = true, Disabled = true });
            course.AvailableClasses?.Insert(0, new SelectListItem() { Value = "", Text = "Wybierz klasę", Selected = true, Disabled = true });

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTeacher(int id, string? teacherId)
        {
            if(teacherId == null)
            {
                return RedirectToAction("ViewCourses");
            }

            var result = await _courseService.AddTeacherAsync(id, teacherId);

            //handle error
            if(!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(int id, string teacherId)
        {
            var result = await _courseService.DeleteTeacherAsync(id, teacherId);
            
            //handle error
            if(!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClass(int id, int? classId)
        {
            if(classId == null)
            {
                return RedirectToAction("ViewCourses");
            }

            var result = await _courseService.AddClassAsync(id, (int)classId);

            if (!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteClass(int id, int? classId)
        {
            if (classId == null)
            {
                return RedirectToAction("ViewCourses");
            }

            var result = await _courseService.DeleteClassAsync(id, (int)classId);

            if (!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseByIdAsync(id);

            if(!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("ViewCourses");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCourse(string name)
        {
            if(ModelState.IsValid)
            {
                var result = await _courseService.AddCourseAsync(name);

                //error handle
                if(!result)
                {
                    return RedirectToAction("ViewCourses");
                }

                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("ViewCourses");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewCourses()
        {
            List<CourseListViewModel> list = await _courseService.GetCoursesListAsync();

            return View(list);
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> ViewClasses(int id)
        {
            var classes = await _courseService.GetClassListViewModelAsync(id);

            return View(new { classes = classes, courseId = id });
        }
    }
}
