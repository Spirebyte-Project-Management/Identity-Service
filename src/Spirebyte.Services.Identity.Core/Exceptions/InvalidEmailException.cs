using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public override string Code { get; } = "invalid_email";

        public InvalidEmailException(string email) : base($"Invalid email: {email}.")
        {
        }
    }
}
