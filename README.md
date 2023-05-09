## 介绍
Ligg.LightSap(Lightweight System Applications and Products in Data Processing)是在融入了作者在多年的实施运维工作中对众多企业信息管理系统设计理念的理解(如Saleforce CRM的定制、SEP的策略、Oracle EBS的二次开发、Dynamics系列的集成、Pro-e/ideas CAD软件的（变）参数化设计...)，特别是SAP ECC的基于配置和变参的系统架构，同时参考优秀的.Net开源项目如ABP、YiShaAdmin、OpenAuth、Furion、vue-element-admin-site的技术框架后，打造的一款轻型的、B端和C端通用的、基于配置和策略及原创的文本编程技术、具有易于扩展、少码化、普适化特点的Web编程框架。既可作为企业信息管理系统（OA、PLM、ERP、MES...）和RPA、上位机、自动化运维系统的后端，也适合C端应用如资讯站点、博客、文库、网盘、电商等平台。
#### 当前版本: 3.1.1.0
## 特点
#### 应用配置和策略，从业务层面支撑可扩展的切面式(AOP)编程，满足需求的个性化、多样化、应对需求的可伸缩性、多变性、不确定性；
> 1. 配置包括开发配置、租户配置、运维配置、定制配置、个人配置、业务配置, 业务配置包括制造业7个业务模块(协作、人力、财务、研发、生产、销售、质量)的配置点，
> 2. 策略包括ActionLog策略、EntryLog策略、AuditLog策略、Cors策略

#### 采用原创的文本编程技术实现定制化和无码化，同时支持多种配置文件（.xlsx、 .xls、.csv、 .xml、 .json）和文本数据（Lstring、Larray、Ldict、Ltable、Lson）及其文件（.lstr、.larr、.ldict、.ltb、.lson），详见[《Ligg.EasyRPADesk说明》](https://github.com/liggin2019/Ligg.EasyRpaDesk)；
#### 基于社会治理模型的权限体系：权(职权，Authority, what can do or not)、限(限定范围，Confine, restrict to a scope)分离，多级授权（Owner、Manager、Producer、Consumer），唯一关联，多层、多维验证和筛选；
#### 动态Linq表达式, 包括DynamicExpressionEx、DynamicFieldFilter、DynamicExpressionBuilder；
#### 同时支持多种类型的Entity主键，包括雪花Id、自增长Id、Guid、字符型Id（在Entity类里定义）;
#### 缓存支持Redis、MemoryCache、EnyimCache(在appsettings.json里定义);
#### 用户验证(Authencation)在Mvc端采用Cookie或Session+本地缓存+jwt, WebApi端采用本地缓存+jwt;
#### 支持多应用部署、多客户端、多门户、多业务模块、多组织，说明如下：
> 1. 示例应用部署包括MvcBasis、WebApiMis, 
> 2. 示例客户端包括MvcAdmin、MvcSite（无码化Mvc客户端）、Vue客户端、LRD客户端(无码化Winform和Console客户端)，
> 3. 示例门户包括系统管理(MvcAdmin)、内容管理(cms,MvcAdmin)、官网(MvcSite,响应式页面)、销售内联网(MvcSite,响应式页面), 可以通过配置定制扩展,  
> 4. 按业务模块的组织配置点（如公司、工厂、账套、成本核算范围等，见系统管理-（行政）组织管理）进行扩展开发或无码化定制, 模块可共用或独享应用部署、客户端、门户，并按策略分表分库

