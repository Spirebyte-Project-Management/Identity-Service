using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Identity.Core.Constants;

namespace Spirebyte.Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string Fullname { get; private set; }
        public string NORMALIZED_FULLNAME { get; private set; }
        public string Pic { get; private set; }
        public string Role { get; private set; }
        public string SecurityStamp { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int AccessFailedCount { get; private set; }
        public DateTime LockoutEnd { get; private set; }
        public bool IsLockedOut => LockoutEnd > DateTime.Now;
        public IEnumerable<string> Permissions { get; private set; }

        public User(Guid id, string email, string fullname, string pic, string password, string role, string securityStamp, int accessFailedCount, DateTime lockoutEnd, DateTime createdAt,
            IEnumerable<string> permissions = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException(email);
            }

            if (string.IsNullOrWhiteSpace(fullname))
            {
                throw new InvalidFullnameException(fullname);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            if (!Entities.Role.IsValid(role))
            {
                throw new InvalidRoleException(role);
            }

            Id = id;
            Email = email.ToLowerInvariant();
            Fullname = fullname;
            NORMALIZED_FULLNAME = fullname.ToUpperInvariant();
            Pic = pic;
            Password = password;
            SecurityStamp = securityStamp ?? Guid.NewGuid().ToString();
            Role = role.ToLowerInvariant();
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
            AccessFailedCount = accessFailedCount;
            LockoutEnd = lockoutEnd;
            Permissions = permissions ?? Enumerable.Empty<string>();
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            Password = password;
            SecurityStamp = Guid.Empty.ToString();

            AccessFailedCount = 0;
            LockoutEnd = DateTime.MinValue;
        }

        public void ValidLogin()
        {
            AccessFailedCount = 0;
            LockoutEnd = DateTime.MinValue;
        }
        public void InvalidLogin()
        {
            if (AccessFailedCount >= AuthConstants.LoginFailuresBeforeLockout)
            {
                LockoutEnd = DateTime.Now.Add(AuthConstants.DefaultLockoutTime);
                AccessFailedCount = 0;
            }

            AccessFailedCount++;
        }
    }
}
