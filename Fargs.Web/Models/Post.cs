using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fargs.Web.Models
{
    public class Post
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Body { get; set; }
    }
}