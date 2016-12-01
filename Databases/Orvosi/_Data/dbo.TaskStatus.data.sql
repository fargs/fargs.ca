INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [DependentPrecedence], [ServiceRequestPrecedence]) VALUES (1, N'Waiting', 1, 2)
INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [DependentPrecedence], [ServiceRequestPrecedence]) VALUES (2, N'ToDo', 2, 1)
INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [DependentPrecedence], [ServiceRequestPrecedence]) VALUES (3, N'Done', 3, 4)
INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [DependentPrecedence], [ServiceRequestPrecedence]) VALUES (4, N'Obsolete', 4, 3)
