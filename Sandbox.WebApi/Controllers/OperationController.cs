using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Sandbox.Contracts;
using Sandbox.Contracts.Queue;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Models;

namespace Sandbox.WebApi.Controllers
{
    [RoutePrefix("api/operation")]
    public class OperationController : BaseController
    {
        private readonly IOperationsQueue _queue = Manager.GetQueue();

        /// <summary>
        /// Tries to get an operation result of the id specified.
        /// If the result is available, it is returned. If the operation is
        /// still in queue, the 202 status code is returned.
        /// </summary>
        /// <param name="id">ID of the operation</param>
        /// <returns>Output of the operation</returns>
        [Route("{id:guid}")]
        [ResponseType(typeof(Output))]
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

        /// <summary>
        /// Requests a new operation to be processed.
        /// </summary>
        /// <param name="input">Operation data</param>
        /// <returns>ID of the operation for later use</returns>
        [Route("")]
        [ResponseType(typeof(RequestInfo))]
        public HttpResponseMessage Post(Input input)
        {
            Guid id = _queue.Enqueue(input);

            return Request.CreateResponse(new RequestInfo { ID = id });
        }

        /// <summary>
        /// Requests a new operation to be processed. 
        /// Request to this method should be of type multipart/form-data, with the Input entity properties
        /// provided as form elements and the request should contain a source file
        /// </summary>
        /// <returns>ID of the operation for later use</returns>
        [Route("file")]
        [ResponseType(typeof(RequestInfo))]
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

            return Request.CreateResponse(new RequestInfo { ID = id });
        }
    }
}
