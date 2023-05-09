using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{

    public enum ConfigIndex
    {
        [Description("租户配置")]//100 0*1000+0*1000+999
        TntConfig = 0,
        [Description("开发配置")]//1000 1*1000+1*1000+999
        DevConfig = 1,
        [Description("运维配置")]//2000 2*1000+2*1000+999
        OrpConfig = 2,
        [Description("客制化配置")]//3000 3*1000+3*1000+999
        CustConfig = 3,
        [Description("个人配置")]//4000 4*1000+4*1000+999
        IndConfig = 4,

        [Description("业务配置")]//10000 10*1000+90*1000+9999
        FuncConfig = 10,
    }

    public enum TntConfigType
    {
        [Description("租户配置1")]
        TntConfig = 100,
        [Description("租户配置2")]
        TntConfig1 = 200,
        [Description("租户配置9")]
        TntConfig2 = 900,
    }
    public enum TntConfigSubType
    {
        //系统参数
        [Description("租户配置10")]
        TntSubConfig01 = 100,
        [Description("租户配置11")]
        TntSubConfig02 = 101,
        [Description("租户配置21")]
        TntSubConfig11 = 201,
        [Description("租户配置22")]
        TntSubConfig12 = 202,
        [Description("租户配置90")]
        TntSubConfig20 = 900,
    }

    public enum DevConfigType
    {
        [Description("基础配置")]
        DevBasis = 1000,
        [Description("权限配置")]
        BackEnd = 1100,
        [Description("文本配置")]
        Text = 1200,

        [Description("前端配置")]
        FrontEnd = 1600,
    }
    public enum DevConfigSubType
    {
        //后端基础配置 1000
        [Description("系统配置定义")]
        SysConfigDefinition = 1000,
        [Description("定制配置定义")]
        CustConfigDefinition = 1001,

        [Description("数据库")]
        Database = 1010,
        [Description("缓存")]
        Cache = 1011,
        [Description("第三方接口")]
        ThirdPartInterface = 1012,

        [Description("国家或地区")]
        Culture = 1020,

        //权限配置 1100
        [Description("控制器")]
        Controller = 1100,
        [Description("路由")]
        Action = 1101,
        [Description("Mvc管理页面按钮")]
        MvcAdminPageButton = 1102,
        [Description("客户端视图")]
        ClientView = 1110,
        [Description("客户端视图按钮")]
        ClientViewButton = 1111,

        //文本配置 1200
        [Description("系统枚举")]
        SystematicEnum = 1200,
        [Description("系统枚举项")]
        SystematicEnumItem = 1201,

        [Description("通用文本")]
        CommonText = 1210,

        [Description("错误提示")]
        Error = 1211,

        [Description("词汇表")]
        Vocabulary = 1220,
        [Description("句子")]
        Sentence = 1221,


        //前端配置
        [Description("页面布局组件")]
        LayoutViewComponent = 1610,
        [Description("页面视图组件")]
        PageViewComponent = 1611,
        [Description("页面局部组件")]
        PagePartialComponent = 1612,//not support by core 3.1

    }

    public enum OrpConfigType
    {
        [Description("基础配置")]
        OrpBasis = 2000,
        [Description("系统参数")]
        SysParam = 2100,
        [Description("运行参数")]
        RunningParam = 2200,
        [Description("权限配置")]
        Authorization = 2300,
        //[Description("数据源")]
        //DataSource = 2400,
        [Description("前端配置")]
        FrontEnd = 2600,
    }

    public enum OrpConfigSubType
    {
        [Description("数据库")]
        Database = 2010,
        [Description("缓存")]
        Cache = 2011,
        [Description("第三方接口")]
        ThirdPartInterface = 2012,

        [Description("语言")]
        Language = 2020,

        //系统参数
        [Description("操作日志策略")]
        ActionLogPolicy = 2100,
        [Description("登录日志策略")]
        EntryLogPolicy = 2101,
        [Description("审核日志策略")]
        AuditLogPolicy = 2103,

        //运行参数
        [Description("上传路径")]
        UploadLocation = 2230,
        [Description("网盘文件上传路径")]
        FtpFileUploadLocation = 2231,

        [Description("实体附件后缀格式")]
        EntityAttachmentSuffixes = 2232,

        [Description("权限等级")]
        AuthorizationLevel = 2300,

        [Description("配置责任人")]
        ConfigOwner = 2310,

        [Description("操作定义")]
        Operation = 2320,

        [Description("管理门户")]
        MvcAdminPortal = 2600,
        [Description("站点门户")]
        MvcSitePortal = 2601,
        [Description("portal布局组件")]
        PortalLayoutComponent = 2610,//footer, floatNav, adv

        [Description("菜单定义")]
        MenuDefinition = 2620,
        [Description("外链菜单项")]
        MenuItemOutlink = 2621,
    }



    public enum CustConfigType
    {
        [Description("文本数据源")]
        TextDataSource = 3010,
        [Description("配置文件数据源")]
        ConfigFileDataSource = 3011,
        [Description("Sql数据源")]
        SqlDataSource = 3012,

        [Description("数据格式")]
        DataFormatDefinition = 3020,

        [Description("页面布局")]
        PageLayout = 3100,
        [Description("页面")]
        Page = 3101,

        [Description("数据表单")]
        DataForm = 3200,
        [Description("流程表单")]
        WorkFlowForm = 3201,

        [Description("流程")]
        WorkFlow = 3300,
    }

    public enum IndConfigType
    {
        [Description("个人配置")]
        IndConfig = 4000,
        [Description("个人配置1")]
        IndConfig1 = 4100,
        [Description("个人配置2")]
        IndConfig2 = 4200,
    }
    public enum IndConfigSubType
    {
        //系统参数
        [Description("个人配置00")]
        IndSubConfig01 = 4000,
        [Description("个人配置10")]
        IndSubConfig10 = 4100,
        [Description("租户配置11")]
        IndSubConfig11 = 4101,
        [Description("租户配置20")]
        IndSubConfig20 = 4200,
    }

    public enum Module
    {
        [Description("系统管理")]
        SYS = 10000,
        [Description("共享沟通协作")]
        SCC = 11000, //Sharing, communication and coorporation
        [Description("人力资源")]
        HRM = 20000, //HR management
        [Description("财务管理")]
        FIN = 30000, //Finance
        [Description("研发管理")]
        RAD = 40000, //Research and development
        [Description("生产管理")]
        PRD = 50000, //production
        [Description("销售管理")]
        SAM = 60000, //Sales and marketing
        [Description("质量管理")]
        QLT = 70000, //Qality

    }
    public enum FuncConfigType
    {   //SCC
        [Description("共享")]
        Sharing = 11100,
        [Description("沟通")]
        Communication = 11200,
        [Description("协作")]
        Coorporation = 11300,

        //HRM
        [Description("行政组织")]
        HrmOrganization = 20000,
        [Description("行政事务")]
        AdminDuty = 20100,
        [Description("薪酬")]
        Wage = 20200,
        [Description("绩效")]
        Performance = 20300,
        [Description("培训")]
        Traning = 20400,
        [Description("职业发展")]
        Devolopment = 20500,
        [Description("内部沟通")]
        InternalCommunication = 20700,
        [Description("公共关系")]
        PublicRelation = 20800,
        [Description("企业文化")]
        Culture = 20900,


        //FIN
        [Description("财务组织")]
        FinOrganization = 30000,
        [Description("财务账务")]
        Accounting = 30100,
        [Description("财务成本")]
        Costing = 30200,
        [Description("信用控制")]
        CreditControl = 30300,
        [Description("固定资产")]
        Asset = 30400,
        [Description("融资")]
        Capital = 30500,
        [Description("投资")]
        Investment = 30600,

        //PRD
        //PrdOrganization=14000, Material=14100, Planning=14200, Procurement=14300, Warehouse=14400, Manufacture Execution=14500
        [Description("生产组织")]
        PrdOrganization = 50000,
        [Description("物料")]
        Material = 50100,
        [Description("生产计划")]
        planning = 50200,
        [Description("采购")]
        Procurement = 14300,
        [Description("仓储")]
        Warehouse = 50400,
        [Description("生产作业")]
        Execution = 50500,

    }
    public enum FuncConfigSubType
    {
        //Sharing=11100, Communication=11200, Coorporation=11300
        //SCC>Sharing=11100
        [Description("项目类型")]
        ProjectType = 11300,

        //Orgnization = 20000, AdminDuty=20100, Salary=20100, Wage=20200, Performance=20300, Devolopment=20500,
        //InternalCommunication=20600, InternalCommunication=20700, PublicRelation=20800, Culture=20900
        [Description("行政组织类型")]
        AdminOrgType = 20000,
        [Description("级别")]
        EmployeeLevel = 20010,
        [Description("职位")]
        EmployeeTitle = 20011,
        [Description("岗位")]
        EmployeePosition = 20012,

        //FIN>FinOrganization=30000, Accounting=30100, Costing=30200, CreditControl=30300, Asset=30400, Investment=30500
        [Description("公司(法人实体)")]
        Company = 30000,
        [Description("账务控制范围(账套)")]
        AccountingScope = 30010,
        [Description("成本控制范围")]
        CostingingScope = 30011,
        [Description("信用控制范围")]
        CreditControlScope = 30012,

        [Description("账务控制策略")]
        AccountingPolicy = 30100,

        [Description("成本控制策略")]
        CostingingPolicy = 30200,

        [Description(">信用控制策略")]
        CreditControlPolicy = 30300,

        [Description("固定资产分类")]
        AssetType = 30401,
        [Description("固定资产折旧分类")]
        DepreciationType = 30402,

        //FIN>Capital = 30500
        [Description("融资分类")]
        CapitalType = 30501,
        //FIN>Investment = 30600
        [Description("投资分类")]
        InvestmentType = 30601,

        //PRD>PrdOrganization=50000, Material=50100, Planning=50200, Procurement=50300, Warehouse=50400, Manufacture Execution=50500
        [Description("工厂")]
        Plant = 50000,
        [Description("生产作业单元")]
        WorkUnit = 50001,

        [Description("物料类型")]
        MaterialType = 50100,
        [Description("物料组")]
        MaterialGroup = 50101,
        [Description("物料基本单位")]
        BasicUnit = 50110,

        [Description("MPS策略")]
        MPSPolicy = 50200,
        [Description("Mrp类型")]
        MrpType = 50201,

        [Description("仓库类型")]
        WareHouseType = 50400,
        [Description("仓库")]
        Warehouse = 50401,
        [Description("库位")]
        WarehouseBin = 50402,
        [Description("移动类型")]
        MovementType = 50410,
        //SAM>Customer=60100,  Marketing=60200, Sales=60300,
    }




}
