if exists (select 1 from dbo.[Plan] where Id in (1,2))
delete from dbo.[Plan] where Id in (1,2)
	
SET IDENTITY_INSERT dbo.[Plan] ON;

insert into dbo.[Plan](Id, Title, [Description], [Begin], [End], StatusId, IsDeleted, UserId)
select 1, 'short title', 'Simple Description', '2016-02-16', '2016-02-20', 1, 0, 1
union all
select 2, 'very very very huge and long title', 'Simple loooooooooooooooooong Description', '2016-03-16', '2016-04-20', 1, 0, 1

SET IDENTITY_INSERT dbo.[Plan] Off;
go