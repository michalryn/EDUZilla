using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IQueryable<Student> GetStudents()
        {
            return GetAll();
        }

        public async Task<Student> GetByIdAsync(string id)
        {
            var result = await DataContext.Students.FindAsync(id);

            return result;
        }

        public Student GetStudent(string id)
        {
            var result = DataContext.Students.Where(s => s.Id == id).FirstOrDefault();
            return result;
        }

        public IQueryable<Student> GetStudentById(string id)
        {
            var result = DataContext.Students.Where(s => s.Id == id);

            return result;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            await UpdateAndSaveChangesAsync(student);
        }

        public IQueryable<Student> GetStudentsWithoutClass()
        {
            var result = DataContext.Students.Where(s => s.ClassId == null);

            return result;
        }
    }
}
