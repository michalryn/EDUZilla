using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class GradeRepository : BaseRepository<Grade>
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {

        }


    }
}
