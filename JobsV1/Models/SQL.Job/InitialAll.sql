﻿insert into Cities(Name) values('Davao'),('Cebu'),('Makati'),('Manila');
insert into Branches(Name, CityId, Remarks, Address, Landline, Mobile) 
values ('AJ88',1,'Davao Main','Door 1, Travellers Inn Bldg, Matina Pangi Rd, Matina Crossing','082 297-1831',''),
	   ('RealBreeze',1,'Davao Main','Door 1, Travellers Inn Bldg, Matina Pangi Rd, Matina Crossing','082 297-1831',''),
	   ('RealWheels',1,'Davao Main','Door 1, Travellers Inn Bldg, Matina Pangi Rd, Matina Crossing','082 297-1831','');

insert into JobStatus([Status]) values('INQUIRY'),('RESERVATION'),('CONFIRMED'),('CLOSED'),('CANCELLED'),('TEMPLATE');
insert into JobThrus([Desc]) values('PHONE'),('EMAIL'),('WALKIN');

insert into Banks([BankName],[BankBranch],[AccntName],[AccntNo])
 values ('Cash','Davao','Cash','0'),
		('BDO','SM-Ecoland Davao','AJ88 Car Rental Services','00 086 072 9575'),
		('BPI','SM-Ecoland Davao','Abel S. Salvatierra','870 303 5125'),
		('Personal Guarantee','Realbreeze-Davao','Personal Guarantee','0'),
		('Paypal','RealWheels-Paypal','Paypal','0');

insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values('<< New Customer >>','--','--',' ',' ','ACT');
insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values('RealBreeze-Davao','realbreezedavao@gmail.com','Elvie/0916-755-8473','','','ACT');
insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values('John Doe','johndoe@gmail.com','09123456789','','','ACT');
	
insert Into Destinations([Description],[Remarks],[CityId]) 
values 
('Eden Nature Park','','1 '),
('Philippine Eagle','','1 '),
('Malagos Garden'  ,'','1 '),
('Japanese Tunnel' ,'','1 '),
('Peoples Park' ,'','1 '),
('Jacks Ridge' ,'','1 '),
('Apo ni Lola' ,'','1 '),
('Aldivenco Shopping Center' ,'','1 ');


-- 
-- Dumping data for table Countries(Code, Name)
-- 
INSERT INTO Countries( Code, Name) VALUES ( 'AF', 'Afghanistan');
INSERT INTO Countries( Code, Name) VALUES ( 'AU', 'Australia');
INSERT INTO Countries( Code, Name) VALUES ( 'BR', 'Brazil');
INSERT INTO Countries( Code, Name) VALUES ( 'CA', 'Canada');
INSERT INTO Countries( Code, Name) VALUES ( 'CN', 'China');
INSERT INTO Countries( Code, Name) VALUES ( 'EC', 'Ecuador');
INSERT INTO Countries( Code, Name) VALUES ( 'EG', 'Egypt');
INSERT INTO Countries( Code, Name) VALUES ( 'FR', 'France');
INSERT INTO Countries( Code, Name) VALUES ( 'GR', 'Greece');
INSERT INTO Countries( Code, Name) VALUES ( 'HK', 'Hong Kong');
INSERT INTO Countries( Code, Name) VALUES ( 'ID', 'Indonesia');
INSERT INTO Countries( Code, Name) VALUES ( 'IQ', 'Iraq');
INSERT INTO Countries( Code, Name) VALUES ( 'IE', 'Ireland');
INSERT INTO Countries( Code, Name) VALUES ( 'IL', 'Israel');
INSERT INTO Countries( Code, Name) VALUES ( 'IT', 'Italy');
INSERT INTO Countries( Code, Name) VALUES ( 'KR', 'Korea, Republic of');
INSERT INTO Countries( Code, Name) VALUES ( 'MY', 'Malaysia');
INSERT INTO Countries( Code, Name) VALUES ( 'NP', 'Nepal');
INSERT INTO Countries( Code, Name) VALUES ( 'NL', 'Netherlands');
INSERT INTO Countries( Code, Name) VALUES ( 'NZ', 'New Zealand');
INSERT INTO Countries( Code, Name) VALUES ( 'NO', 'Norway');
INSERT INTO Countries( Code, Name) VALUES ( 'OM', 'Oman');
INSERT INTO Countries( Code, Name) VALUES ( 'PK', 'Pakistan');
INSERT INTO Countries( Code, Name) VALUES ( 'PE', 'Peru');
INSERT INTO Countries( Code, Name) VALUES ( 'PH', 'Philippines');
INSERT INTO Countries( Code, Name) VALUES ( 'RU', 'Russian Federation');
INSERT INTO Countries( Code, Name) VALUES ( 'SA', 'Saudi Arabia');
INSERT INTO Countries( Code, Name) VALUES ( 'SE', 'Sweden');
INSERT INTO Countries( Code, Name) VALUES ( 'CH', 'Switzerland');
INSERT INTO Countries( Code, Name) VALUES ( 'TW', 'Taiwan');
INSERT INTO Countries( Code, Name) VALUES ( 'TH', 'Thailand');
INSERT INTO Countries( Code, Name) VALUES ( 'UA', 'Ukraine');
INSERT INTO Countries( Code, Name) VALUES ( 'AE', 'United Arab Emirates');
INSERT INTO Countries( Code, Name) VALUES ( 'GB', 'United Kingdom');
INSERT INTO Countries( Code, Name) VALUES ( 'US', 'United States');

-- ------------------------------------------------------------
-- Sales Lead Configuration
-- ------------------------------------------------------------

insert into CustCategories([Name],[iconPath])
values ('PRIORITY','Images/Customers/Category/star-filled-40.png'),
	   ('ACTIVE','Images/Customers/Category/Active-30.png'),
	   ('SUSPENDED','Images/Customers/Category/suspended-64.png'),
	   ('BAD ACCOUNT','Images/Customers/Category/cancel-40.png'),
	   ('CAR-RENTAL','Images/Customers/Category/Active-30.png'),
	   ('TOUR','Images/Customers/Category/star-filled-40.png'),
	   ('CLIENT','Images/Customers/Category/Active-30.png'),
	   ('COMPANY','Images/Customers/Category/star-filled-40.png'); 
	   
insert into CustEntAccountTypes ([Name],[SysCode]) values
('Regular','Company');

insert into CustEntMains([Name],[Address],[Contact1],[Contact2],[CustEntAccountTypeId])
values ('NEW (not yet defined)',' ',' ',' ',1);

insert into SalesStatusTypes([Type])
values  ('ALL'),('SALES'),('PROCUREMENT');

insert into SalesStatusCodes([SeqNo],[Name],[OrderNo],[SalesStatusTypeId])
values (1,'NEW', 1, 2), (2,'ASSESMENT', 2, 2), (3, 'PROPOSAL SENT', 3, 2), (4, 'NEGOTIATION', 4, 2),
	   (5, 'ACCEPTED', 5, 2), (6, 'REJECTED', 6, 2), (7, 'CLOSE', 7, 2);

insert into CustEntActStatus([Status])
values ('Open'), ('For Client Comment'), ('For Meeting'), ('Awarded'), ('Close');

insert into CustEntActTypes([Type])
values ('Others'), ('Indicated Price'), ('Bidding Only'), ('Firm Inquiry'), ('Buying Inquiry');

insert into CustEntActivityTypes([Type],[Points])
values ('Quotation',8), ('Meeting',8), ('Sales',15),('Procurement',8), ('Calls/Email',2), ('Others',1);

insert into SalesActCodes([Name],[Desc],[SysCode],[iconPath],[DefaultActStatus])
values 
('RFQ','Request for quotation', 'RFQ','~/Images/SalesLead/Quotation101.png',1), 
('CALL-REQUEST','Return Call request','CALL REQUEST','~/Images/SalesLead/Phone103.png',1),   
('EMAIL-REQUEST','Request to Check/reply Email','EMAIL REQUEST','~/Images/SalesLead/Email102.jpg',1),   
('CALL-DONE','Call is done', 'CALL DONE','~/Images/SalesLead/Phone103.png',2), 
('MEETING-REQUEST','Schedule an appointment','APPOINTMENT','~/Images/SalesLead/meeting102.jpg',1),   
('MEETING-DONE','Meeting done', 'APPOINTMENT_DONE','~/Images/SalesLead/meeting102.jpg',2); 

insert into SalesActStatus([Name])
values ('REQUEST'),('DONE'),('SUSPEND');

insert into SalesLeadCatCodes([CatName],[SysCode],[iconPath])
values	('Priority','PRIORITY','~/Images/SalesLead/high-importance.png'), 
		('HighMargin','HIGHMARGIN','~/Images/SalesLead/GreenArrow.png'),
		('LongTerm','LONGTERM','~/Images/SalesLead/Longterm.png'), 
		('Corporate','CORPORATE ACCOUNT','~/Images/SalesLead/ShakeHands.png'), 
		('HardOne', 'HARDONE','~/Images/SalesLead/unhappy.jpg');
		
insert into SalesStatusStatus([Status])
values ('Active'), ('Inactive');

-- ----------------------------------------
-- Services Configuration
-- ----------------------------------------
insert into SupplierTypes(Description) 
values ('Rent-a-car'),('Boat'),('Tour'),('Airline'),('Hotel');
insert Into Suppliers([Name],[Contact1],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId]) 
values ('<< New Supplier >>','--',' ', '--','1','1','ACT',1),
	  ('AJ Davao Car Rental','Abel / 0995-085-0158',' ', 'AJDavao88@gmail.com','1','1','ACT',1);

