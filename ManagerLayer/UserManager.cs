
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
        public async Task AddUser(User user)
        {  
            await this.repository.AddAsync(user);
    
        }


        public async Task<bool> IsUserExists(RegisterUserDto user)
        {
            return await repository.GetFirstByCriteria<User>(x => x.Email == user.Email) != null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await repository.GetFirstByCriteria<User>(x => x.Email == email);
        }
    }
}