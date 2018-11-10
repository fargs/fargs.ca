using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models.Features
{
    public class Physicians : FeatureBase
    {
        public const string ID = "8aadf5d6-61c4-42fb-8c69-f6473ed7aa84";
        public Physicians()
        {
            Id = new Guid(ID);
            Name = "Physicians";
        }
    }
}
