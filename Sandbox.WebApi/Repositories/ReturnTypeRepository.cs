using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Models;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.WebApi.Repositories
{
    public class ReturnTypeRepository
    {
        public IEnumerable<ReturnType> GetAll()
        {
            return (from VariableType type in Enum.GetValues(typeof(VariableType))
                    select new ReturnType
                    {
                        ID = (int)type,
                        Name = type.ToString()
                    });
        }
    }
}