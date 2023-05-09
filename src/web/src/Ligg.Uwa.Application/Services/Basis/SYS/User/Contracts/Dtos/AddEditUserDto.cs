using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;

using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    public class AddEditUserDto
    {
        public AddEditUserDto()
        {
            Email = "@"; 

        }

        public string Id { get; set; }

        [Description("账号")]//为了excel import/export匹配字段
        public string Account { get; set; }

        [Description("密码")]
        public string Password { get; set; }


        [Description("姓名")]
        public string Name { get; set; }

        [Description("邮件")]
        public string Email { get; set; }

        [Description("手机号")]
        public string Mobile { get; set; }

        [Description("微信")]
        public string WeChat { get; set; }

        [Description("QQ")]
        public string Qq { get; set; }

        [Description("性别")]  
        public int? Gender { get; set; }

        [Description("生日")]
        public string Birthday { get; set; }

        [Description("备注")]
        public string Description { get; set; }

        public int Status { get; set; }

        public string MasterId { get; set; }
        public string OrganizationPath { get; set; }

    }


}
