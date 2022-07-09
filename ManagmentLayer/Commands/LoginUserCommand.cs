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
    public class LoginUserCommand : ICommand<LoginUserDto, Task<LoginResponseUserDto>>
    {
        private readonly UserManager userManager;
        private readonly IMapper mapper;
        private readonly TokenService tokenService;
        private readonly HashingService hashingService;

        public LoginUserCommand(UserManager userManager, IMapper mapper, TokenService tokenService, HashingService hashingService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.hashingService = hashingService;
        }
        public async Task<LoginResponseUserDto> Execute(LoginUserDto user)
        {
            var existingUser = await userManager.GetUserByEmail(user.Email);
            if (existingUser == null) throw new StoreException("no user with this email exists");


            var isPasswordsMatch = hashingService.CheckHashEquality(user.Password, existingUser.PasswordHash, existingUser.PasswordSalt);
            if(!isPasswordsMatch) throw new StoreException("password is wrong");

            var userResponse = mapper.Map<User, LoginResponseUserDto>(existingUser);
            userResponse.Token = tokenService.CreateToken(existingUser);
            return userResponse;
        }
    }



}
