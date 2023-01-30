using EDUZilla.Data.Repositories;
using EDUZilla.ViewModels.Parent;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class ParentService
    {
        #region Properties

        private readonly ParentRepository _parentRepository;
        private readonly ClassRepository _classRepository;
        private readonly StudentRepository _studentRepository;

        #endregion

        #region Constructors

        public ParentService(ParentRepository parentRepository, ClassRepository classRepository, StudentRepository studentRepository)
        {
            _parentRepository = parentRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
        }

        #endregion

        #region Methods

        public async Task<List<ParentListViewModel>> GetParentsListAsync()
        {
            var parents = await _parentRepository.GetParents().ToListAsync();
            var parentsList = new List<ParentListViewModel>();
            if (parents.Any())
            {
                foreach (var parent in parents)
                {
                    parentsList.Add(new ParentListViewModel()
                    {
                        Id = parent.Id,
                        Email = parent.Email,
                        FirstName = parent.FirstName,
                        LastName = parent.LastName
                    });
                }
            }
            return parentsList;
        }

        public async Task<ParentListViewModel> GetParentByIdAsync(string id)
        {
            var parent = await _parentRepository.GetByIdAsync(id);

            if (parent == null)
            {
                return null;
            }

            ParentListViewModel parentListViewModel = new ParentListViewModel()
            {
                Id = parent.Id,
                Email = parent.Email,
                FirstName = parent.FirstName,
                LastName = parent.LastName
            };

            return parentListViewModel;
        }
        public async Task<bool> CheckIfParentOrStudent(int classId, string userId)
        {
            var chosenClass = await _classRepository.GetClassById(classId).Include("Students").SingleAsync();
            if (chosenClass.Students == null)
            {
                return false;
            }
            foreach (var student in chosenClass.Students)
            {
                if(student.Id == userId)
                {
                    return true;
                }
                if (student.ParentId == userId)
                {
                    return true;
                }

            }
            return false;
        }
        public async Task<List<ParentListViewModel?>?> GetParents(int classId)
        {
            var chosenClass = await _classRepository.GetClassById(classId).Include("Students").SingleAsync();
            List<ParentListViewModel?> parentsMail = new List<ParentListViewModel?>();
            if (chosenClass.Students == null)
            {
                return null;
            }
            foreach (var student in chosenClass.Students)
            {

                if (student.ParentId != null)
                {
                    ParentListViewModel parent = await GetParentByIdAsync(student.ParentId);
                    parentsMail.Add(parent);
                }

            }
            return parentsMail;

        }

        public async Task<string> GetParentIdAsync(string email)
        {
            var parent = await _parentRepository.GetParents().Where(x => x.Email == email).FirstOrDefaultAsync();

            if (parent == null)
            {
                return null;
            }

            return parent.Id;
        }

        #endregion

    }
}
