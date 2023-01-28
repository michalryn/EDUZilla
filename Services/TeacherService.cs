using EDUZilla.Data.Repositories;
using EDUZilla.ViewModels.Course;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class TeacherService
    {
        #region Properties

        private readonly TeacherRepository _teacherRepository;

        #endregion

        #region Constructors
        public TeacherService(TeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        #endregion

        #region Methods

        public async Task<List<CourseListViewModel>> GetAssignedCoursesAsync(string name)
        {
            var result = _teacherRepository.GetTeacherByName(name).Include("Courses");

            if(!result.Any())
            {
                return new List<CourseListViewModel>();
            }

            var teacher = await result.SingleAsync();

            if (teacher == null)
                return new List<CourseListViewModel>();

            List<CourseListViewModel> list = new List<CourseListViewModel>();
            if (teacher.Courses == null)
                return list;

            foreach(var course in teacher.Courses)
            {
                list.Add(new CourseListViewModel()
                {
                    Id = course.Id,
                    Name = course.Name
                });
            }

            return list;
        }

        #endregion
    }
}
