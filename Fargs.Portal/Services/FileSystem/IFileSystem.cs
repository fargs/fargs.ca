using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Fargs.Portal.Services.FileSystem
{
    public interface IFileSystem
    {
        Task<byte[]> DownloadFileAsync(string fileId);
        Task UploadFileAsync(byte[] source, string destination);
    }
}
