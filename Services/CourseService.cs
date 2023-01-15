using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Course;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class CourseService
    {
        #region Properties

        private readonly CourseRepository _courseRepository;

        #endregion

        #region Constructors

        public CourseService(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        #endregion

        #region Methods

        public async Task<bool> AddCourseAsync(string name)
        {
            try
            {
                Course course = new Course()
                {
                    Name = name
                };
                await _courseRepository.AddAndSaveChangesAsync(course);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<List<CourseListViewModel>> GetCoursesListAsync()
        {
            var result = await _courseRepository.GetCourses().ToListAsync();
            List<CourseListViewModel> courseList = new List<CourseListViewModel>();
            
            if(!result.Any())
            {
                return courseList;
            }

            foreach (var course in result)
            {
                courseList.Add(new CourseListViewModel()
                {
                    Id = course.Id,
                    Name = course.Name
                });
            }

            return courseList;
        }

        #endregion
    }
}
