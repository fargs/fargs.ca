CREATE TABLE [dbo].[ServiceRequestTaskDependent] (
    [ParentId] INT NOT NULL,
    [ChildId]  INT NOT NULL,
    CONSTRAINT [PK_ServiceRequestTaskDependent] PRIMARY KEY CLUSTERED ([ParentId] ASC, [ChildId] ASC),
    CONSTRAINT [FK_ServiceRequestTaskDependent_Dependent] FOREIGN KEY ([ChildId]) REFERENCES [dbo].[ServiceRequestTask] ([Id]),
    CONSTRAINT [FK_ServiceRequestTaskDependent_ServiceRequestTask] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[ServiceRequestTask] ([Id])
);





