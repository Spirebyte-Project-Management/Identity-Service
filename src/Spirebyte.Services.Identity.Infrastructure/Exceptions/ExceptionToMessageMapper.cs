using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Identity.Application.Events.Rejected;
using Spirebyte.Services.Identity.Core.Exceptions;

namespace Spirebyte.Services.Identity.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch

            {
                EmailInUseException ex => new SignUpRejected(ex.Email, ex.Message, ex.Code),
                InvalidCredentialsException ex => new SignInRejected(ex.Email,ex.Message, ex.Code),
                InvalidEmailException ex => message switch
                {
                    SignInRejected command => new SignInRejected(command.Email, ex.Message, ex.Code),
                    SignUpRejected command => new SignUpRejected(command.Email, ex.Message, ex.Code),
                    _ => null
                },
                _ => null
            };
    }
}
