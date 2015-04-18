using System.IO;

namespace Sandbox.Environment.Wrapper
{
    internal interface IWrapper
    {
        void ToStream(Stream stream);
    }
}