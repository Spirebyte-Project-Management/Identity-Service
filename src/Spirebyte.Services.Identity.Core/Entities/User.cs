using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;

namespace Spirebyte.Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string Fullname { get; private set; }
        public string Pic { get; private set; }
        public string Role { get; private set; }
        public string SecurityStamp { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IEnumerable<string> Permissions { get; private set; }

        public User(Guid id, string email, string fullname, string pic, string password, string role, string securityStamp, DateTime createdAt,
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
            Pic = pic;
            Password = password;
            SecurityStamp = securityStamp ?? Guid.NewGuid().ToString();
            Role = role.ToLowerInvariant();
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
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
        }
    }
}
