using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IGeneric<T> where T: class 
    {
        public Task<T> GetAsync(T id);

        public Task<IReadOnlyList<T>> GetAllAsync();

        public Task<T> GetCountAsync();

        public Task AddAsync(T entity);
        public void DeleteAsync(T entity);
        public void UpdateAsync(T entity);

        public Task<T> GetFirstAsync(Expression<Func<T, bool>> filter);


    }
}
