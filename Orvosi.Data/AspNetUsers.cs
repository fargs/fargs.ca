using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    public partial class AspNetUser
    {
        public AspNetRole GetRole()
        {
            return AspNetUserRoles.FirstOrDefault()?.AspNetRole;
        }

        public Guid GetRoleId()
        {
            return GetRole().Id;
        }

        public byte? GetRoleCategory()
        {
            return GetRole().RoleCategoryId;
        }
    }
}
