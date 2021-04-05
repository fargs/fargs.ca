using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.BlazorServer.Services.FileSystem
{
    public class FileSystemOptions
    {
        public const string SectionName = "FileSystem";
        public string RootPath { get; set; }
    }
}
