using Models.DataTransferObjects.JoggingDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataTransferObjects.UserDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<JoggingDto> Joggings { get; set; }

    }
}
