using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ImeHub.Portal.Services.FileSystem
{
    public interface IFileSystemProvider
    {
        string RootPath { get; init; }
        FileStream DownloadFile();
    }
}
