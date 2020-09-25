using System;
using System.Collections.Generic;
using System.Text;

namespace Spirebyte.Services.Identity.Core.Exceptions.Base
{
    public abstract class DomainException : Exception
    {
        public virtual string Code { get; }

        protected DomainException(string message) : base(message)
        {
        }
    }
}
