﻿using EDUZilla.Models;

namespace EDUZilla.Data.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>
    {
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IQueryable<Teacher> GetTeachers()
        {
            return GetAll();
        }

        public IQueryable<Teacher> GetTeacherByName(string name)
        {
            var result = DataContext.Teachers.Where(t => t.UserName == name);
            
            return result;
        }

        public IQueryable<Teacher> GetTeacherById(string id)
        {
            var result = DataContext.Teachers.Where(t => t.Id == id);

            return result;
        }
    }
}
