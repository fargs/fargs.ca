using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.FileSystem
{
    public class AzureBlobStorage : IFileSystem
    {
        private readonly string _connectionString;
        private readonly string _rootContainer;

        public AzureBlobStorage(IOptions<AzureBlobStorageOptions> options) : this(options.Value) { }
        public AzureBlobStorage(AzureBlobStorageOptions options)
        {
            _connectionString = options.ConnectionString;
            _rootContainer = options.RootContainer;
        }

        public async Task<byte[]> DownloadFileAsync(string source)
        {
            BlobClient blobClient = new(_connectionString, _rootContainer, source);

            using (var memorystream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(memorystream);
                return memorystream.ToArray();
            }
        }
        
        public async Task UploadFileAsync(byte[] source, string destination)
        {
            BlobClient blobClient = new(_connectionString,  _rootContainer, destination);

            await blobClient.UploadAsync(new MemoryStream(source), true);
        }
    }
}
