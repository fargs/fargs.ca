namespace WebApp.Areas.Dashboard.Views.Home
{
    public interface ILinkedProfileViewModel
    {
        string Email { get; set; }
        string Name { get; set; }
    }
    public class GoogleProfileViewModel : ILinkedProfileViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}