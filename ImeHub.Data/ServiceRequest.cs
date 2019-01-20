// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.7
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace ImeHub.Data
{

    public partial class ServiceRequest
    {
        public System.Guid Id { get; set; }
        public System.Guid PhysicianId { get; set; }
        public string CaseNumber { get; set; }
        public string AlternateKey { get; set; }
        public string ClaimantName { get; set; }
        public string Title { get; set; }
        public System.DateTime? RequestedDate { get; set; }
        public System.Guid? RequestedBy { get; set; }
        public byte StatusId { get; set; }
        public System.Guid? StatusChangedById { get; set; }
        public System.DateTime? StatusChangedDate { get; set; }
        public System.Guid ServiceId { get; set; }
        public string FolderUrl { get; set; }
        public System.DateTime? DueDate { get; set; }
        public System.Guid? AvailableSlotId { get; set; }
        public System.DateTime? AppointmentDate { get; set; }
        public System.TimeSpan? StartTime { get; set; }
        public System.TimeSpan? EndTime { get; set; }
        public System.Guid? AddressId { get; set; }
        public byte CancellationStatusId { get; set; }
        public System.DateTime? CancellationStatusChangedDate { get; set; }
        public System.Guid? CancellationStatusChangedById { get; set; }
        public bool HasErrors { get; set; }
        public bool HasWarnings { get; set; }
        public byte? MedicolegalTypeId { get; set; }
        public string ReferralSource { get; set; }


        public virtual Address Address { get; set; }

        public virtual AvailableSlot AvailableSlot { get; set; }

        public virtual CancellationStatu CancellationStatu { get; set; }

        public virtual MedicolegalType MedicolegalType { get; set; }

        public virtual Physician Physician { get; set; }

        public virtual Service Service { get; set; }

        public virtual ServiceRequestStatu ServiceRequestStatu { get; set; }

        public ServiceRequest()
        {
            HasErrors = false;
            HasWarnings = false;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
