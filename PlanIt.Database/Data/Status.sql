SET IDENTITY_INSERT [Status] ON;

MERGE INTO [Status] AS TARGET
	USING (
	VALUES
		(1, 'Not started'),
		(2, 'In progress'),
		(3, 'Done'))
	AS SOURCE ([Id], [Name])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [Name] = SOURCE.[Name]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [Name])
			VALUES ([Id], [Name])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [Status] OFF;