using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.MySql
{
    class SqlLibrary
    {
        public SqlLibrary()
        {
            this.Tasks = new HashSet<SqlTask>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public PlatformType Platform { get; set; }

        public virtual ICollection<SqlTask> Tasks { get; set; }
    }
}
