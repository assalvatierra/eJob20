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

/** Active Jobs for QuickList
 * with date filter and order by date start and time 
 */

 SELECT TOP 50 js.Id,  js.JobMainId ,js.Particulars, JobName = j.Description , Service = ( SELECT s.Name FROM Services s WHERE js.ServicesId = s.Id ),
                    Customer = (SELECT c.Name FROM Customers c WHERE j.CustomerId = c.Id) , 
                    Item = (SELECT sup.Description FROM SupplierItems sup WHERE js.SupplierItemId = sup.Id ),
                    CONVERT(varchar, CAST( js.DtStart as DATETIME), 107) as DtStart,
                    CONVERT(varchar, CAST( js.DtEnd as DATETIME), 107) as DtEnd,
                    CONVERT(varchar, CAST( jp.JsDate as DATETIME), 107) as JsDate ,
                    CAST( convert(varchar, isnull(jp.JsTime,'00:00:00'), 8) as TIME) as JsTime, 
			
                    SORTDATE = DATEADD(hh, CAST(SUBSTRING( CAST( CAST( CONVERT(varchar, isnull(jp.JsTime,'00:00:00'), 8)  as TIME) as VARCHAR),1,2 ) as INT),DtStart)
					
                    FROM JobServices js
                    LEFT JOIN JobMains j ON js.JobMainId = j.Id
                    LEFT JOIN JobServicePickups jp ON jp.JobServicesId = js.Id
					WHERE j.JobStatusId < 4 
					ORDER BY SORTDATE ASC
					;


SELECT js.Id,  js.JobMainId ,js.Particulars, JobName = j.Description , Service = ( SELECT s.Name FROM Services s WHERE js.ServicesId = s.Id ),
                    Customer = (SELECT c.Name FROM Customers c WHERE j.CustomerId = c.Id) ,  
                    Item = (SELECT sup.Description FROM SupplierItems sup WHERE js.SupplierItemId = sup.Id ), 
                    CONVERT(varchar, CAST( js.DtStart as DATETIME), 107) as DtStart,
                    CONVERT(varchar, CAST( js.DtEnd as DATETIME), 107) as DtEnd, 
                    CONVERT(varchar, CAST(jp.JsDate as DATETIME),  107) as JsDate,  
                    CONVERT(TIME, CAST( isnull(jp.JsTime, '00:00:00') as TIME),8) as JsTime, jp.JsLocation, 
                    SORTDATE = CAST( DATEADD(hh, CAST(SUBSTRING( CAST( CAST( CONVERT(varchar, isnull(jp.JsTime,'00:00:00'), 8)  as TIME) as VARCHAR),1,2 ) as INT),DtStart)  as DATETIME)
                    FROM JobServices js 
                    LEFT JOIN JobMains j ON js.JobMainId = j.Id 
                    LEFT JOIN JobServicePickups jp ON jp.JobServicesId = js.Id ORDER BY SORTDATE;

-- GET Company List with filtering and sort
SELECT cem.*,
Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) 
FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ),
ContactPerson = (SELECT TOP 1 Name = (SELECT CONCAT(Name,' - ',Contact1,' / ',Contact2) FROM Customers cust  WHERE cust.Id = ce.CustomerId)
FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId)
FROM CustEntMains cem 

-- Get Sales Lead List
SELECT * ,
City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),
SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id )
FROM Suppliers sup 

SELECT counting = (SELECT Id FROM SalesLeadItems sli WHERE sli.SalesLeadId = sl.Id) FROM SalesLeads sl 
    
SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId )
	FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), 
	City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId) ,
	ContactPerson = (SELECT TOP 1 Name = (SELECT CONCAT(Name, ' <br /> ', Contact1, ' / ', Contact2) 
	FROM Customers cust WHERE cust.Id = ce.CustomerId) 
	FROM CustEntities ce 
	WHERE cem.Id = ce.CustEntMainId) FROM CustEntMains cem
	WHERE  Name Like '%Manila%' 

-------- Search supplier inv items ----------

