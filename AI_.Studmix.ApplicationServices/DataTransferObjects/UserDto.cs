using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class UserDto
    {
        public int ID { get; set; }

        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }

        public string Password { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Секретный вопрос")]
        public string PasswordQuestion { get; set; }

        [DisplayName("Ответ на секретный вопрос")]
        public string PasswordAnswer { get; set; }

        [DisplayName("Подтвержден")]
        public bool IsApproved { get; set; }

        [DisplayName("Заблокирован")]
        public bool IsLocked { get; set; }

        [DisplayName("Дата последнего действия")]
        public DateTime? LastActivityDate { get; set; }

        [DisplayName("Дата регистрации")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Дата последнего входа")]
        public DateTime? LastLoginDate { get; set; }

        [DisplayName("Дата последней блокировки")]
        public DateTime? LastLockoutDate { get; set; }

        [DisplayName("Дата последней смены пароля")]
        public DateTime? LastPasswordChangedDate { get; set; }

        [DisplayName("Баланс")]
        public decimal Balance { get; set; }

        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }

        public IEnumerable<PropertyStateDto> States { get; set; }

        public UserDto()
        {
            States = new List<PropertyStateDto>();
        }
    }
}