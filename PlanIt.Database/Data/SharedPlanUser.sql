SET IDENTITY_INSERT [SharedPlanUser] ON;

MERGE INTO [SharedPlanUser] AS TARGET
	USING (
	VALUES
		(1, 1, DATEADD(DAY,  6, GETDATE()), 1, 2),
		(2, 1, DATEADD(DAY,  8, GETDATE()), 2, 3),
		(3, 4, DATEADD(DAY,  7, GETDATE()), 3, 3),
		(4, 3, DATEADD(DAY,  10, GETDATE()), 2, 2),
		(5, 7, DATEADD(DAY,  9, GETDATE()), 1, 1))
	AS SOURCE ([Id], [PlanId], [SharingDateTime], [SharingStatusId], [UserId])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [PlanId] = SOURCE.[PlanId],
					   [SharingDateTime] = SOURCE.[SharingDateTime],
					   [SharingStatusId] = SOURCE.[SharingStatusId],
					   [UserId] = SOURCE.[UserId]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [PlanId], [SharingDateTime], [SharingStatusId], [UserId])
			VALUES ([Id], [PlanId], [SharingDateTime], [SharingStatusId], [UserId])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [SharedPlanUser] OFF;