SELECT * FROM
	( SELECT * ,  
	 Country = (SELECT Name FROM Countries cty WHERE sup.CountryId = cty.Id ),
	 City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),
	 SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id ),
	 Items = SUBSTRING( (SELECT (SELECT ii.Description  as [text()] FROM InvItems ii WHERE sii.InvItemId = ii.Id FOR XML PATH('')) + ', '
		FROM SupplierInvItems sii WHERE sup.Id = sii.SupplierId FOR XML PATH('')),1,100 )
	 FROM Suppliers sup ) as ItemList
 WHERE (ItemList.Status = 'ACT' OR ItemList.Status = 'ACC') AND ItemList.Name LIKE '%Steels%'


 
  SELECT * , 
 City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),
 SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id ),
 Items = SUBSTRING( (SELECT sii.Id as [text()]
	FROM SupplierInvItems sii WHERE sup.Id = sii.SupplierId 
	FOR XML PATH('')),2,100 ) 
 FROM Suppliers sup


 SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), 
                City =  (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), 
                ContactPerson = (SELECT TOP 1 Name = (SELECT CONCAT(Name, ' <br /> ', Contact1, ' / ', Contact2)
                FROM Customers cust WHERE cust.Id = ce.CustomerId) FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId), 
				
				ContactPersonPos = (SELECT TOP 1 Name = (SELECT Name FROM Customers cust WHERE cust.Id = ce.CustomerId) 
				FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId) ,
				
				ContactPersonPos = (SELECT TOP 1 Position 
				FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId) 

				FROM CustEntMains cem ;

--- Company Table Result
SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), 
                 City =  (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), 
                 ContactPerson = (SELECT TOP 1 Name = (SELECT Name 
                 FROM Customers cust WHERE cust.Id = ce.CustomerId) FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId) ,
                 ContactPersonPos = (SELECT TOP 1 Position FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId) 
                 FROM CustEntMains cem;

SELECT * FROM (
	SELECT * ,
    Country = (SELECT Name FROM Countries cty WHERE sup.CountryId = cty.Id ),
    City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),
    SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id )
    FROM Suppliers sup ) as suplist

	WHERE suplist.supType LIKE '%Supplier%'

SELECT * ,Country = (SELECT Name FROM Countries cty WHERE sup.CountryId = cty.Id ),
          City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),
          SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id )
          FROM Suppliers sup 

Select UserName from AspNetUsers Where UserName NOT IN ('jahdielvillosa@gmail.com','jahdielsvillosa@gmail.com','assalvatierra@gmail.com');

SELECT * , Country = (SELECT Name FROM Countries cty WHERE sup.CountryId = cty.Id ),
		City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),
		SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id )
		FROM Suppliers sup

		SELECT  ItemName = (SELECT i.Description as[text()]  FROM InvItems i WHERE si.InvItemId=i.Id ) + ', '  FROM SupplierInvItems si WHERE SupplierId = 2  FOR XML PATH('') 

SELECT * FROM ( SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ),
       City =  (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), 
       ContactPerson = (SELECT TOP 1 Name = (SELECT Name 
       FROM Customers cust WHERE cust.Id = ce.CustomerId) FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId), 
       ContactPersonPos = (SELECT TOP 1 Position FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId ORDER BY ce.Id DESC) 
       FROM CustEntMains cem ) as com 
	  WHERE  com.Name LIKE '%ABC%'

SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), 
        City =  (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), 
        ContactPerson = (SELECT TOP 1 Name = (SELECT Name FROM Customers cust WHERE cust.Id = ce.CustomerId)
	    FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId Order By ce.Id DESC), 
        Mobile = (SELECT TOP 1 Contact = (SELECT cust.Contact2 FROM Customers cust WHERE cust.Id = ce.CustomerId)
	    FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId Order By ce.Id DESC),
        ContactPersonPos = (SELECT TOP 1 Position FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId ORDER BY ce.Id DESC) 
        FROM CustEntMains cem ) as com 

SELECT Id,Name, Contact1, Contact2 , Status,
       JobCount = (SELECT Count(x.Id) FROM [JobMains] x WHERE x.CustomerId = c.Id ) ,
       Company = (SELECT Top(1)  CompanyName = (SELECT Top(1) cem.Name FROM [CustEntMains] cem where ce.CustEntMainId = cem.Id )
       FROM [CustEntities] ce WHERE ce.CustomerId = c.Id ORDER BY ce.Id DESC) FROM Customers c

select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());;
    
