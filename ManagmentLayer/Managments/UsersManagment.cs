
using DomainModel.Dtos.User;
using Infrastructure.Interfaces;
using ManagmentLayer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentLayer.Managments
{
    public class UsersManagment : IManagment
    {
        private readonly IResolver resolver;

        public UsersManagment(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public async Task<LoginResponseUserDto> RegisterUser(RegisterUserDto user)
        {
            var res = await resolver.Resolve<AddUserCommand>().Execute(user);
            return res;
        }
    }
}
