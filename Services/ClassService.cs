using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Class;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class ClassService
    {
        #region Properties

        private readonly ClassRepository _classRepository;

        #endregion

        #region Constructors

        public ClassService(ClassRepository classRepository)
        {
            _classRepository = classRepository;
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

        #endregion
    }
}
