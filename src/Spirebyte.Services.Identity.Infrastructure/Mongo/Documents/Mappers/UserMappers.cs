using System.Linq;
using Spirebyte.Services.Identity.Application.Users.DTO;
using Spirebyte.Services.Identity.Core.Entities;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents.Mappers;

internal static class UserMappers
{
    public static User AsEntity(this UserDocument document)
    {
        return document;
    }

    public static UserDocument AsDocument(this User entity)
    {
        return entity as UserDocument;
    }

    public static UserDto AsDto(this User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            Fullname = entity.UserName,
            Pic = entity.Pic,
            Roles = entity.Roles,
            Claims = entity.Claims.Select(c => c.Value),
            CreatedAt = entity.CreatedOn
        };
    }

    public static UserDto AsDto(this UserDocument document)
    {
        return new UserDto
        {
            Id = document.Id,
            Email = document.Email,
            PhoneNumber = document.PhoneNumber,
            Fullname = document.UserName,
            Pic = document.Pic,
            Roles = document.Roles,
            Claims = document.Claims.Select(c => c.Value),
            CreatedAt = document.CreatedOn
        };
    }
}