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

    public partial class InviteStatu
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public virtual System.Collections.Generic.ICollection<TeamMemberInvite> TeamMemberInvites { get; set; }

        public InviteStatu()
        {
            TeamMemberInvites = new System.Collections.Generic.List<TeamMemberInvite>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