SELECT * FROM (
SELECT  sii.Id, ii.Description as Name, Supplier = ( SELECT supp.Name FROM Suppliers supp WHERE sii.SupplierId = supp.Id )
	, sii.SupplierId, sir.ItemRate, 
	su.Unit, sir.DtValidFrom, sir.DtValidTo, sir.Remarks, sup.Status
	FROM SupplierInvItems sii LEFT JOIN Suppliers sup ON sii.Id = sup.Id
	LEFT JOIN SupplierItemRates sir on sii.Id = sir.SupplierInvItemId
	LEFT JOIN InvItems ii ON sii.InvItemId = ii.Id
	LEFT JOIN SupplierUnits su ON sir.SupplierUnitId = su.Id ) as prods
	WHERE prods.Name LIKE '%Steel Pipe%' 
	ORDER BY prods.ItemRate ASC

SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), 
                 City =  (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), 
                 
				 cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber,
				 cet.Position 

                 FROM CustEntMains cem 
				 LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id 
				 LEFT JOIN Customers cust ON cust.Id = cet.CustomerId 
				 ) as com 

				WHERE (Exclusive = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE' AND AssignedTo='jecca.raelbreeze@gmail.com'))
-- Company 
--Sales Lead
SELECT sl.*, slc.CustEntMainId FROM SalesLeads sl
	LEFT JOIN SalesLeadCompanies slc ON sl.Id = slc.SalesLeadId

SELECT * FROM SalesLeads


-- Trip Listing
--SELECT * FROM JobServices  js
   
  SELECT js.JobMainId, js.Id as JobServicesId, js.DtStart, js.DtEnd, js.Particulars, jm.Description, jm.JobStatusId, js.ActualAmt
	            FROM JobServices  js
	            LEFT JOIN JobMains jm ON jm.Id = js.JobMainId 
	            WHERE js.DtEnd >= DATEADD(DAY, -30, GETDATE())


 SELECT sii.Id, sii.SupplierId, sii.InvItemId, sir.*
 FROM SupplierInvItems sii
 LEFT JOIN SupplierItemRates sir on sir.SupplierInvItemId = sii.Id


SELECT * FROM (
 SELECT sii.Id, sii.SupplierId, sii.InvItemId, sir.Id as SupplierInvRateId, sir.SupplierInvItemId, sir.ItemRate, sir.SupplierUnitId,
 sir.Remarks, sir.DtValidFrom, sir.DtValidTo, sir.Particulars, Sir.Material, sir.ProcBy, sir.TradeTerm, sir.Tolerance, sir.DtEntered
 FROM SupplierInvItems sii
 LEFT JOIN SupplierItemRates sir on sir.SupplierInvItemId = sii.Id
  ) 
 as sup
 ORDER BY sup.DtValidFrom DESC


 -- Trip Listing
   SELECT js.JobMainId, js.Id as JobServicesId, js.DtStart, js.DtEnd, js.Particulars, jm.Description, jm.JobStatusId, js.ActualAmt
				, items = SUBSTRING((SELECT item = (SELECT ii.Description FROM InvItems ii WHERE ii.Id = jsi.InvItemId ) FROM JobServiceItems jsi WHERE jsi.InvItemId = js.Id FOR XML PATH('')),2,100)
	            FROM JobServices  js
	            LEFT JOIN JobMains jm ON jm.Id = js.JobMainId 
	            WHERE js.DtEnd >= DATEADD(DAY, -30, GETDATE()) AND JobStatusId > 1 AND JobStatusId < 4 
			
	--Items = SUBSTRING( (SELECT sii.Id as [text()]
	--FROM SupplierInvItems sii WHERE sup.Id = sii.SupplierId 
	--FOR XML PATH('')),2,100 ) 


	SELECT * FROM (SELECT sii.Id, ii.Description as Name, Supplier = ( SELECT supp.Name FROM Suppliers supp WHERE sii.SupplierId = supp.Id ),
                sii.SupplierId, sir.ItemRate, su.Unit, sir.DtEntered, sir.DtValidFrom, sir.DtValidTo, sir.Remarks, sup.Status 
                FROM SupplierInvItems sii LEFT JOIN Suppliers sup ON sii.Id = sup.Id 
                LEFT JOIN SupplierItemRates sir on sii.Id = sir.SupplierInvItemId 
                LEFT JOIN InvItems ii ON sii.InvItemId = ii.Id 
                LEFT JOIN SupplierUnits su ON sir.SupplierUnitId = su.Id) as prods 

				ORDER BY DtEntered DESC

-- Drivers Trip search by Driver ID
SELECT je.*, js.DtStart, js.DtEnd, jm.Description, js.Particulars, ii.Description as Name, ii.ItemCode  FROM JobExpenses je
	LEFT JOIN JobMains jm ON jm.Id = je.JobMainId
	LEFT JOIN JobServices js ON js.Id = je.JobServicesId
	LEFT JOIN JobServiceItems jsi ON jsi.JobServicesId = js.Id
	LEFT JOIN InvItems ii ON ii.Id = jsi.InvItemId
	WHERE ii.Id = 7 AND ForRelease = 1

-- Customer Activities Report
SELECT	UserName,
		Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.Type = 'Quotation' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'2020-01-15') AND convert(datetime, ca.Date) < convert(datetime,'2020-02-20') ),
		Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.Type = 'Meeting' AND au.UserName = ca.Assigned ),
		Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.Type = 'Sales' AND au.UserName = ca.Assigned ),
		Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.Type = 'Sales' AND au.UserName = ca.Assigned )
 FROM AspNetUsers au 
 ORDER BY Sales DESC, Meeting DESC, Quotation Desc

