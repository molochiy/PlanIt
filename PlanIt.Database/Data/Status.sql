if not exists (select 1 from dbo.[Status] where id = 1)
insert into dbo.[Status](Name)
select 'Created'
go