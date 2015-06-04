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
    public class LibraryController : BaseController
    {
        readonly LibraryRepository _repository = new LibraryRepository();

        public IEnumerable<Library> Get()
        {
            return _repository.GetAll();
        }

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
            Type libraryType = typeof (Library);

            foreach (HttpContent content in provider.Contents)
            {
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    file.Filename = content.Headers.ContentDisposition.FileName.Trim('"');
                    file.Contents = await content.ReadAsByteArrayAsync();
                }
                else
                {
                    string name = content.Headers.ContentDisposition.Name.Trim('"');
                    string value = await content.ReadAsStringAsync();
                    PropertyInfo prop = libraryType.GetProperty(name);
                    
                    if (prop != null)
                    {
                        prop.SetValue(library, GetValue(value, prop.PropertyType));
                    }
                }
            }

            Validate(library);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ModelState));
            }

            _repository.Add(library, file);

            return library;
        }

        private object GetValue(string value, Type propertyType)
        {
            if (propertyType == typeof (string))
            {
                return value;
            }
            if (propertyType == typeof (double))
            {
                double val;
                double.TryParse(value, out val);
                return val;
            }
            if (propertyType == typeof (int))
            {
                int val;
                int.TryParse(value, out val);
                return val;
            }
            if (propertyType.IsEnum)
            {
                return Enum.Parse(propertyType, value);
            }
            
            throw new NotSupportedException("Type of name [" + propertyType.Name + "] is not supported");
        }

        public Library Put([FromUri] int id, Library library)
        {
            library.ID = id;
            return _repository.Update(library);
        }

        public Library Delete([FromUri] int id)
        {
            return _repository.Delete(id);
        }
    }
}