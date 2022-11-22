using EDUZilla.ViewModels.Parent;

namespace EDUZilla.ViewModels.Student
{
    public class StudentAddParentViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ParentId { get; set; }
        public string? ParentFirstName { get; set; }
        public string? ParentLastName { get; set; }
        public IList<ParentListViewModel>? Parents { get; set; }

    }
}
