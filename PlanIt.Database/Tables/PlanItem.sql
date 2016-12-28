CREATE TABLE [dbo].[PlanItem]
(
	[Id] INT identity(1,1) NOT NULL, 
	[PlanId] INT NOT NULL,
    [Title] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Begin] DATETIME NULL, 
    [End] DATETIME NULL, 
    [StatusId] INT NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0,
	[ImageId] INT NULL,
	[Weight] INT NOT NULL
	constraint PK_PlanItem primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
