using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.WebApi.Models;
using Sandbox.WebApi.Repositories;

namespace Sandbox.WebApi.Controllers
{
    public class VariableController: BaseController
    {
        readonly VariableRepository _repository = new VariableRepository();

        public IEnumerable<Variable> Get()
        {
            return _repository.GetAll();
        }
    }
}