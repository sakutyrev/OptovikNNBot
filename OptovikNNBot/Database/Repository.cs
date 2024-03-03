using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Database
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public T GetByTgId(long telegramId)
        {
            if (telegramId <= 0)
            {
                throw new ArgumentNullException("значение telegramId не может быть пустым или быть меньше нуля");
            }
            return _dbSet.Find(telegramId)!;
        }

        public T GetById(int id)  
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("значение ID не может быть меньше нуля");
            }
            return _dbSet.Find(id)!;
            
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

    }
}
