using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Environment
{
    class NativeHelper
    {
        public static IEnumerable<string> GetAllHeaders(IEnumerable<string> libraries)
        {
            List<string> result = new List<string>();
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"extensions\native");

            if (libraries != null)
            {
                foreach (string library in libraries)
                {
                    result.AddRange(
                        Helper.GetAllFilesWithExtension(Path.Combine(path, library), "h")
                        .Select(Path.GetFileNameWithoutExtension));
                }
            }

            return result;
        }
    }
}
