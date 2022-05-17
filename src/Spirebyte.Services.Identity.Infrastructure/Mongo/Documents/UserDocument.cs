using System;
using System.Collections.Generic;
using Convey.Types;
using Spirebyte.Services.Identity.Core.Entities;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;

internal sealed class UserDocument : User, IIdentifiable<Guid>
{
  
}