using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }
    }
}