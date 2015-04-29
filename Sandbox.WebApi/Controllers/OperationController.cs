using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;
using Sandbox.Sandbox;
using Sandbox.WebApi.Models;

namespace Sandbox.WebApi.Controllers
{
    public class OperationController : BaseController
    {
        private ISandbox _sandbox = Manager.GetSandbox(false);

        static OperationController()
        {
            Mapper.CreateMap<Input, EnvironmentInput>();
            Mapper.CreateMap<EnvironmentOutput, Output>();
        }

        public Output Post(Input input)
        {
            EnvironmentInput envInput = Mapper.Map<EnvironmentInput>(input);
            
            if (string.IsNullOrEmpty(envInput.PackageName))
            {
                envInput.PackageName = "test";
            }

            EnvironmentOutput output = _sandbox.Run(envInput);

            return Mapper.Map<Output>(output);
        }
    }
}
