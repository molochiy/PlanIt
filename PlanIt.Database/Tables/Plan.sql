CREATE TABLE [dbo].[Plan]
(
	[Id] INT identity(1,1) NOT NULL, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Begin] DATETIME2 NULL, 
    [End] DATETIME2 NULL, 
    [StatusId] INT NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0,
	[UserId] INT NOT NULL,
	constraint PK_Plan primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
