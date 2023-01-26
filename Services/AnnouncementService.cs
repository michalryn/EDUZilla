using EDUZilla.Data.Repositories;
using EDUZilla.Models;
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

        public async Task<bool> AddAnnouncementAsync(AnnouncementViewModel announcementViewModel)
        {
            try
            {
                Announcement announcement = new Announcement()
                {
                    Topic = announcementViewModel.Topic,
                    Content = announcementViewModel.Content,
                    CreatedDate = announcementViewModel.Created,
                    Sender = announcementViewModel.Sender,
                    Receiver = announcementViewModel.Class
                };
                await _announcementRepository.AddAndSaveChangesAsync(announcement);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}
