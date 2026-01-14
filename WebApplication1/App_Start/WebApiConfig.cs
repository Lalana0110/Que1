using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;  // 关键命名空间，提供 EnableCors 扩展方法


namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 启用跨域
            config.EnableCors(new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*"));

            // 2. 关键：强制返回JSON格式，禁用XML
            config.Formatters.Remove(config.Formatters.XmlFormatter); // 移除XML格式化器
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // 忽略循环引用（避免EF实体报错）
                DateFormatString = "yyyy-MM-dd" // 日期格式统一（如CSRQ字段）
            };

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
