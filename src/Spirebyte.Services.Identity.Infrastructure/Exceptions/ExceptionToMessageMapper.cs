﻿using System;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Identity.Application.Authentication.Events.Rejected;
using Spirebyte.Services.Identity.Application.Authentication.Requests;
using Spirebyte.Services.Identity.Application.Users.Commands;
using Spirebyte.Services.Identity.Application.Users.Events.Rejected;
using Spirebyte.Services.Identity.Core.Exceptions;

namespace Spirebyte.Services.Identity.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
    {
        return exception switch

        {
            EmailInUseException ex => new SignUpRejected(ex.Email, ex.Message, ex.Code),
            InvalidCredentialsException ex => new SignInRejected(ex.Email, ex.Message, ex.Code),
            InvalidEmailException ex => message switch
            {
                ForgotPassword command => new ForgotPasswordRejected(command.Email, ex.Message, ex.Code),
                SignIn command => new SignInRejected(command.Email, ex.Message, ex.Code),
                SignUp command => new SignUpRejected(command.Email, ex.Message, ex.Code),
                _ => null
            },
            _ => null
        };
    }
}