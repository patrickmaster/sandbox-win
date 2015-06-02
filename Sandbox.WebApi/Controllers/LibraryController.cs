using System.Collections.Generic;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Repositories;
using Sandbox.Contracts.MySql;


namespace Sandbox.WebApi.Controllers
{
    public class LibraryController : BaseController
    {
        readonly LibraryRepository _repository = new LibraryRepository();

        public IEnumerable<Library> Get()
        {
            return _repository.GetAll();
        }

    }
}