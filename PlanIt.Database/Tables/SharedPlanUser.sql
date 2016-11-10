CREATE TABLE [dbo].[SharedPlanUser]
(
	[Id] INT identity(1,1) NOT NULL, 
    [PlanId] INT NOT NULL,  
    [SharingDateTime] DATETIME NOT NULL, 
    [SharingStatusId] INT NOT NULL,
	[UserId] INT NOT NULL,
	constraint PK_SharedPlanUser primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
