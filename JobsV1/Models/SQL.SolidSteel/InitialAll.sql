insert into Cities(Name) values('Davao'),('Cebu'),('Makati'),('Manila');
insert into Countries(Name) values('Philippines'),('Singapore'),('Hong Kong'),('Japan');
insert into Branches(Name, CityId, Remarks, Address, Landline, Mobile) 
values ('AJ88',1,'Davao Main','2nd Floor Sulit Bldg, Mac Arthur Hwy, Matina','082 297-1831',''),
	   ('RealBreeze',1,'Davao Main','2nd Floor Sulit Bldg, Mac Arthur Hwy, Matina','082 297-1831',''),
	   ('RealWheels',1,'Davao Main','2nd Floor Sulit Bldg, Mac Arthur Hwy, Matina','082 297-1831','');

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
insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values('John Doe','jahdielsvillosa@gmail.com','09123456789','','','ACT');
	
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

insert into CustEntMains([Name],[Address],[Contact1],[Contact2])
values ('NEW (not yet defined)',' ',' ',' ');

insert into SalesStatusCodes([SeqNo],[Name])
values (1,'NEW'), (2,'ASSESMENT'), (3, 'PROPOSAL SENT'), (4, 'NEGOTIATION'), (5, 'ACCEPTED'), (6, 'REJECTED'), (7, 'CLOSE');

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
		
-- ----------------------------------------------
-- Inventory Configuration 
-- ----------------------------------------------
insert into InvItemCats([Name],[Remarks],[ImgPath],[SysCode])
Values
('Pipe','Pipes','~/Images/CarRental/Repair101.png','PIPE'),
('Others','Other Types','~/Images/CarRental/Repair101.png','OTHER');


insert into InvItems ([ItemCode],[Description],[Remarks],[ContactInfo],[ViewLabel],[OrderNo] )
values
('CSTPP','Carbon Steel Pipe','Carbon Steel Pipe','','',100),
('CSTPL','Carbon Steel Plate','Carbon Steel Plate','','',100),
('SSTPP','Stainless Steel Pipe','Stainless Steel Pipe','','',100),
('SSTPL','Stainless Steel Plate','Stainless Steel Plate','','',100);

insert into InvItemCategories([InvItemId],[InvItemCatId])
values
(1,1);

-- ---------------------------------------- 
-- Suppliers Configuration
-- ---------------------------------------- 
insert into SupplierTypes(Description) values
('Stockiest/Trader'),('Supplier'),('Installer');
insert Into Suppliers([Name],[Contact1],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId] ) values('<< New Supplier >>','--',' ', '--','1','1','ACT',1);
insert Into Suppliers([Name],[Contact1],[Contact2],[Contact3],[Website],[Address],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId] )
	values('Solid Steel','(82) 085-0158','(82 085-0151)','0912-564-9877','solidsteelserver.com','Davao City', '','AJDavao88@gmail.com','1','1','ACT',1);
-- ----------------------------
-- Supplier Items / Products
insert into SupplierItems([Description],[SupplierId],[Remarks],[InCharge],[Status]) values 
('Default','1','Item by supplier','Supplier','ACT');

insert into SupplierInvItems([SupplierId],[InvItemId]) values
(2,1),(2,2);

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
-- Supplier PO Configuration
-- ----------------------------------------------
insert into SupplierPoStatus ([Status],[OrderNo]) values ('REQUEST',1),('APPROVED',2),('CANCELLED',2),('CONFIRMED',3),('DELIVERED',4),('CLOSE',5);

-- ----------------------------------------------
-- Paypal Accounts
-- ----------------------------------------------
insert into PaypalAccounts ([SysCode],[Key],[Secret])
values
('Realwheels', 'ASTv_oxNk66nZW4tVTbt78dtocU-70VVoDDmgtdMSzv1Aqmw8QK6lJ01vzn6lO6jPio3DbfbT_6G6F6b' , 'EAYtPcgQYKu5UfA4WV5lzE_iPj1WFiGnPC_8XvSgYrjoISJEnZAezmdcofe5oRyJZzPToJO6QUMlgmS2');

