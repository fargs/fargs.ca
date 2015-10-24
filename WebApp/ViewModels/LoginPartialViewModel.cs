namespace WebApp.ViewModels
{
    public class LoginPartialViewModel
    {
        public LoginPartialViewModel()
        {
        }

        public bool IsAuthenticated { get; set; }
        public string UserDisplayName { get; set; }
        public string ProfilePicture { get; set; }
    }
}