CREATE TABLE [dbo].[SharedPlanItemUser]
(
	[Id] INT identity(1,1) NOT NULL, 
    [PlanItemId] INT NOT NULL, 
    [SharingDateTime] DATETIME NOT NULL, 
    [SharingStatusId] INT NOT NULL,
	[UserOwnerId] INT NOT NULL,
	[UserReceiverId] INT NOT NULL,
	constraint PK_SharedPlanItemUser primary key clustered 
	(
		Id asc, UserOwnerId, UserReceiverId
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
