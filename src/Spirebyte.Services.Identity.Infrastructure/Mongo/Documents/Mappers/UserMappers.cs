using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Core.Entities;
using System.Linq;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers
{
    internal static class UserMappers
    {
        public static User AsEntity(this UserDocument document)
            => new User(document.Id, document.Email, document.Fullname, document.Pic, document.Password, document.Role, document.SecurityStamp, document.AccessFailedCount, document.LockoutEnd, document.CreatedAt,document.Permissions);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                Email = entity.Email,
                Fullname = entity.Fullname,
                NORMALIZED_FULLNAME = entity.NORMALIZED_FULLNAME,
                Pic = entity.Pic,
                Password = entity.Password,
                SecurityStamp = entity.SecurityStamp,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt,
                AccessFailedCount = entity.AccessFailedCount,
                LockoutEnd = entity.LockoutEnd,
                Permissions = entity.Permissions ?? Enumerable.Empty<string>()
            };

        public static UserDto AsDto(this User entity)
            => new UserDto
            {
                Id = entity.Id,
                Email = entity.Email,
                Fullname = entity.Fullname,
                Pic = entity.Pic,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt,
                Permissions = entity.Permissions ?? Enumerable.Empty<string>()
            };

        public static UserDto AsDto(this UserDocument document)
            => new UserDto
            {
                Id = document.Id,
                Email = document.Email,
                Fullname = document.Fullname,
                Pic = document.Pic,
                Role = document.Role,
                CreatedAt = document.CreatedAt,
                Permissions = document.Permissions ?? Enumerable.Empty<string>()
            };
    }
}
