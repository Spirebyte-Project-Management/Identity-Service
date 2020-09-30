using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.WebApi.CQRS;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Spirebyte.Services.Identity.API.RequestDispatcher.Interfaces
{
    public interface IExtendedDispatcherEndpointsBuilder : IDispatcherEndpointsBuilder
    {
        IExtendedDispatcherEndpointsBuilder Post<TRequest, TResult>(string path,
            Func<TRequest, TResult, HttpContext, Task> afterDispatch = null,
            Action<IEndpointConventionBuilder> endpoint = null, bool auth = false, string roles = null,
            params string[] policies) where TRequest : class, IRequest;
    }
}
