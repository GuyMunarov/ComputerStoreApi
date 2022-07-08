
using AutoMapper;
using DomainModel.Dtos.User;
using DomainModel.Entities;
using Infrastructure.Interfaces;
using ManagerLayer;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentLayer.Commands
{
    public class AddUserCommand: ICommand<RegisterUserDto, Task<LoginResponseUserDto>>
    {
        private readonly UserManager userManager;
        private readonly IMapper mapper;
        private readonly TokenService tokenService;

        public AddUserCommand(UserManager userManager, IMapper mapper, TokenService tokenService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }
        public async Task<LoginResponseUserDto> Execute(RegisterUserDto user)
        {
            User createdUser = await userManager.AddUser(user);
            if (createdUser == null || createdUser.Id == 0) throw new Exception("error while creating the user");
            var userResponse = mapper.Map<User, LoginResponseUserDto>(createdUser);
            userResponse.Token = tokenService.CreateToken(createdUser);
            return userResponse;
        }
    }


   
}
