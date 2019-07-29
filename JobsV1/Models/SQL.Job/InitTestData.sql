--insert into JobMains([JobDate],[CustomerId],[Description],[NoOfPax],[NoOfDays],[JobRemarks],[JobStatusId],[StatusRemarks],[BranchId],[JobThruId],[AgreedAmt])
--values
--('4-25-2019',1,'Test Job 101',10,1,'TEST DATA 0101',3,'N/A',1,1,5000),
--('4-28-2019',1,'Item scheduling',3,1,'TEST DATA 0102',3,'N/A',1,1,3000);


--insert into JobServices([JobMainId],[ServicesId],[SupplierId],[Particulars],[QuotedAmt],[SupplierAmt],[ActualAmt],[Remarks],[SupplierItemId],[DtStart],[DtEnd])
--values
--(1,1,2,'Car Rental sample data R1',5000,5000,5000,'Sample only. Disregard once seen on production',1,'4-15-2019','4-22-2019'),
--(1,1,2,'Car Rental sample data R2',3000,3000,3000,'Sample only. Disregard once seen on production',1,'4-26-2019','4-29-2019'),
--(2,1,2,'SUV Rental R1',2000,2000,2000,'Sample only. Disregard once seen on production',1,'4-27-2019','4-28-2019'),
--(2,1,2,'SUV Rental R2',1000,1000,1000,'Sample only. Disregard once seen on production',1,'4-29-2019','4-30-2019');

----insert into InvItems([ItemCode],[Description],[Remarks])
----values ('RNY301','Toyota Innova E M/T 2013 Dsl',''),
----('EOK873','Honda City A/T 2018 1.5E',''),
----('ADP2264','Ford Everest A/T 2016 2.2',''),
----('AbelS','Abel Salvatierra','');


--insert into InvItemCategories([InvItemId],[InvItemCatId])
--values (1,1), (2,1), (3,1), (4,2);


--Insert into JobServiceItems([JobServicesId],[InvItemId])
--values(1,2),(1,3),
--(2,3),(2,4),
--(3,3),(3,4),
--(4,3),(4,4);

---- Supplier PO Samples
--insert into SupplierPoHdrs([PoDate],[Remarks],[SupplierId],[SupplierPoStatusId],[RequestBy],[DtRequest])
--values ('07-25-2018','Test Po',1,1,'Abel','07-25-2018');

--insert into SupplierPoDtls([SupplierPoHdrId],[Remarks],[Amount],[JobServicesId])
--values (1,'10 seater vehicle',3500,1), (1,'14 seater vehicle',4000,1);

