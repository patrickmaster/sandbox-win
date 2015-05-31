using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.MySql
{
    internal class SandboxContext : DbContext
    {
        private static SandboxContext _instance;

        public static SandboxContext Instance
        {
            get { return _instance ?? (_instance = new SandboxContext()); }
        }

        private SandboxContext()
            : base("MySqlConnectionString")
        {
            Database.SetInitializer(new SandboxInitializer());
        }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Library> Libraries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
