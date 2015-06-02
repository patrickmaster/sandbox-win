using System.Collections.Generic;
using Sandbox.Contracts;
using Sandbox.Contracts.Api;
using Sandbox.Contracts.Types;

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