---- Customer PO Samples
--insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
--values('John Doe','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

--insert into CustCats(CustomerId, CustCategoryId) 
--values(3,2),(3,1);

--insert into CustEntMains(Name, Address, Contact1, Contact2, iconPath) 
--values('NewCompany.Inc','Davao City','09950753794','09950753794','Images/Customers/Company/organization-40.png');

--insert into CustEntities(CustEntMainId, CustomerId) 
--values(2,3);

--update CustCategories set iconPath = 'Images/Customers/Category/star-filled-40.png' where Id = 1; 
--update CustCategories set iconPath = 'Images/Customers/Category/Active-30.png' where Id = 2; 
--update CustCategories set iconPath = 'Images/Customers/Category//suspended-64.png' where Id = 3; 
--update CustCategories set iconPath = 'Images/Customers/Category/cancel-40.png' where Id = 4;  

--- sql query get customers with job count
Select Id,Name, Contact1, Contact2 , Status,
	JobCount = (Select Count(x.Id) from [JobMains] x where x.CustomerId = c.Id ),
	Company  = (Select top(1) CompanyName = (Select top(1) cem.Name from [CustEntMains] cem where ce.CustEntMainId = cem.Id  ORDER BY cem.Id DESC)
			   	from [CustEntities] ce where ce.CustomerId = c.Id) 
From Customers c ORDER BY JobCount DESC , Id DESC;


Select Name, Contact1, Contact2 , Status,
	JobCount = (Select Count(x.Id) from [JobMains] x where x.CustomerId = c.Id ),
	Company  = (Select CompanyName = (Select cem.Name from [CustEntMains] cem where ce.CustEntMainId = cem.Id)
			   	from [CustEntities] ce where ce.CustomerId = c.Id) 
From Customers c where c.Status = '*' and c.Name LIKE '%*%';

-- work in progress
select a.Id ItemId, c.JobMainId, c.Id ServiceId, c.Particulars, c.DtStart, c.DtEnd from 
InvItems a
left outer join JobServiceItems b on b.InvItemId = a.Id 
left outer join JobServices c on b.JobServicesId = c.Id
left outer join JobMains d on c.JobMainId = d.Id
where d.JobStatusId < 4 AND c.DtStart >= DATEADD(DAY,-30, GETDATE())
;

select a.Id ItemId, c.JobMainId, c.Id ServiceId, c.Particulars, c.DtStart, c.DtEnd, d.JobDate from 
InvItems a
left outer join JobServiceItems b on b.InvItemId = a.Id 
left outer join JobServices c on b.JobServicesId = c.Id
left outer join JobMains d on c.JobMainId = d.Id
where d.JobStatusId < 4
;

select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY,-10, GETDATE())

;

--previous
select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate < GETDATE() 
AND j.JobDate >= DATEADD(DAY, -30, GETDATE()) 

--


SELECT Id,Name, Contact1, Contact2 , Status,
 JobCount = (SELECT Count(x.Id) FROM [JobMains] x WHERE x.CustomerId = c.Id ) ,
 Company = (SELECT Top(1)  CompanyName = (SELECT Top(1) cem.Name FROM [CustEntMains] cem WHERE ce.CustEntMainId = cem.Id)
 FROM [CustEntities] ce WHERE ce.CustomerId = c.Id) FROM Customers c
 ORDER BY JobCount DESC , Id DESC


 SELECT j.Id
  FROM JobMains j WHERE j.JobStatusId < 4 AND j.JobDate < GETDATE() 
  AND j.Id = (SELECT Top(1) svc.JobMainId FROM JobServices svc WHERE svc.JobMainId = j.id )
  ;

  

 SELECT j.Id ,
  unit = (SELECT unit = (SELECT jsvc.InvItemId FROM JobServiceItems jsvc WHERE jsvc.InvItemId = 2 GROUP BY jsvc.InvItemId) FROM JobServices svc WHERE svc.JobMainId = j.id GROUP BY svc.JobMainId ),
  svc_count = (SELECT COUNT(svc.JobMainId) FROM JobServices svc WHERE svc.JobMainId = j.id GROUP BY svc.JobMainId )

  FROM JobMains j WHERE j.JobStatusId > 0 AND j.JobDate < GETDATE() 
  ;
  
SELECT jobcount = COUNT(svc.JobMainId) , svc.JobMainId FROM JobServices svc GROUP BY svc.JobMainId 

SELECT jsvc.JobServicesId, jsvc.InvItemId FROM JobServiceItems jsvc WHERE jsvc.InvItemId = 2 



SELECT j.Id FROM JobMains j WHERE j.JobStatusId = 4 AND j.JobDate < GETDATE();

SELECT j.Id FROM JobMains j WHERE (j.JobStatusId = 4 AND j.JobDate < GETDATE()) OR j.JobDate >= DATEADD(DAY, -30, GETDATE()) AND j.JobStatusId = 4;

SELECT * From JobMains j WHERE j.Id = (SELECT * FROM JobEntMains WHERE CustEntMainId = 1)

SELECT * FROM JobEntMains WHERE CustEntMainId = 1 GROUP BY JobMainId ORDER BY Id

SELECT * FROM JobServices WHERE JobMainId = 2
