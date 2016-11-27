if object_id(N'dbo.User', N'U') is not null and  object_id (N'FK_User_Image') is null
	alter table dbo.[User] with check add constraint FK_User_Image foreign key (ProfileId)
	references dbo.[Profile] (ID)