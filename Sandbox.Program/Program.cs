using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Sandbox;

namespace Sandbox.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ISandbox sandbox = Manager.GetSandbox();

            string output = sandbox.Run(@"sandbox\Sandbox.Environment.exe");

            Console.WriteLine("output is: " + output);
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }
    }
}
