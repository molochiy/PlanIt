CREATE TABLE [dbo].[Comment]
(
	[Id] INT identity(1,1) NOT NULL, 
    [PlanId] INT NULL , 
    [PlanItemId] INT NULL, 
	[Text] NVARCHAR(MAX) NOT NULL, 
    [CreatedTime] DATETIME NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0,
	[UserId] INT NOT NULL,
	constraint PK_Comment primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
