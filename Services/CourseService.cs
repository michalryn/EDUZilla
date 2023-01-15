using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Course;
using EDUZilla.ViewModels.Teacher;
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

        public async Task<EditCourseViewModel> GetEditCourseViewModel(int id)
        {
            var course = await _courseRepository.GetCourseById(id).Include(c => c.Teachers).Include(c => c.Classes).SingleAsync();

            if (course == null)
            {
                return null;
            }

            IList<TeacherListViewModel> teachers = new List<TeacherListViewModel>();

            if (course.Teachers != null)
            {
                foreach (var teacher in course.Teachers)
                {
                    teachers.Add(new TeacherListViewModel()
                    {
                        Id = teacher.Id,
                        Email = teacher.Email,
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName
                    });
                }
            }

            EditCourseViewModel viewModel = new EditCourseViewModel()
            {
                Id = course.Id,
                Name = course.Name,
                Teachers = teachers
            };

            return viewModel;
        }

        public async Task<bool> DeleteCourseByIdAsync(int id)
        {
            try
            {
                await _courseRepository.RemoveByIdAndSaveChangesAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

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

            if (!result.Any())
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
