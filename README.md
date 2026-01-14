# 教师信息管理系统 (Teacher Information Management System)

一个基于 ASP.NET Web API 和 Vue.js 的前后端分离项目，实现了教师信息的全流程 CRUD 管理。系统包含完整的用户认证、数据管理、批量操作与数据导入导出功能。


**后端技术:** ASP.NET Web API, Entity Framework, SQL Server
**前端技术:** Vue.js, Element UI, Axios

<img width="458" height="408" alt="e05ca377-92a4-4642-b856-05a39f0c1b1f" src="https://github.com/user-attachments/assets/9c392161-aeb6-4555-a10a-fc6715ab30eb" />
<img width="394" height="363" alt="image" src="https://github.com/user-attachments/assets/ace4a1fe-5c57-4c0d-9bc5-7243f3d6ba1c" />
<img width="1919" height="1006" alt="c36e4f80-2c14-48d2-866b-ea81d0385a5b" src="https://github.com/user-attachments/assets/0b4fffee-dd52-4ae8-9493-9e182caa75ee" />






## ✨ 功能特性

-   **用户认证**: 基于 Session 的登录/注册，支持验证码与 MD5 密码加密。
-   **数据管理**: 对教师信息进行增、删、改、查，支持多条件查询（按工号/姓名/性别）。
-   **分页与排序**: 支持表格分页显示，所有列均支持后端排序。
-   **批量操作**:
    -   **批量编辑**: 可多选教师记录，批量更新其性别、政治面貌、民族等信息。
    -   **Excel 导入/导出**: 提供模板下载，支持通过 Excel 文件批量导入数据，并可导出全部数据为 Excel。
-   **权限控制**: 自定义授权过滤器，未登录用户无法访问数据接口。

## 🚀 快速开始

1.  **克隆项目**
bash
git clone https://github.com/your-username/your-repository-name.git
cd your-repository-name

### 环境要求

-   .NET Framework 4.5+
-   SQL Server
-   现代浏览器 (支持 ES6+)
-   
1.  **克隆项目**
    bash
-   git clone https://github.com/Lalana0110/Que1.git
    cd Que1
    
2.  **数据库配置**
-   在 SQL Server 中创建名为 `SCHOOL` 的数据库。
-   修改 `Web.config` 文件中的连接字符串，确保与你的数据库信息匹配。
xml
<connectionStrings>
<add name="SchoolDbContext" connectionString="Server=192.168.1.173,1433; Database=SCHOOL; User Id=sa; Password=Admin_8619110;" providerName="System.Data.SqlClient" />
</connectionStrings>

3.  **运行后端**
-   使用 Visual Studio 打开项目解决方案文件 (`.sln`)。
-   编译并运行项目。Web API 服务将启动（通常为 `http://localhost:端口号`）。

4.  **访问前端**
-   项目运行后，直接在浏览器中打开 `Login.html` 页面即可开始使用。
### 默认账号（如果已初始化数据）

-   用户名: `test2026`
-   密码: `test2026`

## 📁 项目结构
Teacher-Management-System/
├── Controllers/ # Web API 控制器 (AccountController, JSXXBApiController)
├── Models/ # 实体模型 (JSXXB, YHB) 和 DbContext
├── Filters/ # 自定义过滤器 (ApiAuthorizeAttribute)
├── Helpers/ # 工具类 (CaptchaHelper)
├── Login.html # 登录页面
├── HtmlPage1.html # 教师信息管理主页面
└── README.md
