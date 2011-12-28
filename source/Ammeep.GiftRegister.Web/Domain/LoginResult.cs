namespace Ammeep.GiftRegister.Web.Domain
{
    public class LoginResult
    {
        public LoginResult(bool isSuccessful)
        {
            SuccessfulLogin = isSuccessful;
        }

        public bool SuccessfulLogin { get; private set; }

        public string Errors { get; private set; }
    }
}