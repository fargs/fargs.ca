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

    // Workflow
    public partial class Workflow
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public System.Guid? PhysicianId { get; set; } // PhysicianId

        // Reverse navigation

        /// <summary>
        /// Child WorkItems where [WorkItem].[WorkflowId] point to this entity (FK_WorkItem_Workflow)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<WorkItem> WorkItems { get; set; } // WorkItem.FK_WorkItem_Workflow

        public Workflow()
        {
            WorkItems = new System.Collections.Generic.List<WorkItem>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
