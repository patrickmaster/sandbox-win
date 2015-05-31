using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.MySql
{
    public class Library
    {
        public Library()
        {
            this.Tasks = new HashSet<MySql.Task>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public PlatformType Platform { get; set; }

        public virtual ICollection<MySql.Task> Tasks { get; set; }
    }
}
