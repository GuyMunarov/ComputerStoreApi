
using AutoMapper;
using DomainModel.Dtos.User;
using DomainModel.Entities;
using Infrastructure.Exceptions;
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
        private readonly HashingService hashingService;

        public AddUserCommand(UserManager userManager, IMapper mapper, TokenService tokenService,HashingService hashingService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.hashingService = hashingService;
        }
        public async Task<LoginResponseUserDto> Execute(RegisterUserDto user)
        {
            bool isUserExists = await userManager.IsUserExists(user);
            if (isUserExists) throw new StoreException("a user with this email already exists");

            hashingService.HashPassword(user.Password, out byte[] hash, out byte[] salt);

            User userToCreate = mapper.Map<RegisterUserDto, User>(user);
            userToCreate.PasswordSalt = salt;
            userToCreate.PasswordHash = hash;

            await userManager.AddUser(userToCreate);

            var userResponse = mapper.Map<User, LoginResponseUserDto>(userToCreate);
            userResponse.Token = tokenService.CreateToken(userToCreate);
            return userResponse;
        }
    }


   
}
