using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class FileModelRepository : BaseRepository<FileModel>
    {
        public FileModelRepository(ApplicationDbContext context) : base(context) { }


    }
}
