CREATE TABLE [dbo].[SharingStatus]
(
	[Id] INT identity(1,1) NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL,
	constraint PK_SharingStatus primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
