using EDUZilla.Data;

namespace EDUZilla.Data.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : class, new()
    {
        #region Private Members

        private readonly ApplicationDbContext _dataContext;

        #endregion Private Members

        #region Constructors

        protected BaseRepository(ApplicationDbContext trackerDb)
        {
            _dataContext = trackerDb;
        }

        #endregion Constructors

        #region Properties

        protected ApplicationDbContext DataContext
        {
            get
            {
                return _dataContext;
            }
        }

        #endregion Properties

        #region Repository<TInterface> Members

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            await _dataContext.AddEntityAsync(entity);

            return entity;
        }

        public virtual async Task<TEntity> AddAndSaveChangesAsync(TEntity entity)
        {
            await _dataContext.AddEntityAndSaveChangesAsync(entity);

            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await _dataContext.AddEntitiesRangeAsync(entity);
        }

        public virtual async Task AddRangeAndSaveChangesAsync(IEnumerable<TEntity> entity)
        {
            await _dataContext.AddEntitiesRangeAndSaveChangesAsync(entity);
        }

        public virtual void Attach(TEntity entity)
        {
            _dataContext.Set<TEntity>().Attach(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dataContext.UpdateEntity(entity);
        }

        public virtual async Task UpdateAndSaveChangesAsync(TEntity entity)
        {
            await _dataContext.UpdateEntityAndSaveChangesAsync(entity);
        }

        public virtual async Task UpdateRangeAndSaveChangesAsync(IEnumerable<TEntity> entities)
        {
            await _dataContext.UpdateEntitiesRangeAndSaveChangesAsync(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _dataContext.RemoveEntity(entity);
        }

        public virtual async Task RemoveById(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                Remove(entity);
            }
        }

        public virtual async Task RemoveByIdAndSaveChangesAsync(int id)
        {
            await RemoveById(id);
            await SaveChangesAsync();
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            _dataContext.RemoveEntitiesRange(entity);
        }

        public virtual async Task RemoveRangeAndSaveChangesAsync(IEnumerable<TEntity> entity)
        {
            await _dataContext.RemoveEntitiesRangeAndSaveChangesAsync(entity);
        }

        public virtual void DetectChanges()
        {
            _dataContext.ChangeTracker.DetectChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DataContext.Set<TEntity>();
        }

        #endregion Repository<TInterface> Members

        #region IDisposable Members

        public void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }

        #endregion IDisposable Members

        #region Public Methods

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var result = await DataContext.Set<TEntity>().FindAsync(id);

            return result;
        }

        #endregion Public Methods
    }
}
