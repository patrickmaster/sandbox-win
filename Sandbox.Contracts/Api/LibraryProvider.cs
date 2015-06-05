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
            string path = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\") + @"Documents\Visual Studio 2013\Projects\sandbox-win\build\sandbox\extensions";
            switch(library.Platform)
            {
                case PlatformType.DotNet:
                    path += @"\dotnet";
                    break;
                case PlatformType.Java:
                    path += @"\java";
                    break;
                case PlatformType.Native:
                    path += @"\native";
                    break;
                case PlatformType.Python:
                    path += @"\python";
                    break;
            }
            if (!Directory.Exists(path + @"\" + library.Name))
            {
                _context.Libraries.Add(lib);
                _context.SaveChanges();
                Directory.CreateDirectory(path + @"\" + library.Name);
                // Open file for reading
                FileStream _FileStream =
                   new System.IO.FileStream(path + @"\" + library.Name + @"\" + fileBytes.Filename, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(fileBytes.Contents, 0, fileBytes.Contents.Length);

                // close file stream
                _FileStream.Close();
                //Regex regexp = new Regex(@"\w(.n't)|\w+"); //zip file handler
                //Match match = regexp.Match(fileBytes.Filename);
                //string[] compressedExtenstions = { ".rar", ".zip", ".7z" };
                //if(compressedExtenstions.Contains(match.ToString()))
                //{
                //    ZipFile.ExtractToDirectory(path + @"\" + library.Name + @"\" + fileBytes.Filename, path + @"\" + library.Name);
                //}
                
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
                string path = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\") + @"Documents\Visual Studio 2013\Projects\sandbox-win\build\sandbox\extensions";
                switch (library.Platform)
                {
                    case PlatformType.DotNet:
                        path += @"\dotnet";
                        break;
                    case PlatformType.Java:
                        path += @"\java";
                        break;
                    case PlatformType.Native:
                        path += @"\native";
                        break;
                    case PlatformType.Python:
                        path += @"\python";
                        break;
                }
                if (!Directory.Exists(path + @"\" + library.Name))
                {
                    Directory.Move(path + @"\" + itemToUpdate.Name, path + @"\" + library.Name);
                    _context.Entry(itemToUpdate).CurrentValues.SetValues(library);
                    _context.SaveChanges();                  
                }      
            }
        }

        public void Delete(Library library)
        {
            SqlLibrary itemToRemove = _context.Libraries.SingleOrDefault(x => x.ID == library.ID);

            if (itemToRemove != null)
            {
                string path = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\") + @"Documents\Visual Studio 2013\Projects\sandbox-win\build\sandbox\extensions";
                switch (library.Platform)
                {
                    case PlatformType.DotNet:
                        path += @"\dotnet";
                        break;
                    case PlatformType.Java:
                        path += @"\java";
                        break;
                    case PlatformType.Native:
                        path += @"\native";
                        break;
                    case PlatformType.Python:
                        path += @"\python";
                        break;
                }
                DirectoryInfo dir = new DirectoryInfo(path + @"\" + library.Name);
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }
                Directory.Delete(path + @"\" + library.Name);
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
    }
}
