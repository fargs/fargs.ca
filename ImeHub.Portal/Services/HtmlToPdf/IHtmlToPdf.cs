using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.HtmlToPdf
{
    public interface IHtmlToPdf
    {
        Task<byte[]> GenerateAsync(string content);
    }
}