insert into SupplierItems([Description],[SupplierId],[Remarks],[InCharge],[Status]) 
values ('Default','1','Item by supplier','Supplier','ACT');

insert into SupplierActStatus([Status])
values ('Open'), ('For Client Comment'), ('Awarded'), ('Close');

insert into SupplierActivityTypes([Type],[Points])
values ('Job Order',15), ('Procurement',8), ('Meeting',8), ('Close',1);

insert into Services([Name],[Description],[Status]) 
values
('Car Rental','Bus, Car, Van and other Transportation arrangements','1'),
('Boat Rental','Boat Arrangement, Island Hopping','1'),
('Tour Package','Tour Package, Land arrangements','1'),
('AirTicket','Airline Ticket','1'),
('Accommodation','Hotels, Rooms, Houses, etc','1'),
('Activity','Water Rafting, Scuba Diving, Caving','1'),
('Other','Other types of services','1');

insert into SrvActionCodes([CatCode],[SortNo])
values
('Arrangement',1),('Partial Payment',2),('Notification',3),('OnGoing',4),('Billing',5),('Full Payment',6),('Closing',7);

insert into SrvActionItems([Desc],[Remarks],[SortNo],[SrvActionCodeId],[ServicesId])
values
-- Car Rental Activities --
('Arrange Vehicle','',1,1,1),
('Partial Payment','',2,2,1),
('Notify Driver',''  ,3,3,1),
('Notify Operator','',4,3,1),
('Notify Guest',''   ,5,3,1),
('On Progress',''    ,6,4,1),
('Payment',''        ,7,6,1),
('Closing',''        ,8,7,1),

-- Boat Rental --
('Special Request','',1,1,2),
('Partial Payment','',2,2,2),
('Notify Operator','',3,3,2),
('Notify Guest',''   ,4,3,2),
('On Progress',''    ,5,4,2),
('Full Payment',''   ,6,6,2),
('Closing',''        ,7,7,2),

-- Tour Package Activities --
('Special Request',''    ,1,1,3),
('Partial Payment',''    ,1,1,3),
('Book Airticket',''     ,1,1,3),
('Book Transportation','',1,1,3),
('Book Hotel',''         ,1,1,3),
('Book Destinations',''  ,1,1,3),
('Book Meals',''         ,1,1,3),
('Book Tour Guide',''    ,1,1,3),
('Notify Driver',''      ,2,3,3),
('Notify Tour Guide',''  ,3,3,3),
('Notify Guest',''       ,4,3,3),
('On progress',''        ,5,4,3),
('Full Payment',''       ,6,6,3),
('Closing',''            ,7,7,3);


insert into SrvActionItems([Desc],[Remarks],[SortNo],[SrvActionCodeId],[ServicesId])
values

-- Air Ticket --
('Special Request','',1,1,4),
('Partial Payment','',2,2,4),
('Notify Operator','',3,3,4),
('Notify Guest',''   ,4,3,4),
('On Progress',''    ,5,4,4),
('Full Payment',''   ,6,6,4),
('Closing',''        ,7,7,4),

-- Accommodation --
('Special Request','',1,1,5),
('Partial Payment','',2,2,5),
('Notify Operator','',3,3,5),
('Notify Guest',''   ,4,3,5),
('On Progress',''    ,5,4,5),
('Full Payment',''   ,6,6,5),
('Closing',''        ,7,7,5),

-- Activity --
('Special Request','',1,1,6),
('Partial Payment','',2,2,6),
('Notify Operator','',3,3,6),
('Notify Guest',''   ,4,3,6),
('On Progress',''    ,5,4,6),
('Full Payment',''   ,6,6,6),
('Closing',''        ,7,7,6),


-- Others --
('Special Request','',1,1,7),
('Partial Payment','',2,2,7),
('Notify Operator','',3,3,7),
('Notify Guest',''   ,4,3,7),
('On Progress',''    ,5,4,7),
('Full Payment',''   ,6,6,7),
('Closing',''        ,7,7,7);


-- ----------------------------------------------
-- Inventory Configuration 
-- ----------------------------------------------
insert into InvItemCats([Name],[Remarks],[ImgPath],[SysCode])
Values
('Vehicle','Vehicle','~/Images/CarRental/car101.png','CAR'),
('Driver','Driver','~/Images/CarRental/Driver102.png','DRIVER'),

('VAN','Any VAN','~/Images/CarRental/car101.png','VAN'),
('Grandia','Grandia','~/Images/CarRental/Van101.jpg','VAN'),
('Super Grandia','Super Grandia','~/Images/CarRental/Van102.jpg','VAN'),

('SUV','SUV/Fortuner/Everest/Innova','~/Images/CarRental/suv-101.png','SUV'),
('MPV','AUV/Innova','~/Images/CarRental/suv-101.png','SUV'),
('Pickup','Pick-up','~/Images/CarRental/car101.png','Pickup'),
('4x4','4x4 Vehicles','~/Images/CarRental/4x4.101.png','4x4'),
('OffRoad','OffRoad Vehicles','~/Images/CarRental/OffRoad101.png','OFFROAD'),
('Sedan','Sedan','~/Images/CarRental/sedan-101.png','Sedan'),

('Others','Other Types','~/Images/CarRental/Repair101.png','OTHER');


insert into InvItems ([ItemCode],[Description],[Remarks],[ContactInfo],[ViewLabel],[OrderNo] )
values
('NA','Unassigned','','','',10),
('RNY301','Toyota Innova','M/T 2.5 Diesel 2013 Brown','','UNIT',100),
('AAF8980','Toyota Innova','M/T 2.5 Diesel 2013 Silver','','UNIT',100),
('NEO380','Toyota Fortuner','A/T 3.0 Diesel 2009 Gold','','UNIT',100),
('ADP22640','Ford Everest','A/T 2.2 Diesel 2016 White','','UNIT',100),
('EOK873','Honda City','A/T 1.5 Gasoline 2018 White','','UNIT',100),
('Abel','Abel Salvatierra','','','DRIVER',200),
('Aeron','Aeron James','','','DRIVER',200),
('Rhean','Rhean Nicole','','','GUIDE',200);

insert into InvItemCategories([InvItemId],[InvItemCatId])
values
(1,7),(2,7),(3,10),(4,6),(5,11),(6,2),(7,2),(8,2);


insert into InvItems ([ItemCode],[Description],[Remarks],[ContactInfo],[ViewLabel],[OrderNo] )
values
('LAF4920','Toyota Innova','M/T 2.5 Diesel 2013 Brown','','UNIT',100),
('GAN7604','Toyota Innova','M/T 2.5 Diesel 2013 Red','','UNIT',100),
('AAF9396 ','Mitsubishi Montero','A/T 3.0 Diesel White','','UNIT',100),
('GAL8852','Toyota Rush','A/T Diesel 2019 Mettalic Gray','','UNIT',100),
('AOA5108','Toyota Grandia','A/T Diesel White','','UNIT',100),
('GAI3946','Nissan Premium','A/T Diesel Gray','','UNIT',100),
('GAN4445','Toyota Grandia Tourer','A/T Diesel White','','UNIT',100),
('GAO4974','Toyota Hilux','A/T Diesel Red','','UNIT',100),
('GAT4724','Toyota Fortuner','A/T Diesel Gray','','UNIT',100),

('Jehiel','Jehiel Bufe','','','DRIVER',200),
('Jeremiah','Jeremiah Tolentino ','','','DRIVER',200),
('Danny','Danny Escaner','','','GUIDE',200),
('Romie','Romie Bajadi','','','GUIDE',200),
('Melchor','Melchor Toribio','','','GUIDE',200),
('Joremly','Joremly Baluya','','','GUIDE',200);

insert into InvCarRecordTypes([Description], [SysCode], [OdoInterval], [DaysInterval], [IconPath], [OrderNo])
values
('Oil Change (Fully Synthetic) ', 'COFS', 10000, 180, '/Images/Icons/Maintenance/icons-oil-industry-black.png', 1),
('Aircon Cleaning', 'AC', 10000, 180, '/Images/Icons/Maintenance/icons-aircon.png', 2),
('Brakepad Change', 'OIL', 10000, 150, '/Images/Icons/Maintenance/icons-brake-discs.png',3),
('Transmission Oil (Automatic)', 'OIL', 10000, 360, '/Images/Icons/Maintenance/icons-oil-industry-white.png',4),
('Tire Change', 'OIL', 10000, 360, '/Images/Icons/Maintenance/icons-change-tire.png',5),
('Brake Shoe', 'OIL', 10000, 700, '/Images/Icons/Maintenance/icons-brake-discs.png',6),
('Gear Oil', 'OIL', 10000, 365, '/Images/Icons/Maintenance/icons-gears.png',7),
('Battery Change', 'OIL', 10000, 547, '/Images/Icons/Maintenance/icons-car-battery.png',8),
('Others', 'OTHERS', 10000, 365, '/Images/Icons/Maintenance/icons-car-battery.png',11),
('Insurance', 'INS', 10000, 365, '/Images/Icons/Maintenance/icons-insurance.png',9),
('Registration', 'OIL', 10000, 365, '/Images/Icons/Maintenance/icons-registration.png',10)
;

insert into InvCarMntPriorities([Priority],[Order],[IconSrc])
values 
('LOW',1,'/Images/Icons/Maintenance/icons-low-priority.png'),
('REGULAR',2,'/Images/Icons/Maintenance/icons-mid-priority.png'),
('HIGH',3,'/Images/Icons/Maintenance/icons-high-priority.png');


insert into InvCarRcmdStatus([Status])
values ('Request'),('Approved'),('Cancel');

