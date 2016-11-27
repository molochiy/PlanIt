SET IDENTITY_INSERT [Profile] ON;

MERGE INTO [Profile] AS TARGET
	USING (
	VALUES
		(1, 'FirstName1', 'LastName1', null),
		(2, 'FirstName2', 'LastName2', '5112'),
		(3, 'FirstName3', 'LastName3', null),
		(4, 'FirstName4', 'LastName4', '4341'),
		(5, 'FirstName5', 'LastName5', '5151'))
	AS SOURCE ([Id], [FirstName], [LastName], [Phone])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [FirstName] = SOURCE.[FirstName],
					   [LastName] = SOURCE.[LastName],
					   [Phone] = SOURCE.[Phone]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [FirstName], [LastName], [Phone])
			VALUES ([Id], [FirstName], [LastName], [Phone])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [Profile] OFF;