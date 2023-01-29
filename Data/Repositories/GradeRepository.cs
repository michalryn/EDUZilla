using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class GradeRepository : BaseRepository<Grade>
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IQueryable<Grade> GetGradeById(int id)
        {
            var result = DataContext.Grades.Where(x => x.Id == id);

            return result;
        }
    }
}
