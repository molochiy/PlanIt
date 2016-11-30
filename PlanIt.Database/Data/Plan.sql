SET IDENTITY_INSERT [Plan] ON;

MERGE INTO [Plan] AS TARGET
	USING (
	VALUES
		(1, 'Title1', 'Description1', DATEADD(DAY,  5, GETDATE()), DATEADD(DAY,  10, GETDATE()), 1, 0, 1),
		(2, 'Title2', 'Description2', DATEADD(DAY,  1, GETDATE()), DATEADD(DAY,  10, GETDATE()), 1, 0, 1),
		(3, 'Title3', 'Description3', DATEADD(DAY,  10, GETDATE()), DATEADD(DAY,  12, GETDATE()), 1, 1, 1),
		(4, 'Title4', 'Description4', DATEADD(DAY,  5, GETDATE()), DATEADD(DAY,  9, GETDATE()), 2, 0, 2),
		(5, 'Title5', 'Description5', DATEADD(DAY,  13, GETDATE()), DATEADD(DAY,  17, GETDATE()), 2, 0, 2),
		(6, 'Title6', 'Description6', DATEADD(DAY,  6, GETDATE()), DATEADD(DAY,  10, GETDATE()), 3, 0, 3),
		(7, 'Title7', 'Description7', DATEADD(DAY,  8, GETDATE()), DATEADD(DAY,  12, GETDATE()), 3, 1, 3))
	AS SOURCE ([Id], [Title], [Description], [Begin], [End], [StatusId], [IsDeleted], [UserId])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [Title] = SOURCE.[Title],
					   [Description] = SOURCE.[Description],
					   [Begin] = SOURCE.[Begin],
					   [End] = SOURCE.[End],
					   [StatusId] = SOURCE.[StatusId],
					   [IsDeleted] = SOURCE.[IsDeleted],
					   [UserId] = SOURCE.[UserId]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [Title], [Description], [Begin], [End], [StatusId], [IsDeleted], [UserId])
			VALUES ([Id], [Title], [Description], [Begin], [End], [StatusId], [IsDeleted], [UserId])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [Plan] OFF;