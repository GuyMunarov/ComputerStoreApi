
using Autofac;
using DomainModel.Dtos.User;
using ManagmentLayer.Managments;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStoreApi.Controllers
{

    public class UsersController : BaseController
    {
        private readonly UsersManagment userManagment;

        public UsersController(UsersManagment  userManagment)
        {
            this.userManagment = userManagment;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<LoginResponseUserDto>> RegisterUser(RegisterUserDto user)
        {

            var res = await userManagment.RegisterUser(user);
            return Ok(res);
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseUserDto>> LoginUser(LoginUserDto user)
        {

            var res = await userManagment.LoginUser(user);
            return Ok(res);
        }
    }

}