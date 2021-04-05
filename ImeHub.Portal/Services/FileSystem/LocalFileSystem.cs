using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.FileSystem
{
    public class LocalFileSystem : IFileSystemProvider
    {
        public string RootPath { get; init; }
        public LocalFileSystem(IOptions<FileSystemOptions> options)
        {
            RootPath = options.Value.RootPath;
        }
        public FileStream DownloadFile()
        {
            throw new NotImplementedException();
        }
    }
}