SELECT * FROM CustEntActivities

-- Supplier Products
SELECT * FROM (SELECT sii.Id, ii.Description as Name, Supplier = ( SELECT supp.Name FROM Suppliers supp WHERE sii.SupplierId = supp.Id ),
               
                sii.SupplierId, sir.ItemRate, su.Unit, sir.DtEntered, sir.DtValidFrom, sir.DtValidTo, sir.Remarks, sup.Status ,
				IsValid =  IIF(convert(datetime, sir.DtValidTo) < convert(datetime, GETDATE()), 1 , 0)
                FROM SupplierInvItems sii LEFT JOIN Suppliers sup ON sii.Id = sup.Id 
                LEFT JOIN SupplierItemRates sir on sii.Id = sir.SupplierInvItemId  
                LEFT JOIN InvItems ii ON sii.InvItemId = ii.Id					   
                LEFT JOIN SupplierUnits su ON sir.SupplierUnitId = su.Id) as prods 
				WHERE  (ISNULL(prods.ItemRate,'0') = '0') OR convert(datetime, prods.DtValidTo) > convert(datetime, DATEADD(DAY, -180, GETDATE())) 
				AND prods.Name Like '%pipe%'  
--Customer List  
SELECT c.Id,c.Name, c.Contact1, c.Contact2 , c.Status,
       JobCount = (SELECT Count(x.Id) FROM [JobMains] x WHERE x.CustomerId = c.Id ) ,
       Company = (SELECT Top(1)  CompanyName = (SELECT Top(1) cem.Name FROM [CustEntMains] cem where ce.CustEntMainId = cem.Id ORDER BY cem.Id DESC)

	   FROM [CustEntities] ce WHERE ce.CustomerId = c.Id  ORDER BY ce.Id DESC) 
	   FROM Customers c
	   INNER JOIN CustEntities cen ON cen.CustomerId = c.Id                                          
	   LEFT JOIN CustEntMains cem ON cem.Id = cen.CustEntMainId 
	   WHERE (cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC') OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = 'demo@gmail.com')     

SELECT * FROM CustEntMains cem 
WHERE (cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC') OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = 'demo@gmail.com')     

-- Cust Notification Recipients --	                                       
SELECT *,	
	Name = (SELECT Name FROM Customers cu WHERE cu.Id = cr.CustomerId),
	Email = (SELECT Email FROM NotifRecipients nr WHERE nr.Id = cr.NotifRecipientId),
	Mobile = (SELECT Mobile FROM NotifRecipients nr WHERE nr.Id = cr.NotifRecipientId)
FROM CustNotifRecipients cr
WHERE CustNotifId = 1				

-- Cust Notification Activities
SELECT cna.Id,
	   DtActivity = cna.DtActivity,
	   Title = cn.MsgTitle,
	   Status = cna.Status,
	   Recipient = (SELECT Email FROM NotifRecipients nr WHERE nr.Id = cnr.NotifRecipientId)
FROM CustNotifActivities cna 	
LEFT JOIN CustNotifRecipients cnr ON cnr.Id = cna.CustNotifRecipientId
LEFT JOIN CustNotifs cn ON cn.Id = cnr.CustNotifId	               

-- Get Pending list --
SELECT cna.Id,
	   DtActivity = cna.DtActivity,
	   Title = cn.MsgTitle,
	   Content = cn.MsgBody,
	   IsEmail = cn.IsEmail,
	   IsSMS = cn.IsSMS,
	   Status = cna.Status,
	   Occurence = cn.Occurence,
	   Recipient = (SELECT Email FROM CustNotifRecipientLists nr WHERE nr.Id = cnr.NotifRecipientId)
FROM CustNotifActivities cna 	
LEFT JOIN CustNotifRecipients cnr ON cnr.Id = cna.CustNotifRecipientId
LEFT JOIN CustNotifs cn ON cn.Id = cnr.CustNotifId	
WHERE cna.DtActivity <= convert(datetime, GETDATE()) AND cna.Status = 'PENDING'            

--Company List--                                                           
SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), 
                 City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), 
                 cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber, 
                 cet.Position as ContactPosition 
                 FROM CustEntMains cem 
                 LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id 
                 LEFT JOIN Customers cust ON cust.Id = cet.CustomerId ) as com 
                 WHERE Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE')                 
				 ORDER BY 
                         CASE com.Status
                              WHEN 'PRI' THEN 1
                              WHEN 'ACT' THEN 2
                              WHEN 'ACC' THEN 3
                              WHEN 'ACP' THEN 4
                              WHEN 'BIL' THEN 5
                              ELSE 6 END; 
                             

