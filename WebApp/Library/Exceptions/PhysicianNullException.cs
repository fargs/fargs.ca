using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class PhysicianNullException : Exception
    {
        public PhysicianNullException() : base("Physician is required")
        {
        }
    }
}