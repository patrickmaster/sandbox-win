using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Sandbox.Contracts;
using Sandbox.Contracts.Queue;
using Sandbox.Contracts.Types;

namespace Sandbox.WebApi.Controllers
{
    [RoutePrefix("api/operation")]
    public class OperationController : BaseController
    {
        private readonly IOperationsQueue _queue = Manager.GetQueue();

        [Route("{id:guid}")]
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

        [Route("")]
        public HttpResponseMessage Post(Input input)
        {
            Guid id = _queue.Enqueue(input);

            return Request.CreateResponse(new { ID = id });
        }

        [Route("file")]
        public async Task<HttpResponseMessage> PostFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            
            Input input = new Input();

            foreach (HttpContent content in provider.Contents)
            {
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    input.Code = await content.ReadAsStringAsync();
                }
                else
                {
                    await SetPropertyFromContent(content, input);
                }
            }

            ValidateEntity(input); 
            Guid id = _queue.Enqueue(input);

            return Request.CreateResponse(new { ID = id });
        }
    }
}
