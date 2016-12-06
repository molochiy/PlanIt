if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_PlanItem') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUserr_PlanItem foreign key (PlanItemId)
	references dbo.[PlanItem] (ID)

if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_SharingStatus') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUser_SharingStatus foreign key (SharingStatusId)
	references dbo.[SharingStatus] (ID)

if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_UserOwner') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUser_UserOwner foreign key (UserOwnerId)
	references dbo.[User] (ID)

if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_UserReceiver') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUser_UserReceiver foreign key (UserReceiverId)
	references dbo.[User] (ID)