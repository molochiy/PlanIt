SET IDENTITY_INSERT [SharedPlanItemUser] ON;

MERGE INTO [SharedPlanItemUser] AS TARGET
	USING (
	VALUES
		(1, 1, DATEADD(DAY,  7, GETDATE()), 1, 2),
		(2, 5, DATEADD(DAY,  6, GETDATE()), 2, 3),
		(3, 5, DATEADD(DAY,  9, GETDATE()), 3, 2),
		(4, 13, DATEADD(DAY,  14, GETDATE()), 2, 3),
		(5, 19, DATEADD(DAY,  9, GETDATE()), 1, 1))
	AS SOURCE ([Id], [PlanItemId], [SharingDateTime], [SharingStatusId], [UserId])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [PlanItemId] = SOURCE.[PlanItemId],
					   [SharingDateTime] = SOURCE.[SharingDateTime],
					   [SharingStatusId] = SOURCE.[SharingStatusId],
					   [UserId] = SOURCE.[UserId]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [PlanItemId], [SharingDateTime], [SharingStatusId], [UserId])
			VALUES ([Id], [PlanItemId], [SharingDateTime], [SharingStatusId], [UserId])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [SharedPlanItemUser] OFF;