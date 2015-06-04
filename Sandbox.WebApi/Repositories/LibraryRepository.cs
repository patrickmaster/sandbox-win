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

        public Library Add(Library library, LibraryFile file)
        {
            _provider.Add(library, file);
            return library;
        }

        public Library Update(Library library)
        {
            _provider.Update(library);
            return library;
        }

        public Library Delete(int id)
        {
            return _provider.Delete(id);
        }
    }
}