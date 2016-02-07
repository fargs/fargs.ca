using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.DiagnosticViewModels
{
    public class DropboxFolderDetails
    {
        public Dropbox.Api.Files.Metadata Folder { get; set; }
        public List<Dropbox.Api.Team.MembersGetInfoItem> Members { get; set; }
    }
}