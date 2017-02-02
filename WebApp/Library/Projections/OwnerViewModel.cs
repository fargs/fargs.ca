using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library.Projections
{
    public class OwnerViewModel : IEquatable<OwnerViewModel>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public bool Equals(OwnerViewModel other)
        {
            if (Id == other.Id && Name == other.Name)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hashId = Id.HasValue ? 0 : Id.Value.GetHashCode();
            int hashName = Name == null ? 0 : Name.GetHashCode();

            return hashId ^ hashName;
        }
    }
}