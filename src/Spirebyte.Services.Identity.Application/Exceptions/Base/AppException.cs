using System;
using System.Collections.Generic;
using System.Text;

namespace Spirebyte.Services.Identity.Application.Exceptions.Base
{
    public abstract class AppException : Exception
    {
        public virtual string Code { get; }

        protected AppException(string message) : base(message)
        {
        }
    }
}
