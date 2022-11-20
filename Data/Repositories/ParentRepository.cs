using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class ParentRepository : BaseRepository<Parent>
    {
        public ParentRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IQueryable<Parent> GetParents()
        {
            return GetAll();
        }

        public Parent GetParent(string id)
        {
            var result = DataContext.Parents.Where(p => p.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<Parent> GetByIdAsync(string id)
        {
            var result = await DataContext.Parents.FindAsync(id);

            return result;
        }
    }
}
