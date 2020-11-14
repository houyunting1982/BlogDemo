using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Blog.Data.Persistence;
using Blog.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private BlogContext _context;
        public EntityBaseRepository(BlogContext context) {
            _context = context;
        }

        public virtual void Add(T entity) {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity) {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete(T entity) {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate) {
            var entities = _context.Set<T>().Where(predicate);
            foreach (var entity in entities) {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public void Commit() {
            _context.SaveChanges();
        }

        public IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties) {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }

            return query.AsEnumerable();
        }

        public IEnumerable<T> GetAll() {
            return _context.Set<T>().AsEnumerable();
        }

        public int Count() {
            return _context.Set<T>().Count();
        }

        public T GetSingle(string id) {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate) {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate) {
            return _context.Set<T>().Where(predicate);
        }
    }
}
