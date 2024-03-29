﻿using System;
using Convey.MessageBrokers.RabbitMQ;

namespace Spirebyte.Services.Identity.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
    {
        return exception switch

        {
            _ => null
        };
    }
}