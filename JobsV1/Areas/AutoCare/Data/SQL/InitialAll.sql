insert into Cities(Name) values('Davao'),('Tagum'),('Digos'),('Kidapawan'),('Gensan');
insert into Branches(Name, CityId, Remarks, Address, Landline, Mobile) 
values ('Branch-Main',1,'Davao Main','Davao City','082 297-1831',''),
	   ('Branch-Maa',1,'Maa','2nd Floor Sulit Bldg, Mac Arthur Hwy, Matina','082 297-1831',''),
	   ('RealWheels',1,'Matina','Matina Crossing, Matina','082 297-1831','');

insert into JobStatus([Status]) values('INQUIRY'),('RESERVATION'),('CONFIRMED'),('CLOSED'),('CANCELLED'),('TEMPLATE');
insert into JobThrus([Desc]) values('WALKIN'),('PHONE'),('ONLINE');

insert into Banks([BankName],[BankBranch],[AccntName],[AccntNo])
 values ('Cash','Davao','Cash','0'),
		('BDO','SM-Ecoland Davao','AJ88 Car Rental Services','00 086 072 9575'),
		('BPI','SM-Ecoland Davao','Abel S. Salvatierra','870 303 5125'),
		('Personal Guarantee','Realbreeze-Davao','Personal Guarantee','0'),
		('Paypal','RealWheels-Paypal','Paypal','0');

insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values
    ('<< New Customer >>','--','--',' ',' ','ACT'),
	('Mark Dalton','markdalton@gmail.com','0916-755-8473','','','ACT'),
	('John Doe','johndoe@gmail.com','09123456789','','','ACT'),
	('Lebron James','lebron.james@gmail.com','09654-987-9898','998-3321','','ACT'),
	('Stephen Curry','stephen.curry@gmail.com','0915-225-1221','','','ACT'),
	('Kevin Durant','Kevin.Durant@gmail.com','0954-321-65454','084-9898','','ACT');

-- ------------------------------------------------------------
-- Sales Lead Configuration
-- ------------------------------------------------------------

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

insert into SalesStatusCodes([SeqNo],[Name])
values (1,'NEW'), (2,'ASSESMENT'), (3, 'PROPOSAL SENT'), (4, 'NEGOTIATION'), (5, 'ACCEPTED'), (6, 'REJECTED'), (7, 'CLOSE');

insert into SalesActStatus([Name])
values ('REQUEST'),('DONE'),('SUSPEND');

insert into CustEntActTypes([Type])
values ('Others'), ('Indicated Price'), ('Bidding Only'), ('Firm Inquiry'), ('Buying Inquiry');


insert into CustEntActivityTypes([Type],[Points])
values ('Quotation',8), ('Meeting',8), ('Sales',15),('Procurement',8), ('Calls/Email',2), ('Others',1);

insert into SalesLeadCatCodes([CatName],[SysCode],[iconPath])
values	('Priority','PRIORITY','~/Images/SalesLead/high-importance.png'), 
		('HighMargin','HIGHMARGIN','~/Images/SalesLead/GreenArrow.png'),
		('LongTerm','LONGTERM','~/Images/SalesLead/Longterm.png'), 
		('Corporate','CORPORATE ACCOUNT','~/Images/SalesLead/ShakeHands.png'), 
		('HardOne', 'HARDONE','~/Images/SalesLead/unhappy.jpg');

-- ----- Auto shop ------
insert into Services([Name],[Description],[Status]) values
	('Change Oil','Change Oil','1'),
	('Under Chassis','Alignment, bushing, etc ','1'),
	('Body repair','Painting','1'),
	('Other','Other types of services','1');

insert into SrvActionCodes([CatCode],[SortNo]) values
('Arrangement',1),('Partial Payment',2),('Notification',3),('OnGoing',4),('Billing',5),('Full Payment',6),('Closing',7);



-- ----------------------------------------------
-- Inventory Configuration 
-- ----------------------------------------------
insert into InvItemCats([Name],[Remarks],[ImgPath],[SysCode])
Values
('Pipe','Pipes','~/Images/CarRental/Repair101.png','PIPE'),
('Others','Other Types','~/Images/CarRental/Repair101.png','OTHER');


