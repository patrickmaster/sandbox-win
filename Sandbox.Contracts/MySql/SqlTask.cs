using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;

// ReSharper disable once InconsistentNaming

namespace Sandbox.Contracts.MySql
{
    class SqlTask
    {
        public SqlTask() { }

        public int ID { get; set; }
        
        public Guid SyncGuid { get; set; }

        public string Input { get; set; }

        public string Output { get; set; }

        public string Error { get; set; }

        public long Timestamp { get; set; }

        public virtual ICollection<SqlLibrary> Libraries { get; set; }

        public bool Resolved { get; set; }

        public bool Processed { get; set; }
    }
}
