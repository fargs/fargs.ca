using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.FileSystem
{
    public class AzureBlobStorageOptions
    {
        public const string SectionName = "FileSystem:AzureBlobStorage";
        public string ConnectionString { get; set; }
        public string RootContainer { get; set; }
    }
}
