if object_id(N'dbo.Plan', N'U') is not null and  object_id (N'FK_Plan_Status') is null
	alter table dbo.[Plan] with check add constraint FK_Plan_Status foreign key (StatusId)
	references dbo.[Status] (ID)

if object_id(N'dbo.Plan', N'U') is not null and  object_id (N'FK_Plan_User') is null
	alter table dbo.[Plan] with check add constraint FK_Plan_User foreign key (UserId)
	references dbo.[User] (ID)
