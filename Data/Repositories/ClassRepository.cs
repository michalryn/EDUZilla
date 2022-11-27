using EDUZilla.Models;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Data.Repositories
{
    public class ClassRepository : BaseRepository<Class>
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IQueryable<Class> GetClasses()
        {
            return GetAll().Include("Students");
        }

        public IQueryable<Class> GetClassById(int id)
        {
            var result = DataContext.Classes.Where(c => c.Id == id);

            return result;
        }

        public async Task<bool> CheckIfClassExistsAsync(string name)
        {
            var result = await DataContext.Classes.AnyAsync(c => c.Name == name);

            return result;
        }
    }
}
