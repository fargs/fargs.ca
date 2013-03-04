using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using io = System.IO;
using Fargs.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;

namespace Fargs.Web.Controllers
{
    public class BlogController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.ImageContainer = this.GetImageContainer();
            var posts = this.LoadIndex();
            return View(posts);
        }

        public ActionResult Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }

            var post = this.LoadPost(id.ToLower());

            // If a view does not exist, use the default view
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, id, null);
            if (viewResult.View == null)
            {
                return View("Default", post);
            }

            // Otherwise, return the view
            return View(id, post);
        }

        private IEnumerable<Post> LoadIndex()
        {
            // load metadata
            IEnumerable<Post> posts = null;
            using (io.StreamReader reader = io.File.OpenText(ConstructMetadataPath()))
            {
                var s = new Newtonsoft.Json.JsonSerializer();
                posts = s.Deserialize<IEnumerable<Post>>(new JsonTextReader(reader));
            }
            return posts;
        }

        private Post LoadPost(string name)
        {
            var index = this.LoadIndex();
            var post = index.Single(c => c.Name == name);

            // load content
            var path = this.ConstructContentPath(name);
            io.File.ReadAllText(path);
            post.Body = io.File.ReadAllText(path);
            return post;
        }

        private string ConstructContentPath(string name)
        {
            var folderPath = ConstructFolderPath();
            var filePath = io.Path.Combine(folderPath, name) + ".md";
            if (!io.File.Exists(filePath))
            {
                throw new System.IO.IOException("The file must have an .md extension");
            }
            return filePath;
        }

        private string ConstructMetadataPath()
        {
            var folderPath = ConstructFolderPath();
            var filePath = io.Path.Combine(folderPath, "index") + ".js";
            if (!io.File.Exists(filePath))
            {
                throw new System.IO.IOException("The file must have a .js extension");
            }
            return filePath;
        }

        private string GetImageContainer()
        {
            // Connect to the cloud storage account
            var cs = ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            var storage = CloudStorageAccount.Parse(cs);

            // Get a client object for the blob storage
            var blobClient = storage.CreateCloudBlobClient();

            // Get a reference to the blob container
            var blobImageContainer = blobClient.GetContainerReference("images");

            return blobImageContainer.Uri.ToString();
        }

        private string ConstructFolderPath()
        {
            return this.HttpContext.Server.MapPath("~/App_Data");
        }

    }
}