SELECT  jm.Id, jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd,
Customer = (SELECT c.Name FROM Customers c WHERE c.Id = jm.CustomerId)
FROM JobMains jm
LEFT JOIN JobServices js ON jm.Id = js.JobMainId

-- Job Order Revision Listing --
SELECT * FROM (
SELECT jm.Id, jm.JobDate, jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd,
Customer = (SELECT c.Name FROM Customers c WHERE c.Id = jm.CustomerId)
FROM JobMains jm
LEFT JOIN JobServices js ON jm.Id = js.JobMainId ) job
WHERE job.DtStart >= convert(datetime, GETDATE()) OR (job.DtStart <= convert(datetime, GETDATE()) AND job.DtEnd >= convert(datetime, GETDATE()))
AND job.JobStatusId < 4
GROUP BY job.Id


SELECT  Id = MIN(job.Id), DtStart = MIN(job.DtStart), DtEnd = MIN(job.DtEnd), 
	    Description = MIN(job.Description), Customer = MIN(job.Customer), Status = MIN(job.JobStatusId)  
	    FROM ( SELECT jm.Id,  jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd,
		Customer = (SELECT c.Name FROM Customers c WHERE c.Id = jm.CustomerId)
		FROM JobMains jm
		LEFT JOIN JobServices js ON jm.Id = js.JobMainId ) job
		WHERE job.DtStart >= convert(datetime, GETDATE()) OR (job.DtStart <= convert(datetime, GETDATE()) AND job.DtEnd >= convert(datetime, GETDATE()))
		AND job.JobStatusId < 4
		GROUP BY job.Id ORDER BY DtStart
	
SELECT *, Company = (SELECT Name FROM CustEntMains cem WHERE cem.Id = act.CustEntMainId )  
      FROM CustEntActivities act WHERE
      Assigned = 'jahdielvillosa@gmail.com' AND (act.Date >= convert(datetime, '03/01/2020') AND act.Date <= convert(datetime, '03/30/2020'))
	  ORDER BY Date DESC;

--GET USER ROLE--
SELECT UserRole = (SELECT Name FROM AspNetRoles r WHERE r.Id = ur.RoleId) FROM AspNetUsers u
	LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id
	WHERE UserName = 'jahdielvillosa@gmail.com' ;

-- Procurement Activities --
SELECT *, Company = (SELECT Name FROM Suppliers sup WHERE sup.Id = act.SupplierId ),
          Points = (SELECT Points FROM SupplierActivityTypes type WHERE type.Type = act.ActivityType),
		  DtActivity as Date
          FROM SupplierActivities act 

