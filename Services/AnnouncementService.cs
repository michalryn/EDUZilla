using EDUZilla.Data.Repositories;
using EDUZilla.ViewModels.Announcement;

namespace EDUZilla.Services
{
    public class AnnouncementService
    {
        private readonly AnnouncementRepository _announcementRepository;

        public AnnouncementService(AnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        
    }
}
