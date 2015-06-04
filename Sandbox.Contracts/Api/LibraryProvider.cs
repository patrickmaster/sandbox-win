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

        public void Add(Library library, LibraryFile fileBytes)
        {
            SqlLibrary lib = Mapper.Map<SqlLibrary>(library);
            _context.Libraries.Add(lib);
            _context.SaveChanges();
        }

        public void Update(Library library)
        {
            SqlLibrary itemToUpdate = _context.Libraries.SingleOrDefault(x => x.ID == library.ID);

            if (itemToUpdate != null)
            {
                _context.Entry(itemToUpdate).CurrentValues.SetValues(library);
                _context.SaveChanges();
            }
        }

        public void Delete(Library library)
        {
            SqlLibrary itemToRemove = _context.Libraries.SingleOrDefault(x => x.ID == library.ID);

            if (itemToRemove != null)
            {
                _context.Libraries.Remove(itemToRemove);
                _context.SaveChanges();
            }
        }

        public Library Delete(int id)
        {
            List<Library> libs = GetAll().ToList();
            Library toDelete = libs.Find(x => x.ID == id);
            Delete(toDelete);
            return toDelete;
        }
    }
}
