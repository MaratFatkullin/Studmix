using System;
using System.Collections.Generic;
using System.Linq;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? LastLockoutDate { get; set; }

        public DateTime? LastPasswordChangedDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<PropertyState> PropertyStates { get; set; }

        public decimal Balance { get; protected set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public User()
        {
            Balance = 0;
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r.RoleName == role);
        }

        public void OutcomeMoney(decimal amount)
        {
            if (Balance < amount)
                throw new InvalidOperationException();
            Balance -= amount;
        }

        public void IncomeMoney(decimal amount)
        {
            Balance += amount;
        }
    }
}