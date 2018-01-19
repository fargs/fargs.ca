namespace WebApp.FormModels
{
    public class ChangeClaimantForm
    {
        public ChangeClaimantForm()
        {
        }

        public string ClaimantName { get; set; }
        
        public int ServiceRequestId { get; set; }
    }
}