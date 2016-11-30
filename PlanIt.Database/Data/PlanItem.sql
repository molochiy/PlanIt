SET IDENTITY_INSERT [PlanItem] ON;

MERGE INTO [PlanItem] AS TARGET
	USING (
	VALUES
		(1, 1,'Title1', 'Description1', DATEADD(DAY,  5, GETDATE()), DATEADD(DAY,  6, GETDATE()), 1, 0, NULL),
		(2, 1,'Title2', 'Description2', DATEADD(DAY,  6, GETDATE()), DATEADD(DAY,  7, GETDATE()), 1, 0, NULL),
		(3, 1,'Title3', 'Description3', DATEADD(DAY,  5, GETDATE()), DATEADD(DAY,  10, GETDATE()), 1, 1, NULL),
		(4, 1,'Title4', 'Description4', DATEADD(DAY,  8, GETDATE()), DATEADD(DAY,  9, GETDATE()), 2, 0, NULL),
		(5, 2,'Title5', 'Description5', DATEADD(DAY,  1, GETDATE()), DATEADD(DAY,  10, GETDATE()), 2, 0, NULL),
		(6, 2,'Title6', 'Description6', DATEADD(DAY,  7, GETDATE()), DATEADD(DAY,  8, GETDATE()), 3, 0, NULL),
		(7, 3,'Title7', 'Description7', DATEADD(DAY,  10, GETDATE()), DATEADD(DAY,  12, GETDATE()), 3, 1, NULL),
		(8, 3,'Title8', 'Description8', DATEADD(DAY,  11, GETDATE()), DATEADD(DAY,  12, GETDATE()), 1, 0, NULL),
		(9, 3,'Title9', 'Description9', DATEADD(DAY,  10, GETDATE()), DATEADD(DAY,  11, GETDATE()), 1, 0, NULL),
		(10, 4,'Title10', 'Description10', DATEADD(DAY,  5, GETDATE()), DATEADD(DAY,  7, GETDATE()), 1, 1, NULL),
		(11, 5,'Title11', 'Description11', DATEADD(DAY,  15, GETDATE()), DATEADD(DAY,  17, GETDATE()), 2, 0, NULL),
		(12, 4,'Title12', 'Description12', DATEADD(DAY,  6, GETDATE()), DATEADD(DAY,  9, GETDATE()), 2, 0, NULL),
		(13, 5,'Title13', 'Description13', DATEADD(DAY,  13, GETDATE()), DATEADD(DAY,  14, GETDATE()), 3, 0, NULL),
		(14, 5,'Title14', 'Description14', DATEADD(DAY,  14, GETDATE()), DATEADD(DAY,  16, GETDATE()), 3, 1, NULL),
		(15, 1,'Title15', 'Description15', DATEADD(DAY,  5, GETDATE()), DATEADD(DAY,  7, GETDATE()), 1, 0, NULL),
		(16, 1,'Title16', 'Description16', DATEADD(DAY,  6, GETDATE()), DATEADD(DAY,  9, GETDATE()), 1, 0, NULL),
		(17, 6,'Title17', 'Description17', DATEADD(DAY,  6, GETDATE()), DATEADD(DAY,  8, GETDATE()), 1, 1, NULL),
		(18, 6,'Title18', 'Description18', DATEADD(DAY,  8, GETDATE()), DATEADD(DAY,  9, GETDATE()), 2, 0, NULL),
		(19, 6,'Title19', 'Description19', DATEADD(DAY,  9, GETDATE()), DATEADD(DAY,  10, GETDATE()), 2, 0, NULL),
		(20, 7,'Title20', 'Description20', DATEADD(DAY,  8, GETDATE()), DATEADD(DAY,  11, GETDATE()), 3, 0, NULL),
		(21, 7,'Title21', 'Description21', DATEADD(DAY,  10, GETDATE()), DATEADD(DAY,  12, GETDATE()), 3, 1, NULL))
	AS SOURCE ([Id], [PlanId], [Title], [Description], [Begin], [End], [StatusId], [IsDeleted], [ImageId])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [Title] = SOURCE.[Title],
					   [PlanId] = SOURCE.[PlanId],
					   [Description] = SOURCE.[Description],
					   [Begin] = SOURCE.[Begin],
					   [End] = SOURCE.[End],
					   [StatusId] = SOURCE.[StatusId],
					   [IsDeleted] = SOURCE.[IsDeleted],
					   [ImageId] = SOURCE.[ImageId]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [PlanId], [Title], [Description], [Begin], [End], [StatusId], [IsDeleted], [ImageId])
			VALUES ([Id], [PlanId], [Title], [Description], [Begin], [End], [StatusId], [IsDeleted], [ImageId])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [PlanItem] OFF;