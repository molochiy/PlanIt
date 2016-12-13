SET IDENTITY_INSERT [Comment] ON;

MERGE INTO [Comment] AS TARGET
	USING (
	VALUES
		(1, 1, null, 'asdasda', DATEADD(DAY,  6, GETDATE()), 0, 1),
		(2, 1, null, 'asfasfasfasf', DATEADD(DAY,  9, GETDATE()), 0, 1),
		(3, 2, null, 'qazcz', DATEADD(DAY,  5, GETDATE()), 0, 1),
		(4, null, 1, 'sdgsfasfqw', DATEADD(DAY,  8, GETDATE()), 0, 1),
		(5, null, 2, 'brereber', DATEADD(DAY,  10, GETDATE()), 1, 1),
		(6, 3, null, 'asdasda', DATEADD(DAY,  9, GETDATE()), 1, 1),
		(7, 4, null, 'asfasfasfasf', DATEADD(DAY,  8, GETDATE()), 0, 1),
		(8, null, 2, 'qazcz', DATEADD(DAY,  10, GETDATE()), 0, 1),
		(9, null, 5, 'sdgsfasfqw', DATEADD(DAY,  4, GETDATE()), 0, 1),
		(10, null, 5, 'brereber', DATEADD(DAY,  6, GETDATE()), 1, 1))
	AS SOURCE ([Id], [PlanId], [PlanItemId], [Text], [CreatedTime], [IsDeleted], [UserId])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [PlanId] = SOURCE.[PlanId],
					   [PlanItemId] = SOURCE.[PlanItemId],
					   [Text] = SOURCE.[Text],
					   [CreatedTime] = SOURCE.[CreatedTime],
					   [IsDeleted] = SOURCE.[IsDeleted],
					   [UserId] = SOURCE.[UserId]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [PlanId], [PlanItemId], [Text], [CreatedTime], [IsDeleted], [UserId])
			VALUES ([Id], [PlanId], [PlanItemId], [Text], [CreatedTime], [IsDeleted], [UserId])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [Comment] OFF;