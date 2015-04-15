using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Polenter.Serialization;

namespace Sandbox.Contracts.Serialization
{
    class XmlSerializationProvider : ISerializer
    {
        public void Serialize<T>(T args, string filename)
        {
            SharpSerializer serializer = new SharpSerializer();
            serializer.Serialize(args, filename);
        }

        public T Deserialize<T>(string filename)
        {
            SharpSerializer serializer = new SharpSerializer();
            return (T)serializer.Deserialize(filename);
        }
    }
}
