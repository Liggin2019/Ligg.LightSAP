
using  System.ComponentModel;
namespace Ligg.Infrastructure.DataModels
{
    public enum FileType
    {
        [Description("未知格式文件")]
        Undefined = 0,
        [Description("文本文件")]
        Text =1,
        [Description("图片文件")]
        Image = 2,
        [Description("音频文件")]
        Sound = 3,
        [Description("视频文件")]
        Vedio = 4,
        [Description("可执行文件")]
        Executable = 5,
        
    }
}
