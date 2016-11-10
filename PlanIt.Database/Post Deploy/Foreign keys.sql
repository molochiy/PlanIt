--Comment
if object_id(N'dbo.Comment', N'U') is not null and  object_id (N'FK_Comment_Plan') is null
	alter table dbo.[Comment] with check add constraint FK_Comment_Plan foreign key (PlanID)
	references dbo.[Plan] (ID)

if object_id(N'dbo.Comment', N'U') is not null and  object_id (N'FK_Comment_PlanItem') is null
	alter table dbo.[Comment] with check add constraint FK_Comment_PlanItem foreign key (PlanItemId)
	references dbo.[PlanItem] (Id)

--Plan
if object_id(N'dbo.Plan', N'U') is not null and  object_id (N'FK_Plan_Status') is null
	alter table dbo.[Plan] with check add constraint FK_Plan_Status foreign key (StatusId)
	references dbo.[Status] (ID)

if object_id(N'dbo.Plan', N'U') is not null and  object_id (N'FK_Plan_User') is null
	alter table dbo.[Plan] with check add constraint FK_Plan_User foreign key (UserId)
	references dbo.[User] (ID)

--PlanItem
if object_id(N'dbo.PlanItem', N'U') is not null and  object_id (N'FK_PlanItem_Plan') is null
	alter table dbo.[PlanItem] with check add constraint FK_PlanItem_Plan foreign key (PlanID)
	references dbo.[Plan] (ID)

if object_id(N'dbo.PlanItem', N'U') is not null and  object_id (N'FK_PlanItem_Status') is null
	alter table dbo.[PlanItem] with check add constraint FK_PlanItem_Status foreign key (StatusId)
	references dbo.[Status] (ID)

if object_id(N'dbo.PlanItem', N'U') is not null and  object_id (N'FK_PlanItem_Image') is null
	alter table dbo.[PlanItem] with check add constraint FK_PlanItem_Image foreign key (ImageId)
	references dbo.[Image] (ID)

--User
if object_id(N'dbo.User', N'U') is not null and  object_id (N'FK_User_Image') is null
	alter table dbo.[User] with check add constraint FK_User_Image foreign key (ProfileId)
	references dbo.[Profile] (ID)

--SharedPlanUser
if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_Plan') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_Plan foreign key (PlanId)
	references dbo.[Plan] (ID)

if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_SharingStatus') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_SharingStatus foreign key (SharingStatusId)
	references dbo.[SharingStatus] (ID)

if object_id(N'dbo.SharedPlanUser', N'U') is not null and  object_id (N'FK_SharedPlanUser_User') is null
	alter table dbo.[SharedPlanUser] with check add constraint FK_SharedPlanUser_User foreign key (UserId)
	references dbo.[User] (ID)

--SharedPlanItemUser
if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_PlanItem') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUserr_PlanItem foreign key (PlanItemId)
	references dbo.[PlanItem] (ID)

if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_SharingStatus') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUser_SharingStatus foreign key (SharingStatusId)
	references dbo.[SharingStatus] (ID)

if object_id(N'dbo.SharedPlanItemUser', N'U') is not null and  object_id (N'FK_SharedPlanItemUser_User') is null
	alter table dbo.[SharedPlanItemUser] with check add constraint FK_SharedPlanItemUser_User foreign key (UserId)
	references dbo.[User] (ID)