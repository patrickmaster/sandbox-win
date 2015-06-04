using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Contracts.Types
{
    public class LibraryFile
    {
        public byte[] Contents { get; set; }

        public string Filename { get; set; }
    }
}
