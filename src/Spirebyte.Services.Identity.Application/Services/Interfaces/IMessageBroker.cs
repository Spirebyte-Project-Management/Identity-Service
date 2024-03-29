﻿using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Services.Interfaces;

public interface IMessageBroker
{
    Task PublishAsync(params IEvent[] events);
}