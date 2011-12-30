using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.Infrastructure
{
    public abstract class DataAccessObject
    {
        public IUnitOfWork UnitOfWork { get; protected set; }

        protected DataAccessObject(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}