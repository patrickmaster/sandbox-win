using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.WebApi.Models;
using Sandbox.WebApi.Repositories;

namespace Sandbox.WebApi.Controllers
{
    public class ReturnTypeController : BaseController
    {
        readonly ReturnTypeRepository _repository = new ReturnTypeRepository();

        public IEnumerable<ReturnType> Get()
        {
            return _repository.GetAll();
        }
    }
}