using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _courseService.GetEditCourseViewModel(id);

            if(course == null)
            {
                return View();
            }

            return View(course);
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
    }
}
