using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blog.Model.Entities;

namespace Blog.Data.Repositories
{
    public interface IEntityBaseRepository<T> where  T : class, IEntityBase, new()
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        void Commit();

        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();
        int Count();
        T GetSingle(string id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}