-- linking jobs vechiles to triplogs units
insert into InvItemCrLogUnits([InvItemId], [CrLogUnitId]) 
values 
(2,1),
(3,2),
(4,6),
(5,5),
(6,8);

-- Sample Maintenance Records --
insert into InvCarRecords([InvItemId], [InvCarRecordTypeId], [Odometer], [dtDone], [NextOdometer], [NextSched], [Remarks])
values 
(2, 1, 17500, '6/05/2020', 27500, '12/05/2020', ''),
(3, 2, 12500, '8/27/2020', 22300, '04/27/2021', ''),
(2, 3, 17500, '6/05/2020', 22500, '12/05/2020', ''),
(4, 1, 6900,  '3/12/2021', 16900, '9/13/2021', '');

insert into InvCarGateControls([InvItemId], [In_Out_flag], [Odometer], [dtControl], [Remarks], [Driver], [Inspector], [JobMainId], [CustomerId], [DriverId])
values 
(2, 1, 19500, '8/15/2020', '', 'Kelvin', 'Peter', null, null, null),
(2, 1, 21000, '9/15/2020', '', 'Kelvin', 'Peter', null, null, null),
(2, 1, 23500, '10/15/2020', '', 'Kelvin', 'Peter', null, null, null),
(2, 1, 26500, '11/15/2020', '', 'Kelvin', 'Peter', null, null, null),

(3, 1, 14500, '3/15/2021', '', 'Xerxes', 'Peter', null, null, null),
(3, 1, 15500, '4/15/2021', '', 'Xerxes', 'Peter', null, null, null);

-- ----------------------------------------------
-- Supplier PO Configuration
-- ----------------------------------------------
insert into SupplierPoStatus ([Status],[OrderNo]) values ('REQUEST',1),('APPROVED',2),('CANCELLED',2),('CONFIRMED',3),('DELIVERED',4),('CLOSE',5);

-- ----------------------------------------------
-- Paypal Accounts
-- ----------------------------------------------
insert into PaypalAccounts ([SysCode],[Key],[Secret])
values
('Realwheels', 'ASTv_oxNk66nZW4tVTbt78dtocU-70VVoDDmgtdMSzv1Aqmw8QK6lJ01vzn6lO6jPio3DbfbT_6G6F6b' , 'EAYtPcgQYKu5UfA4WV5lzE_iPj1WFiGnPC_8XvSgYrjoISJEnZAezmdcofe5oRyJZzPToJO6QUMlgmS2');

insert into JobMains([JobDate],[CustomerId],[Description],[NoOfPax],[NoOfDays],[JobRemarks],[JobStatusId],[StatusRemarks],[BranchId],[JobThruId],[AgreedAmt],[CustContactEmail],[CustContactNumber],[AssignedTo],[DueDate])
values
('08-20-2022',1,'Test Job 101',4,1,'TEST DATA 0101',3,'N/A',1,1,5000,'test','09123456','jahdielvillosa@gmail.com', null),
('08-22-2022',1,'Test Job 102',3,1,'TEST DATA 0102',3,'N/A',1,1,3000,'test','09123456','jahdielvillosa@gmail.com', null),
('08-26-2022',1,'Test City Tour',3,2,'TEST DATA 0103',2,'N/A',1,1,3500,'test','09123456','jahdielvillosa@gmail.com','09-10-2022'),
('08-25-2022',1,'Davao City Tour',3,2,'Template Test',6,'N/A',1,1,3500,'test','09123456','jahdielvillosa@gmail.com','09-15-2022');


insert into JobServices([JobMainId],[ServicesId],[SupplierId],[Particulars],[QuotedAmt],[SupplierAmt],[ActualAmt],[Remarks],[SupplierItemId],[DtStart],[DtEnd])
values
--Test Job 101--
(1,1,2,'Car Rental sample data R1',5000,5000,5000,'Sample only. Disregard once seen on production',1,'04-20-2020','04-22-2020'),
(1,1,2,'Car Rental sample data R2',3000,3000,3000,'Sample only. Disregard once seen on production',1,'04-24-2020','04-25-2020'),
--Test Job 102--
(2,1,2,'SUV Rental R1',2000,2000,2000,'Sample only. Disregard once seen on production',1,'04-28-2020','04-30-2020'),
(2,1,2,'SUV Rental R2',1000,1000,1000,'Sample only. Disregard once seen on production',1,'05-01-2020','05-05-2020'),
--Test City Tour--
(3,1,2,'MPV Rental sample 1',2000,2000,2000,'Sample only. Disregard once seen on production',1,'05-03-2020','05-05-2020'),
(3,1,2,'MPV Rental sample 2',1000,1000,1000,'Sample only. Disregard once seen on production',1,'05-07-2020','05-10-2020'),
--Davao City Tour--
(4,1,2,'Day 1: Country Side Tour',2000,2000,2000,'Sample only. Disregard once seen on production',1,'01-27-2020','01-27-2020'),
(4,1,2,'Day 2: Country Side Tour',1500,1500,1000,'Sample only. Disregard once seen on production',1,'01-28-2020','01-28-2020');

insert into JobItineraries([JobMainId],[DestinationId],[ActualRate],[Remarks],[ItiDate],[SvcId])
values
(3,1,0,'','01-27-2019',5),
(3,2,0,'','01-27-2019',5),
(3,5,0,'','01-28-2019',6),
(3,6,0,'','01-28-2019',6);

insert into JobTrails([RefTable],[RefId],[dtTrail],[user],[Action],[IPAddress])
values 
('joborder',1,'02-10-2020','demo@gmail.com','Create Job','NA'),
('joborder',2,'02-15-2020','demo@gmail.com','Create Job','NA'),
('joborder',3,'02-20-2020','demo@gmail.com','Create Job','NA');

--insert into InvItems([ItemCode],[Description],[Remarks])
--values ('RNY301','Toyota Innova E M/T 2013 Dsl',''),
--('EOK873','Honda City A/T 2018 1.5E',''),
--('ADP2264','Ford Everest A/T 2016 2.2',''),
--('AbelS','Abel Salvatierra','');

insert into InvItemCategories([InvItemId],[InvItemCatId])
values (1,1), (2,1), (3,1), (4,2);

Insert into JobServiceItems([JobServicesId],[InvItemId])
values		(1,2),(1,7),
			(2,3),(2,7),
			(3,3),(3,8),
			(4,3),(4,8),
			(4,3),(4,8);

-- Supplier PO Samples
insert into SupplierPoHdrs([PoDate],[Remarks],[SupplierId],[SupplierPoStatusId],[RequestBy],[DtRequest])
values ('11-25-2018','Test Po',1,1,'Abel','11-25-2018');

insert into SupplierPoDtls([SupplierPoHdrId],[Remarks],[Amount],[JobServicesId])
values (1,'10 seater vehicle',3500,1), (1,'14 seater vehicle',4000,1);

insert into SupplierUnits([Unit])
values ('Meter'),('Inch'),('Feet'),('Box'),('Package');
insert into SupplierContactStatus([Name]) values ('Active'),('Resigned');

