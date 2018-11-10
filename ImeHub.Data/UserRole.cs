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

    // UserRole
    public partial class UserRole
    {
        public System.Guid UserId { get; set; } // UserId (Primary key)
        public System.Guid RoleId { get; set; } // RoleId (Primary key)

        // Foreign keys

        /// <summary>
        /// Parent Role pointed by [UserRole].([RoleId]) (FK_UserRole_Role)
        /// </summary>
        public virtual Role Role { get; set; } // FK_UserRole_Role

        /// <summary>
        /// Parent User pointed by [UserRole].([UserId]) (FK_UserRole_User)
        /// </summary>
        public virtual User User { get; set; } // FK_UserRole_User

        public UserRole()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
