using System;
using System.IO;
using Newtonsoft.Json;

namespace Sandbox.Contracts.Serialization
{
    class JsonSerializationProvider : ISerializer
    {
        public void Serialize<T>(T args, string filename)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (TextWriter textWriter = new StreamWriter(fileStream))
            {
                JsonWriter jsonWriter = new JsonTextWriter(textWriter);
                serializer.Serialize(jsonWriter, args, typeof (T));
            }
        }

        public T Deserialize<T>(string filename)
        {
            JsonSerializer serializer = new JsonSerializer {MaxDepth = 10};
            serializer.TypeNameHandling = TypeNameHandling.All;
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            using (TextReader textReader = new StreamReader(fileStream))
            {
                JsonReader jsonReader = new JsonTextReader(textReader);
                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}