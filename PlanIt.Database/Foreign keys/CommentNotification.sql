if object_id(N'dbo.CommentNotification', N'U') is not null and  object_id (N'FK_CommentNotification_Comment') is null
	alter table dbo.[CommentNotification] with check add constraint FK_CommentNotification_Comment foreign key (CommentId)
	references dbo.[Comment] (ID)

if object_id(N'dbo.CommentNotification', N'U') is not null and  object_id (N'FK_CommentNotification_Receiver') is null
	alter table dbo.[CommentNotification] with check add constraint FK_CommentNotification_Receiver foreign key (ReceiverId)
	references dbo.[User] (ID)