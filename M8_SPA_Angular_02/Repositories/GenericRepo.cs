﻿using M8_SPA_Angular_02.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace M8_SPA_Angular_02.Repositories
{
    public class GenericRepo<T> : IGenericRepository<T> where T : class, new()
    {
        private DbContext db = default!;
        private DbSet<T> dbSet = default!;
        public GenericRepo(DbContext db)
        {
            this.db = db;
            this.dbSet = this.db.Set<T>();
        }
        public async Task AddAsync(T item)
        {
            await dbSet.AddAsync(item);
        }

        public async Task AddRange(IEnumerable<T> items)
        {
            await dbSet.AddRangeAsync(items);
        }

        public async Task DeleteAsync(T item)
        {
            var r = dbSet.Remove(item);
            await Task.FromResult(r);
        }

        public Task DeleteRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(string[] includes)
        {
            var data = dbSet.AsQueryable(); ;
            foreach (var s in includes)
            {
                data = data.Include(s);
            }
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            var data = dbSet.AsQueryable();

            data = includes(data);

            return await data.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var data = dbSet.AsQueryable();

            return await data.FirstAsync(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            var data = dbSet.AsQueryable();
            if (includes != null)
            {
                data = includes(data);
            }
            return await data.FirstAsync(predicate);
        }

        public  Task UpdateAsync(T item)
        {
            this.db.Entry(item).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
