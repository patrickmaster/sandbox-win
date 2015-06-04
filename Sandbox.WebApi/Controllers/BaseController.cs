using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper.Internal;
using Newtonsoft.Json;

namespace Sandbox.WebApi.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected void ValidateEntity<T>(T entity)
        {
            Validate(entity);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ModelState));
            }
        }

        protected static async Task SetPropertyFromContent(HttpContent content, object obj)
        {
            Type type = obj.GetType();
            string name = content.Headers.ContentDisposition.Name.Trim('"');
            string value = await content.ReadAsStringAsync();
            PropertyInfo prop = type.GetProperty(name);

            if (prop != null)
            {
                prop.SetValue(obj, GetValue(value, prop.PropertyType));
            }
        }

        private static object GetValue(string value, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return value;
            }
            if (propertyType == typeof(double))
            {
                double val;
                double.TryParse(value, out val);
                return val;
            }
            if (propertyType == typeof(int))
            {
                int val;
                int.TryParse(value, out val);
                return val;
            }
            if (propertyType.IsEnum)
            {
                return Enum.Parse(propertyType, value);
            }
            if (propertyType.IsClass || propertyType.IsInterface)
            {
                return JsonConvert.DeserializeObject(value, propertyType);
            }

            throw new NotSupportedException("Type of name [" + propertyType.Name + "] is not supported");
        }
    }
}