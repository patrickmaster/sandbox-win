using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.Api
{
    class LibraryProvider : ILibraryProvider
    {
        readonly SandboxContext _context = SandboxContext.Create();

        public IEnumerable<Library> GetAll()
        {
            return _context.Libraries.ToList();
        }

        public void Add(Library library)
        {
            _context.Libraries.Add(library);
            _context.SaveChanges();
        }
    }
}
