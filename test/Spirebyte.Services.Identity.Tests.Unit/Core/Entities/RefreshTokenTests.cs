using FluentAssertions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using System;
using Xunit;

namespace Spirebyte.Services.Identity.Tests.Unit.Core.Entities
{
    public class RefreshTokenTests
    {
        [Fact]
        public void given_empty_token_refreshtoken_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = new AggregateId();
            var token = "";
            var createdAt = DateTime.Now;

            Action act = () => new RefreshToken(id, userId, token, createdAt);
            act.Should().Throw<EmptyRefreshTokenException>();
        }

        [Fact]
        public void given_revoked_is_true_refreshtoken_revoked_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = new AggregateId();
            var token = "fdb8fdbecf1d03ce5e6125c067733c0d51de209c";
            var createdAt = DateTime.Now;
            var refreshToken = new RefreshToken(id, userId, token, createdAt);

            refreshToken.Revoke(DateTime.Now);

            Action act = () => refreshToken.Revoke(DateTime.Now);
            act.Should().Throw<RevokedRefreshTokenException>();
        }

        [Fact]
        public void given_revokeddate_token_refreshtoken_should_be_revoked()
        {
            var id = new AggregateId();
            var userId = new AggregateId();
            var token = "fdb8fdbecf1d03ce5e6125c067733c0d51de209c";
            var createdAt = DateTime.Now;
            var refreshToken = new RefreshToken(id, userId, token, createdAt);

            refreshToken.Revoke(DateTime.Now);

            refreshToken.Revoked.Should().BeTrue();
        }

        [Fact]
        public void given_valid_input_refreshtoken_should_be_created()
        {
            var id = new AggregateId();
            var userId = new AggregateId();
            var token = "fdb8fdbecf1d03ce5e6125c067733c0d51de209c";
            var createdAt = DateTime.Now;
            var refreshToken = new RefreshToken(id, userId, token, createdAt);

            refreshToken.Should().NotBeNull();
            refreshToken.Id.Should().Be(id);
            refreshToken.UserId.Should().Be(userId);
            refreshToken.Token.Should().Be(token);
            refreshToken.CreatedAt.Should().Be(createdAt);
            refreshToken.Revoked.Should().BeFalse("The revoked field should be empty on a new object");
        }
    }
}