## 架构扩展
> 1. 主ORM采用EF Core,Db first模式，辅助ORM采用工厂模式（缺省FreeSql）、可以扩展到支持SugarSql、Dapper; 可以增加一个项目DbMigrator, 通过EF Core扩展成Code first, 
> 2. 示例数据库采用Mysql，可以通过扩展主/辅ORM到支持各主流关系型数据库如Sqlserver、PostgreSql、Sqlite,
> 3. 日志采用Nlog和工厂模式、可以扩展到支持Net4log、SeriLog, 
> 4. 应用部署扩展：保留MvcBasis，可以按模块或功能增加Mvc或WebApi应用，并按策略分表分库，
> 5. 多语言支持扩展：按配置DevConfigure_Culture、DevConfigure_Text、OrpConfigure_Language，利用TextHandler、Portal-data.js、Portal-data.js进行扩展,
> 6. 多租户支持扩展：按租户配置（TntConfig），利用TenantService、中间件、主/辅ORM进行分表分库，实现多租户支持，
> 7. MvcAdmin门户和MvcSite门户的示例模板和样式各只有一对: _admin.cshtml/admin-theme.css、_site.cshtml/site-theme.css, 可以对等增加多个，如_admin1.cshtml/admin1-theme.css/admin1-theme1.css和_site1.cshtml/site1-theme.css/site1-theme1.css, 
> 8. 页面分为SystematicPage和CustPage, 后者为无码化页面、示例只支持定制文章; 可以在现有的框架基础上，通过CustConfig_DataSource、CustConfig_DataFormat、CustConfig_PageLayout、CustConfig_Page、CustConfig_Form、CustConfig_Workflow配置以及ViewComponent扩展到无码化的定制表单、定制流程、定制报表、定制Echar报表、可视化大屏，
> 9. 关于无码化：通过配置和策略以及IOC、ORM以及动态Linq表达式等技术可以把SystematicPage/Form变为CustPage/CustForm，通用化Action（增、删、改、查、批准、版本变更等）直接把数据保存到表,  但是按作者对制造业需求复杂性和业务关联集成程度的理解，局部的无码化是可行的，如检测数据、自动化监控的数据采集等；可以利用本框架只是对上述功能及展示层面的功能做无码化扩展

## 功能扩展
> 1. 操作者目前只支持用户， 已预留其他如手机和机器（后者适用于RPA、上位机、自动化运维系统）的接口； 可以增加Mobile/Machie Entity, 把UserGroup修改为OeratorGroup进行扩展, 
> 2. 系统管理模块(SYS)功能: 租户管理、菜单管理、组织管理、用户管理、角色管理、权限组管理、通讯组管理、配置管理, 已经非常齐全；如果在WebApi应用部署里使用本地缓存，应该增加缓存管理功能，以保存与MvcBasis的缓存同步, 
> 3. 共享沟通协作模块(SCC)功能: 示例只包含标签管理、目录管理、内容管理-文章，内容管理可以扩展到网盘、图文档、题库、问卷调查等，
> 4. 按照框架的权限体系，内容管理可以实现B端的企业网盘、内部知识库、图文档/合同管理系统等；启用个人配置和Category、ConfigItem的Owner字段, 内容管理+定制页面可以实现C端应用如资讯站点、博客、文库、网盘等，
> 5. 按照框架的开发体系和范式以及业务配置，扩展开发，可以实现企业信息管理系统（OA、PLM、ERP、MES...）各功能


## 采用技术包括 
net core 3.1(可以快速扩展到Net 5/6/7)、Mvc、WebAPI、EF core、FreeSql、Nlog、Swagger、AutoFac、Quartz、AutoMapper、Jwt、Mock、NUnit、Jquery、BootStrap、yiSha.js、VUE、Element-ui、Ligg.EasyRPADesk（无码化桌面端）

## 开发环境
> - Microsoft Visual Studio 2019 Version 16.9.2
> - net core 3.1
> - mysql  Ver 8.0.22
> - vsCode 1.65.0(Vue客户端)

## 调试运行

克隆项目
> git clone https://github.com/Liggin2019/Ligg.LightSap.git</br>
> git clone https://gitee.com/Liggin2019/Ligg.LightSap.git

