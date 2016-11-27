SET IDENTITY_INSERT [User] ON;

MERGE INTO [User] AS TARGET
	USING (
	VALUES
		(1, 'email1@gmail.com', 0, 'pass1', 1),
		(2, 'email2@gmail.com', 0, 'pass2', 2),
		(3, 'email3@gmail.com', 1, 'pass3', 3),
		(4, 'email4@gmail.com', 1, 'pass4', 4),
		(5, 'email5@gmail.com', 1, 'pass5', 5))
	AS SOURCE ([Id], [Email], [IsEmailConfirmed], [Password], [ProfileId])
	ON TARGET.[Id] = SOURCE.[Id]
		WHEN MATCHED THEN
			update set [Email] = SOURCE.[Email],
					   [IsEmailConfirmed] = SOURCE.[IsEmailConfirmed],
					   [Password] = SOURCE.[Password],
					   [ProfileId] = SOURCE.[ProfileId]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([Id], [Email], [IsEmailConfirmed], [Password], [ProfileId])
			VALUES ([Id], [Email], [IsEmailConfirmed], [Password], [ProfileId])
		WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT [User] OFF;