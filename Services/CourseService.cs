using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Course;
using EDUZilla.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class CourseService
    {
        #region Properties

        private readonly CourseRepository _courseRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly ClassRepository _classRepository;

        #endregion

        #region Constructors

        public CourseService(CourseRepository courseRepository, TeacherRepository teacherRepository, ClassRepository classRepository)
        {
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _classRepository = classRepository;
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

            var notFilteredTeachers = _teacherRepository.GetAll().Include(t => t.Courses).AsEnumerable();
            var filteredTeachers = notFilteredTeachers.Where(t => t.Courses == null || t.Courses.All(c => c.Id != course.Id)).ToList();

            var notFilteredClasses = _classRepository.GetAll().Include(c => c.Courses).AsEnumerable();
            var filteredClasses = notFilteredClasses.Where(c => c.Courses == null || c.Courses.All(c => c.Id != course.Id)).ToList();

            IList<TeacherListViewModel> teachers = new List<TeacherListViewModel>();
            List<SelectListItem> availableTeachers = new List<SelectListItem>();

            IList<ClassListViewModel> classes = new List<ClassListViewModel>();
            IList<SelectListItem> availableClasses = new List<SelectListItem>();

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

            if (course.Classes != null)
            {
                foreach (var group in course.Classes)
                {
                    classes.Add(new ClassListViewModel()
                    {
                        Id = group.Id,
                        Name = group.Name
                    });
                }
            }

            foreach (var group in filteredClasses)
            {
                availableClasses.Add(new SelectListItem()
                {
                    Value = "" + group.Id,
                    Text = group.Name
                });
            }


            foreach (var teacher in filteredTeachers)
            {
                availableTeachers.Add(new SelectListItem()
                {
                    Value = teacher.Id,
                    Text = teacher.FirstName + " " + teacher.LastName + " " + teacher.Email
                });
            }

            EditCourseViewModel viewModel = new EditCourseViewModel()
            {
                Id = course.Id,
                Name = course.Name,
                Teachers = teachers,
                AvailableTeachers = availableTeachers,
                Classes = classes,
                AvailableClasses = availableClasses
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

        public async Task<bool> AddTeacherAsync(int id, string teacherId)
        {
            try
            {
                Course course = await _courseRepository.GetCourseById(id).Include("Teachers").SingleAsync();
                Teacher teacher = await _teacherRepository.GetTeacherById(teacherId).SingleAsync();
                if (course == null)
                    return false;

                course.Teachers?.Add(teacher);

                await _courseRepository.UpdateAndSaveChangesAsync(course);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteTeacherAsync(int id, string teacherId)
        {
            try
            {
                Course course = await _courseRepository.GetCourseById(id).Include("Teachers").SingleAsync();
                Teacher teacher = await _teacherRepository.GetTeacherById(teacherId).SingleAsync();

                if (course == null)
                    return false;

                if (teacher == null)
                    return false;

                course.Teachers?.Remove(teacher);

                await _courseRepository.UpdateAndSaveChangesAsync(course);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddClassAsync(int id, int classId)
        {
            try
            {
                Course course = await _courseRepository.GetCourseById(id).Include("Classes").SingleAsync();
                Class group = await _classRepository.GetClassById(classId).Include("Courses").SingleAsync();
                
                if (course == null)
                    return false;

                if (group == null)
                    return false;

                course.Classes?.Add(group);

                await _courseRepository.UpdateAndSaveChangesAsync(course);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteClassAsync(int id, int classId)
        {
            try
            {
                Course course = await _courseRepository.GetCourseById(id).Include("Classes").SingleAsync();
                Class group = await _classRepository.GetClassById(classId).Include("Courses").SingleAsync();
                if (course == null)
                    return false;

                if (group == null)
                    return false;

                course.Classes?.Remove(group);

                await _courseRepository.UpdateAndSaveChangesAsync(course);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
