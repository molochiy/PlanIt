CREATE TABLE [dbo].[CommentNotification]
(
	[Id] INT identity(1,1) NOT NULL, 
    [CommentId] INT NULL, 
    [ReceiverId] INT NULL, 
    [WasNotified] BIT NOT NULL DEFAULT 0,
	constraint PK_CommentNotification primary key clustered 
	(
		Id asc
	)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [primary]
	) on [primary]
