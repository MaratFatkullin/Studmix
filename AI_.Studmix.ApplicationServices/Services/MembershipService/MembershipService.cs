﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.DataTransferObjects.Mapper;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Responses;
using AI_.Studmix.ApplicationServices.Specifications;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.ApplicationServices.Services.MembershipService
{
    public class MembershipService : IMembershipService
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public MembershipService(IUnitOfWork unitOfWork,
                                 IMembershipConfiguration configuration)
        {
            UnitOfWork = unitOfWork;
            Configuration = configuration;
        }

        #region IMembershipService Members

        public IMembershipConfiguration Configuration { get; set; }

        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            if (Configuration.RequiresEmail)
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new CreateUserResponse(MembershipCreateStatus.InvalidEmail);
                }
            }

            if (Configuration.RequiresUniqueEmail
                && !string.IsNullOrEmpty(request.Email)
                && GetUserByEmail(request.Email) != null)
            {
                return new CreateUserResponse(MembershipCreateStatus.DuplicateEmail);
            }

            if (Configuration.RequiresQuestionAndAnswer)
            {
                if (string.IsNullOrEmpty(request.PasswordQuestion))
                {
                    return new CreateUserResponse(MembershipCreateStatus.InvalidQuestion);
                }
                if (string.IsNullOrEmpty(request.PasswordAnswer))
                {
                    return new CreateUserResponse(MembershipCreateStatus.InvalidAnswer);
                }
            }

            if (!ValidatingPassword(request.Password))
            {
                return new CreateUserResponse(MembershipCreateStatus.InvalidPassword);
            }

            var existingUser = GetUser(request.UserName);
            if (existingUser != null)
            {
                return new CreateUserResponse(MembershipCreateStatus.DuplicateUserName);
            }

            var factory = new UserFactory();
            var user = factory.CreateUser(request.UserName,
                                          request.Password,
                                          request.Email,
                                          request.PhoneNumber);
            user.PasswordAnswer = request.PasswordAnswer;
            user.PasswordQuestion = request.PasswordQuestion;

            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var response = new CreateUserResponse(MembershipCreateStatus.Success, user.UserName);
            return response;
        }


        public ValidateUserResponce ValidateUser(ValidateUserRequest request)
        {
            var user = GetUser(request.UserName);
            if (user == null || user.IsLocked || !user.IsApproved)
                return new ValidateUserResponce(false);
            return new ValidateUserResponce(user.Password == request.Password);
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            var user = GetUser(request.UserName);
            if (user == null)
            {
                return new ChangePasswordResponse(false);
            }
            if (user.Password != request.OldPassword)
            {
                return new ChangePasswordResponse(false);
            }
            user.Password = request.NewPassword;
            return new ChangePasswordResponse(true);
        }

        public GetUserListResponse GetUserList(GetUserListRequest request)
        {
            var pageSize = request.PageSize;
            var pageIndex = request.PageIndex - 1;
            var repository = UnitOfWork.GetRepository<User>();
            var allUsers = repository.Get();
            var users = allUsers.Skip(pageSize * pageIndex).Take(pageSize);
            return new GetUserListResponse(DtoMapper.MapSequence<UserDto>(users), allUsers.Count);
        }

        public GetUserResponse GetUser(GetUserRequest request)
        {
            var user = UnitOfWork.GetRepository<User>()
                .Get(new GetUserByUserName(request.UserName))
                .SingleOrDefault();
            var orders = UnitOfWork.GetRepository<Order>().Get(o => o.User.ID == user.ID, includeProperties: "ContentPackage");
            var packages = orders.Select(o => o.ContentPackage).ToList();
            if (user == null)
            {
                throw new ApplicationException("Пользователь с указанным именем пользователя не существует.");
            }
            IEnumerable<Property> properties = UnitOfWork.GetRepository<Property>().Get();
            if (!request.NeedAllProperties)
            {
                properties = properties.Where(p => p.IsUserProperty);
            }
            return new GetUserResponse
                   {
                       User = DtoMapper.Map<UserDto>(user),
                       BoughtPackages = DtoMapper.MapSequence<ContentPackageDto>(packages),
                       Properties = DtoMapper.MapSequence<PropertyDto>(properties)
                   };
        }

        public UpdateUserResponse UpdateUser(UpdateUserRequest request)
        {
            var userDto = request.User;
            var user = UnitOfWork.GetRepository<User>().GetByID(userDto.ID);

            var propertiesRepository = UnitOfWork.GetRepository<Property>();
            user.PropertyStates.Clear();
            foreach (var stateDto in request.User.States)
            {
                var property = propertiesRepository.GetByID(stateDto.Key);
                var state = property.GetState(stateDto.Value);
                user.PropertyStates.Add(state);
            }

            UnitOfWork.Save();
            return new UpdateUserResponse();
        }

        #endregion

        private bool ValidatingPassword(string password)
        {
            if (password.Length < Configuration.MinRequiredPasswordLength)
                return false;

            if (password.Contains(" "))
                return false;

            return true;
        }

        private User GetUser(string username)
        {
            return UnitOfWork.GetRepository<User>()
                .Get(user => user.UserName == username.ToLower())
                .SingleOrDefault();
        }

        private User GetUserByEmail(string email)
        {
            return UnitOfWork.GetRepository<User>()
                .Get(us => us.Email == email.ToLower())
                .SingleOrDefault();
        }
    }
}