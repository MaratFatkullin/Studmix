using System;

namespace AI_.Studmix.Domain.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
        void Save();
    }
}