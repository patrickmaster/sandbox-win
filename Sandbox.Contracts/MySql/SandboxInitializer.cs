using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.MySql
{
    class SandboxInitializer : CreateDatabaseIfNotExists<SandboxContext>
    {
        protected override void Seed(SandboxContext context)
        {
            context.Libraries.Add(new Library
            {
                Name = "exampleDll",
                Platform = PlatformType.DotNet
            });
            context.Libraries.Add(new Library
            {
                Name = "Simple",
                Platform = PlatformType.Java
            });
            context.Libraries.Add(new Library
            {
                Name = "mydll",
                Platform = PlatformType.Native
            });
            context.Libraries.Add(new Library
            {
                Name="equation",
                Platform = PlatformType.Python
            });

            base.Seed(context);
        }
    }
}