insert into JobMains([JobDate],[CustomerId],[Description],[NoOfPax],[NoOfDays],[JobRemarks],[JobStatusId],[StatusRemarks],[BranchId],[JobThruId],[AgreedAmt])
values
('11-15-2019',1,'Test Job 101',10,1,'TEST DATA 0101',3,'N/A',1,1,5000),
('11-27-2019',1,'Item scheduling',3,1,'TEST DATA 0102',3,'N/A',1,1,3000),
('11-27-2019',1,'Davao City Tour',3,2,'Template Test',6,'N/A',1,1,3500);


insert into JobServices([JobMainId],[ServicesId],[SupplierId],[Particulars],[QuotedAmt],[SupplierAmt],[ActualAmt],[Remarks],[SupplierItemId],[DtStart],[DtEnd])
values
(1,1,2,'Car Rental sample data R1',5000,5000,5000,'Sample only. Disregard once seen on production',1,'11-15-2019','11-22-2019'),
(1,1,2,'Car Rental sample data R2',3000,3000,3000,'Sample only. Disregard once seen on production',1,'11-24-2019','11-28-2019'),
(2,1,2,'SUV Rental R1',2000,2000,2000,'Sample only. Disregard once seen on production',1,'11-27-2019','11-28-2019'),
(2,1,2,'SUV Rental R2',1000,1000,1000,'Sample only. Disregard once seen on production',1,'11-29-2019','11-30-2019'),
(3,1,2,'Day 1: Country Side Tour',2000,2000,2000,'Sample only. Disregard once seen on production',1,'11-27-2019','11-27-2019'),
(3,1,2,'Day 1: Country Side Tour',1500,1500,1000,'Sample only. Disregard once seen on production',1,'11-28-2019','11-28-2019');

insert into JobItineraries([JobMainId],[DestinationId],[ActualRate],[Remarks],[ItiDate],[SvcId])
values
(3,1,0,'','11-27-2019',5),
(3,2,0,'','11-27-2019',5),
(3,5,0,'','11-28-2019',6),
(3,6,0,'','11-28-2019',6);

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
values ('11-25-2018','Test Po',1,1,'Abel','11-25-2018');

insert into SupplierPoDtls([SupplierPoHdrId],[Remarks],[Amount],[JobServicesId])
values (1,'10 seater vehicle',3500,1), (1,'14 seater vehicle',4000,1);

insert into SupplierUnits([Unit])
values ('Meter'),('Inch'),('Feet'),('Box'),('Package');

