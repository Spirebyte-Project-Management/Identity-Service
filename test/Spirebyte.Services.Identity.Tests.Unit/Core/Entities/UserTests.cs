using FluentAssertions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using System;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Unit.Core.Entities
{
    public class UserTests
    {
        [Fact]
        public void given_valid_input_user_should_be_created()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = Guid.NewGuid().ToString();
            var user = new User(id, email, fullname, "test.nl/image", password, role, securityStamp, DateTime.UtcNow, new string[] { });

            user.Should().NotBeNull();
            user.Id.Should().Be(id);
            user.Email.Should().Be(email);
            user.Fullname.Should().Be(fullname);
            user.Password.Should().Be(password);
            user.Role.Should().Be(role);
        }

        [Fact]
        public void given_empty_email_user_should_throw_an_exeption()
        {
            var id = new AggregateId();
            var email = "";
            var fullname = "fullname";
            var password = "secret";
            var role = Role.User;
            var securityStamp = Guid.NewGuid().ToString();

            Action act = () => new User(id, email, fullname, "test.nl/image", password, role, securityStamp, DateTime.UtcNow, new string[] { });
            act.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public void given_empty_username_user_should_throw_an_exeption()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "";
            var password = "secret";
            var role = Role.User;
            var securityStamp = Guid.NewGuid().ToString();

            Action act = () => new User(id, email, fullname, "test.nl/image", password, role, securityStamp, DateTime.UtcNow, new string[] { });
            act.Should().Throw<InvalidFullnameException>();
        }

        [Fact]
        public void given_empty_password_user_should_throw_an_exeption()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "";
            var role = Role.User;
            var securityStamp = Guid.NewGuid().ToString();

            Action act = () => new User(id, email, fullname, "test.nl/image", password, role, securityStamp, DateTime.UtcNow, new string[] { });
            act.Should().Throw<InvalidPasswordException>();
        }

        [Fact]
        public void given_invalid_role_user_should_throw_an_exeption()
        {
            var id = new AggregateId();
            var email = "test@mail.com";
            var fullname = "fullname";
            var password = "secret";
            var role = "";
            var securityStamp = Guid.NewGuid().ToString();

            Action act = () => new User(id, email, fullname, "test.nl/image", password, role, securityStamp, DateTime.UtcNow, new string[] { });
            act.Should().Throw<InvalidRoleException>();
        }
    }
}