-- Customer PO Samples
insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
values('Juan Dela Cruz','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

insert into CustCats(CustomerId, CustCategoryId) 
values(3,2),(3,1);

--- CustEntMains ----
insert into CustEntMains(Name, Address, Contact1, Contact2, iconPath,AssignedTo, CustEntAccountTypeId) 
values	('Acer Phils Inc.','Davao City','09950753794','09950753794','Images/Customers/Company/organization-40.png','jahdielvillosa@gmail.com',1),
		('Hp Enterprise','Davao City','09950753794','09950753794','Images/Customers/Company/organization-40.png','jahdielvillosa@gmail.com',1),
		('Honda Motors Inc.','Davao City','09950753794','09950753794','Images/Customers/Company/organization-40.png','jahdielvillosa@gmail.com',1);

insert into CustEntAssigns(Assigned, Remarks, Date, CustEntMainId)
values  ('jahdielsvillosa@gmail.com','','2/13/2020',2),
		('jahdielsvillosa@gmail.com','','2/13/2020',3),
		('jahdielsvillosa@gmail.com','','2/13/2020',4);

insert into CustEntities(CustEntMainId, CustomerId) 
values (2,3),(3,3),(4,3);

update CustCategories set iconPath = 'Images/Customers/Category/star-filled-40.png' where Id = 1; 
update CustCategories set iconPath = 'Images/Customers/Category/Active-30.png' where Id = 2; 
update CustCategories set iconPath = 'Images/Customers/Category//suspended-64.png' where Id = 3; 
update CustCategories set iconPath = 'Images/Customers/Category/cancel-40.png' where Id = 4;  

--- CustEntMains : Activities ---
insert into CustEntActActionCodes([Name],[Desc],[SysCode],[IconPath],[DefaultActStatus], [SeqNo])
values 
('RFQ','Request for quotation', 'RFQ','~/Images/SalesLead/Quotation101.png',1, 1), 
('CALL-REQUEST','Return Call request','CALL REQUEST','~/Images/SalesLead/Phone103.png',1, 2),   
('EMAIL-REQUEST','Request to Check/reply Email','EMAIL REQUEST','~/Images/SalesLead/Email102.jpg',1, 3),   
('CALL-DONE','Call is done', 'CALL DONE','~/Images/SalesLead/Phone103.png',2, 4), 
('MEETING-REQUEST','Schedule an appointment','APPOINTMENT','~/Images/SalesLead/meeting102.jpg',1, 5),   
('MEETING-DONE','Meeting done', 'APPOINTMENT_DONE','~/Images/SalesLead/meeting102.jpg',3, 6),   
('AWARDED','Awarded', 'AWARDED','~/Images/SalesLead/meeting102.jpg',4, 7),   
('CLOSED','Closed', 'CLOSED','~/Images/SalesLead/meeting102.jpg',2 ,8); 

insert into CustEntActActionStatus([ActionStatus])
values ('REQUEST'),('DONE'),('SUSPEND');

insert into CustEntActivities (Date, Assigned, ProjectName, SalesCode, Amount, Status, Remarks, CustEntMainId, Type, ActivityType, SalesLeadId, CustEntActActionStatusId, CustEntActActionCodesId, CustEntActStatusId)
values	('03-05-2020', 'jahdielvillosa@gmail.com', 'Test001 Proj', 'TP001', 250000, 'Open', 'N/A', 2, 'Firm Inquiry', 'Quotation', null, 1, 1, 1),
		('03-08-2020', 'jahdielvillosa@gmail.com', 'Test001 Proj', 'TP001', 250000, 'Open', 'N/A', 2, 'Indicated Price', 'Quotation', null, 1, 1, 1),
		('03-10-2020', 'jahdielvillosa@gmail.com', 'Test001 Proj', 'TP001', 0, 'Open', 'N/A', 2, 'Buying Inquiry', 'Meeting', null, 1, 1, 1),
		('03-15-2020', 'jahdielvillosa@gmail.com', 'Test001 Proj', 'TP001', 230000, 'Close', 'N/A', 2, 'Buying Inquiry', 'Sales', null, 1, 1, 1),

		('03-07-2020', 'jahdielvillosa@gmail.com', 'Test002 Proj', 'TP002', 0, 'Open', 'N/A', 3, 'Bidding Only', 'Meeting', null, 1, 1, 1),
		('03-12-2020', 'jahdielvillosa@gmail.com', 'Test002 Proj', 'TP002', 230000, 'Open', 'N/A', 3, 'Buying Inquiry', 'Sales', null, 1, 1, 1),

		('03-18-2020', 'jahdielvillosa@gmail.com', 'Test002 Proj', 'TP002', 230000, 'Open', 'N/A', 4, 'Buying Inquiry', 'Sales', null, 1, 1, 1);



insert into CarCategories (Description, Remarks)
values ('Van',''),('SUV',''),('MPV',''),('Sedan',''),('Pickup','');

insert into CarUnits ( Description, Remarks, CarCategoryId , SelfDrive, SortOrder, Status) 
values 
	   ('Van (14 seater)','Nissan Premium'		,1,1,1, 'ACTIVE'),
	   ('Van (10 seater)','Gl Grandia'			,1,1,2, 'ACTIVE'),
	   ('SUV'            ,'Ford Everest'		,2,0,3, 'ACTIVE'),
	   ('MPV/AUV/MiniVan','Toyota Innova'		,3,0,4, 'ACTIVE'),
	   ('Sedan'          ,'Honda City'			,4,0,5, 'INACTIVE'),
	   ('Pickup'         ,'Pickups'				,5,0,6, 'ACTIVE'),
	   ('Van (14 seater)','GL Grandia Tourer'	,1,1,1, 'ACTIVE'),
	   ('MPV'			 ,'Toyota Rush'			,3,0,3, 'ACTIVE');

insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (1,'glgrandia/Toyota-Grandia-side.jpg'    ,'','MAIN'),
       (2,'nissanPremium/Nissan-Premium-2018.jpg','','MAIN'),
       (3,'ford/ford-everest-front.jpg'			 ,'','MAIN'),
       (4,'innova/toyota-innova.jpg'			 ,'','MAIN'),
       (5,'hondacity/honda-city-front.jpg'       ,'','MAIN'),
       (6,'pickup/pickup-default.jpg'            ,'','MAIN'),
       (7,'tourer/Toyota-Grandia-Tourer-2019-side.jpg' ,'','MAIN'),
       (8,'rush/Toyota-Rush-2019.jpg' ,'','MAIN');

	   
insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (2,'glgrandia/Toyota-Grandia-side.jpg'    ,'','VIEW'),
	   (2,'glgrandia/Toyota-Grandia.jpg'    ,'','VIEW'),
	   (2,'glgrandia/Toyota-Grandia-seats.jpg'    ,'','VIEW'),
	   (2,'glgrandia/Toyota-Grandia-front-seats.jpg'    ,'','VIEW'),

       (1,'nissanPremium/Nissan-Premium-front-side.jpg','','VIEW'),
       (1,'nissanPremium/Nissan-Premium-front.jpg','','VIEW'),
       (1,'nissanPremium/Nissan-Premium-side.jpg','','VIEW'),
       (1,'nissanPremium/Nissan-Premium-2019-back-seat.jpg','','VIEW'),

       (3,'ford/ford-everest-front-side.jpg','','VIEW'),
       (3,'ford/ford-everest-back.jpg'		,'','VIEW'),
       (3,'ford/ford-everest-side.jpg'		,'','VIEW'),
       (3,'ford/fordeverest_interior.jpg'	,'','VIEW'),

       (4,'innova/toyota-innova-2015.jpg'		,'','VIEW'),
       (4,'innova/toyota-innova-2015-front.jpg'	,'','VIEW'),
       (4,'innova/toyota-innova-2015-side.jpg'  ,'','VIEW'),
       (4,'innova/toyotainnova_interior.jpg'    ,'','VIEW'),

       (5,'hondacity/honda-city-2018-front-side.jpg'       ,'','VIEW'),
       (5,'hondacity/honda-city-2018-back-2.jpg'  ,'','VIEW'),
       (5,'hondacity/honda-city-2018-side.jpg'    ,'','VIEW'),
       (5,'hondacity/hondacity_interior.jpg'    ,'','VIEW'),

       (6,'pickup/Toyota-Hilux-2020.jpg'          ,'','VIEW'),
       (6,'pickup/Toyota-Hilux-2020-side.jpg'            ,'','VIEW'),
       (6,'pickup/Toyota-Hilux-2020-back.jpg'            ,'','VIEW'),
       (6,'pickup/Toyota-Hilux-2020-seats.jpg'            ,'','VIEW'),

       (7,'tourer/Toyota-Grandia-Tourer-2019-side.jpg' ,'','VIEW'),
       (7,'tourer/Toyota-Grandia-Tourer-2019-interior.jpg' ,'','VIEW'),
       (7,'tourer/Toyota-Grandia-Tourer-2019-seats.jpg' ,'','VIEW'),
       (7,'tourer/Toyota-Grandia-Tourer-2019-back.jpg' ,'','VIEW'),

       (8,'rush/Toyota-Rush-2019.jpg' ,'','VIEW'),
       (8,'rush/Toyota-Rush-2019-back.jpg' ,'','VIEW'),
       (8,'rush/Toyota-Rush-2019-seat.jpg' ,'','VIEW'),
       (8,'rush/Toyota-Rush-interior.jpg' ,'','VIEW')
	   ;
	   
insert into CarViewPages (CarUnitId, Viewname)
values (1,'CarDetail_van'),
	   (2,'CarDetail_van'),
	   (3,'CarDetail_suv'),
	   (4,'CarDetail_mpv'),
	   (5,'CarDetail_sedan'),
	   (6,'CarDetail_pickup'),
	   (7,'CarDetail_tourer'),
	   (8,'CarDetail_mpv');

insert into CarDetails(CarUnitId,Fuel,Class,Transmission,Usage,Passengers,Remarks) 
values	(1,'2.5/3.0 Diesel ','Van','5 speed MT/AT','','Driver + 14 persons ','3-6 trolleys'),
		(2,'2.5 Diesel','Van','5 speed MT/AT','','Driver + 10-12 persons ','2-4 trolleys'),
		(3,'2.2 Diesel','SUV','6 speed AT','','Driver + 5-7 persons ','2-4 trolleys'),
		(4,'2.5 Diesel','MPV','MT','','Driver + 5-7 persons','2-4 trolleys'),
		(5,'1.5 Diesel','Sedan','AT','','Driver + 4 persons','2 trolleys'),
		(6,'2.5 Diesel','Pickup','MT','','Driver + 4 persons',' '),
		(7,'2.8 Diesel','Van','6 Speed MT/AT','','Driver + 14 persons','2-4 trolleys'),
		(8,'2.5 Diesel','MPV','MT','','Driver + 5-7 persons','2-4 trolleys')
;

insert into CarRates (Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate)
values (3000,2500,2250,100,5,1,300), --grandia
	   (3500,2500,2250,100,5,2,300), --premium
	   (3000,2500,2250,100,5,3,300), --everest
	   (2500,1800,1500,100,5,4,300), --innova
	   (2500,1800,1500,100,5,5,300), --honda
	   (3500,2500,2250,100,5,6,300), --grandia
	   (3500,2500,2250,100,5,7,300), --grandia
	   (2500,2500,2250,100,5,8,300); --rush



insert into CarUnitMetas(carUnitId,PageTitle,MetaDesc,HomeDesc)
values
	-- Van -- 
	(1, 'Vehicle (Toyota Grandia GL) for Rent in Davao City, Reliable car rental company in Davao',
	'Toyota Grandia GL is 10-12 seater van for business, tour and family travel needs. Car Rental offers affordable and flexible rental rates',
	'Comfortable 10 Seater van for rent for business and family occasions. Popular unit for Rent-A-Car in Davao.'
	),

	-- Nissan Premium -- 
	(2, 'Vehicle (Nissan Urvan Premium) for Rent in Davao City, Reliable van rental company in Davao City',
	'Nissan Urvan Premium is comfortable 10-14 seater van for business, tour and family travel needs. Very few rent-a-car company in Davao offers this type of vehicle',
	'Highroof van that can accommodate 14pax with individual reclining seats. No jump seats. Very few rent-a-car company in Davao offers this type of vehicle.'
	),

	-- SUVs -- 
	(3, 'SUV (Sports Utility Vehicle) for Rent in Davao City, Rent-A-Car company in Davao City',
	'SUV for rent in Davao City for your business needs and personal needs that can accommodate 7 persons. Open for SelfDrive or with driver rental option. Available units: Ford Everest, Toyota Fortuner, Toyota Innova',
	'SUV for business and personal needs that can accommodate 7persons. Units: Ford Everest, Toyota Fortuner, Toyota Innova.'
	),

	-- AUVs-- 
	(4, 'MPV (Multi purpose vehicle) for Rent in Davao City, Rent-A-Car company in Davao City',
	'MPV/AUV - Toyota Innova - for rent in Davao City for your travel needs. Open for SelfDrive or with driver rental option.',
	'Toyota Innova is a versatile vehicle for family and business use. Accommodates like a small van or SUV yet rides like a car.'
	),

	-- Sedan -- 
	(5, 'Sedan for Rent in Davao City, Car Rental company in Davao City',
	'Sedan - Honda City - for rent in Davao City for your business and travel needs. Open for SelfDrive or with driver rental option.',
	'Sedan is a practical vehicle for light travel for personal and business use. Available units: Honda City, Toyota Vios'
	),

	-- Pickup -- 
	(6, 'Pickup for Rent in Davao City, 4x4 rental, rent-a-car',
	'Pickup 4x4 for rent in Davao City for difficult terrain or bigger luggages. Units: Mitsubishi strada 4x4, Toyota Hilux 4x4',
	'Pickup 4x4 is best for difficult and unknown terrains. Can also use in hauling huge luggages.'
	),

	-- Toyota Touer -- 
	(7, 'Vehicle (Nissan Urvan Tourer) for Rent in Davao City, Reliable van rental company in Davao City',
	'Nissan Urvan Premium is comfortable 10-14 seater van for business, tour and family travel needs. Very few rent-a-car company in Davao offers this type of vehicle',
	'Highroof van that can accommodate 14pax with individual reclining seats. No jump seats. Very few rent-a-car company in Davao offers this type of vehicle.'
	),

	-- Toyota Rush -- 
	(8, 'Toyota Rush for Rent in Davao City, Reliable rental company in Davao City',
	'MPV/AUV - Toyota Innova - for rent in Davao City for your travel needs. Open for SelfDrive or with driver rental option.',
	'Toyota Innova is a versatile vehicle for family and business use. Accommodates like a small van or SUV yet rides like a car.'
	);

	Insert into CarRatePackages (Description,Remarks,DailyMeals,DailyRoom,DaysMin, Status)
	values 
	('Custom',           'Fuel by renter', 0,0,0,    'SYS'),
	('Davao City Tour',  'Car Rental for City (Downtown) Tour Package', 0,0,0,'ACT' ),
	('Davao CountrySide Tour', 'Eden Nature Park, Philipine Eagle, Malagos, Japanese Tunnel, Shrine, Jacks Ridge', 0,0,0,'ACT'),
	('Samal Tour',       'Bat cave, Hagimit falls, Maxima Aquafun, Penaplata', 0,0,0,'ACT' ),
	('Panabo',           'One round trip',300,400,1, 'ACT'), 
	('Tagum',            'One round trip',300,400,1, 'ACT'), 
	('Davao Del Norte',  'One round trip',300,400,1, 'ACT'), 
	('Comval',           'One round trip',300,400,1, 'ACT'), 
	('Governor Generoso','One round trip',300,400,1, 'INC'),
	('Mati City',        'One round trip',300,400,1, 'ACT'),
	('Davao Oriental',   'One round trip',300,400,1, 'INC'),
	('Agusan Del Sur',   'One round trip',300,400,1, 'INC'),
	('Agusan Del Norte', 'One round trip',300,400,1, 'INC'),
	('Surigao Del Sur',  'One round trip',300,400,1, 'INC'),
	('Surigao Del Norte','One round trip',300,400,1, 'INC'),

	('Marilog',          'One round trip', 0,0,0 ,   'ACT'),
	('Buda',             'Seagull',        0,0,0 ,   'ACT'),
	('Valencia',         'One round trip', 0,0,0 ,   'INC'),
	('Malaybalay',       'One round trip', 0,0,0 ,   'INC'),
	('Manolo fortich',   'One round trip', 0,0,0 ,   'INC'),
	('Cagayan De Oro',   'One round trip', 0,0,0 ,   'INC'),
	('Misamis Oriental', 'One round trip', 0,0,0 ,   'INC'),
	('Iligan',           'One round trip', 0,0,0 ,   'INC'),

	('Santa Cruz',       'Davao Del Sur. One round trip', 0,0,0 , 'ACT' ),
	('Digos',            'Davao Del Sur. One round trip', 0,0,0 , 'ACT' ),
	('Davao Del Sur',    'One round trip', 0,0,0 , 'ACT' ),
	('Malita',           'One round trip', 0,0,0 , 'ACT' ),
	('Don Marcelino',    'One round trip', 0,0,0 , 'INC' ),
	('General Santos',   'One round trip', 0,0,0 , 'ACT' ),
	('Sarangani',        'One round trip', 0,0,0 , 'INC' ),
	('Koronadal',        'One round trip', 0,0,0 , 'INC' ),
	('Isulan',           'One round trip', 0,0,0 , 'INC' ),
	('Sultan Kudarat',   'One round trip', 0,0,0 , 'INC' ),

	('Kidapawan',        'Kidapawan. One round trip',     0,0,0 , 'ACT' ),
	('Ilomavis',         'Ilomavis. One round trip',      0,0,0 , 'INC' ),
	('Kabacan',          'Kabacan. One round trip',       0,0,0 , 'INC' ),
	('Arakan',           'Arakan. One round trip',        0,0,0 , 'INC' ),
	('Midsayap',         'Midsayap. One round trip',      0,0,0 , 'INC' ),
	('Cotabato City',    'Cotabato City. One round trip', 0,0,0 , 'INC' ),
	('North Cotabato',   'Kidapawan. One round trip',     0,0,0 , 'INC' )
	;

Insert into CarRateUnitPackages (CarUnitId,CarRatePackageId,DailyRate,FuelLonghaul,FuelDaily,DailyAddon, Status)
values
-- regular van ( Grandia GL )
( 1, 1, 0,  0,   0,   0 , 'ACTIVE'), --+ selfdrive
( 1, 2, 3500,  0, 500,   0 , 'DEFAULT'), --+ city
( 1, 3, 0,  0, 800, 500 , 'ACTIVE'), --+ Countryside
( 1, 4, 0,  0, 800, 500 , 'ACTIVE'), --+ Samal

( 1, 5,	 0, 0, 500, 1000 , 'ACTIVE'), --panabo
( 1, 6,  0, 0, 500, 1000 , 'ACTIVE'), --tagum
( 1, 7,  0, 0, 500, 700  , 'ACTIVE'), --davao del norte
( 1, 8,  0, 0, 700, 2000 , 'ACTIVE'), --comval
( 1, 9,  0, 0, 500, 2000 , 'ACTIVE'), --govgen
( 1, 10, 0, 0, 1500, 2000 , 'ACTIVE'), --Mati
( 1, 11, 1000, 2000, 700, 1000 , 'ACTIVE'), --Davao Oriental
( 1, 12, 1500, 2500, 700, 1000 , 'ACTIVE'), --Agusan del sur
( 1, 13, 2000, 2500, 700, 1000 , 'ACTIVE'), --Agusan del Norte
( 1, 14, 1500, 2500, 700, 1000 , 'ACTIVE'), --Surigao del sur
( 1, 15, 2500, 3500, 700, 1000 , 'ACTIVE'), --Surigao del Norte

( 1, 16,    0,    0, 600, 1000 , 'ACTIVE'),  --Marilog
( 1, 17,    0,  400, 600, 1000 , 'ACTIVE'),  --Buda
( 1, 18,    0,  900, 600, 1000 , 'ACTIVE'), --Valencia
( 1, 19, 1000, 1500, 600, 1000 , 'ACTIVE'), --Malaybalay
( 1, 20, 1000, 2000, 600, 1000 , 'ACTIVE'), --Manolo fortich
( 1, 21, 1500, 2500, 600, 1000 , 'ACTIVE'), --Cagayan
( 1, 22, 2000, 3000, 700, 1000 , 'ACTIVE'), --Misamis
( 1, 23, 2000, 3000, 700, 1000 , 'ACTIVE'), --Iligan

( 1, 24, 0,   0, 600, 1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 1, 25, 0,   0, 500, 1000 , 'ACTIVE'), --Digos, davao del sur
( 1, 26, 0,   0, 700, 1000 , 'ACTIVE'), --davao del sur
( 1, 27, 0,   0, 1000, 2000 , 'ACTIVE'), --Malita
( 1, 28, 500, 1000, 600,1000 , 'ACTIVE'), --Don Marcelino
( 1, 29, 500,    0, 500, 1500 , 'ACTIVE'), --General Santos
( 1, 30, 500, 1400, 700,1000  , 'ACTIVE'), --Saranggani
( 1, 31, 500, 1500, 600,1000  , 'ACTIVE'), --Koronadal
( 1, 32, 500, 1500, 700,1000  , 'ACTIVE'), --Isulan
( 1, 33, 500, 1500, 700,1000  , 'ACTIVE'), --Sultan Kudarat
( 1, 34,   0,    0, 1000,2000 , 'ACTIVE'), --Kidapawan
( 1, 35, 500, 1000, 600,1000  , 'ACTIVE'), --Ilomavis
( 1, 36, 500, 1000, 600,1000  , 'ACTIVE'), --Kabacan
( 1, 37, 1000, 1200, 600,1000 , 'ACTIVE'), --Arakan
( 1, 38, 1000, 1500, 600,1000 , 'ACTIVE'), --Midsayap
( 1, 39, 1500, 2000, 600,1000 , 'ACTIVE'), --Cotabato City
( 1, 40, 1500, 2000, 700,1000 , 'ACTIVE'), --North Cotabato

-- big van ( Nissan Premium)
( 2, 1, 0,  0,   0,   0 , 'ACTIVE'), --+ selfdrive
( 2, 2, 3500,  0, 500,   0 , 'DEFAULT'), --+ city
( 2, 3, 0,  0, 800, 1200 , 'ACTIVE'), --+ Countryside
( 2, 4, 0,  0, 800, 1200 , 'ACTIVE'), --+ Samal

( 2, 5,	 0,    0, 500, 1200 , 'ACTIVE'), --panabo
( 2, 6,  0,  200, 500, 1000 , 'ACTIVE'), --tagum
( 2, 7,  0,  700, 700, 300  , 'ACTIVE'), --davao del norte
( 2, 8,  0,  700, 700, 1800 , 'ACTIVE'), --comval
( 2, 9,  0,  900, 600, 1000 , 'ACTIVE'), --govgen
( 2, 10,  200,  900, 600, 1700 , 'ACTIVE'), --Mati
( 2, 11, 1200, 2000, 700, 1000 , 'ACTIVE'), --Davao Oriental
( 2, 12, 1700, 2500, 700, 1000 , 'ACTIVE'), --Agusan del sur
( 2, 13, 2200, 2500, 700, 1000 , 'ACTIVE'), --Agusan del Norte
( 2, 14, 1700, 2500, 700, 1000 , 'ACTIVE'), --Surigao del sur
( 2, 15, 2700, 3500, 700, 1000 , 'ACTIVE'), --Surigao del Norte

( 2, 16,    0,    0, 600, 1000 , 'ACTIVE'), --Marilog
( 2, 17,    0,  400, 600, 1000 , 'ACTIVE'), --Buda
( 2, 18,    0,  900, 600, 1000 , 'ACTIVE'), --Valencia
( 2, 19, 1200, 1500, 600, 1000 , 'ACTIVE'), --Malaybalay
( 2, 20, 1200, 2000, 600, 1000 , 'ACTIVE'), --Manolo fortich
( 2, 21, 1700, 2500, 600, 1000 , 'ACTIVE'), --Cagayan
( 2, 22, 2200, 3000, 700, 1000 , 'ACTIVE'), --Misamis
( 2, 23, 2200, 3000, 700, 1000 , 'ACTIVE'), --Iligan

( 2, 24, 0, 0, 600,1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 2, 25, 0, 400, 600,1000 , 'ACTIVE'), --Digos, davao del sur
( 2, 26, 0, 600, 700,1000 , 'ACTIVE'), --davao del sur
( 2, 27, 0, 900, 600,1000 , 'ACTIVE'), --Malita
( 2, 28, 700, 1000, 600,1000 , 'ACTIVE'), --Don Marcelino
( 2, 29, 700, 1000, 600,1000 , 'ACTIVE'), --General Santos
( 2, 30, 700, 1400, 700,1000 , 'ACTIVE'), --Saranggani
( 2, 31, 700, 1500, 600,1000 , 'ACTIVE'), --Koronadal
( 2, 32, 700, 1500, 700,1000 , 'ACTIVE'), --Isulan
( 2, 33, 700, 1500, 700,1000 , 'ACTIVE'), --Sultan Kudarat
( 2, 34,   0, 1000, 600,1000 , 'ACTIVE'), --Kidapawan
( 2, 35, 700, 1000, 600,1000 , 'ACTIVE'), --Ilomavis
( 2, 36, 700, 1000, 600,1000 , 'ACTIVE'), --Kabacan
( 2, 37, 1200, 1200, 600,1000 , 'ACTIVE'), --Arakan
( 2, 38, 1200, 1500, 600,1000 , 'ACTIVE'), --Midsayap
( 2, 39, 1700, 2000, 600,1000 , 'ACTIVE'), --Cotabato City
( 2, 40, 1200, 2000, 700,1000 , 'ACTIVE'), --North Cotabato

-- SUV ( Ford Everest / fortuner )
( 3, 1, 0,  0,   0, 0 , 'ACTIVE'), --+ selfdrive
( 3, 2, 3000 , 0, 500, 0 , 'DEFAULT'), --+ city
( 3, 3, 0,  0, 800, 300 , 'ACTIVE'), --+ Cuntryside
( 3, 4, 0,  0, 800, 300 , 'ACTIVE'), --+ Samal

( 3, 5,	 0,    0, 500, 1000 , 'ACTIVE'), --panabo
( 3, 6,  0,  200, 500, 1000 , 'ACTIVE'), --tagum
( 3, 7,  0,  700, 700, 1000 , 'ACTIVE'), --davao del norte
( 3, 8,  0,  700, 700, 1000 , 'ACTIVE'), --comval
( 3, 9,  0,  900, 600, 1000 , 'ACTIVE'), --govgen
( 3, 10, 0,  900, 600, 1000 , 'ACTIVE'), --Mati
( 3, 11, 1000, 2000, 700, 1000 , 'ACTIVE'), --Davao Oriental
( 3, 12, 1500, 2500, 700, 1000 , 'ACTIVE'), --Agusan del sur
( 3, 13, 2000, 2500, 700, 1000 , 'ACTIVE'), --Agusan del Norte
( 3, 14, 1500, 2500, 700, 1000 , 'ACTIVE'), --Surigao del sur
( 3, 15, 2500, 3500, 700, 1000 , 'ACTIVE'), --Surigao del Norte

( 3, 16,    0,    0, 600,1000 , 'ACTIVE'), --Marilog
( 3, 17,    0,  400, 600,1000 , 'ACTIVE'), --Buda
( 3, 18,    0,  900, 600,1000 , 'ACTIVE'), --Valencia
( 3, 19, 1000, 1500, 600,1000 , 'ACTIVE'), --Malaybalay
( 3, 20, 1000, 2000, 600,1000 , 'ACTIVE'), --Manolo fortich
( 3, 21, 1500, 2500, 600,1000 , 'ACTIVE'), --Cagayan
( 3, 22, 2000, 3000, 700,1000 , 'ACTIVE'), --Misamis
( 3, 23, 2000, 3000, 700,1000 , 'ACTIVE'), --Iligan

( 3, 24, 0, 0, 600,1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 3, 25, 0, 400, 600,1000 , 'ACTIVE'), --Digos, davao del sur
( 3, 26, 0, 600, 700,1000 , 'ACTIVE'), --davao del sur
( 3, 27, 0, 900, 600,1000 , 'ACTIVE'), --Malita
( 3, 28, 500, 1000, 600,1000 , 'ACTIVE'), --Don Marcelino
( 3, 29, 500, 1000, 600,1000 , 'ACTIVE'), --General Santos
( 3, 30, 500, 1400, 700,1000 , 'ACTIVE'), --Saranggani
( 3, 31, 500, 1500, 600,1000 , 'ACTIVE'), --Koronadal
( 3, 32, 500, 1500, 700,1000 , 'ACTIVE'), --Isulan
( 3, 33, 500, 1500, 700,1000 , 'ACTIVE'), --Sultan Kudarat
( 3, 34,   0, 1000, 600,1000 , 'ACTIVE'), --Kidapawan
( 3, 35, 500, 1000, 600,1000 , 'ACTIVE'), --Ilomavis
( 3, 36, 500, 1000, 600,1000 , 'ACTIVE'), --Kabacan
( 3, 37, 1000, 1200, 600,1000 , 'ACTIVE'), --Arakan
( 3, 38, 1000, 1500, 600,1000 , 'ACTIVE'), --Midsayap
( 3, 39, 1500, 2000, 600,1000 , 'ACTIVE'), --Cotabato City
( 3, 40, 1500, 2000, 700,1000 , 'ACTIVE'), --North Cotabato

-- MPV ( Innova )
( 4, 1, 0,  0,   0,   0 , 'ACTIVE'), --+ selfdrive
( 4, 2, 2500,  0, 500, 300 , 'DEFAULT'), --+ city
( 4, 3, 0,  0, 800, 1000 , 'ACTIVE'), --+ Countryside
( 4, 4, 0,  0, 800, 1000 , 'ACTIVE'), --+ Samal

( 4, 5,	 0,    0, 500, 1000 , 'ACTIVE'), --panabo
( 4, 6,  0,    0, 500, 1000 , 'ACTIVE'), --tagum
( 4, 7,  0,  700, 500, 1000 , 'ACTIVE'), --davao del norte
( 4, 8,  0,  700, 500, 1000 , 'ACTIVE'), --comval
( 4, 9,  0,  900, 500, 1000 , 'ACTIVE'), --govgen
( 4, 10, 0,    0, 1400, 2500 , 'ACTIVE'), --Mati
( 4, 11, 1000, 2000, 500, 1000 , 'ACTIVE'), --Davao Oriental
( 4, 12, 1500, 2500, 500, 1000 , 'ACTIVE'), --Agusan del sur
( 4, 13, 2000, 2500, 500, 1000 , 'ACTIVE'), --Agusan del Norte
( 4, 14, 1500, 2500, 500, 1000 , 'ACTIVE'), --Surigao del sur
( 4, 15, 2500, 3500, 500, 1000 , 'ACTIVE'), --Surigao del Norte

( 4, 16,    0,    0, 500,1000 , 'ACTIVE'), --Marilog
( 4, 17,    0,  400, 500,1000 , 'ACTIVE'), --Buda
( 4, 18,    0,  900, 500,1000 , 'ACTIVE'), --Valencia
( 4, 19, 1000, 1500, 500,1000 , 'ACTIVE'), --Malaybalay
( 4, 20, 1000, 2000, 500,1000 , 'ACTIVE'), --Manolo fortich
( 4, 21, 1500, 2500, 500,1000 , 'ACTIVE'), --Cagayan
( 4, 22, 2000, 3000, 500,1000 , 'ACTIVE'), --Misamis
( 4, 23, 2000, 3000, 500,1000 , 'ACTIVE'), --Iligan

( 4, 24, 0,   0, 500,1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 4, 25, 0,   0, 500,1000 , 'ACTIVE'), --Digos, davao del sur
( 4, 26, 0,   0, 500,   0 , 'ACTIVE'), --davao del sur
( 4, 27, 0,   0, 500,2500 , 'ACTIVE'), --Malita
( 4, 28, 500, 1000, 500,1000 , 'ACTIVE'), --Don Marcelino
( 4, 29, 500,    0, 500,2000 , 'ACTIVE'), --General Santos
( 4, 30, 500, 1400, 500,1000 , 'ACTIVE'), --Saranggani
( 4, 31, 500, 1500, 500,1000 , 'ACTIVE'), --Koronadal
( 4, 32, 500, 1500, 500,1000 , 'ACTIVE'), --Isulan
( 4, 33, 500, 1500, 500,1000 , 'ACTIVE'), --Sultan Kudarat
( 4, 34,   0,    0, 500,2500 , 'ACTIVE'), --Kidapawan 
( 4, 35, 500, 1000, 500,1000 , 'ACTIVE'), --Ilomavis
( 4, 36, 500, 1000, 500,1000 , 'ACTIVE'), --Kabacan
( 4, 37, 1000, 1200, 500,1000 , 'ACTIVE'), --Arakan
( 4, 38, 1000, 1500, 500,1000 , 'ACTIVE'), --Midsayap
( 4, 39, 1500, 2000, 500,1000 , 'ACTIVE'), --Cotabato City
( 4, 40, 1500, 2000, 500,1000 , 'ACTIVE'), --North Cotabato

-- Sedan ( Honda City )
( 5, 1, 0,  0,   0, 0 , 'ACTIVE'), --+ selfdrive
( 5, 2, 2500,  0, 500,   0 , 'DEFAULT'), --+ city
( 5, 3, 0,  0, 800, 300 , 'ACTIVE'), --+ Cuntryside
( 5, 4, 0,  0, 800, 300 , 'ACTIVE'), --+ Samal
( 5, 5,	 0,    0, 500, 1000 , 'ACTIVE'), --panabo
( 5, 6,  0,  200, 500, 1000 , 'ACTIVE'), --tagum
( 5, 7,  0,  700, 500, 1000 , 'ACTIVE'), --davao del norte
( 5, 8,  0,  700, 500, 1000 , 'ACTIVE'), --comval
( 5, 9,  0,  900, 500, 1000 , 'ACTIVE'), --govgen
( 5, 10, 0,  900, 500, 1000 , 'ACTIVE'), --Mati
( 5, 11, 1000, 2000, 500, 1000 , 'ACTIVE'), --Davao Oriental
( 5, 12, 1500, 2500, 500, 1000 , 'ACTIVE'), --Agusan del sur
( 5, 13, 2000, 2500, 500, 1000 , 'ACTIVE'), --Agusan del Norte
( 5, 14, 1500, 2500, 500, 1000 , 'ACTIVE'), --Surigao del sur
( 5, 15, 2500, 3500, 500, 1000 , 'ACTIVE'), --Surigao del Norte

( 5, 16,    0,    0, 500,1000 , 'ACTIVE'), --Marilog
( 5, 17,    0,  400, 500,1000 , 'ACTIVE'), --Buda
( 5, 18,    0,  900, 500,1000 , 'ACTIVE'), --Valencia
( 5, 19, 1000, 1500, 500,1000 , 'ACTIVE'), --Malaybalay
( 5, 20, 1000, 2000, 500,1000 , 'ACTIVE'), --Manolo fortich
( 5, 21, 1500, 2500, 500,1000 , 'ACTIVE'), --Cagayan
( 5, 22, 2000, 3000, 500,1000 , 'ACTIVE'), --Misamis
( 5, 23, 2000, 3000, 500,1000 , 'ACTIVE'), --Iligan

( 5, 24, 0, 0, 500,1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 5, 25, 0, 400, 500,1000 , 'ACTIVE'), --Digos, davao del sur
( 5, 26, 0, 600, 500,1000 , 'ACTIVE'), --davao del sur
( 5, 27, 0, 900, 500,1000 , 'ACTIVE'), --Malita
( 5, 28, 500, 1000, 500,1000 , 'ACTIVE'), --Don Marcelino
( 5, 29, 500, 1000, 500,1000 , 'ACTIVE'), --General Santos
( 5, 30, 500, 1400, 500,1000 , 'ACTIVE'), --Saranggani
( 5, 31, 500, 1500, 500,1000 , 'ACTIVE'), --Koronadal
( 5, 32, 500, 1500, 500,1000 , 'ACTIVE'), --Isulan
( 5, 33, 500, 1500, 500,1000 , 'ACTIVE'), --Sultan Kudarat
( 5, 34,   0, 1000, 500,1000 , 'ACTIVE'), --Kidapawan
( 5, 35, 500, 1000, 500,1000 , 'ACTIVE'), --Ilomavis
( 5, 36, 500, 1000, 500,1000 , 'ACTIVE'), --Kabacan
( 5, 37, 1000, 1200, 500,1000 , 'ACTIVE'), --Arakan
( 5, 38, 1000, 1500, 600,1000 , 'ACTIVE'), --Midsayap
( 5, 39, 1500, 2000, 600,1000 , 'ACTIVE'), --Cotabato City
( 5, 40, 1500, 2000, 700,1000 , 'ACTIVE'), --North Cotabato

-- Pickup ( strada / hilux )
( 6, 1, 0,  0,   0, 0 , 'ACTIVE'), --+ selfdrive
( 6, 2, 3500,  0, 500,   0 , 'DEFAULT'), --+ city
( 6, 3, 0,  0, 800, 300 , 'ACTIVE'), --+ Cuntryside
( 6, 4, 0,  0, 800, 300 , 'ACTIVE'), --+ Samal

( 6, 5,	 0,    0, 500, 1000 , 'ACTIVE'), --panabo
( 6, 6,  0,  200, 500, 1000 , 'ACTIVE'), --tagum
( 6, 7,  0,  700, 700, 1000 , 'ACTIVE'), --davao del norte
( 6, 8,  0,  700, 700, 1000 , 'ACTIVE'), --comval
( 6, 9,  0,  900, 600, 1000 , 'ACTIVE'), --govgen
( 6, 10, 0,  900, 600, 1000 , 'ACTIVE'), --Mati
( 6, 11, 1000, 2000, 700, 1000 , 'ACTIVE'), --Davao Oriental
( 6, 12, 1500, 2500, 700, 1000 , 'ACTIVE'), --Agusan del sur
( 6, 13, 2000, 2500, 700, 1000 , 'ACTIVE'), --Agusan del Norte
( 6, 14, 1500, 2500, 700, 1000 , 'ACTIVE'), --Surigao del sur
( 6, 15, 2500, 3500, 700, 1000 , 'ACTIVE'), --Surigao del Norte

( 6, 16,    0,    0, 600,1000 , 'ACTIVE'), --Marilog
( 6, 17,    0,  400, 600,1000 , 'ACTIVE'), --Buda
( 6, 18,    0,  900, 600,1000 , 'ACTIVE'), --Valencia
( 6, 19, 1000, 1500, 600,1000 , 'ACTIVE'), --Malaybalay
( 6, 20, 1000, 2000, 600,1000 , 'ACTIVE'), --Manolo fortich
( 6, 21, 1500, 2500, 600,1000 , 'ACTIVE'), --Cagayan
( 6, 22, 2000, 3000, 700,1000 , 'ACTIVE'), --Misamis
( 6, 23, 2000, 3000, 700,1000 , 'ACTIVE'), --Iligan

( 6, 24, 0, 0, 600,1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 6, 25, 3500, 400, 600,1000 , 'DEFAULT'), --Digos, davao del sur
( 6, 26, 0, 600, 700,1000 , 'ACTIVE'), --davao del sur
( 6, 27, 0, 900, 600,1000 , 'ACTIVE'), --Malita
( 6, 28, 500, 1000, 600,1000 , 'ACTIVE'), --Don Marcelino
( 6, 29, 500, 1000, 600,1000 , 'ACTIVE'), --General Santos
( 6, 30, 500, 1400, 700,1000 , 'ACTIVE'), --Saranggani
( 6, 31, 500, 1500, 600,1000 , 'ACTIVE'), --Koronadal
( 6, 32, 500, 1500, 700,1000 , 'ACTIVE'), --Isulan
( 6, 33, 500, 1500, 700,1000 , 'ACTIVE'), --Sultan Kudarat
( 6, 34,   0, 1000, 600,1000 , 'ACTIVE'), --Kidapawan
( 6, 35, 500, 1000, 600,1000 , 'ACTIVE'), --Ilomavis
( 6, 36, 500, 1000, 600,1000 , 'ACTIVE'), --Kabacan
( 6, 37, 1000, 1200, 600,1000 , 'ACTIVE'), --Arakan
( 6, 38, 1000, 1500, 600,1000 , 'ACTIVE'), --Midsayap
( 6, 39, 1500, 2000, 600,1000 , 'ACTIVE'), --Cotabato City
( 6, 40, 1500, 2000, 700,1000 , 'ACTIVE'), --North Cotabato


-- Premium ( Toyota Grandia Tourer)
( 7, 1, 0,  0,   0,   0 , 'ACTIVE'), --+ selfdrive
( 7, 2, 3500,  0, 500,   0 , 'DEFAULT'), --+ city
( 7, 3, 0,  0, 800, 1200 , 'ACTIVE'), --+ Countryside
( 7, 4, 0,  0, 800, 1200 , 'ACTIVE'), --+ Samal

( 7, 5,	 0,    0, 500, 1200 , 'ACTIVE'), --panabo
( 7, 6,  0,  200, 500, 1000 , 'ACTIVE'), --tagum
( 7, 7,  0,  700, 700, 300  , 'ACTIVE'), --davao del norte
( 7, 8,  0,  700, 700, 1800 , 'ACTIVE'), --comval
( 7, 9,  0,  900, 600, 1000 , 'ACTIVE'), --govgen
( 7, 10,  200,  900, 600, 1700 , 'ACTIVE'), --Mati
( 7, 11, 1200, 2000, 700, 1000 , 'ACTIVE'), --Davao Oriental
( 7, 12, 1700, 2500, 700, 1000 , 'ACTIVE'), --Agusan del sur
( 7, 13, 2200, 2500, 700, 1000 , 'ACTIVE'), --Agusan del Norte
( 7, 14, 1700, 2500, 700, 1000 , 'ACTIVE'), --Surigao del sur
( 7, 15, 2700, 3500, 700, 1000 , 'ACTIVE'), --Surigao del Norte

( 7, 16,    0,    0, 600, 1000 , 'ACTIVE'), --Marilog
( 7, 17,    0,  400, 600, 1000 , 'ACTIVE'), --Buda
( 7, 18,    0,  900, 600, 1000 , 'ACTIVE'), --Valencia
( 7, 19, 1200, 1500, 600, 1000 , 'ACTIVE'), --Malaybalay
( 7, 20, 1200, 2000, 600, 1000 , 'ACTIVE'), --Manolo fortich
( 7, 21, 1700, 2500, 600, 1000 , 'ACTIVE'), --Cagayan
( 7, 22, 2200, 3000, 700, 1000 , 'ACTIVE'), --Misamis
( 7, 23, 2200, 3000, 700, 1000 , 'ACTIVE'), --Iligan

( 7, 24, 0, 0, 600,1000   , 'ACTIVE'), --Santa Cruz, davao del sur
( 7, 25, 0, 400, 600,1000 , 'ACTIVE'), --Digos, davao del sur
( 7, 26, 0, 600, 700,1000 , 'ACTIVE'), --davao del sur
( 7, 27, 0, 900, 600,1000 , 'ACTIVE'), --Malita
( 7, 28, 700, 1000, 600,1000 , 'ACTIVE'), --Don Marcelino
( 7, 29, 700, 1000, 600,1000 , 'ACTIVE'), --General Santos
( 7, 30, 700, 1400, 700,1000 , 'ACTIVE'), --Saranggani
( 7, 31, 700, 1500, 600,1000 , 'ACTIVE'), --Koronadal
( 7, 32, 700, 1500, 700,1000 , 'ACTIVE'), --Isulan
( 7, 33, 700, 1500, 700,1000 , 'ACTIVE'), --Sultan Kudarat
( 7, 34,   0, 1000, 600,1000 , 'ACTIVE'), --Kidapawan
( 7, 35, 700, 1000, 600,1000 , 'ACTIVE'), --Ilomavis
( 7, 36, 700, 1000, 600,1000 , 'ACTIVE'), --Kabacan
( 7, 37, 1200, 1200, 600,1000 , 'ACTIVE'), --Arakan
( 7, 38, 1200, 1500, 600,1000 , 'ACTIVE'), --Midsayap
( 7, 39, 1700, 2000, 600,1000 , 'ACTIVE'), --Cotabato City
( 7, 40, 1200, 2000, 700,1000 , 'ACTIVE'), --North Cotabato


-- MPV ( Rush )
( 8, 1, 0,  0,   0,   0 , 'ACTIVE'), --+ selfdrive
( 8, 2, 2500,  0, 500, 300 , 'DEFAULT'), --+ city
( 8, 3, 0,  0, 800, 1000 , 'ACTIVE'), --+ Countryside
( 8, 4, 0,  0, 800, 1000 , 'ACTIVE'), --+ Samal

( 8, 5,	 0,    0, 500, 1000 , 'ACTIVE'), --panabo
( 8, 6,  0,    0, 500, 1000 , 'ACTIVE'), --tagum
( 8, 7,  0,  700, 500, 1000 , 'ACTIVE'), --davao del norte
( 8, 8,  0,  700, 500, 1000 , 'ACTIVE'), --comval
( 8, 9,  0,  900, 500, 1000 , 'ACTIVE'), --govgen
( 8, 10, 0,    0, 1400, 2500 , 'ACTIVE'), --Mati
( 8, 11, 1000, 2000, 500, 1000 , 'ACTIVE'), --Davao Oriental
( 8, 12, 1500, 2500, 500, 1000 , 'ACTIVE'), --Agusan del sur
( 8, 13, 2000, 2500, 500, 1000 , 'ACTIVE'), --Agusan del Norte
( 8, 14, 1500, 2500, 500, 1000 , 'ACTIVE'), --Surigao del sur
( 8, 15, 2500, 3500, 500, 1000 , 'ACTIVE'), --Surigao del Norte

( 8, 16,    0,    0, 500,1000 , 'ACTIVE'), --Marilog
( 8, 17,    0,  400, 500,1000 , 'ACTIVE'), --Buda
( 8, 18,    0,  900, 500,1000 , 'ACTIVE'), --Valencia
( 8, 19, 1000, 1500, 500,1000 , 'ACTIVE'), --Malaybalay
( 8, 20, 1000, 2000, 500,1000 , 'ACTIVE'), --Manolo fortich
( 8, 21, 1500, 2500, 500,1000 , 'ACTIVE'), --Cagayan
( 8, 22, 2000, 3000, 500,1000 , 'ACTIVE'), --Misamis
( 8, 23, 2000, 3000, 500,1000 , 'ACTIVE'), --Iligan

( 8, 24, 0,   0, 500,1000 , 'ACTIVE'), --Santa Cruz, davao del sur
( 8, 25, 0,   0, 500,1000 , 'ACTIVE'), --Digos, davao del sur
( 8, 26, 0,   0, 500,   0 , 'ACTIVE'), --davao del sur
( 8, 27, 0,   0, 500,2500 , 'ACTIVE'), --Malita
( 8, 28, 500, 1000, 500,1000 , 'ACTIVE'), --Don Marcelino
( 8, 29, 500,    0, 500,2000 , 'ACTIVE'), --General Santos
( 8, 30, 500, 1400, 500,1000 , 'ACTIVE'), --Saranggani
( 8, 31, 500, 1500, 500,1000 , 'ACTIVE'), --Koronadal
( 8, 32, 500, 1500, 500,1000 , 'ACTIVE'), --Isulan
( 8, 33, 500, 1500, 500,1000 , 'ACTIVE'), --Sultan Kudarat
( 8, 34,   0,    0, 500,2500 , 'ACTIVE'), --Kidapawan 
( 8, 35, 500, 1000, 500,1000 , 'ACTIVE'), --Ilomavis
( 8, 36, 500, 1000, 500,1000 , 'ACTIVE'), --Kabacan
( 8, 37, 1000, 1200, 500,1000 , 'ACTIVE'), --Arakan
( 8, 38, 1000, 1500, 500,1000 , 'ACTIVE'), --Midsayap
( 8, 39, 1500, 2000, 500,1000 , 'ACTIVE'), --Cotabato City
( 8, 40, 1500, 2000, 500,1000 , 'ACTIVE'); --North Cotabato

--add car rate groups
insert into RateGroups (GroupName) 
values ('N/A'),
       ('Davao Del Sur'),
	   ('Davao Del Norte');

--car rate group
insert into CarRateGroups (RateGroupId,CarRatePackageId) 
values	(2,1),(2,2),(2,3),(2,4),
		(3,5),(3,6),(3,7),(3,8);

insert CarResTypes (Type) 
values ('Reservation'), ('Quotation');


-------------------- SQL.SYS --------------------------------


-------------------- Jobs Expenses --------------------------

insert into ExpensesCategories(Description, Remarks)
values	('Car Rental',''),
		('Tour' ,''),
		('Company',''),
		('Allowance',''),
		('Others',''),
		('Suppliers','')
;

insert into Expenses ( Name, Remarks, SeqNo, ExpensesCategoryId )
values	('Fuel'		 ,'',10,1),
		('Car Wash'  ,'',10,2),
		('Driver'	 ,'',10,3),
		('Allowance' ,'',10,4),
		('CA','Cash Advance',11,5),
		('Others'	 ,'',15,5),

		('Supplier Expense','',11,6),
		('Supplier Comi','',11,6),
		('Agency Fee','',12,4),
		('Tax','',11,3),
		('Entrances','',11,2),
		('Barge Fee','',11,2),
		('Car Rental','',11,1),
		('Security Deposit','',11,1)
		;



		