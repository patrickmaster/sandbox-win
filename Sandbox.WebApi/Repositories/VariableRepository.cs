using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Models;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.WebApi.Repositories
{
    public class VariableRepository
    {
        public IEnumerable<Variable> GetAll()
        {
            return (from VariableType type in Enum.GetValues(typeof(VariableType))
                    select new Variable
                    {
                        ID = (int)type,
                        Name = type.ToString()
                    });
        }
    }
}