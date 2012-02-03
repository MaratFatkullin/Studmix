using System;
using System.Collections.ObjectModel;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Factories
{
    public class UserFactory
    {
        public User CreateUser(string username,
                               string password,
                               string email,
                               string phoneNumber,
                               Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var userPrinciple = InternalCreateUser(username, password, email, phoneNumber);
            userPrinciple.Roles.Add(role);

            return userPrinciple;
        }

        public User CreateUser(string username,
                               string password,
                               string email,
                               string phoneNumber)
        {
            var userPrinciple = InternalCreateUser(username, password, email, phoneNumber);
            return userPrinciple;
        }

        private User InternalCreateUser(string username, string password, string email, string phoneNumber)
        {
            return new User
                   {
                       Email = email,
                       IsApproved = true,
                       IsLocked = false,
                       UserName = username,
                       Password = password,
                       PasswordAnswer = null,
                       PasswordQuestion = null,
                       PhoneNumber = phoneNumber,
                       Orders = new Collection<Order>(),
                       Roles = new Collection<Role>(),
                       PropertyStates = new Collection<PropertyState>(),
                       Invoices = new Collection<Invoice>()
                   };
        }
    }
}