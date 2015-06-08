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

        /// <summary>
        /// Obtains a collection of all libraries in the system
        /// </summary>
        /// <returns>Library collection</returns>
        [Route("")]
        public IEnumerable<Library> Get()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Adds a new library.
        /// Request to this method should be of type multipart/form-data, with the Library entity properties
        /// provided as form elements and the request should contain a zip file with a valid library implementation
        /// </summary>
        /// <returns>Library entity</returns>
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

        /// <summary>
        /// Updates a library. Only the name of the library can be effectively updated
        /// </summary>
        /// <param name="id">ID of the library to update</param>
        /// <param name="library">Library entity</param>
        /// <returns>Library entity</returns>
        [Route("{id:int}")]
        public Library Put([FromUri] int id, Library library)
        {
            library.ID = id;
            return _repository.Update(library);
        }

        /// <summary>
        /// Deletes a library from the system
        /// </summary>
        /// <param name="id">ID of the library to remove</param>
        /// <returns>Library entity</returns>
        [Route("{id:int}")]
        public Library Delete([FromUri] int id)
        {
            return _repository.Delete(id);
        }
    }
}