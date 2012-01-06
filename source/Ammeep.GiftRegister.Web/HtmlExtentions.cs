using System.Web;
using System.Web.Mvc;

namespace Ammeep.GiftRegister.Web
{
    public static class HtmlExtensions
    {
        public static IHtmlString JavaScriptEncode(this HtmlHelper html, string item)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(item));
        }
    }
}