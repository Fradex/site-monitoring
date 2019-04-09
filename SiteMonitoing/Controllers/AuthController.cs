using Microsoft.AspNetCore.Mvc;
using SiteMonitoring.Model.Model.Requests;
using SiteMonitoring.Model.Model.Response;
using SiteMonitoring.Services.Interfaces;

namespace SiteMonitoring.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        protected IUserService UserService;
        public AuthController(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Залогиниться/получить токен
        /// </summary>
        [HttpPost, Route("Login")]
        public ActionResult<LoginResponseModel> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = this.UserService.GetUserToken(model.Login, model.Password);
            return new LoginResponseModel
            {
                Success = token != null,
                Token = token
            };
        }
    }
}
