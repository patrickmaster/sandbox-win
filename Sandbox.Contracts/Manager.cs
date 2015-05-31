using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Api;
using Sandbox.Contracts.Queue;
using Sandbox.Contracts.Serialization;

namespace Sandbox.Contracts
{
    public class Manager
    {
        public static ILibraryProvider GetLibraryProvider()
        {
            return new LibraryProvider();
        }

        public static IOperationsQueue GetQueue()
        {
            return new MySqlOperationsQueue();
        }

        public static IOperationsDequeue GetDequeue()
        {
            return new MySqlOperationsDequeue();
        }

        public static ISerializer GetSerializer(SerializerType type)
        {
            switch (type)
            {
                case SerializerType.Json:
                    return new JsonSerializationProvider();
                case SerializerType.Xml:
                    return new XmlSerializationProvider();
            }

            throw new NotImplementedException();
        }

        public static ISerializer GetSerializer(string type)
        {
            SerializerType serializerType;
            switch (type.ToLower())
            {
                case "json":
                    serializerType = SerializerType.Json;
                    break;
                case "xml":
                    serializerType = SerializerType.Xml;
                    break;
                default:
                    throw new NotImplementedException(string.Format("No serializer for \"{0}\"", type));
            }

            return GetSerializer(serializerType);
        }
    }

    public enum SerializerType
    {
        Json,
        Xml
    }
}