-- Customer PO Samples
insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
values('Juan Dela Cruz','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

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



insert into CarCategories (Description, Remarks)
values ('Rental','');

insert into CarUnits ( Description, Remarks, CarCategoryId , SelfDrive, SortOrder) 
values ('Van (10 seater)','Gl Grandia'    ,1,1,4),
	   ('Van (14 seater)','Nissan Premium',1,1,5),
	   ('SUV'            ,'Ford Everest'  ,1,0,3),
	   ('MPV/AUV/MiniVan','Toyota Innova' ,1,0,2),
	   ('Sedan'          ,'Honda City'    ,1,0,1),
	   ('Pickup'         ,'Pickups'       ,1,0,6);

insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (1,'glgrandia-car-rental.png'     ,'','MAIN'),
       (2,'nissan-premium-car-rental.png','','MAIN'),
       (3,'ford-everest-car-rental.png'  ,'','MAIN'),
       (4,'toyota-innova-car-rental.png' ,'','MAIN'),
       (5,'honda-city-car-rental.png'    ,'','MAIN'),
       (6,'pickup-car-rental.png'        ,'','MAIN');
	   
insert into CarViewPages (CarUnitId, Viewname)
values (1,'CarDetail_van'),
	   (2,'CarDetail_van'),
	   (3,'CarDetail_suv'),
	   (4,'CarDetail_mpv'),
	   (5,'CarDetail_sedan'),
	   (6,'CarDetail_pickup');

insert into CarRates (Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate)
values (3000,2500,2250,100,5,1,300), --grandia
	   (3500,2500,2250,100,5,2,300), --premium
	   (3000,2500,2250,100,5,3,300), --everest
	   (2500,1800,1500,100,5,4,300), --innova
	   (2500,1800,1500,100,5,5,300), --honda
	   (3500,2500,2250,100,5,6,300); --pickup



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

Insert into CarRateUnitPackages (CarUnitId,CarRatePackageId,DailyRate,FuelLonghaul,FuelDaily,DailyAddon)
values
-- regular van ( Grandia GL )
( 1, 1, 0,  0,   0,   0 ), --+ selfdrive
( 1, 2, 0,  0, 500,   0 ), --+ city
( 1, 3, 0,  0, 800, 500 ), --+ Countryside
( 1, 4, 0,  0, 800, 500 ), --+ Samal

( 1, 5,	 0, 0, 500, 1000 ), --panabo
( 1, 6,  0, 0, 500, 1000 ), --tagum
( 1, 7,  0, 0, 500, 700 ), --davao del norte
( 1, 8,  0, 0, 700, 2000 ), --comval
( 1, 9,  0, 0, 500, 2000 ), --govgen
( 1, 10, 0, 0, 1500, 2000 ), --Mati
( 1, 11, 1000, 2000, 700, 1000 ), --Davao Oriental
( 1, 12, 1500, 2500, 700, 1000 ), --Agusan del sur
( 1, 13, 2000, 2500, 700, 1000 ), --Agusan del Norte
( 1, 14, 1500, 2500, 700, 1000 ), --Surigao del sur
( 1, 15, 2500, 3500, 700, 1000 ), --Surigao del Norte

( 1, 16,    0,    0, 600, 1000 ),  --Marilog
( 1, 17,    0,  400, 600, 1000 ),  --Buda
( 1, 18,    0,  900, 600, 1000 ), --Valencia
( 1, 19, 1000, 1500, 600, 1000 ), --Malaybalay
( 1, 20, 1000, 2000, 600, 1000 ), --Manolo fortich
( 1, 21, 1500, 2500, 600, 1000 ), --Cagayan
( 1, 22, 2000, 3000, 700, 1000 ), --Misamis
( 1, 23, 2000, 3000, 700, 1000 ), --Iligan

( 1, 24, 0,   0, 600, 1000 ), --Santa Cruz, davao del sur
( 1, 25, 0,   0, 500, 1000 ), --Digos, davao del sur
( 1, 26, 0,   0, 700, 1000 ), --davao del sur
( 1, 27, 0,   0, 1000, 2000 ), --Malita
( 1, 28, 500, 1000, 600,1000 ), --Don Marcelino
( 1, 29, 500,    0, 500, 1500 ), --General Santos
( 1, 30, 500, 1400, 700,1000 ), --Saranggani
( 1, 31, 500, 1500, 600,1000 ), --Koronadal
( 1, 32, 500, 1500, 700,1000 ), --Isulan
( 1, 33, 500, 1500, 700,1000 ), --Sultan Kudarat
( 1, 34,   0,    0, 1000,2000 ), --Kidapawan
( 1, 35, 500, 1000, 600,1000 ), --Ilomavis
( 1, 36, 500, 1000, 600,1000 ), --Kabacan
( 1, 37, 1000, 1200, 600,1000 ), --Arakan
( 1, 38, 1000, 1500, 600,1000 ), --Midsayap
( 1, 39, 1500, 2000, 600,1000 ), --Cotabato City
( 1, 40, 1500, 2000, 700,1000 ), --North Cotabato

-- big van ( Nissan Premium)
( 2, 1, 0,  0,   0,   0 ), --+ selfdrive
( 2, 2, 0,  0, 500,   0 ), --+ city
( 2, 3, 0,  0, 800, 1200 ), --+ Countryside
( 2, 4, 0,  0, 800, 1200 ), --+ Samal

( 2, 5,	 0,    0, 500, 1200 ), --panabo
( 2, 6,  0,  200, 500, 1000 ), --tagum
( 2, 7,  0,  700, 700, 300 ), --davao del norte
( 2, 8,  0,  700, 700, 1800 ), --comval
( 2, 9,  0,  900, 600, 1000 ), --govgen
( 2, 10,  200,  900, 600, 1700 ), --Mati
( 2, 11, 1200, 2000, 700, 1000 ), --Davao Oriental
( 2, 12, 1700, 2500, 700, 1000 ), --Agusan del sur
( 2, 13, 2200, 2500, 700, 1000 ), --Agusan del Norte
( 2, 14, 1700, 2500, 700, 1000 ), --Surigao del sur
( 2, 15, 2700, 3500, 700, 1000 ), --Surigao del Norte

( 2, 16,    0,    0, 600, 1000 ), --Marilog
( 2, 17,    0,  400, 600, 1000 ), --Buda
( 2, 18,    0,  900, 600, 1000 ), --Valencia
( 2, 19, 1200, 1500, 600, 1000 ), --Malaybalay
( 2, 20, 1200, 2000, 600, 1000 ), --Manolo fortich
( 2, 21, 1700, 2500, 600, 1000 ), --Cagayan
( 2, 22, 2200, 3000, 700, 1000 ), --Misamis
( 2, 23, 2200, 3000, 700, 1000 ), --Iligan

( 2, 24, 0, 0, 600,1000 ), --Santa Cruz, davao del sur
( 2, 25, 0, 400, 600,1000 ), --Digos, davao del sur
( 2, 26, 0, 600, 700,1000 ), --davao del sur
( 2, 27, 0, 900, 600,1000 ), --Malita
( 2, 28, 700, 1000, 600,1000 ), --Don Marcelino
( 2, 29, 700, 1000, 600,1000 ), --General Santos
( 2, 30, 700, 1400, 700,1000 ), --Saranggani
( 2, 31, 700, 1500, 600,1000 ), --Koronadal
( 2, 32, 700, 1500, 700,1000 ), --Isulan
( 2, 33, 700, 1500, 700,1000 ), --Sultan Kudarat
( 2, 34,   0, 1000, 600,1000 ), --Kidapawan
( 2, 35, 700, 1000, 600,1000 ), --Ilomavis
( 2, 36, 700, 1000, 600,1000 ), --Kabacan
( 2, 37, 1200, 1200, 600,1000 ), --Arakan
( 2, 38, 1200, 1500, 600,1000 ), --Midsayap
( 2, 39, 1700, 2000, 600,1000 ), --Cotabato City
( 2, 40, 1200, 2000, 700,1000 ), --North Cotabato

-- SUV ( Ford Everest / fortuner )
( 3, 1, 0,  0,   0, 0 ), --+ selfdrive
( 3, 2, 0,  0, 500,   0 ), --+ city
( 3, 3, 0,  0, 800, 300 ), --+ Cuntryside
( 3, 4, 0,  0, 800, 300 ), --+ Samal

( 3, 5,	 0,    0, 500, 1000 ), --panabo
( 3, 6,  0,  200, 500, 1000 ), --tagum
( 3, 7,  0,  700, 700, 1000 ), --davao del norte
( 3, 8,  0,  700, 700, 1000 ), --comval
( 3, 9,  0,  900, 600, 1000 ), --govgen
( 3, 10, 0,  900, 600, 1000 ), --Mati
( 3, 11, 1000, 2000, 700, 1000 ), --Davao Oriental
( 3, 12, 1500, 2500, 700, 1000 ), --Agusan del sur
( 3, 13, 2000, 2500, 700, 1000 ), --Agusan del Norte
( 3, 14, 1500, 2500, 700, 1000 ), --Surigao del sur
( 3, 15, 2500, 3500, 700, 1000 ), --Surigao del Norte

( 3, 16,    0,    0, 600,1000 ), --Marilog
( 3, 17,    0,  400, 600,1000 ), --Buda
( 3, 18,    0,  900, 600,1000 ), --Valencia
( 3, 19, 1000, 1500, 600,1000 ), --Malaybalay
( 3, 20, 1000, 2000, 600,1000 ), --Manolo fortich
( 3, 21, 1500, 2500, 600,1000 ), --Cagayan
( 3, 22, 2000, 3000, 700,1000 ), --Misamis
( 3, 23, 2000, 3000, 700,1000 ), --Iligan

( 3, 24, 0, 0, 600,1000 ), --Santa Cruz, davao del sur
( 3, 25, 0, 400, 600,1000 ), --Digos, davao del sur
( 3, 26, 0, 600, 700,1000 ), --davao del sur
( 3, 27, 0, 900, 600,1000 ), --Malita
( 3, 28, 500, 1000, 600,1000 ), --Don Marcelino
( 3, 29, 500, 1000, 600,1000 ), --General Santos
( 3, 30, 500, 1400, 700,1000 ), --Saranggani
( 3, 31, 500, 1500, 600,1000 ), --Koronadal
( 3, 32, 500, 1500, 700,1000 ), --Isulan
( 3, 33, 500, 1500, 700,1000 ), --Sultan Kudarat
( 3, 34,   0, 1000, 600,1000 ), --Kidapawan
( 3, 35, 500, 1000, 600,1000 ), --Ilomavis
( 3, 36, 500, 1000, 600,1000 ), --Kabacan
( 3, 37, 1000, 1200, 600,1000 ), --Arakan
( 3, 38, 1000, 1500, 600,1000 ), --Midsayap
( 3, 39, 1500, 2000, 600,1000 ), --Cotabato City
( 3, 40, 1500, 2000, 700,1000 ), --North Cotabato

-- MPV ( Innova )
( 4, 1, 0,  0,   0,   0 ), --+ selfdrive
( 4, 2, 0,  0, 500, 300 ), --+ city
( 4, 3, 0,  0, 800, 1000 ), --+ Countryside
( 4, 4, 0,  0, 800, 1000 ), --+ Samal

( 4, 5,	 0,    0, 500, 1000 ), --panabo
( 4, 6,  0,    0, 500, 1000 ), --tagum
( 4, 7,  0,  700, 500, 1000 ), --davao del norte
( 4, 8,  0,  700, 500, 1000 ), --comval
( 4, 9,  0,  900, 500, 1000 ), --govgen
( 4, 10, 0,    0, 1400, 2500 ), --Mati
( 4, 11, 1000, 2000, 500, 1000 ), --Davao Oriental
( 4, 12, 1500, 2500, 500, 1000 ), --Agusan del sur
( 4, 13, 2000, 2500, 500, 1000 ), --Agusan del Norte
( 4, 14, 1500, 2500, 500, 1000 ), --Surigao del sur
( 4, 15, 2500, 3500, 500, 1000 ), --Surigao del Norte

( 4, 16,    0,    0, 500,1000 ), --Marilog
( 4, 17,    0,  400, 500,1000 ), --Buda
( 4, 18,    0,  900, 500,1000 ), --Valencia
( 4, 19, 1000, 1500, 500,1000 ), --Malaybalay
( 4, 20, 1000, 2000, 500,1000 ), --Manolo fortich
( 4, 21, 1500, 2500, 500,1000 ), --Cagayan
( 4, 22, 2000, 3000, 500,1000 ), --Misamis
( 4, 23, 2000, 3000, 500,1000 ), --Iligan

( 4, 24, 0,   0, 500,1000 ), --Santa Cruz, davao del sur
( 4, 25, 0,   0, 500,1000 ), --Digos, davao del sur
( 4, 26, 0,   0, 500,   0 ), --davao del sur
( 4, 27, 0,   0, 500,2500 ), --Malita
( 4, 28, 500, 1000, 500,1000 ), --Don Marcelino
( 4, 29, 500,    0, 500,2000 ), --General Santos
( 4, 30, 500, 1400, 500,1000 ), --Saranggani
( 4, 31, 500, 1500, 500,1000 ), --Koronadal
( 4, 32, 500, 1500, 500,1000 ), --Isulan
( 4, 33, 500, 1500, 500,1000 ), --Sultan Kudarat
( 4, 34,   0,    0, 500,2500 ), --Kidapawan 
( 4, 35, 500, 1000, 500,1000 ), --Ilomavis
( 4, 36, 500, 1000, 500,1000 ), --Kabacan
( 4, 37, 1000, 1200, 500,1000 ), --Arakan
( 4, 38, 1000, 1500, 500,1000 ), --Midsayap
( 4, 39, 1500, 2000, 500,1000 ), --Cotabato City
( 4, 40, 1500, 2000, 500,1000 ), --North Cotabato

-- Sedan ( Honda City )
( 5, 1, 0,  0,   0, 0 ), --+ selfdrive
( 5, 2, 0,  0, 500,   0 ), --+ city
( 5, 3, 0,  0, 800, 300 ), --+ Cuntryside
( 5, 4, 0,  0, 800, 300 ), --+ Samal
( 5, 5,	 0,    0, 500, 1000 ), --panabo
( 5, 6,  0,  200, 500, 1000 ), --tagum
( 5, 7,  0,  700, 500, 1000 ), --davao del norte
( 5, 8,  0,  700, 500, 1000 ), --comval
( 5, 9,  0,  900, 500, 1000 ), --govgen
( 5, 10, 0,  900, 500, 1000 ), --Mati
( 5, 11, 1000, 2000, 500, 1000 ), --Davao Oriental
( 5, 12, 1500, 2500, 500, 1000 ), --Agusan del sur
( 5, 13, 2000, 2500, 500, 1000 ), --Agusan del Norte
( 5, 14, 1500, 2500, 500, 1000 ), --Surigao del sur
( 5, 15, 2500, 3500, 500, 1000 ), --Surigao del Norte

( 5, 16,    0,    0, 500,1000 ), --Marilog
( 5, 17,    0,  400, 500,1000 ), --Buda
( 5, 18,    0,  900, 500,1000 ), --Valencia
( 5, 19, 1000, 1500, 500,1000 ), --Malaybalay
( 5, 20, 1000, 2000, 500,1000 ), --Manolo fortich
( 5, 21, 1500, 2500, 500,1000 ), --Cagayan
( 5, 22, 2000, 3000, 500,1000 ), --Misamis
( 5, 23, 2000, 3000, 500,1000 ), --Iligan

( 5, 24, 0, 0, 500,1000 ), --Santa Cruz, davao del sur
( 5, 25, 0, 400, 500,1000 ), --Digos, davao del sur
( 5, 26, 0, 600, 500,1000 ), --davao del sur
( 5, 27, 0, 900, 500,1000 ), --Malita
( 5, 28, 500, 1000, 500,1000 ), --Don Marcelino
( 5, 29, 500, 1000, 500,1000 ), --General Santos
( 5, 30, 500, 1400, 500,1000 ), --Saranggani
( 5, 31, 500, 1500, 500,1000 ), --Koronadal
( 5, 32, 500, 1500, 500,1000 ), --Isulan
( 5, 33, 500, 1500, 500,1000 ), --Sultan Kudarat
( 5, 34,   0, 1000, 500,1000 ), --Kidapawan
( 5, 35, 500, 1000, 500,1000 ), --Ilomavis
( 5, 36, 500, 1000, 500,1000 ), --Kabacan
( 5, 37, 1000, 1200, 500,1000 ), --Arakan
( 5, 38, 1000, 1500, 600,1000 ), --Midsayap
( 5, 39, 1500, 2000, 600,1000 ), --Cotabato City
( 5, 40, 1500, 2000, 700,1000 ), --North Cotabato

-- Pickup ( strada / hilux )
( 6, 1, 0,  0,   0, 0 ), --+ selfdrive
( 6, 2, 0,  0, 500,   0 ), --+ city
( 6, 3, 0,  0, 800, 300 ), --+ Cuntryside
( 6, 4, 0,  0, 800, 300 ), --+ Samal

( 6, 5,	 0,    0, 500, 1000 ), --panabo
( 6, 6,  0,  200, 500, 1000 ), --tagum
( 6, 7,  0,  700, 700, 1000 ), --davao del norte
( 6, 8,  0,  700, 700, 1000 ), --comval
( 6, 9,  0,  900, 600, 1000 ), --govgen
( 6, 10, 0,  900, 600, 1000 ), --Mati
( 6, 11, 1000, 2000, 700, 1000 ), --Davao Oriental
( 6, 12, 1500, 2500, 700, 1000 ), --Agusan del sur
( 6, 13, 2000, 2500, 700, 1000 ), --Agusan del Norte
( 6, 14, 1500, 2500, 700, 1000 ), --Surigao del sur
( 6, 15, 2500, 3500, 700, 1000 ), --Surigao del Norte

( 6, 16,    0,    0, 600,1000 ), --Marilog
( 6, 17,    0,  400, 600,1000 ), --Buda
( 6, 18,    0,  900, 600,1000 ), --Valencia
( 6, 19, 1000, 1500, 600,1000 ), --Malaybalay
( 6, 20, 1000, 2000, 600,1000 ), --Manolo fortich
( 6, 21, 1500, 2500, 600,1000 ), --Cagayan
( 6, 22, 2000, 3000, 700,1000 ), --Misamis
( 6, 23, 2000, 3000, 700,1000 ), --Iligan

( 6, 24, 0, 0, 600,1000 ), --Santa Cruz, davao del sur
( 6, 25, 0, 400, 600,1000 ), --Digos, davao del sur
( 6, 26, 0, 600, 700,1000 ), --davao del sur
( 6, 27, 0, 900, 600,1000 ), --Malita
( 6, 28, 500, 1000, 600,1000 ), --Don Marcelino
( 6, 29, 500, 1000, 600,1000 ), --General Santos
( 6, 30, 500, 1400, 700,1000 ), --Saranggani
( 6, 31, 500, 1500, 600,1000 ), --Koronadal
( 6, 32, 500, 1500, 700,1000 ), --Isulan
( 6, 33, 500, 1500, 700,1000 ), --Sultan Kudarat
( 6, 34,   0, 1000, 600,1000 ), --Kidapawan
( 6, 35, 500, 1000, 600,1000 ), --Ilomavis
( 6, 36, 500, 1000, 600,1000 ), --Kabacan
( 6, 37, 1000, 1200, 600,1000 ), --Arakan
( 6, 38, 1000, 1500, 600,1000 ), --Midsayap
( 6, 39, 1500, 2000, 600,1000 ), --Cotabato City
( 6, 40, 1500, 2000, 700,1000 ); --North Cotabato


--add car rate groups
insert into RateGroups (GroupName) 
values ('N/A'),
       ('Davao Del Sur'),
	   ('Davao Del Norte');

--car rate group
insert into CarRateGroups (RateGroupId,CarRatePackageId) 
values	(2,1),(2,2),(2,3),(2,4),
		(3,5),(3,6),(3,7),(3,8);

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



		