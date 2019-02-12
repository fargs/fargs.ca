﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp.Library.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder RemoveLine(this StringBuilder @this, string lineText)
        {
            string[] stringSeparators = new string[] { "\r\n" };
            var arr = @this.ToString().Split(stringSeparators, StringSplitOptions.None).ToList();
            var index = arr.FindIndex(c => c.Contains(lineText));
            arr.RemoveAt(index);
            return @this.Clear().Append(string.Join("\r\n", arr));
        }

    }
}