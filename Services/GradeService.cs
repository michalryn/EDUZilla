using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Grade;
using EDUZilla.ViewModels.Student;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class GradeService
    {
        #region Properties

        private readonly ClassRepository _classRepository;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly GradeRepository _gradeRepository;

        #endregion

        #region Constructors

        public GradeService(ClassRepository classRepository, StudentRepository studentRepository, CourseRepository courseRepository, GradeRepository gradeRepository)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _gradeRepository = gradeRepository;
        }

        #endregion

        #region Methods

        public async Task<ClassGradeViewModel> GetClassGradeViewModelAsync(int classId, int courseId)
        {
            var group = _classRepository.GetClassById(classId).Include("Students.Grades");

            ClassGradeViewModel viewModel = new ClassGradeViewModel()
            {
                CourseId = courseId,
                ClassId = classId
            };

            if (group == null)
            {
                return viewModel;
            }

            var students = group.Select(g => g.Students);

            if (!students.Any())
            {
                return viewModel;
            }

            var studentsGrades = await students.SingleAsync();

            List<StudentCourseGradesViewModel> studentViewModel = new List<StudentCourseGradesViewModel>();

            foreach (var student in studentsGrades)
            {
                List<GradeViewModel> gradesViewModel = new List<GradeViewModel>();

                var grades = student.Grades?.Where(g => g.CourseId == courseId);

                if (grades != null)
                {
                    foreach (Grade grade in grades)
                    {
                        gradesViewModel.Add(new GradeViewModel()
                        {
                            Id = grade.Id,
                            Value = grade.Value,
                            Description = grade.Description,
                            CreatedDate = grade.CreatedDate
                        });
                    }
                }

                studentViewModel.Add(new StudentCourseGradesViewModel()
                {
                    StudentId = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    Grades = gradesViewModel
                });
            }

            viewModel.Students = studentViewModel;
            var className = group.Select(g => g.Name).Single();
            viewModel.ClassName = className;

            return viewModel;
        }

        public async Task<bool> AddGradeAsync(AddGradeForm form)
        {
            try
            {
                var student = await _studentRepository.GetStudentById(form.StudentId).SingleAsync();
                var course = await _courseRepository.GetCourseById(form.CourseId).SingleAsync();

                if (student == null || course == null)
                {
                    return false;
                }

                Grade grade = new Grade()
                {
                    Value = form.GradeValue,
                    Description = form.Description,
                    CreatedDate = form.CreatedDate,
                    Student = student,
                    Course = course
                };

                await _gradeRepository.AddAndSaveChangesAsync(grade);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<GradeViewModel> GetGradeDetailsAsync(int id)
        {
            var grade = await _gradeRepository.GetGradeById(id).Include("Course").SingleAsync();

            if (grade == null)
            {
                return null;
            }

            GradeViewModel viewModel = new GradeViewModel()
            {
                Id = id,
                Value = grade.Value,
                Description = grade.Description,
                CreatedDate = grade.CreatedDate,
                CourseName = grade.Course?.Name
            };

            return viewModel;
        }

        public async Task<bool> DeleteGradeAsync(int id)
        {
            try
            {
                await _gradeRepository.RemoveByIdAndSaveChangesAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<StudentGradesSummary> GetStudentGradesSummary(string id, DateTime startDate, DateTime endDate)
        {
            var student = _studentRepository.GetStudentById(id);

            if (!student.Any())
            {
                return null;
            }

            var grades = await student.Include("Grades.Course").Select(s => s.Grades.Where(g => g.CreatedDate >= startDate && g.CreatedDate <= endDate)).SingleAsync();

            StudentGradesSummary summary = new StudentGradesSummary()
            {
                CoursesSummary = new List<CourseGradesSummary>()
            };

            var courses = grades.Select(g => g.Course.Name).Distinct();

            var courseSum = 0d;
            foreach(var course in courses)
            {
                CourseGradesSummary courseSummary = new CourseGradesSummary()
                {
                    CourseName = course,
                    Grades = new List<int>(),
                    GradesAverage = 0d,
                };
                var gradesSum = 0;
                foreach (var grade in grades.Where(g => g.Course.Name == course))
                {
                    gradesSum += grade.Value;
                    courseSummary.Grades.Add(grade.Value);
                }

                courseSummary.GradesAverage = gradesSum / courseSummary.Grades.Count();
                courseSum += Math.Round(courseSummary.GradesAverage);
                summary.CoursesSummary.Add(courseSummary);
            }

            var studentDetails = await student.SingleAsync();

            summary.StudentId = studentDetails.Id;
            summary.StudentName = studentDetails.FirstName + " " + studentDetails.LastName;
            summary.OverallAverege = courseSum / (summary.CoursesSummary.Count() == 0 ? 1 : summary.CoursesSummary.Count());

            return summary;
        }

        public async Task<StudentGradesViewModel> GetStudentGradesAsync(string id)
        {
            var student = _studentRepository.GetStudentById(id);

            if (!student.Any())
            {
                return null;
            }

            var grades = await student.Include("Grades.Course").Select(s => s.Grades).SingleAsync();

            StudentGradesViewModel viewModel = new StudentGradesViewModel()
            {
                CourseGrades = new List<CourseGrades>()
            };

            var courses = grades?.Select(g => g.Course.Name).Distinct();

            if (courses == null)
            {
                return viewModel;
            }

            foreach(var course in courses)
            {
                CourseGrades courseGrades = new CourseGrades()
                {
                    CourseName = course,
                    Grades = new List<GradeViewModel>()
                };

                foreach(var grade in grades.Where(g => g.Course?.Name == course))
                {
                    courseGrades.Grades.Add(new GradeViewModel()
                    {
                        Id = grade.Id,
                        Value = grade.Value,
                        Description = grade.Description,
                        CreatedDate = grade.CreatedDate,
                        CourseName = grade.Course?.Name
                    });
                }
                viewModel.CourseGrades.Add(courseGrades);
            }

            return viewModel;
        }

        #endregion
    }
}