### 后端
> 1. 本机新建mySql数据库，数据库名:lightsap, 修改\src\web\src\Ligg.Uwa.Mvc\appsettings.json, \src\web\src\Ligg.Uwa.WepApi\appsettings.json的DbConnectionString的密码到实际密码 
> 2. 执行脚本\src\Web\Src\Data\lightsap.sql
> 3. Visual Studio打开\src\web\src\LightSap.sln,  设Ligg.Uwa.Mvc或Ligg.Uwa.WepApi为启动项目， F5开始调试: 
>> - 按项目的方式启动项目Mvc端口为5001, Webapi端口为6001
>> - 按iisExpress的方式启动项目Mvc端口为5002, Webapi端口为6002
> 4. 对于Mvc项目
>> - http://localhost:5001/Home/Index/os 为官网门户（不需登录）
>> - http://localhost:5001/Home/Index/sys 为系统管理门户
>> -  http://localhost:5001/Home/Index/cms 为内容管理门户
>> -  http://localhost:5001/Home/Index/si 为销售内联网门户
>> - 按登录界面用户登录（root为超级用户）进行权限测试

### Vue客户端
> 1. VsCode 打开项目下目录\src\vue\
> 2. 安装依赖： 命令行运行（建议不要直接使用 cnpm 安装以来，会有各种诡异的 bug。可以通过如下操作解决 npm 下载速度慢的问题）
    npm install --registry=https://registry.npm.taobao.org
> 3. 启动服务
    npm run dev
> 4. 注意： 在启动服务之前，需要在Visual Studio用IISExpress运行WepApi项目, 此时Webapi端口为6002； 务必与\src\vue\vue.config.js 文件里的"target: 'http://localhost:6002'" 保持一致

### Lrd客户端-基于Ligg.EasyRPADesk的无码化桌面端(Winform和Console))

#### Lrd客户端运行(以OA为例子)
> 1. Visual Studio里，IISExpress运行WepApi项目， 此时Webapi端口为6002； 务必与\src\lrd\debug\Data\Apps\Oa\HttpClientBaseUrl.lstr的"http://localhost:6002"保持一致
> 2. 在启动程序位置，双击\src\lrd\debug\oa.exe运行；oa.exe有一个oa.ini文件指明了Oa的执行程序和配置路径; 这2文件必须与Conf、Program、Data文件夹处于同级的位置
> 3. 打开菜单"行政人事-考勤管理" 进入主界面，点击![structure](https://liggin2019.github.io/static/images/lrd/lrd-icon-calc.png)跳转到控制台程序
> 4. 运行其他的启动程序， 在各个z-started文件夹取出.exe和.ini文件至当前位置，双击运行
##### 启动程序位置
![structure](https://liggin2019.github.io/static/images/lrd/lrd-dir.png)

#### Lrd客户端的无码化编程代码调试，见[《Ligg.EasyRPADesk说明》](https://www.github.com/Ligg.EasyRPADesk) [《Ligg.EasyRPADesk文档》](https://liggin2019.github.io/docs/)


## 项目结构 
![structure](https://liggin2019.github.io/static/images/uwa/uwa-structure.png)

## 客户端界面
### Mvc客户端
#### MvcAdmin客户端</br>
![structure](https://liggin2019.github.io/static/images/uwa/uwa-mvc-admin.png)
####  MvcSite客户端(无码化客户端)-PC端</br>
![structure](https://liggin2019.github.io/static/images/uwa/uwa-mvc-site.png)
####   MvcSite客户端(无码化客户端)-手机端</br>
![structure](https://liggin2019.github.io/static/images/uwa/uwa-mvc-site-phone.png)

### Vue客户端
#### vue客户端-登录</br>
![structure](https://liggin2019.github.io/static/images/uwa/uwa-vue-logon.png)</br>
#### vue客户端-主界面</br>
![structure](https://liggin2019.github.io/static/images/uwa/uwa-vue.png)
### LRD客户端
#### LRD客户端-控制台界面</br>
![structure](https://liggin2019.github.io/static/images/lrd/lrd-console-att-calc.png)</br>
#### LRD客户端-Winform界面-登录</br>
![structure](https://liggin2019.github.io/static/images/lrd/lrd-logon.png)</br>
#### LRD客户端-Winform界面-Nested菜单</br>
![structure](https://liggin2019.github.io/static/images/lrd/lrd-oa.png)
#### LRD客户端-Winform界面-水平垂直菜单</br>
![structure](https://liggin2019.github.io/static/images/lrd/lrd-erp.png)

## 致谢
> 向本文提到的或本框架引用的开源项目作者和团队致谢，对你们高超的技术水平和分享精神点赞！




