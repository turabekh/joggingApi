using Models.DataTransferObjects.JoggingDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataTransferObjects.UserDtos
{
    public class SingleUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<JoggingDto> Joggings { get; set; }
    }
}
