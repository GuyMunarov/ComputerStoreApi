using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Dtos.User
{
    public class LoginResponseUserDto
    {

        public string FullName { get; set; }


        public string Email { get; set; }


        public string Token { get; set; }
    }
}
