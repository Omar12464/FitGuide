using Core;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepo<T> : IGeneric<T> where T : ModelBase
    {
        private readonly FitGuideContext _dbcontext;

        public GenericRepo(FitGuideContext dbcontext)
        {
            _dbcontext = dbcontext;
            
        }
        public async Task AddAsync(T entity)
        {
           await _dbcontext.AddAsync(entity);
           await _dbcontext.SaveChangesAsync();

        }

        public void DeleteAsync(T entity)
        {
             _dbcontext.Remove(entity);
             _dbcontext.SaveChanges();

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(T id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public Task<T> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> filter)
        {
           return await _dbcontext.Set<T>().FirstOrDefaultAsync(filter);
        }

        public void UpdateAsync(T entity)
        {
           _dbcontext.Update(entity);
            _dbcontext.SaveChanges();

        }
    }
}
