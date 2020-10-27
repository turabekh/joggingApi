using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataTransferObjects.UserDtos
{
    public class UserRolesDto
    {
        public int UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
