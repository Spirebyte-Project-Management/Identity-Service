using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions
{
    public class InvalidFullnameException : DomainException
    {
        public override string Code { get; } = "invalid_fullname";

        public InvalidFullnameException(string fullname) : base($"Invalid fullname: {fullname}.")
        {
        }
    }
}
