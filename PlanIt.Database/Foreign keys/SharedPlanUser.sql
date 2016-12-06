if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_Plan') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_Plan foreign key (PlanId)
	references dbo.[Plan] (ID)

if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_SharingStatus') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_SharingStatus foreign key (SharingStatusId)
	references dbo.[SharingStatus] (ID)

if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_UserOwner') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_UserOwner foreign key (UserOwnerId)
	references dbo.[User] (ID)

if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_UserReceiver') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_UserReceiver foreign key (UserReceiverId)
	references dbo.[User] (ID)