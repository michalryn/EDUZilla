using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class ClassRepository : BaseRepository<Class>
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IQueryable<Class> GetClasses()
        {
            return GetAll();
        }

    }
}
