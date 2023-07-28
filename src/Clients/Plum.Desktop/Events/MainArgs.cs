namespace Plum.Events
{
    public class SignUpArgs
    {
        public string SessionId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VerificationCode { get; set; }
    }

    public class SignInArgs
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}