namespace Fargs.Portal.Services.Accounting
{
    public class AccountingProviderOptions
    {
        public const string SectionName = "AccountingProviders";
        public QuickbooksOptions QuickbooksOptions { get; set; }
    }
    public class QuickbooksOptions
    {
        public const string SectionName = "AccountingProviders:Quickbooks";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Environment { get; set; }
    }
}