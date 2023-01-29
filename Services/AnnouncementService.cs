using EDUZilla.Data.Repositories;
using EDUZilla.Models;
using EDUZilla.ViewModels.Announcement;
using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Services
{
    public class AnnouncementService
    {
        private readonly AnnouncementRepository _announcementRepository;
        private readonly ClassRepository _classRepository;
        private readonly TeacherRepository _teacherRepository;
        public AnnouncementService(AnnouncementRepository announcementRepository, ClassRepository classRepository, TeacherRepository teacherRepository)
        {
            _announcementRepository = announcementRepository;
            _classRepository = classRepository;
            _teacherRepository = teacherRepository;
        }
        public async Task<Class?> GetClassesAsync(string id)
        {
            var classTutor = await _teacherRepository.GetTeacherById(id).Include("TutorClass").SingleAsync();
            if (classTutor.TutorClass == null)
            {
                return null;
            }
            return classTutor.TutorClass;

        }
        public async Task<bool> AddAnnouncementAsync(AnnouncementViewModel announcementViewModel)
        {
            try
            {
                if (announcementViewModel.SenderId == null)
                {
                    return false;
                }
                Teacher teacher = await _teacherRepository.GetTeacherById((string)announcementViewModel.SenderId).SingleAsync();

                if (announcementViewModel.ChosenClassId == null)
                {
                    Announcement announcementNoClass = new Announcement()
                    {
                        Topic = announcementViewModel.Topic,
                        Content = announcementViewModel.Content,
                        CreatedDate = announcementViewModel.Created,
                        Sender = teacher
                    };
                    await _announcementRepository.AddAndSaveChangesAsync(announcementNoClass);

                    return true;
                }
                Class group = await _classRepository.GetClassById((int)announcementViewModel.ChosenClassId).SingleAsync();
                Announcement announcement = new Announcement()
                {
                    Topic = announcementViewModel.Topic,
                    Content = announcementViewModel.Content,
                    CreatedDate = announcementViewModel.Created,
                    Sender = teacher,
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

        internal object GetParents(int? chosenClassId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShowAnnoucementViewModel>> GetAnnouncementListAsync()
        {
            var result = await _announcementRepository.GetAll().Include("Sender").Include("Receiver").ToListAsync();
            List<ShowAnnoucementViewModel> announcementList = new List<ShowAnnoucementViewModel>();
            if (!result.Any())
            {
                return announcementList;
            }
            foreach (var item in result)
            {

                if (item.Receiver == null)
                {
                    announcementList.Add(new ShowAnnoucementViewModel
                    {
                        AnnouncementId = item.Id,
                        Topic = item.Topic,
                        Content = item.Content,
                        Created = item.CreatedDate,
                        SenderEmail = item.Sender.Email

                    });
                }
                else
                {
                    announcementList.Add(new ShowAnnoucementViewModel
                    {
                        AnnouncementId = item.Id,
                        Topic = item.Topic,
                        Content = item.Content,
                        Created = item.CreatedDate,
                        SenderEmail = item.Sender.Email,
                        ChosenClassId = item.Receiver.Id

                    });
                }

            }
            return announcementList;

        }

    }
}
