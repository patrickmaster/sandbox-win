using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sandbox.Contracts;
using Sandbox.Contracts.Api;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Models;

namespace Sandbox.WebApi.Repositories
{
    public class LibraryRepository
    {
        private readonly ILibraryProvider _provider = Manager.GetLibraryProvider();

        public IEnumerable<Library> GetAll()
        {
            return _provider.GetAll();
        }
    }
}