-- ----- Auto shop ------
insert into InvItems ([ItemCode],[Description],[Remarks],[ContactInfo],[ViewLabel],[OrderNo] )
values
('B1.0809','Bay 1 ( Slot 8-9 am )','','','',100),
('B1.0910','Bay 1 ( Slot 9-10 am )','','','',101), 
('B1.1011','Bay 1 ( Slot 10-11 am )','','','',102), 
('B2.0809','Bay 2 ( Slot 8-9 am )','','','',103), 
('B2.0910','Bay 2 ( Slot 9-10 am )','','','',104), 
('B2.1011','Bay 2 ( Slot 10-11 am )','','','',105); 

insert into InvItemCategories([InvItemId],[InvItemCatId])
values
(1,1);

-- ----- Auto shop (Vehicle) ------
insert into VehicleBrands ([Brand]) values('Toyota'),('Nissan'),('Mitsubishi'),('Isuzu'),('Hyundai'),('Honda'),('Ford');
insert into VehicleTypes([Type]) values('VAN'),('SUV'),('MPV'),('CAR'),('Pickup'),('Truck'),('Others');
insert into VehicleTransmissions([Type]) values('A/T'),('M/T'),('CVT');
insert into VehicleFuels([Fuel]) values('Gasoline'),('Diesel');
insert into VehicleDrives([Drive]) values('4x2'),('4x4');
insert into VehicleModels([Make],[Variant],[VehicleBrandId],[VehicleTypeId],[VehicleTransmissionId],[VehicleFuelId],[VehicleDriveId])
values
('Grandia','GL',1,1,2,2,1),
('Grandia','Super',1,2,2,2,1),
('Commuter','',1,1,2,2,1),
('Fortuner','G',1,2,2,2,2),
('Monterosports','GLX',3,2,1,2,2),
('Vios','G',1,4,1,1,1);






-- ---------------------------------------- 
-- Suppliers Configuration
-- ---------------------------------------- 
INSERT INTO Countries( Code, Name) VALUES ( 'PH', 'Philippines');
insert into SupplierTypes(Description) values
	('Repair Shop'),('Machine Shop'),('Installer');
insert Into Suppliers([Name],[Contact1],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId],[Code] ) values('<< New Supplier >>','--',' ', '--','1','1','ACT',1,'SUP01');
insert Into Suppliers([Name],[Contact1],[Contact2],[Contact3],[Website],[Address],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId],[Code] )
	values('Auto Repair Sub-1','(082) 223-22222','(082 223-6565)','0912-564-7777','AutoRepair.com','Davao', '','subcon.supply@gmail.com','1','1','ACT',1,'SUP02'),
	      ('Engine Repair sub-1','(082) 333-8888','(082 333-4440)','0915-554-1111','enginerepair.com','Davao City', '','enginerepair@gmail.com','1','1','ACT',1,'SUP03'),
	      ('Parts Supply','(0086) 11236548','(0086 11238840)','-','partsdavao.com','Davao City', '','partsdavao@gmail.com','1','1','ACT',1,'SUP04');

insert into SupplierItems([Description],[SupplierId],[Remarks],[InCharge],[Status]) values 
	('Default','1','Item by supplier','Supplier','ACT');

insert into SupplierUnits([Unit])
values ('Meter'),('Inch'),('Feet'),('Box'),('Package');


-- Customer and Companies Initial

insert into CustCategories([Name],[iconPath])
values 
	   --('PRIORITY','Images/Customers/Category/star-filled-40.png'),
	   --('ACTIVE','Images/Customers/Category/star-filled-40.png'),
	   --('INACTIVE','Images/Customers/Category/star-filled-40.png'),
	   --('BAD ACCOUNT','Images/Customers/Category/star-filled-40.png'),
	   --('SUSPENDED','Images/Customers/Category/star-filled-40.png'),
	   --('BILLING/TERMS','Images/Customers/Category/star-filled-40.png'),
	   --('ACCREDITATION ON PROCESS','Images/Customers/Category/star-filled-40.png'),
	   ('General Engg','Images/Customers/Category/star-filled-40.png'),
	   ('Electrical','Images/Customers/Category/Active-30.png'),
	   ('Mechanical','Images/Customers/Category/suspended-64.png'),
	   ('Civil','Images/Customers/Category/cancel-40.png'),
	   ('Structural','Images/Customers/Category/Active-30.png'),
	   ('Plumbing and Sanitary Work','Images/Customers/Category/star-filled-40.png'),
	   ('Mechanical Piping','Images/Customers/Category/Active-30.png'),
	   ('Airconditioning and Refrigeration','Images/Customers/Category/star-filled-40.png'),
	   ('Fire Protection','Images/Customers/Category/star-filled-40.png'),
	   ('Well Drilling Works','Images/Customers/Category/star-filled-40.png'); 

