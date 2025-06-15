// Repositories/LiteRepository.cs
using LiteDbCRUDLibrary.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LiteDbCRUDLibrary.Repositories
{
    public class LiteRepository<T> where T : class
    {
        private readonly string _dbPath;

        public LiteRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public int Create(T entity)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Insert(entity);
        }

        public T? Read(int id)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().FindById(id);
        }

        public IEnumerable<T> ReadAll()
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().FindAll().ToList();
        }


        public bool Update(T entity)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Update(entity);
        }

        public bool Delete(int id)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Delete(id);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Find(predicate);
        }
    }
}
