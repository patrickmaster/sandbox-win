using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Configuration;

namespace Sandbox.Contracts.Api
{
    class LibraryProvider : ILibraryProvider
    {
        static LibraryProvider()
        {
            Mapper.CreateMap<SqlLibrary, Library>();
            Mapper.CreateMap<Library, SqlLibrary>();
        }

        readonly SandboxContext _context = SandboxContext.Create();

        public IEnumerable<Library> GetAll()
        {
            IEnumerable<SqlLibrary> libs = _context.Libraries.ToList();
            return Mapper.Map<IEnumerable<SqlLibrary>, IEnumerable<Library>>(libs);
        }

        public void Add(Library library, LibraryFile fileBytes)
        {
            SqlLibrary lib = Mapper.Map<SqlLibrary>(library);
            string buildPath = GetBuildPath();
            switch(library.Platform)
            {
                case PlatformType.DotNet:
                    buildPath += @"\dotnet";
                    break;
                case PlatformType.Java:
                    buildPath += @"\java";
                    break;
                case PlatformType.Native:
                    buildPath += @"\native";
                    break;
                case PlatformType.Python:
                    buildPath += @"\python";
                    break;
            }
            if (!Directory.Exists(buildPath + @"\" + library.Name))
            {
                Directory.CreateDirectory(buildPath + @"\" + library.Name);
                // Open file for reading
                FileStream _FileStream =
                   new System.IO.FileStream(buildPath + @"\" + library.Name + @"\" + fileBytes.Filename, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(fileBytes.Contents, 0, fileBytes.Contents.Length);

                // close file stream
                _FileStream.Close();
                Regex regexp = new Regex(@"\..{2,}"); //zip file handler
                Match match = regexp.Match(fileBytes.Filename);
                if (match.ToString() == ".zip")
                {
                    ZipFile.ExtractToDirectory(buildPath + @"\" + library.Name + @"\" + fileBytes.Filename, buildPath + @"\" + library.Name);
                    FileInfo fi = new FileInfo(buildPath + @"\" + library.Name + @"\" + fileBytes.Filename);
                    fi.Delete();
                }
                _context.Libraries.Add(lib);
                _context.SaveChanges();
                
            }
            else
            {
                throw new Exception("Name already in use");
            }
        }

        public void Update(Library library)
        {
            SqlLibrary itemToUpdate = _context.Libraries.SingleOrDefault(x => x.ID == library.ID);

            if (itemToUpdate != null)
            {
                string buildPath = GetBuildPath();
                switch (library.Platform)
                {
                    case PlatformType.DotNet:
                        buildPath += @"\dotnet";
                        break;
                    case PlatformType.Java:
                        buildPath += @"\java";
                        break;
                    case PlatformType.Native:
                        buildPath += @"\native";
                        break;
                    case PlatformType.Python:
                        buildPath += @"\python";
                        break;
                }
                if (!Directory.Exists(buildPath + @"\" + library.Name))
                {
                    Directory.Move(buildPath + @"\" + itemToUpdate.Name, buildPath + @"\" + library.Name);
                    _context.Entry(itemToUpdate).CurrentValues.SetValues(library);
                    _context.SaveChanges();                  
                }
                else
                {
                    throw new Exception("Name already in use");
                }
            }
        }

        public void Delete(Library library)
        {
            SqlLibrary itemToRemove = _context.Libraries.SingleOrDefault(x => x.ID == library.ID);

            if (itemToRemove != null)
            {
                string buildPath = GetBuildPath();
                switch (library.Platform)
                {
                    case PlatformType.DotNet:
                        buildPath += @"\dotnet";
                        break;
                    case PlatformType.Java:
                        buildPath += @"\java";
                        break;
                    case PlatformType.Native:
                        buildPath += @"\native";
                        break;
                    case PlatformType.Python:
                        buildPath += @"\python";
                        break;
                }
                DirectoryInfo dir = new DirectoryInfo(buildPath + @"\" + library.Name);
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }
                Directory.Delete(buildPath + @"\" + library.Name);
                _context.Libraries.Remove(itemToRemove);
                _context.SaveChanges();
            }
        }

        public Library Delete(int id)
        {
            List<Library> libs = GetAll().ToList();
            Library toDelete = libs.Find(x => x.ID == id);
            Delete(toDelete);
            return toDelete;
        }

        private string GetBuildPath()
        {
            string configPath = ConfigurationManager.AppSettings["BuildPath"];
            return string.IsNullOrEmpty(configPath) ? "buildPath" : configPath;
        }
    }
}
