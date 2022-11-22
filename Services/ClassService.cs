using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Student;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class ClassService
    {
        #region Properties

        private readonly ClassRepository _classRepository;
        private readonly StudentRepository _studentRepository;

        #endregion

        #region Constructors

        public ClassService(ClassRepository classRepository, StudentRepository studentRepository)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
        }

        #endregion

        #region Methods

        public async Task<List<ClassListViewModel>> GetClassesListAsync()
        {
            var result = await _classRepository.GetClasses().ToListAsync();
            List<ClassListViewModel> classList = null;
            if (result.Any())
            {
                classList = new List<ClassListViewModel>();
                foreach (var item in result)
                {
                    classList.Add(new ClassListViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                    });
                }
            }

            return classList;
        }

        public async Task<bool> CheckIfClassExists(string name)
        {
            var exists = await _classRepository.CheckIfClassExistsAsync(name);

            return exists;
        }

        public async Task<bool> AddClassAsync(string name)
        {
            try
            {
                Class newClass = new Class()
                {
                    Name = name
                };

                await _classRepository.AddAndSaveChangesAsync(newClass);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<EditClassViewModel> GetEditClassViewModelAsync(int id)
        {
            var result = await _classRepository.GetClassById(id).SingleAsync();
            var notAssigned = await _studentRepository.GetStudentsWithoutClass().ToListAsync();

            if (result == null)
            {
                return null;
            }

            if (!notAssigned.Any())
            {
                notAssigned = new List<Student>();
            }
            if (result.Students == null)
            {
                result.Students = new List<Student>();
            }

            IList<StudentListViewModel> AssignedStudents = new List<StudentListViewModel>();
            IList<StudentListViewModel> NotAssignedStudents = new List<StudentListViewModel>();

            foreach (var student in result.Students)
            {
                AssignedStudents.Add(new StudentListViewModel()
                {
                    StudentId = student.Id,
                    Email = student.Email,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                });
            }

            foreach (var student in notAssigned)
            {
                NotAssignedStudents.Add(new StudentListViewModel()
                {
                    StudentId = student.Id,
                    Email = student.Email,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                });
            }


            var viewModel = new EditClassViewModel()
            {
                ClassId = result.Id,
                Name = result.Name,
                AssignedStudents = AssignedStudents,
                NotAssignedStudents = NotAssignedStudents
            };



            return viewModel;
        }

        public async Task<bool> EditClassAsync(EditClassForm form)
        {
            try
            {
                Class group = await _classRepository.GetClassById(form.ClassId).SingleAsync();

                if(group == null)
                    return false;

                if(group.Students == null)
                {
                    group.Students = new List<Student>();
                }

                foreach (string studentId in form.AddStudent ?? new string[] { })
                {
                    Student student = await _studentRepository.GetByIdAsync(studentId);
                    if (student != null)
                    {
                        group.Students.Add(student);
                    }
                }

                foreach (string studentId in form.DeleteStudent ?? new string[] { })
                {
                    Student student = await _studentRepository.GetByIdAsync(studentId);
                    if (student != null)
                    {
                        group.Students.Remove(student);
                    }
                }

                await _classRepository.UpdateAndSaveChangesAsync(group);

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
