
select
'New SalesLead' as StatusCategory, 
a.DtEntered dtTaken,  
a.Id refId, a.CustName + ' / ' + a.Details Details 
from SalesLeads a
where datediff(day, getdate(),a.DtEntered) > - 1

union

select 
'SalesLead Activity' as StatusCategory,
a.DtActivity dtTaken,
a.SalesLeadId refId,
c.Name + ' - ' + a.Particulars Details
from 
SalesActivities a
left join SalesLeads as s on s.Id = a.SalesLeadId 
left join Customers as c on c.Id = s.CustomerId 
where a.SalesActStatusId=2 AND datediff(day, getdate(),a.DtActivity) > -2

union

select 
'SalesStatus Activity' as StatusCategory,
a.DtStatus dtTaken,
a.SalesLeadId refId,
s.CustName + ' - ' + s.Details + ' - '+ sc.Name as Details
from 
SalesStatus a
left join SalesLeads as s on s.Id = a.SalesLeadId 
left join Customers as c on c.Id = s.CustomerId 
left join SalesStatusCodes as sc on sc.Id = a.SalesStatusCodeId 
where datediff(day, getdate(),a.DtStatus) > -1

union

select 
'New JobOrder' as StatusCategory,
j.JobDate as dtTaken,
j.Id as refId,
c.Name +' - '+ j.Description as Details
from 
JobMains j 
left join Customers as c on c.Id = j.CustomerId 
where datediff(day, getdate(),j.JobDate) > - 1

union

select 
'JobOrder Activity' as StatusCategory,
j.DtPerformed as dtTaken,
j.JobServicesId as refId,
cu.Name +' - '+ js.Particulars +' / '+ 
c.CatCode +' - '+ j.Remarks as Details
from 
JobActions j 
left join SrvActionItems as s on s.Id = j.SrvActionItemId 
left join SrvActionCodes as c on c.Id = s.SrvActionCodeId 
left join JobServices as js on js.Id = j.JobServicesId 
left join JobMains as m on m.Id = js.JobMainId 
left join Customers as cu on cu.Id = m.CustomerId 

where datediff(day, getdate(),j.DtPerformed) > - 1

union

select 
'Gate Activity' as StatusCategory,
g.dtControl as dtTaken,
g.Id as refId,
'('+c.ItemCode +')'+c.Description  as Details
from 
InvCarGateControls g 
left join InvItems as c on c.Id = g.InvItemId 
where datediff(day, getdate(),g.dtControl) > - 1

union

select 
'Maintenance Activity' as StatusCategory,
g.dtControl as dtTaken,
g.Id as refId,
'('+c.ItemCode +')'+c.Description as Details
from 
InvCarGateControls g 
left join InvItems as c on c.Id = g.InvItemId 
where datediff(day, getdate(),g.dtControl) > - 1

Order by dtTaken
;

