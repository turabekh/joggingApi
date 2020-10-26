using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.IdentityModels
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
