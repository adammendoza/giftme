namespace Ammeep.GiftRegister.Web.Attributes
{
    public class DataBindHtmlAttribute
    {
        public DataBindHtmlAttribute(string className)
        {
            Data_Bind = className;
        }

        public string Data_Bind { get; set; }
    }

}