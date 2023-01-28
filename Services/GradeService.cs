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
            var group = _classRepository.GetClassById(classId).Include("Students");

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

                if(student == null || course == null)
                {
                    return false;
                }

                Grade grade = new Grade()
                {
                    Value = form.GradeValue,
                    CreatedDate = form.CreatedDate,
                    Student = student,
                    Course = course
                };

                await _gradeRepository.UpdateAndSaveChangesAsync(grade);
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
