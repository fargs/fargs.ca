using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using io = System.IO;
using Fargs.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fargs.Web.Controllers
{
    public class BlogController : Controller
    {
        //
        // GET: /Blog/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            var model = new Post();
            this.Load(this.HttpContext.Server.MapPath("~/App_Data"), id.ToLower(), ref model);

            // If a view does not exist, use the default view
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, id, null);
            if (viewResult.View == null)
            {
                return View("Default", model);
            }

            // Otherwise, return the view
            return View(id, model);
        }

        private void Load(string folderPath, string name, ref Post model)
        {
            var mp = this.ConstructMetadataPath(folderPath, name);
            using (io.StreamReader reader = io.File.OpenText(mp))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                model.Title = o.Value<string>("Title");
            }

            io.File.ReadAllText(this.ConstructContentPath(folderPath, name));
            model.Body = io.File.ReadAllText(this.ConstructContentPath(folderPath, name));
        }

        private string ConstructContentPath(string folderPath, string name)
        {
            var filePath = io.Path.Combine(folderPath, name) + ".md";
            if (!io.File.Exists(filePath))
            {
                throw new System.IO.IOException("The file must have an .md extension");
            }
            return filePath;
        }

        private string ConstructMetadataPath(string folderPath, string name)
        {
            var filePath = io.Path.Combine(folderPath, name) + ".js";
            if (!io.File.Exists(filePath))
            {
                throw new System.IO.IOException("The file must have a .js extension");
            }
            return filePath;
        }

    }
}
