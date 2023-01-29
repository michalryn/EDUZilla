using EDUZilla.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EDUZilla.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Grade>()
                .HasOne(g => g.Course)
                .WithMany(c => c.Grades)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(c => c.Grades)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public void AttachEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Set<TEntity>().Attach(entity);
        }

        public async Task AddEntityAsync<TEntity>(TEntity entity) where TEntity : class, new()
        {
            await Set<TEntity>().AddAsync(entity);
        }

        public async Task AddEntityAndSaveChangesAsync<TEntity>(TEntity entity) where TEntity : class, new()
        {
            await AddEntityAsync(entity);
            await SaveChangesAsync();
        }

        public async Task AddEntitiesRangeAsync<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            await Set<TEntity>().AddRangeAsync(entity);
        }

        public async Task AddEntitiesRangeAndSaveChangesAsync<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            await AddEntitiesRangeAsync(entity);
            await SaveChangesAsync();
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateEntityAndSaveChangesAsync<TEntity>(TEntity entity) where TEntity : class, new()
        {
            UpdateEntity(entity);
            await SaveChangesAsync();
        }

        public void UpdateEntitiesRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity);
            }
        }

        public async Task UpdateEntitiesRangeAndSaveChangesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            UpdateEntitiesRange(entities);
            await SaveChangesAsync();
        }

        public void RemoveEntity<TEntity>(TEntity entity) where TEntity : class, new()
        {
            Set<TEntity>().Remove(entity);
        }

        public void RemoveEntitiesRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            Set<TEntity>().RemoveRange(entity);
        }

        public async Task RemoveEntitiesRangeAndSaveChangesAsync<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, new()
        {
            RemoveEntitiesRange(entity);
            await SaveChangesAsync();
        }

    }
}