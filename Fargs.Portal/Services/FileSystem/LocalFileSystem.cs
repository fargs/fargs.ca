using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.FileSystem
{
    public class LocalFileSystem : IFileSystem
    {
        public string RootPath { get; init; }
        public LocalFileSystem(IOptions<LocalFileSystemOptions> options) : this(options.Value) { }
        public LocalFileSystem(LocalFileSystemOptions options)
        {
            RootPath = options.RootPath;
        }
        public async Task<byte[]> DownloadFileAsync(string fileId)
        {
            var file = await File.ReadAllBytesAsync(Path.Join(RootPath, "invoices", fileId));

            return file;
        }
        public async Task UploadFileAsync(byte[] source, string destination)
        {
            await File.WriteAllBytesAsync(destination, source);
        }
    }
}
