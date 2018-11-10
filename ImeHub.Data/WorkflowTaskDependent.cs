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

    // WorkflowTaskDependent
    public partial class WorkflowTaskDependent
    {
        public System.Guid ParentId { get; set; } // ParentId (Primary key)
        public System.Guid ChildId { get; set; } // ChildId (Primary key)

        // Foreign keys

        /// <summary>
        /// Parent WorkflowTask pointed by [WorkflowTaskDependent].([ChildId]) (FK_WorkflowTaskDependent_Dependent)
        /// </summary>
        public virtual WorkflowTask Child { get; set; } // FK_WorkflowTaskDependent_Dependent

        /// <summary>
        /// Parent WorkflowTask pointed by [WorkflowTaskDependent].([ParentId]) (FK_WorkflowTaskDependent_WorkflowTask)
        /// </summary>
        public virtual WorkflowTask Parent { get; set; } // FK_WorkflowTaskDependent_WorkflowTask

        public WorkflowTaskDependent()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
