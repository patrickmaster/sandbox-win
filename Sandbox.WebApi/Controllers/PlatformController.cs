using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Sandbox.WebApi.Models;
using Sandbox.WebApi.Repositories;

namespace Sandbox.WebApi.Controllers
{
    [RoutePrefix("api/platform")]
    public class PlatformController : BaseController
    {
        readonly PlatformRepository _repository = new PlatformRepository();

        [Route("")]
        public IEnumerable<Platform> Get()
        {
            return _repository.GetAll();
        }
    }
}