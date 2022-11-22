using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Student;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EDUZilla.Services
{
    public class StudentService
    {
        #region Properties

        private readonly StudentRepository _studentRepository;
        private readonly ParentRepository _parentRepository;

        #endregion

        #region Constructors
        public StudentService(StudentRepository studentRepository, ParentRepository parentRepository)
        {
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
        }

        #endregion

        #region Methods

        public async Task<List<StudentListViewModel>> GetStudentsListAsync()
        {
            var students = await _studentRepository.GetAll().ToListAsync();
            var studentsList = new List<StudentListViewModel>();
            
            if (students.Any())
            {
                foreach (var student in students)
                {
                    studentsList.Add(new StudentListViewModel()
                    {
                        StudentId = student.Id,
                        Email = student.Email,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                    });
                }
            }

            return studentsList;
        }

        public async Task<StudentAddParentViewModel> GetStudentAddParentAsync(string id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            var parent = await _parentRepository.GetByIdAsync(student.ParentId);
            StudentAddParentViewModel studentViewModel = new StudentAddParentViewModel()
            {
                Id = student.Id,
                Email = student.Email,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ParentId = parent.Id,
                ParentFirstName = parent.FirstName,
                ParentLastName = parent.LastName
            };

            return studentViewModel;
        }

        public async Task<bool> AddParentAsync(string studentId, string parentId)
        {
            Student student = await _studentRepository.GetByIdAsync(studentId);
            Parent parent = await _parentRepository.GetByIdAsync(parentId);
            student.Parent = parent;
            try
            {
                await _studentRepository.UpdateStudentAsync(student);
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