-- JobListing / JobOrder --
SELECT DISTINCT job.Id FROM ( 
       SELECT jm.Id, jm.JobDate, jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd, 
       Customer = c.Name
       FROM JobMains jm 
       LEFT OUTER JOIN JobServices js ON jm.Id = js.JobMainId 
	   LEFT OUTER JOIN Customers c ON jm.CustomerId = c.Id
	   ) job 
       WHERE job.DtStart >= convert(datetime, GETDATE()) OR(job.DtStart <= convert(datetime, GETDATE()) AND job.DtEnd >= convert(datetime, GETDATE())) 
       AND job.JobStatusId < 4

        SELECT	UserName,
		                Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Quotation' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'4/01/2020') AND convert(datetime, ca.Date) < convert(datetime,'4/20/2020') ),
                        Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'4/01/2020') AND convert(datetime, ca.Date) < convert(datetime,'4/20/2020') ),
                        Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'4/01/2020') AND convert(datetime, ca.Date) < convert(datetime,'4/20/2020') ),
                        ProcMeeting = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Meeting' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'4/01/2020') AND convert(datetime, sa.DtActivity) < convert(datetime,'4/20/2020') ),
                        Procurement = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Procurement' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'4/01/2020') AND convert(datetime, sa.DtActivity) < convert(datetime,'4/20/2020') ),
                       JobOrder = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'4/01/2020') AND convert(datetime, ca.DtActivity) < convert(datetime,'4/20/2020') ),
                       Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'4/01/2020') AND convert(datetime, ca.Date) < convert(datetime,'4/20/2020') )
                FROM AspNetUsers au 

                 Where UserName NOT IN 
                ('jahdielvillosa@gmail.com' ,'jahdielsvillosa@gmail.com', 'assalvatierra@gmail.com', 
                'admin@gmail.com' ,'demo@gmail.com', 'assalvatierra@yahoo.com' )

                ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;

-- Vehicle Service History --
SELECT DISTINCT jv.*, ISNULL(js.DtStart, jm.JobDate), js.Particulars, js.Remarks,  JobMainId = jm.Id ,
    jsServices = (SELECT s.Name FROM Services s WHERE s.ID = js.ServicesId)
    FROM JobVehicles jv
    LEFT JOIN JobServices js ON js.JobMainId = jv.JobMainId
    LEFT JOIN JobMains jm ON jm.Id = jv.JobMainId
    WHERE jv.VehicleId = 2 
    AND js.DtStart IS NOT NULL

    ORDER BY DtStart DESC


    SELECT jv.*, DtStart = ISNULL(js.DtStart, jm.JobDate), js.Particulars, js.Remarks,  JobMainId = jm.Id, jm.JobStatusId,
          JsServices = (SELECT s.Name FROM Services s WHERE s.ID = js.ServicesId) 
          FROM JobVehicles jv 
          LEFT JOIN JobServices js ON js.JobMainId = jv.JobMainId
          LEFT JOIN JobMains jm ON jm.Id = jv.JobMainId 
          WHERE jv.VehicleId = 10 AND jm.JobStatusId <= 4  ORDER BY DtStart DESC ;

          
-- job post sale query --
SELECT DISTINCT jobPostSalePending.Id FROM (

    SELECT JobServiceId = js.Id, Customer = cu.Name, Mobile = jm.CustContactNumber, Email = jm.CustContactEmail, Service = s.Name,
        jm.JobDate, Vehicle = jm.Description, postSaleId = jps.Id, js.*, SupplierItem = si.Description, si.Interval, 
        FollowUpDate = DATEADD(DAY, ISNULL(si.Interval, 0), js.DtStart), PostSalesStatus = ps.Status, 
        jps.JobPostSalesStatusId, jps.DoneBy, PostSaleRemarks = jps.Remarks 
        FROM JobMains jm 
        LEFT JOIN JobServices js ON js.JobMainId = jm.Id 
        LEFT JOIN Services s ON js.ServicesId = s.Id
        LEFT JOIN SupplierItems si ON js.SupplierItemId = si.Id
        LEFT JOIN JobPostSales jps ON js.Id = jps.JobServicesId 
        LEFT JOIN JobPostSalesStatus ps ON jps.JobPostSalesStatusId = ps.Id 
        LEFT JOIN Customers cu ON jm.CustomerId = cu.Id 
        WHERE JobStatusId = 4 AND ISNULL(si.Interval, 0) > 0 
        AND convert(datetime, GETDATE()) >= DATEADD(DAY, ISNULL(si.Interval, 0), js.DtStart) 
        AND ISNULL(jps.JobPostSalesStatusId, 0) < 3

    ) as jobPostSalePending


