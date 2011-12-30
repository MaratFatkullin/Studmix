using System;
using System.Collections.Generic;
using System.Data.Entity;
using AI_.Studmix.Domain;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.Infrastructure.Repository
{
    public class EntityFrameworkUnitOfWork<TContext>
        : IUnitOfWork
        where TContext : DbContext, new()
    {
        private bool _disposed;
        protected TContext Context { get; private set; }
        private IDictionary<Type, object> Map { get; set; }

        public EntityFrameworkUnitOfWork()
        {
            Context = new TContext();
            Map = new Dictionary<Type, object>();
        }

        #region IUnitOfWork Members

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            var type = typeof (TEntity);
            if (!Map.ContainsKey(type))
                Map.Add(type, new EntityFrameworkRepository<TEntity>(Context));
            return (IRepository<TEntity>) Map[type];
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}