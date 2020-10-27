using Microsoft.AspNetCore.Identity;
using Models.JoggingModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.IdentityModels
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Jogging> Joggings { get; set; }
    }
}
