using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models.Features
{
    public class Work : FeatureBase
    {
        public const string ID = "78914703-30eb-4215-99e9-7157a9667b40";
        public Work()
        {
            Id = new Guid(ID);
            Name = "Work";
        }
    }
}
