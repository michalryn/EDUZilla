using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class CourseRepository : BaseRepository<Course>
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Course> GetCourseById(int id)
        {
            return DataContext.Courses.Where(c => c.Id == id);
        }

        public IQueryable<Course> GetCourses()
        {
            return GetAll();
        }
        
    }
}
