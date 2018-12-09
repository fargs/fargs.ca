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

    // Role
    public partial class Role
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 256)
        public string Code { get; set; } // Code (length: 10)
        public string ColorCode { get; set; } // ColorCode (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child RoleFeatures where [RoleFeature].[RoleId] point to this entity (FK_RoleFeature_Role)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RoleFeature> RoleFeatures { get; set; } // RoleFeature.FK_RoleFeature_Role
        /// <summary>
        /// Child TeamMembers where [TeamMember].[RoleId] point to this entity (FK_TeamMember_Role)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<TeamMember> TeamMembers { get; set; } // TeamMember.FK_TeamMember_Role
        /// <summary>
        /// Child Users where [User].[RoleId] point to this entity (FK_User_Role)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<User> Users { get; set; } // User.FK_User_Role
        /// <summary>
        /// Child UserRoles where [UserRole].[RoleId] point to this entity (FK_UserRole_Role)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<UserRole> UserRoles { get; set; } // UserRole.FK_UserRole_Role

        public Role()
        {
            RoleFeatures = new System.Collections.Generic.List<RoleFeature>();
            TeamMembers = new System.Collections.Generic.List<TeamMember>();
            Users = new System.Collections.Generic.List<User>();
            UserRoles = new System.Collections.Generic.List<UserRole>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>