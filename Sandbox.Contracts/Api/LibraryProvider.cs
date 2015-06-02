using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.Api
{
    class LibraryProvider : ILibraryProvider
    {
        static LibraryProvider()
        {
            Mapper.CreateMap<SqlLibrary, Library>();
            Mapper.CreateMap<Library, SqlLibrary>();
        }

        readonly SandboxContext _context = SandboxContext.Create();

        public IEnumerable<Library> GetAll()
        {
            IEnumerable<SqlLibrary> libs = _context.Libraries.ToList();
            return Mapper.Map<IEnumerable<SqlLibrary>, IEnumerable<Library>>(libs);
        }

        public void Add(Library library)
        {
            SqlLibrary lib = Mapper.Map<SqlLibrary>(library);
            _context.Libraries.Add(lib);
            _context.SaveChanges();
        }

        public void Update(Library library)
        {
            throw new NotImplementedException();
        }

        public void Delete(Library library)
        {
            throw new NotImplementedException();
        }
    }
}
