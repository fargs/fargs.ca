﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Library
{
    public class MultipleViewResult : ActionResult
    {
        public const string ChunkSeparator = "---|||---";
        public IList<PartialViewResult> PartialViewResults { get; private set; }

        public MultipleViewResult(params PartialViewResult[] views)
        {
            if (PartialViewResults == null)
                PartialViewResults = new List<PartialViewResult>();
            foreach (var v in views)
                PartialViewResults.Add(v);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            var total = PartialViewResults.Count;
            for (var index = 0; index < total; index++)
            {
                var pv = PartialViewResults[index];
                pv.ExecuteResult(context);
                if (index < total - 1)
                    context.HttpContext.Response.Output.Write(ChunkSeparator);
            }
        }
    }
}