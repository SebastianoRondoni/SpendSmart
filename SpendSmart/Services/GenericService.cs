using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using System;

namespace SpendSmart.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly SpendSmartDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericService(SpendSmartDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public void SaveChanges() => _context.SaveChanges();
    }

}
