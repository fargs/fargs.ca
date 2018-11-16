using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models.Enums.Features
{
    public class FeatureBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public ImeHub.Data.Feature ToFeature()
        {
            return new Data.Feature() { Id = Id, Name = Name, ParentId = ParentId };
        }
    }
}
