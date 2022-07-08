
using DomainModel.Entities;
using Infrastructure.DatabaseConfiguration;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class Repository : IRepository
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }



        public T Update<T>(T entity) where T : BaseEntity
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
            Detach(entity);
            return this.GetByIdAsync<T>(entity.Id).GetAwaiter().GetResult();
        }


        public async Task<T> UpdateAsync<T>(T entity) where T : BaseEntity
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
            Detach(entity);
            return await this.GetByIdAsync<T>(entity.Id);
        }

        public void Attach<T>(T entity) where T : BaseEntity
        {
            context.Set<T>().Attach(entity);
        }

        public async Task<int> CountByCriteriaAsync<T>(Expression<Func<T, bool>> criteria) where T : BaseEntity
        {
            return await context.Set<T>().CountAsync(criteria);
        }
        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            Detach(entity);
            return await this.GetByIdAsync<T>(entity.Id);
        }



        public async Task Delete<T>(T entity) where T : BaseEntity
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
        public async Task DeleteRange<T>(List<T> entities) where T : BaseEntity
        {
            context.Set<T>().RemoveRange(entities);
            await context.SaveChangesAsync();
        }
        public async Task<T> GetByIdAsync<T>(int id) where T : BaseEntity
        {
            return await context.Set<T>().FindAsync(id);
        }


        public async Task<T> GetFirstByCriteria<T>(Expression<Func<T, bool>> criteria) where T : BaseEntity
        {
            return await context.Set<T>().FirstOrDefaultAsync(criteria);
        }

        public async Task<List<T>> ListAllAsync<T>() where T : BaseEntity
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllByCriteria<T>(Expression<Func<T, bool>> criteria) where T : BaseEntity
        {
            return await context.Set<T>().Where(criteria).ToListAsync();
        }


        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return;
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            context.Set<T>().AddRange(entities);
            return;
        }
        public void Detach<T>(T entity)
        {
            context.Entry(entity).State = EntityState.Detached;
        }


    }
}