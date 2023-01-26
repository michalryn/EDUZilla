﻿using EDUZilla.Models;
namespace EDUZilla.Data.Repositories
{
    public class AnnouncementRepository : BaseRepository<Announcement>
    {
        public AnnouncementRepository(ApplicationDbContext context) : base(context)
        {

        }
        public IQueryable<Announcement> GetAnnouncements()
        {
            return GetAll();
        }
        public async Task UpdateAnnouncementAsync(Announcement announcement)
        {
            await UpdateAndSaveChangesAsync(announcement);
        }

    }
}
