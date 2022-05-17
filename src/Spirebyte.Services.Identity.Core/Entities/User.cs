using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Spirebyte.Services.Identity.Core.Entities;

[CollectionName("users")]
public class User : MongoIdentityUser<Guid>
{
    public string Pic { get; }
}