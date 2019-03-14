insert into JobMains([JobDate],[CustomerId],[Description],[NoOfPax],[NoOfDays],[JobRemarks],[JobStatusId],[StatusRemarks],[BranchId],[JobThruId],[AgreedAmt])
values
('3-25-2019',1,'Test Job 101',10,1,'TEST DATA 0101',3,'N/A',1,1,5000),
('3-28-2019',1,'Item scheduling',3,1,'TEST DATA 0102',3,'N/A',1,1,3000);


insert into JobServices([JobMainId],[ServicesId],[SupplierId],[Particulars],[QuotedAmt],[SupplierAmt],[ActualAmt],[Remarks],[SupplierItemId],[DtStart],[DtEnd])
values
(1,1,2,'Car Rental sample data R1',5000,5000,5000,'Sample only. Disregard once seen on production',1,'12-15-2018','08-22-2018'),
(1,1,2,'Car Rental sample data R2',3000,3000,3000,'Sample only. Disregard once seen on production',1,'12-26-2018','08-29-2018'),
(2,1,2,'SUV Rental R1',2000,2000,2000,'Sample only. Disregard once seen on production',1,'12-27-2018','12-28-2018'),
(2,1,2,'SUV Rental R2',1000,1000,1000,'Sample only. Disregard once seen on production',1,'12-29-2018','12-30-2018');

--insert into InvItems([ItemCode],[Description],[Remarks])
--values ('RNY301','Toyota Innova E M/T 2013 Dsl',''),
--('EOK873','Honda City A/T 2018 1.5E',''),
--('ADP2264','Ford Everest A/T 2016 2.2',''),
--('AbelS','Abel Salvatierra','');


insert into InvItemCategories([InvItemId],[InvItemCatId])
values (1,1), (2,1), (3,1), (4,2);


Insert into JobServiceItems([JobServicesId],[InvItemId])
values(1,2),(1,3),
(2,3),(2,4),
(3,3),(3,4),
(4,3),(4,4);

-- Supplier PO Samples
insert into SupplierPoHdrs([PoDate],[Remarks],[SupplierId],[SupplierPoStatusId],[RequestBy],[DtRequest])
values ('07-25-2018','Test Po',1,1,'Abel','07-25-2018');

insert into SupplierPoDtls([SupplierPoHdrId],[Remarks],[Amount],[JobServicesId])
values (1,'10 seater vehicle',3500,1), (1,'14 seater vehicle',4000,1);

-- Customer PO Samples
insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
values('John Doe','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

insert into CustCats(CustomerId, CustCategoryId) 
values(3,2),(3,1);

insert into CustEntMains(Name, Address, Contact1, Contact2, iconPath) 
values('NewCompany.Inc','Davao City','09950753794','09950753794','Images/Customers/Company/organization-40.png');

insert into CustEntities(CustEntMainId, CustomerId) 
values(2,3);

update CustCategories set iconPath = 'Images/Customers/Category/star-filled-40.png' where Id = 1; 
update CustCategories set iconPath = 'Images/Customers/Category/Active-30.png' where Id = 2; 
update CustCategories set iconPath = 'Images/Customers/Category//suspended-64.png' where Id = 3; 
update CustCategories set iconPath = 'Images/Customers/Category/cancel-40.png' where Id = 4;  
