if object_id(N'dbo.Comment', N'U') is not null and  object_id (N'FK_Comment_Plan') is null
	alter table dbo.[Comment] with check add constraint FK_Comment_Plan foreign key (PlanID)
	references dbo.[Plan] (ID)

if object_id(N'dbo.Comment', N'U') is not null and  object_id (N'FK_Comment_PlanItem') is null
	alter table dbo.[Comment] with check add constraint FK_Comment_PlanItem foreign key (PlanItemId)
	references dbo.[PlanItem] (Id)
