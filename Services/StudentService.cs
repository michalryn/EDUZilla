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

            var parent = student.ParentId != null ? await _parentRepository.GetByIdAsync(student.ParentId) : null;

            StudentAddParentViewModel studentViewModel = new StudentAddParentViewModel()
            {
                Id = student.Id,
                Email = student.Email,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ParentId = parent?.Id,
                ParentFirstName = parent?.FirstName,
                ParentLastName = parent?.LastName
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

        public async Task<string> GetStudentIdAsync(string email)
        {
            var student = await _studentRepository.GetStudents().Where(x => x.Email == email).FirstOrDefaultAsync();

            if(student == null)
            {
                return null;
            }

            return student.Id;
        }

        public async Task<List<StudentListViewModel>> GetChildrenAsync(string email)
        {
            var parent = await _parentRepository.GetParents().Where(x => x.Email == email).FirstOrDefaultAsync();

            if (parent == null)
            {
                return null;
            }

            var students = await _studentRepository.GetAll().Where(s => s.ParentId == parent.Id).ToListAsync();

            if(students == null)
            {
                return new List<StudentListViewModel>();
            }

            List<StudentListViewModel> children = new List<StudentListViewModel>();

            foreach(var student in students)
            {
                children.Add(new StudentListViewModel()
                {
                    StudentId = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email
                });
            }

            return children;
        }

        #endregion
    }
}
