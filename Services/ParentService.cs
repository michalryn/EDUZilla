using EDUZilla.Data.Repositories;
using EDUZilla.ViewModels.Parent;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class ParentService
    {
        #region Properties

        public readonly ParentRepository _parentRepository;

        #endregion

        #region Constructors

        public ParentService(ParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
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

        #endregion

    }
}
