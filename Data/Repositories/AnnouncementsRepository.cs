using EDUZilla.Models;
namespace EDUZilla.Data.Repositories
{
    public class AnnouncementsRepository : BaseRepository<Announcements>
    {
        public AnnouncementsRepository(ApplicationDbContext context) : base(context)
        {

        }
        public IQueryable<Announcements> GetAnnouncements()
        {
            return GetAll();
        }

    }
}
