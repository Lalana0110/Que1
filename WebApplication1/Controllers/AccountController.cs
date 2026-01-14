using System;
using System.Web;
using System.Web.Http;
using System.Linq;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    //验证码功能模块

    [ApiAuthorize] // 全局授权
   
    public class AccountController : ApiController
    {
        private readonly SchoolDbContext _dbContext = new SchoolDbContext();
        [AllowAnonymous] // 允许匿名访问
        // 获取验证码
        [HttpGet]
        [Route("api/Account/Captcha")]
        public IHttpActionResult GetCaptcha()
        {
            try
            {
                var (code, imageData) = CaptchaHelper.GenerateCaptcha();

                // 将验证码存储在Session中
                HttpContext.Current.Session["CaptchaCode"] = code;
                
                return Ok(new { imageData = Convert.ToBase64String(imageData) });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

      
        /*[HttpPost] 标记接口仅接受HTTP POST 请求	1. 用于「提交 / 修改数据」（如登录、新增教师、修改教师）；
                                                    2. POST 参数在请求体中，可传大量数据（如 JSON、文件）；
                                                    3. 非幂等（多次请求可能产生副作用，如重复新增），适合有状态变更的操作。*/
        // 登录模块
        [AllowAnonymous] // 允许匿名访问
        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                // 验证验证码
                var sessionCode = HttpContext.Current.Session["CaptchaCode"] as string;
                if (string.IsNullOrEmpty(sessionCode) ||
                    !sessionCode.Equals(model.Captcha, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("验证码错误123");
                }

                // 验证用户是否存在
                var user = _dbContext.YHB.FirstOrDefault(u => u.UserId == model.UserId);
                if (user == null)
                {
                    return BadRequest("用户名不存在123");
                }

                // 验证密码(MD5加密)
                string encryptedPwd = CaptchaHelper.Md5Encrypt(model.Password);
                // Trim() 是为了防止数据库中 char 类型自动补全空格导致匹配失败
                if (user.UserPwd.Trim() != encryptedPwd)
                {
                    return BadRequest("密码错误123");
                }

                // 登录成功，记录Session
                HttpContext.Current.Session["CurrentUser"] = user;

                return Ok(new
                {
                    Success = true,
                    Message = "登录成功123",
                    User = new
                    {
                        UserId = user.UserId,
                        
                    }
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        //注册模块
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Account/Register")]
        public IHttpActionResult Register([FromBody] LoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Password))
                    return BadRequest("账号或密码不能为空");

                // 1. 检查是否已存在
                if (_dbContext.YHB.Any(u => u.UserId == model.UserId))
                    return BadRequest("用户名已存在");

                // 2. 【核心】将密码进行 MD5 加密
                string encryptedPwd = CaptchaHelper.Md5Encrypt(model.Password);

                // 3. 存储密文
                var newUser = new YHB
                {
                    UserId = model.UserId,
                    UserPwd = encryptedPwd
                };
                _dbContext.YHB.Add(newUser);
                _dbContext.SaveChanges();

                return Ok(new { Success = true, Message = "注册成功" });
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }


        // 退出登录
        [HttpPost]
        [Route("api/Account/Logout")]
        public IHttpActionResult Logout()
        {
            HttpContext.Current.Session.Clear();
            return Ok(new { Success = true, Message = "退出成功" });
        }
    }

    // 登录模型
    public class LoginModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
    }
}