

--insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
--values('John Doe','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

--insert into CustCats(CustomerId, CustCategoryId) 
--values(3,2),(3,1);

--insert into CustEntMains(Name, Address, Contact1, Contact2, iconPath) 
--values('NewCompany.Inc','Davao City','09950753794','09950753794','Images/Customers/Company/organization-40.png');

--insert into CustEntities(CustEntMainId, CustomerId) 
--values(2,3);

--jobmainid = 3 , service id =5 , action.done sub select
select * from srvactionItems as d where d.ServicesId = 5 AND NOT EXISTS ( select SrvActionItemId from jobActions as s where s.JobServicesId = 5 ) ;

--action done
 select SrvActionItemId from jobActions as s where s.JobServicesId = 3;