using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fargs.Web.Models
{
    public class Post
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string ImageName { get; set; }
        public string ImageCreatedBy { get; set; }
        public string ImageProvidedBy { get; set; }
        public string Body { get; set; }
        public string Abstract { get; set; }

        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}