
using AutoMapper;
using DomainModel.Dtos.User;
using DomainModel.Entities;
using Infrastructure.Interfaces;
using Services;

namespace ManagerLayer
{
    public class UserManager : IManager
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly HashingService hashingService;

        public UserManager(IRepository repository, HashingService hashingService, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.hashingService = hashingService;
        }
        public async Task<User> AddUser(RegisterUserDto user)
        {
            if(await repository.GetFirstByCriteria<User>(x => x.Email == user.Email) != null) return null;
            
            hashingService.HashPassword(user.Password, out byte[] hash, out byte[] salt);

            User userToCreate = mapper.Map<RegisterUserDto,User>(user); // should move to command

            userToCreate.PasswordSalt = salt;
            userToCreate.PasswordHash = hash;
            await this.repository.AddAsync(userToCreate);

            return userToCreate;

        
        }
    }
}