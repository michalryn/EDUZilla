using EDUZilla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    [Authorize(Roles = "Teacher, Admin")]
    public class TeacherController : Controller
    {
        #region Properties

        private readonly TeacherService _teacherService;

        #endregion

        #region Constructors
        public TeacherController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        #endregion

        #region Methods
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity?.Name;
            
            if(userName == null)
            {
                return NotFound();
            }

            var coursesList = await _teacherService.GetAssignedCoursesAsync(userName);

            return View(coursesList);
        }

        #endregion


    }
}
