using System.ComponentModel;

namespace Ligg.Uwa.Basis.SCC
{
    public enum ArticleType
    {
        [Description("富文本")]
        RichText = 0,
        [Description("Html文本")]
        HtmlText = 1,
        [Description("Markdown")]
        MarkdownText = 2,
        [Description("外链")]
        OutLink = 3,
        [Description("文章快捷方式")]
        ArticleShortcut = 4,
        [Description("文件快捷方式")]
        HttpFileShortcut = 5,



    }
}