--insert into CustEntMains([Name],[Address],[Contact1],[Contact2])
--values ('NEW (not yet defined)',' ',' ',' ');


insert into CustEntMains(Name,Code, Address, Contact1, Contact2, iconPath, Website, Remarks, CityId, Status, AssignedTo, Mobile, Exclusive) 
values ('Google.Inc','COP-001','Davao City','888-9888','888-9881','','google.com','',5,'ACT','jecca.realbreeze@gmail.com','09126659987','PUBLIC'),
	   ('Acer.Inc.Ph','COP-002','Quezon City','111-9878','','','acer.com.ph','',5,'ACT','jecca.realbreeze@gmail.com','0915-123-6548','PUBLIC'),
	   ('Silicon Valley','COP-003','Makati City','321-6689','888-3215','','jecca.realbreeze@gmail.com','',5,'ACT','silicon.valley@gmail.com','0912-654-9879','PUBLIC'),
	   ('HP Davao','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',5,'ACT','jahdielvillosa@gmail.com','0999-987-9858','PUBLIC'),
	   
	   -- Priority -- Exclusive
	   ('AYES Food Corp','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','jahdielvillosa@gmail.com','0999-987-9858','EXCLUSIVE'),
	   ('San Miguel Brewery Corp','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','jecca.realbreeze@gmail.com','0999-987-9858','EXCLUSIVE'),
	   ('Coca-cola Corp.','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','jecca.realbreeze@gmail.com','0999-987-9858','EXCLUSIVE'),
	   ('SM Megamalls','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','jahdielvillosa@gmail.com','0999-987-9858','EXCLUSIVE')
	   ;

insert into CustEntAssigns(Assigned, Remarks, CustEntMainId, Date)
values ('demo@gmail.com', '' , 2 , '11-30-2019');

insert into CustEntities(CustEntMainId, CustomerId, Position) 
values  (2,2,'Staff'), (2,3,'Manager'),
		(3,4,'Procurement'), 
		(4,5,'Purchaser'),
		(5,6,'Assistant Procurement');
		
insert into CustCats(CustomerId, CustCategoryId) 
values	(1,3),(2,2),
		(3,2),(4,2),(5,2);

insert into CustEntCats(CustEntMainId,CustCategoryId)
values (1,2),(2,3),(3,5),(4,2),(5,1); 

insert into CustEntActivities(CustEntMainId,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,Type)
values	(1,'1/20/2020','jecca.realbreeze@gmail.com','building a project', 'SO-002',50000,'Sales','Meeting Lunch - Sales Activity','Sales'),
		(1,'2/05/2020','jahdielvillosa@gmail.com','building a project', 'SO-002',50000,'Sales','Presentation','Quotation'),
		(2,'1/28/2020','jecca.realbreeze@gmail.com','Supplier Materials', 'SO-005',25000,'Sales','Meeting','Meeting'),
		(3,'1/21/2020','jecca.realbreeze@gmail.com','Davao Bridge', 'SO-009',75000,'Sales','Initial Meeting','Meeting');

update CustCategories set iconPath = 'Images/Customers/Category/star-filled-40.png' where Id = 1; 
update CustCategories set iconPath = 'Images/Customers/Category/Active-30.png' where Id = 2; 
update CustCategories set iconPath = 'Images/Customers/Category//suspended-64.png' where Id = 3; 
update CustCategories set iconPath = 'Images/Customers/Category/cancel-40.png' where Id = 4;  


-- Car Rental Initial
insert into CarCategories (Description, Remarks)
values ('Rental','');

