using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.FileSystem
{
    public class FileSystemFactory
    {
        public IFileSystem Create(FileSystemProvider fileSystemProvider, IConfiguration configuration)
        {
            switch (fileSystemProvider)
            {
                case FileSystemProvider.AzureBlobStorage:
                    var azureBlobStorageOptions = new AzureBlobStorageOptions();
                    configuration.GetSection(AzureBlobStorageOptions.SectionName).Bind(azureBlobStorageOptions);
                    return new AzureBlobStorage(azureBlobStorageOptions);
                case FileSystemProvider.Local:
                    var localFileSystemOptions = new LocalFileSystemOptions();
                    configuration.GetSection(LocalFileSystemOptions.SectionName).Bind(localFileSystemOptions);
                    return new LocalFileSystem(localFileSystemOptions);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