Select job.Id FROM (

select j.Id from JobMains j 

LEFT JOIN Customers c ON j.CustomerId = c.Id
LEFT JOIN JobEntMains jem ON j.Id = jem.JobMainId 
LEFT JOIN CustEntMains cem ON jem.CustEntMainId = cem.Id

WHERE cem.Name LIKE '%Mark%' OR c.Name LIKE '%Mark%' OR j.Description LIKE '%Mark%'

) as job




---Company/Customer Activities Post Sales ---
Select c.* from CustEntActivities c


-- GET SalesCodes
SELECT act.*, cem.Name as Company, cem.Exclusive FROM (
 
SELECT c.SalesCode,
ActivityDate = ( SELECT TOP 1 ca.Date FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
CompanyId    = ( SELECT TOP 1 ca.CustEntMainId FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
ProjectName  = ( SELECT TOP 1 ca.ProjectName FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
Status       = ( SELECT TOP 1 ca.Status FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
ActivityType = ( SELECT TOP 1 ca.ActivityType FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
Amount       = ( SELECT TOP 1 ca.Amount FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
AssignedTo   = ( SELECT TOP 1 ca.Assigned FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
Remarks      = ( SELECT TOP 1 ca.Remarks FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC )

from CustEntActivities c
Group by c.SalesCode 
) as act 
LEFT JOIN CustEntMains cem ON cem.Id = act.CompanyId

-- revised
SELECT act.*, cem.Name as Company, cem.Exclusive FROM (
 
SELECT c.SalesCode, cap.Date 
from CustEntActivities c
 OUTER APPLY ( SELECT TOP 1 ca.* FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ) as cap

Group by c.SalesCode

) as act 

LEFT JOIN CustEntMains cem ON cap.Id = act.CompanyId


-- Filter activities not closed
WHERE act.Status != 'Close' AND convert(datetime, GETDATE()) >= CASE WHEN
    act.ActivityType = 'Quotation' THEN
      DATEADD(DAY, ISNULL(7, 0), act.ActivityDate) 
    ELSE 
      DATEADD(DAY, ISNULL(92, 0), act.ActivityDate) 
    END

    AND (cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC') OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = 'admin@gmail.com')     

ORDER BY CASE WHEN act.ActivityType = 'Quotation' then 1 else 2 end, act.ActivityType DESC, act.ActivityDate;





-- FOR ACTIVE ACTIVITY STATUS LIST ---
SELECT act.*,cem.Name as Company, cem.AssignedTo, cem.Exclusive FROM (
 
    SELECT c.SalesCode,
    ActivityDate = ( SELECT TOP 1 ca.Date FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
    CompanyId    = ( SELECT TOP 1 ca.CustEntMainId FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
    ProjectName  = ( SELECT TOP 1 ca.ProjectName FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
    Status       = ( SELECT TOP 1 ca.Status FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
    ActivityType = ( SELECT TOP 1 ca.ActivityType FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC ),
    Amount       = ( SELECT TOP 1 ca.Amount FROM CustEntActivities ca WHERE ca.SalesCode = c.SalesCode ORDER BY Date DESC )
    from CustEntActivities c

    Group by c.SalesCode 
    ) as act 
LEFT JOIN CustEntMains cem ON cem.Id = act.CompanyId

-- Filter activities not closed
WHERE act.Status != 'Close' 
    AND (cem.Exclusive = 'PUBLIC' OR ISNULL(cem.Exclusive,'PUBLIC') = 'PUBLIC') OR (cem.Exclusive = 'EXCLUSIVE' AND cem.AssignedTo = 'admin@gmail.com')     

ORDER BY act.ActivityDate;


------ Supplier Activity status --------
SELECT act.*, sup.Name FROM (
    SELECT s.Code, 
        DtActivity = ( SELECT TOP 1 su.DtActivity FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ),
        SupId = ( SELECT TOP 1 su.SupplierId FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ),
        StatusId = ( SELECT TOP 1 su.SupplierActStatusId FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ),
        Activity = ( SELECT TOP 1 su.ActivityType FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ) ,
        ActType = ( SELECT TOP 1 su.Type FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ) ,
        Amount = ( SELECT TOP 1 su.Amount FROM SupplierActivities su WHERE su.Code = s.Code ORDER BY DtActivity DESC ) 
        FROM SupplierActivities s 
        GROUP BY s.Code
    ) as act 
    LEFT JOIN Suppliers sup ON sup.Id = act.SupId
    WHERE act.StatusId < 3 


