CREATE TABLE [dbo].[Profile]
(
	[Id] INT identity(1,1) NOT NULL,
	[FirstName] [NVARCHAR](50) NULL,
	[LastName] [NVARCHAR](50) NULL,
	[Phone] [VARCHAR](20) NULL,
	constraint PK_Profile primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]		
