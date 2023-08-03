namespace netzwelt_exam.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Session = new LoginSessionModel();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public LoginSessionModel Session { get; set; }
    }
}
