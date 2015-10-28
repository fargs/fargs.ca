namespace WebApp.Controllers
{
    public class SpecialRequestFormViewModel
    {
        public string PhysicianId { get; set; }
        public short ServiceId { get; set; }
        public short CompanyId { get; set; }
        public string Timeframe { get; set; }
        public string AdditionalNotes { get; set; }
    }
}