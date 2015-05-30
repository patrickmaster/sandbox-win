using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Sandbox.Contracts;
using Sandbox.Contracts.DataLayer;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Models;

namespace Sandbox.WebApi.Controllers
{
    public class OperationController : BaseController
    {
        private IOperationsQueue _queue;

        static OperationController()
        {
            Mapper.CreateMap<Input, EnvironmentInput>();
            Mapper.CreateMap<EnvironmentOutput, Output>();
        }

        public EnvironmentOutput Get(Guid id)
        {
            EnvironmentOutput output;

            if (_queue.TryGet(id, out output))
            {
                return output;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public Guid Post(Input input)
        {
            EnvironmentInput envInput = Mapper.Map<EnvironmentInput>(input);
            
            if (string.IsNullOrEmpty(envInput.PackageName))
            {
                envInput.PackageName = "test";
            }

            Guid id = _queue.Enqueue(envInput);

            return id;
        }
    }
}
