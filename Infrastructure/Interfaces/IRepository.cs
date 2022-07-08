using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(int id) where T : BaseEntity;

        Task<List<T>> ListAllAsync<T>() where T : BaseEntity;

        T Update<T>(T entity) where T : BaseEntity;
        Task<T> UpdateAsync<T>(T entity) where T : BaseEntity;


        void Attach<T>(T entity) where T : BaseEntity;
        Task<T> AddAsync<T>(T entity) where T : BaseEntity;
        Task AddRangeAsync<T>(IEnumerable<T> entity) where T : BaseEntity;
        void AddRange<T>(IEnumerable<T> entity) where T : BaseEntity;

        Task<int> CountByCriteriaAsync<T>(Expression<Func<T, bool>> criteria) where T : BaseEntity;

        Task Delete<T>(T entity) where T : BaseEntity;
        Task DeleteRange<T>(List<T> entity) where T : BaseEntity;
        Task<T> GetFirstByCriteria<T>(Expression<Func<T, bool>> criteria) where T : BaseEntity;

        Task<List<T>> GetAllByCriteria<T>(Expression<Func<T, bool>> criteria) where T : BaseEntity;

        void Detach<T>(T entity);
    }
}
