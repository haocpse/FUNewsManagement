using FUNews.BLL.InterfaceService;
using FUNews.DAL.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.Service
{
    public class BaseService<TEntity, TKey> where TEntity : class
    {

        protected readonly IBaseRepository<TEntity, TKey> _repository;
        public BaseService(IBaseRepository<TEntity, TKey> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(TEntity entity) => _repository.AddAsync(entity);

        public Task DeleteAsync(TKey id) => _repository.DeleteAsync(id);

        public Task<bool> ExistsAsync(TKey id) => _repository.ExistsAsync(id);

        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) => _repository.FindAsync(predicate);

        public Task<List<TEntity>> GetAllAsync() => _repository.GetAllAsync();

        public Task<TEntity?> GetByIdAsync(TKey id) => _repository.GetByIdAsync(id);

        public Task UpdateAsync(TEntity entity) => _repository.UpdateAsync(entity);
    }
}
