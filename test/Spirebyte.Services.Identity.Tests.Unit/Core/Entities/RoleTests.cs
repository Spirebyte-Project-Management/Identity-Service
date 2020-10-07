using FluentAssertions;
using Spirebyte.Services.Identity.Core.Entities;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Unit.Core.Entities
{
    public class RoleTests
    {
        [Fact]
        public void given_valid_role_isvalid_should_return_true()
        {
            var isValid = Role.IsValid(Role.User);
            isValid.Should().BeTrue();
        }

        [Fact]
        public void given_invalid_role_isvalid_should_return_false()
        {
            var isValid = Role.IsValid("InValidRole");
            isValid.Should().BeFalse();
        }
    }
}
