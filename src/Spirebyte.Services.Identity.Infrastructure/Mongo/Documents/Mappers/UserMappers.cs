using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Core.Entities;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers
{
    internal static class UserMappers
    {
        public static User AsEntity(this UserDocument document)
            => new User(document.Id, document.Email, document.Fullname, document.Pic, document.Password, document.Role, document.CreatedAt,
                document.Permissions);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                Email = entity.Email,
                Fullname = entity.Fullname,
                Pic = entity.Pic,
                Password = entity.Password,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt,
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
    }
}
