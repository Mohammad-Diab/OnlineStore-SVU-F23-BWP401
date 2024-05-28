using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace OnlineStore.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UsersController : MyControllerBase
    {

        [HttpGet]
        [ActionName(nameof(IsLogin))]
        public ApiResponse<bool> IsLogin()
        {
            try
            {
                string token = getTokenValue();
                var result = Authentication.IsLogin(token);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }

        }

        [HttpGet]
        [ActionName(nameof(IsAdmin))]
        public ApiResponse<bool> IsAdmin()
        {
            try
            {
                string token = getTokenValue();
                var result = Authentication.IsAdmin(token);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex) { return new ApiResponse<bool>(400, ex.Message); }
        }

        [HttpGet]
        [ActionName(nameof(Login))]
        public ApiResponse<string> Login(string username, string password)
        {
            try
            {
                var token = Authentication.Login(username, password);
                if (string.IsNullOrEmpty(token))
                {
                    return new ApiResponse<string>(400, "خطأ في اسم المستخدم وكلمة المرور");
                }
                return new ApiResponse<string>(token);

            }
            catch (Exception) { return new ApiResponse<string>(400, "خطأ في اسم المستخدم وكلمة المرور"); }
        }

        [HttpPost]
        [ActionName(nameof(CreateUser))]
        public ApiResponse<bool> CreateUser([FromBody] Register user)
        {
            try
            {
                bool result = Logic.CreateUser(user.email, user.username, user.password, user.fullName, user.phoneNumber);

                if (result)
                {
                    return new ApiResponse<bool>(true);
                }
                else
                {
                    return new ApiResponse<bool>(400, "فشل انشاء مستخدم");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(400, ex.Message);
            }
        }

        [HttpGet]
        [ActionName(nameof(Logout))]
        public ApiResponse<bool> Logout()
        {
            string token = getTokenValue();
            Authentication.Logout(token);
            return new ApiResponse<bool>(true);
        }


        //[HttpGet]
        //[ActionName(nameof(GetAllUsers))]
        //public List<UserTable>? GetAllUsers()
        //{
        //    try
        //    {
        //        return Logic.GetAllUsers();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
