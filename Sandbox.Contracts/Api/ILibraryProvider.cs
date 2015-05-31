using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.MySql;

namespace Sandbox.Contracts.Api
{
    public interface ILibraryProvider
    {
        IEnumerable<Library> GetAll();

        void Add(Library library);
    }
}
