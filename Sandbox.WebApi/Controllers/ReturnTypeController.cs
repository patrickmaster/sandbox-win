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
    [RoutePrefix("api/returntype")]
    public class ReturnTypeController : BaseController
    {
        readonly ReturnTypeRepository _repository = new ReturnTypeRepository();

        /// <summary>
        /// Gets all possible return types for wrapped code result
        /// </summary>
        /// <returns>Collection of possible result types</returns>
        [Route("")]
        public IEnumerable<ReturnType> Get()
        {
            return _repository.GetAll();
        }
    }
}