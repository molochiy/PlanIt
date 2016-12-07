SET IDENTITY_INSERT [SharingStatus] ON;

MERGE INTO [SharingStatus] AS TARGET
	USING (
	VALUES
		(1, 'Accepted'),
		(2, 'Pending'),
		(3, 'Declined'),
		(4, 'Notified'))
	AS SOURCE ([Id], [Name])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [Name] = SOURCE.[Name]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [Name])
			VALUES ([Id], [Name])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [SharingStatus] OFF;