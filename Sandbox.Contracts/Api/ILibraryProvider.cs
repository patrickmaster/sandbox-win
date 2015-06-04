using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.Api
{
    public interface ILibraryProvider
    {
        IEnumerable<Library> GetAll();

        void Add(Library library, LibraryFile file);

        void Update(Library library);

        void Delete(Library library);

        Library Delete(int id);
    }
}
