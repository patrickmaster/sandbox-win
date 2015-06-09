using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Environment
{
    class Helper
    {
        public static IEnumerable<string> GetAllFilesWithExtension(string directory, string extension)
        {
            return Directory.GetFiles(directory, string.Format("*.{0}", extension));
        }
    }
}
