using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Sandbox.Contracts;
using Sandbox.Contracts.Queue;
using Sandbox.Contracts.Types;

namespace Sandbox.WebApi.Controllers
{
    public class OperationController : BaseController
    {
        private readonly IOperationsQueue _queue = Manager.GetQueue();

        public HttpResponseMessage Get(Guid id)
        {
            Output output;

            switch (_queue.TryGet(id, out output))
            {
                case OperationStatus.Resolved:
                    return Request.CreateResponse(output);
                case OperationStatus.Queued:
                case OperationStatus.Processing:
                    return Request.CreateResponse(HttpStatusCode.Accepted);
                default:
                    return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        public HttpResponseMessage Post(Input input)
        {
            Guid id = _queue.Enqueue(input);

            return Request.CreateResponse(new { Id = id });
        }
    }
}
