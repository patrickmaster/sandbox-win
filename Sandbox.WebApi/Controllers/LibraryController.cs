using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Sandbox.Contracts.Types;
using Sandbox.WebApi.Repositories;
using Sandbox.Contracts.MySql;
using Sandbox.WebApi.Filters;


namespace Sandbox.WebApi.Controllers
{
    [RoutePrefix("api/library")]
    public class LibraryController : BaseController
    {
        readonly LibraryRepository _repository = new LibraryRepository();

        [Route("")]
        public IEnumerable<Library> Get()
        {
            return _repository.GetAll();
        }

        [Route("")]
        public async Task<Library> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            LibraryFile file = new LibraryFile();
            Library library = new Library();

            foreach (HttpContent content in provider.Contents)
            {
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    file.Filename = content.Headers.ContentDisposition.FileName.Trim('"');
                    file.Contents = await content.ReadAsByteArrayAsync();
                }
                else
                {
                    await SetPropertyFromContent(content, library);
                }
            }

            ValidateEntity(library);

            _repository.Add(library, file);

            return library;
        }

        [Route("{id:int}")]
        public Library Put([FromUri] int id, Library library)
        {
            library.ID = id;
            return _repository.Update(library);
        }

        [Route("{id:int}")]
        public Library Delete([FromUri] int id)
        {
            return _repository.Delete(id);
        }
    }
}