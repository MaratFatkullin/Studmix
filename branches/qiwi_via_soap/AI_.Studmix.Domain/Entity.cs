using System;

namespace AI_.Studmix.Domain
{
    public abstract class Entity : IIdentifiable<int>
    {
        #region IIdentifiable<int> Members

        public int ID { get; set; }

        #endregion

        public DateTime CreateDate { get; protected set; }

        public DateTime? UpdateDate { get; set; }

        protected Entity()
        {
            CreateDate = DateTime.Now;
        }
    }
}