# 教师信息管理系统 (Teacher Information Management System)

一个基于 ASP.NET Web API 和 Vue.js 的前后端分离项目，实现了教师信息的全流程 CRUD 管理。系统包含完整的用户认证、数据管理、批量操作与数据导入导出功能。


**后端技术:** ASP.NET Web API, Entity Framework, SQL Server
**前端技术:** Vue.js, Element UI, Axios
主页面展示：（详细功能在本文末尾）
管理用户登录页面：
<img width="458" height="408" alt="e05ca377-92a4-4642-b856-05a39f0c1b1f" src="https://github.com/user-attachments/assets/9c392161-aeb6-4555-a10a-fc6715ab30eb" />
教师信息页面：
<img width="1914" height="917" alt="d21523225ef86f990398214ea6ac6263" src="https://github.com/user-attachments/assets/ef6dae8a-aaf4-4433-8a7f-cfe931786dee" />


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
git clone https://github.com/Lalana0110/Que1.git
cd Que1

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

注册管理用户功能：
<img width="394" height="363" alt="image" src="https://github.com/user-attachments/assets/ace4a1fe-5c57-4c0d-9bc5-7243f3d6ba1c" />
新增教师信息：
<img width="498" height="622" alt="b4e778b18aa767242e1c10ef0ee85e70" src="https://github.com/user-attachments/assets/9bb4fbb6-c00e-4db4-abf7-d951241263e8" />
导入教师信息：
<img width="607" height="331" alt="fddcc97466335dc7dcc977957a0346c8" src="https://github.com/user-attachments/assets/886037dc-7d4b-4aa6-9ffc-3379f4772ac1" />
导出的excel显示：
<img width="804" height="390" alt="19c2830cca65bd996a358dd84fef853f" src="https://github.com/user-attachments/assets/f48ae363-5f9b-45e3-9fdc-5247e62ebd0c" />
查询功能：
<img width="898" height="331" alt="e840345fd5adb4b18506907dc967c93c" src="https://github.com/user-attachments/assets/8a79038e-d64a-4ad5-a8b8-cf26ff9354b7" />
教师详情：
<img width="501" height="510" alt="d89570cea6eb5efd6f880cabd124ebed" src="https://github.com/user-attachments/assets/3be9965b-9cda-4b27-baac-94f7e07c0b15" />
编辑单个教师：
<img width="497" height="629" alt="f94a555e06eb324f8794a53bbd66144d" src="https://github.com/user-attachments/assets/9ae2d353-16da-48ca-8045-014bb4495f33" />
编辑多个教师：
<img width="1533" height="430" alt="d40f3a1a659c9125b504889be81bfa1f" src="https://github.com/user-attachments/assets/b94ec82a-e27f-4e8f-81b9-f9503334d466" />
<img width="503" height="434" alt="f7fd17b136fca3a28ecd8fd23647e72f" src="https://github.com/user-attachments/assets/aec691ff-9f1b-465b-9f95-f4f8a02f6ef4" />





