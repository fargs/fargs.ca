using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class SpecialRequestFormViewModel
    {
        [Required]
        public string PhysicianId { get; set; }
        [Required]
        public short ServiceId { get; set; }
        public short CompanyId { get; set; }
        [Required]
        public string Timeframe { get; set; }
        [Required]
        public string AdditionalNotes { get; set; }

        public byte ActionState { get; set; }
    }
}