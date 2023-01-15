using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Student;
using EDUZilla.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class ClassService
    {
        #region Properties

        private readonly ClassRepository _classRepository;
        private readonly StudentRepository _studentRepository;
        private readonly TeacherRepository _teacherRepository;

        public int TeacherListVieModel { get; private set; }

        #endregion

        #region Constructors

        public ClassService(ClassRepository classRepository, StudentRepository studentRepository, TeacherRepository teacherRepository)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
        }

        #endregion

        #region Methods

        public async Task<List<ClassListViewModel>> GetClassesListAsync()
        {
            var result = await _classRepository.GetClasses().ToListAsync();
            List<ClassListViewModel> classList = new List<ClassListViewModel>();

            if (!result.Any())
            {
                return classList;
            }

            foreach (var item in result)
            {
                int studentsCount = item.Students?.Count ?? 0;
                classList.Add(new ClassListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Count = studentsCount
                });
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
            var result = await _classRepository.GetClassById(id).Include("Students").SingleAsync();
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
                Class group = await _classRepository.GetClassById(form.ClassId).Include("Students").SingleAsync();

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

        public async Task<EditTutorViewModel?> GetEditTutorViewModelAsync(int id)
        {
            var classData = await _classRepository.GetClassById(id).Include("Tutor").SingleAsync();
            var availableTeachers = await _teacherRepository.GetTeachers().Include("TutorClass").Where(t => t.TutorClass == null).ToListAsync();

            if(classData == null)
            {
                return null;
            }

            if (!availableTeachers.Any())
            {
                availableTeachers = new List<Teacher>();
            }

            List<SelectListItem> teacherListViewModel = new List<SelectListItem>();

            foreach (var teacher in availableTeachers)
            {
                teacherListViewModel.Add(new SelectListItem()
                {
                    Value = teacher.Id,
                    Text = teacher.FirstName + " " + teacher.LastName,
                });
            }

            EditTutorViewModel viewModel = new EditTutorViewModel()
            {
                ClassId = classData.Id,
                ClassName = classData.Name,
                TutorId = classData.Tutor?.Id,
                TutorName = classData.Tutor?.FirstName + " " + classData.Tutor?.LastName,
                AvailableTeachers = teacherListViewModel
            };

            return viewModel;
        }

        public async Task<bool> EditTutorAsync(int id, string teacherId)
        {
            try
            {
                Class group = await _classRepository.GetClassById(id).Include("Tutor").SingleAsync();
                Teacher teacher = await _teacherRepository.GetTeacherById(teacherId).Include("TutorClass").SingleAsync();
                if (group == null)
                    return false;

                group.Tutor = teacher;

                await _classRepository.UpdateAndSaveChangesAsync(group);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteTutorAsync(int id)
        {
            try
            {
                Class group = await _classRepository.GetClassById(id).Include("Tutor").SingleAsync();
                if (group == null)
                    return false;

                group.Tutor = null;

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
