namespace WebApp.ViewModels
{
    public class DashboardViewModel
    {
        public string UserDisplayName { get; set; }
        public string UserCompanyDisplayName  { get; set; }
        public string UserRoleName { get; set; }
        public string UserCompanyLogoCssClass { get; set; }
        public string SchedulingProcess { get; set; }
        public string BookingPageName { get; set; }
        public string Instructions { get; set; }
        public DashboardViewModel()
        {
        }
    }
}