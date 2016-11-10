CREATE TABLE [dbo].[User]
(
	[Id] [INT] IDENTITY(1, 1) NOT NULL,
	[Email] [VARCHAR](50) NOT NULL,
	[IsEmailConfirmed] [BIT] NOT NULL,
	[Password] [VARCHAR](MAX) NOT NULL,
    [ProfileId] INT NOT NULL, 
    constraint PK_User primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]