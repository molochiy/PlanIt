if object_id(N'dbo.PlanItem', N'U') is not null and  object_id (N'FK_PlanItem_Plan') is null
	alter table dbo.[PlanItem] with check add constraint FK_PlanItem_Plan foreign key (PlanID)
	references dbo.[Plan] (ID)

if object_id(N'dbo.PlanItem', N'U') is not null and  object_id (N'FK_PlanItem_Status') is null
	alter table dbo.[PlanItem] with check add constraint FK_PlanItem_Status foreign key (StatusId)
	references dbo.[Status] (ID)

if object_id(N'dbo.PlanItem', N'U') is not null and  object_id (N'FK_PlanItem_Image') is null
	alter table dbo.[PlanItem] with check add constraint FK_PlanItem_Image foreign key (ImageId)
	references dbo.[Image] (ID)