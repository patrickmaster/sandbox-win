using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.WebApi.Models;
using Sandbox.WebApi.Repositories;

namespace Sandbox.WebApi.Controllers
{
    public class PlatformController : BaseController
    {
        readonly PlatformRepository _repository = new PlatformRepository();

        public IEnumerable<Platform> Get()
        {
            return _repository.GetAll();
        }
    }
}