using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Announcement;

namespace EDUZilla.Services
{
    public class AnnouncementService
    {
        private readonly AnnouncementRepository _announcementRepository;
        private readonly ClassRepository _classRepository;
        public AnnouncementService(AnnouncementRepository announcementRepository, ClassRepository classRepository)
        {
            _announcementRepository = announcementRepository;
            _classRepository = classRepository;
        }

        public async Task<bool> AddAnnouncementAsync(AnnouncementViewModel announcementViewModel)
        {
            try
            {
                var group = _classRepository.GetClassById(announcementViewModel);
                Announcement announcement = new Announcement()
                {
                    Topic = announcementViewModel.Topic,
                    Content = announcementViewModel.Content,
                    CreatedDate = announcementViewModel.Created,
                    Sender = announcementViewModel.Sender,
                    Receiver = group
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
