using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Models;

namespace Sandbox.WebApi.Repositories
{
    public class PlatformRepository
    {
        public IEnumerable<Platform> GetAll()
        {
            return (from PlatformType type in Enum.GetValues(typeof (PlatformType))
                select new Platform
                {
                    Id = (int) type, Name = type.ToString()
                });
        }
    }
}