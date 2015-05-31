using System;
using System.Collections.Generic;
using System.Configuration;
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
        public static SandboxContext Create()
        {
            string connectionStringName = ConfigurationManager.AppSettings["UseConnectionString"];
            if (string.IsNullOrEmpty(connectionStringName))
            {
                connectionStringName = "MySqlConnectionString";
            }

            SandboxContext instance = new SandboxContext(connectionStringName);
            return instance;
        }

        private SandboxContext(string connectionStringName)
            : base(connectionStringName)
        {
            Database.SetInitializer(new SandboxInitializer());
        }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Library> Libraries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                        .HasMany<Library>(t => t.Libraries)
                        .WithMany(l => l.Tasks)
                        .Map(tl =>
                        {
                            tl.MapLeftKey("TaskID");
                            tl.MapRightKey("LibraryID");
                            tl.ToTable("TasksLibaries");
                        });
        }
    }
}
