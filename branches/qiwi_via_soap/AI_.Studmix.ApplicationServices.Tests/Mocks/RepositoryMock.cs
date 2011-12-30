﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AI_.Studmix.Domain;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Tests.Mocks
{
    public class RepositoryMock<TEntity>
        : IRepository<TEntity>, IObserver<object>
        where TEntity : Entity
    {
        private readonly IList<Command> _commands;
        private readonly IList<TEntity> _storage;

        public IList<TEntity> Storage
        {
            get { return _storage; }
        }

        public RepositoryMock(IObservable<object> unitOfWork)
        {
            unitOfWork.Subscribe(this);
            _commands = new List<Command>();
            _storage = new List<TEntity>();
        }

        #region IObserver<object> Members

        public void OnNext(object value)
        {
            foreach (var command in _commands)
            {
                command.Execute(_storage);
            }
            _commands.Clear();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRepository<TEntity> Members

        public ICollection<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        string includeProperties = "")
        {
            IQueryable<TEntity> query = _storage.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public ICollection<TEntity> Get(Specification<TEntity> specification)
        {
            return Get(specification.Filter, specification.OrderBy, specification.IncludeProperties);
        }

        public TEntity GetByID(object id)
        {
            return _storage.Where(entity => entity.ID == (int) id).FirstOrDefault();
        }

        public void Insert(TEntity entity)
        {
            _commands.Add(new Command(entity, CommnadType.Insert));
        }

        public void Delete(object id)
        {
            var entityToDelete = _storage.Single(entity => entity.ID == (int) id);
            _commands.Add(new Command(entityToDelete, CommnadType.Delete));
        }

        public void Delete(TEntity entityToDelete)
        {
            Delete(entityToDelete.ID);
        }

        public void Update(TEntity entityToUpdate)
        {
            _commands.Add(new Command(entityToUpdate, CommnadType.Update));
        }

        #endregion
    }


    internal class Command
    {
        public Entity Argument { get; private set; }
        public CommnadType Type { get; private set; }

        public Command(Entity argument, CommnadType type)
        {
            Argument = argument;
            Type = type;
        }

        public void Execute<TEntity>(IList<TEntity> storage) where TEntity : Entity
        {
            var enityt = Argument as TEntity;
            switch (Type)
            {
                case CommnadType.Insert:
                    Argument.ID = storage.Count + 1;
                    storage.Add(enityt);
                    break;
                case CommnadType.Update:
                    var item = storage.First(entity => entity.ID == Argument.ID);
                    Argument.UpdateDate = DateTime.Now;
                    storage.Remove(item);
                    storage.Add(enityt);
                    break;
                case CommnadType.Delete:
                    storage.Remove(enityt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    internal enum CommnadType
    {
        Insert,
        Update,
        Delete
    }
}