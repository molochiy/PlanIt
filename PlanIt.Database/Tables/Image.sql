CREATE TABLE [dbo].[Image]
(
	[Id] INT identity(1,1) NOT NULL, 
    [Image] IMAGE NOT NULL
	constraint PK_Image primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
