using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.FileSystem
{
    public class LocalFileSystemOptions
    {
        public const string SectionName = "FileSystem:Local";
        public string RootPath { get; set; }
    }
}