insert into CarUnits ( Description, Remarks, CarCategoryId , SelfDrive, SortOrder) 
values ('Van (10 seater)','Gl Grandia'    ,1,1,4),
	   ('Van (14 seater)','Nissan Premium',1,1,5),
	   ('SUV'            ,'Ford Everest'  ,1,0,3),
	   ('MPV/AUV/MiniVan','Toyota Innova' ,1,0,2),
	   ('Sedan'          ,'Honda City'    ,1,0,1),
	   ('Pickup'         ,'Pickups'       ,1,0,6),
	   ('Van (14 Seater)','Nissan Premium',1,0,6),
	   ('MPV'            ,'Toyota Rush'   ,1,0,6);

insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (1,'glgrandia/Toyota-Grandia-side.jpg','','MAIN'),
       (2,'nissanPremium/Nissan-Premium-2018.jpg','','MAIN'),
       (3,'ford/ford-everest-front.jpg'		 ,'','MAIN'),
       (4,'innova/toyota-innova.jpg'		 ,'','MAIN'),
       (5,'hondacity/honda-city-front.jpg'   ,'','MAIN'),
       (6,'pickup-car-rental.png'		     ,'','MAIN'),
       (7,'tourer/Toyota-Grandia-Tourer-2019-side.jpg' ,'','MAIN'),
       (8,'rush/Toyota-Rush-2019.jpg'        ,'','MAIN');
	   
insert into CarViewPages (CarUnitId, Viewname)
values (1,'CarDetail_van'),
	   (2,'CarDetail_van'),
	   (3,'CarDetail_suv'),
	   (4,'CarDetail_mpv'),
	   (5,'CarDetail_sedan'),
	   (6,'CarDetail_pickup'),
	   (6,'CarDetail_van'),
	   (6,'CarDetail_mpv');

insert into CarRates (Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate)
values (3000,2500,2250,100,5,1,300), --grandia
	   (3500,2500,2250,100,5,2,300), --premium
	   (3000,2500,2250,100,5,3,300), --everest
	   (2500,1800,1500,100,5,4,300), --innova
	   (2500,1800,1500,100,5,5,300), --honda
	   (3500,2500,2250,100,5,6,300), --honda
	   (3500,2500,2250,100,5,7,300), --honda
	   (2500,2500,2250,100,5,8,300); --pickup



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

	-- Pickup -- 
	(7, 'Pickup for Rent in Davao City, 4x4 rental, rent-a-car',
	'Pickup 4x4 for rent in Davao City for difficult terrain or bigger luggages. Units: Mitsubishi strada 4x4, Toyota Hilux 4x4',
	'Pickup 4x4 is best for difficult and unknown terrains. Can also use in hauling huge luggages.'
	),

	-- Pickup -- 
	(8, 'Pickup for Rent in Davao City, 4x4 rental, rent-a-car',
	'Pickup 4x4 for rent in Davao City for difficult terrain or bigger luggages. Units: Mitsubishi strada 4x4, Toyota Hilux 4x4',
	'Pickup 4x4 is best for difficult and unknown terrains. Can also use in hauling huge luggages.'
	);


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


--------- ACTIVITES ------------------
insert into CustEntActivities( Date, Assigned, ProjectName, SalesCode, Amount, Status, Remarks, CustEntMainId, Type)
values	('01/05/2020', 'demo@gmail.com', '','',0,'Inquiry','', 2 ,'Meeting'),
		('01/20/2020', 'demo@gmail.com', '','',2500,'Bidding Inquiry','', 2 ,'Meeting'),
		('02/12/2020', 'demo@gmail.com', '','',6500,'Bidding Inquiry','', 3 ,'Meeting'),
		('02/15/2020', 'demo@gmail.com', '','SC-001',3500,'Firm Inquiry','', 2,'Meeting' ),
		('02/16/2020', 'demo@gmail.com', '','',25000,'Others','', 3 ,'Sales'); 


--------- APPOINTMENT ----------------
insert into AppointmentStatus([Status]) values 
('NEW'),('ACCEPTED'),('CANCELLED');

insert into AppointmentSlots([Description]) values
('09-10 am Daily'),('10-11 am Daily'),('11-12 nn Daily');

