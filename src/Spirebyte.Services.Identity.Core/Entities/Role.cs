using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Spirebyte.Services.Identity.Core.Entities;

[CollectionName("roles")]
public class Role : MongoIdentityRole<Guid>
{
}