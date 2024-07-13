using Domain.Entities.Users.Role;
using Domain.Shared;
using System;
using System.Data;

namespace Domain.Entities.Users
{
    public class User : Entity
    {
        public UserEmail Email { get; private set; }
        public UserName Name { get; private set; }
        public UserNif Nif { get; private set; }
        public UserPhoneNumber PhoneNumber { get; private set; }
        public UserPassword Password { get; private set; }
        public string IamId { get; private set; }
        public Guid RoleId { get; private set; }
        public UserRole Role { get; private set; }


        protected User()
        {
            // Required by EF!!!
        }

        /// <summary>
        /// Creates a BackOffice User
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="name">User's name</param>
        /// <param name="phoneNumber">User's number</param>
        /// <param name="password">User's password</param>
        /// <param name="role">User's role</param>
        /// <param name="nif">User's nif (only applicable if the user is an enduser)</param>
        public User(string email, string name, string phoneNumber, UserRole role, string nif = null, string password = null)
        {
            Email = new UserEmail(email);
            Name = new UserName(name);
            PhoneNumber = new UserPhoneNumber(phoneNumber);
            Password = password != null ? new UserPassword(password) : null;
            Role = role;
            RoleId = role.Id;
            if (role.Type == RoleType.ENDUSER)
            {
                Nif = new UserNif(nif);
            }
        }

        /// <summary>
        /// Adds the IamId to the user
        /// </summary>
        /// <param name="iamId"></param>
        public void AddIamId(string iamId)
        {
            IamId = iamId ?? throw new ArgumentNullException(nameof(iamId));
        }


        /// <summary>
        /// Updates the user's name
        /// </summary>
        /// <param name="newUserName"></param>
        public void UpdateName(string newUserName)
        {
            Name = new UserName(newUserName);
        }

        /// <summary>
        /// Updates the user's phone number
        /// </summary>
        /// <param name="newUserPhoneNumber"></param>
        public void UpdatePhoneNumber(string newUserPhoneNumber)
        {
            PhoneNumber = new UserPhoneNumber(newUserPhoneNumber);
        }

        /// <summary>
        /// Updates the user's nif
        /// </summary>
        /// <param name="newUserNif"></param>
        public void UpdateNif(string newUserNif)
        { 
            if (Role.Type != RoleType.ENDUSER)
            {
                throw new BusinessRuleValidationException("Only endusers can have a NIF");
            }

            Nif = new UserNif(newUserNif);
        }
    }   
}
