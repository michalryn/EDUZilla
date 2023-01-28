namespace EDUZilla.ViewModels.Announcement
{
    public class ShowAnnoucementViewModel
    {
        public int? AnnouncementId { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string? SenderEmail { get; set; }
        public int? ChosenClassId { get; set; }
    }
}
