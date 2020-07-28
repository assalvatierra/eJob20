﻿
insert into SysCmdIdRefs([CmdId],[Description],[Remarks]) values 
(20,'OPEN MENU','get submenus'), 
(21,'EXECUTE','Execute controller/action');

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('System Access','Access Control',0,'SysAccessUsers','ModuleList','',20,1),
('Sales Leads','Sales Lead',0,'SalesLeads','Index','',20,2),
('Job Orders','Job Orders',0,'JobOrder','Index','',20,3),
('Suppliers','List of Suppliers',0,'Suppliers','Index','',20,4),
('Customers','Customers',0,'CustEntMains','Index','',20,5),
('Resources','Resources',0,'SupplierItems','Index','',20,6),
('Packages','Packages',0,'CarRatePackages','Index','',20,7),
('Reporting','Reports',0,'Reporting','Index','',20,8),
('Notifications','Notifications',0,'JobServices','NotificationList','',20,9),
('Accounts','Accounts',0,'Accounting/AccntMains','Index','',20,10),
('HR','HRIS',0,'Personel/HrPersonels','Index','',20,11),
('Products','Products',0,'Products/SMProducts','Index','',20,12),
('Bookkeeping','Bookkeeping',0,'Accounting/AsMain','Index','span=30',20,13),
('Master List','Master List',0,'Cities','Index','span=30',20,14),
('Admin','Admin',0,'Admin','UserList','',20,15),
('Activities','Activities',0,'Activities','Index','',20,16),
('Vehicles','Vechicles',0,'AutoCare/Vehicles','Index','',20,17),
('Post Sales','Job Post Sales',0,'JobPostSales','Index','',20,18),
('Appointments','Appointments',0,'AutoCare/Appointments','Index','',20,19),
('Cashier','Cashier',0,'Cashier','Index','',20,20)
;


insert into EntBusinesses([Name],[ShortName],[BussRegNo],[User]) values
('Real Breeze Davao','RBD','11-2233','abel');
--Added by jahdiel - 3/6/19

--System Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SA101','System Access','Users Access Control','A','/Images/Erp/icons-key.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (1,1);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Modules','',1,'SysAccessUsers','ModuleList','',21,11), -- id: 18
	('Users','',1,'SysAccessUsers','UsersList','',21,12),    -- id: 19
	('Add Users','',1,'Account','Register','',21,13),		 -- id: 20
	('Password','',1,'Account','ForgotPassword','',21,14);   -- id: 21
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(1,1,'2019/1/1');

--Sales Leads
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SL102','Sales Leads','Leads,Quotations,Reservations','A','/Images/Erp/icons-flag.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (2,2);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Sales Leads','',2,'SalesLeads','Index','',21,21),		  -- id: 22
	('Quotations','',2,'JobMains','JobLeads','',21,22),		  -- id: 23
	('Reservations','',2,'CarReservations','Index','',21,23), -- id: 24
	('Online Reservations', '', 2, 'OnlineReservations','Index','',21,24);
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(2,1,'2019/1/1');

--Job Order
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('JO103','Job Orders','Jobs / Work in Progress','A','/Images/Erp/inprogress.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (3,3);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Work in Progress','',3,'JobOrder','Index','',21,31),		-- id: 25
	('Status','',3,'JobOrder','jobStatus','',21,32),			-- id: 26
	('Quick List','',3,'JobMains','ActiveJobs','',21,33),		-- id: 27
	('Listing','',3,'JobOrder','JobListing','span=30',21,34),	-- id: 28
	('Job Table','',3,'JobMains','JobTable','span=30',21,35),	-- id: 29
	('Availability','',3,'InvItems','Availability','',21,36),	-- id: 30
	('Trip Listing','',3,'JobMains','TripListing' ,'',21,36),	-- id: 30
	('Trip Logs','',3,'Personel/CarRentalLog','Index' ,'',21,36);		-- id: 31
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(3,1,'2019/1/1');

--Suppliers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SU104','Suppliers','List of Suppliers','A','/Images/Erp/icons/icons-exchange.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (4,4);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Master List','',4,'Suppliers','Index','',21,41),			-- id: 32
	('Items','',4,'InvItems','Index','',21,42),					-- id: 33
	('PO List','',4,'SupplierPoHdrs','Index','',21,43),			-- id: 34
	('Coop Members','',4,'CoopMembers','Index','',21,44);		-- id: 35
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(4,1,'2019/1/1');

--Customers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('CU105','Customers','List of Customers','A','/Images/Erp/icons/icons-business.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (5,5);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Companies','',5,'CustEntMains','Index','',21,51),			-- id: 36
	('Contacts','',5,'Customers','Index','',21,52),			-- id: 37
	('Email Blaster','',5,'EmailBlaster','Index','',21,53),  	-- id: 38
	('Customer Portal','',5,'PortalCustomers','Index','',21,54); -- id: 39
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(5,1,'2019/1/1');

--Equipments /Resources
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('EQ106','Resources','List of Resources','A','/Images/Erp/icons/icons-trolley.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (6,6);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Items','',6,'InvItems','Index','',21,62),					-- id: 40
	('Availability','',6,'InvItems','Availability','',21,63),	-- id: 41
	('Gate Control','',6,'InvCarGateControls','Index','',21,64),-- id: 42
	('Maintenance','',6,'InvCarRecords','Index','',21,65),		-- id: 43
	('Service Types','',6,'SupplierItems','Index','',21,61),	-- id: 43
	('Services','',6,'Services','Index','',21,62);		        -- id: 44

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(6,1,'2019/1/1');

--Packages
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('PK107','Packages','Reports','A','/Images/Erp/icons/icons-box.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (7,7);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Package Unit','',7,'CarRatePackages','Index','',21,71),	 -- id: 45
	('Package List','',7,'CarRateUnitPackages','Index','',21,72),-- id: 46
	('Unit Rates','',7,'CarRates','Index','',21,73),			 -- id: 47
	('Groups','',7,'RateGroups','Index','',21,73);				 -- id: 48

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(7,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Reporting
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('RP108','Reporting','Reports','A','/Images/Erp/icons/icons-report.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (8,8);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Job Listing','',8,'Reporting','Index','',21,81),			-- id: 49
	('Payment','',8,'Reporting','Index','',21,82),				-- id: 50
	('Package Rates','',8,'Reporting','Index','',21,83);		-- id: 51
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(8,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Notifications
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO109','Notifications','Reports','A','/Images/Erp/icons/icons-notification.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (9,9);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('SMS','',9,'JobServices','NotificationList','',21,91),		-- id: 52
	('Paypal','',9,'PaypalTransactions','Index','',21,92),		-- id: 53
	('Notification','',9,'CustNotifs','Index','',21,93);		-- id: 54
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values	
(9,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Accounts
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO110','Accounts','Accounting','A','/Images/Erp/icons/icons-accounting.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (10,10);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Main Accounts','',10,'Accounting/AccntMains','Index','',21,101),			-- id: 55
		('Ledgers','',10,'Accounting/Accntledgers','Index','',21,102),				-- id: 56
		('Account Category','',10,'Accounting/AccntCategories','Index','',21,103),	-- id: 57
		('Transactions','',10,'Accounting/AccntTrxHdrs','Index','',21,103);			-- id: 58
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values	
(10,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--HRIS -- Personnel
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO111','HR','Human Resources','A','/Images/Erp/icons/icons-employee.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (11,11);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Personnel Lists','',11,'Personel/HrPersonels','Index','',21,111);		-- id: 59

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(11,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Products
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO112','Products','Products','A','/Images/Erp/icons/icons-products.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (12,12);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Products','',12,'Products/SmProducts','Index','',21,121),		-- id: 60
		('Suppliers' ,'',12,'Products/SmSuppliers','Index','',21,122), 	-- id: 61
		('Ads' ,'',12,'Products/SmProdAds','Index','',21,123);	        -- id: 62
		
--Bookkeeping
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO113','Book keeping','Book Keeping','A','/Images/Erp/icons/icons-bookkeeping.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (13,13);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Expense','',13,'Accounting/AsExpenses','ParamForm','',21,131),		-- id: 63
		('Sales' ,'',13,'Accounting/AsSales','ParamForm','',21,133);	        -- id: 64
		
--Master List
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO114','Master List','Master List of Items','A','/Images/Erp/icons/icons-masterlist.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (14,14);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Cities' ,'',14,'Cities','Index','',21,141),			-- id: 65
		('Categories' ,'',14,'CustCategories','Index','',21,142),-- id: 66
		('Documents' ,'',14,'SupDocuments','Index','',21,143);	 -- id: 67
		
--Admin Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO115','Admin','Admin Access','A','/Images/Erp/icons/icons-admin.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (15,15);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Agent Assignment' ,'',15,'Admin','Index','',21,151);	

--Activities Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO116','Activities','User Activities Listing','A','/Images/Erp/icons/icons-activities.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (16,16);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Activities' ,'',16,'Activities','Index','',21,161),
		('Performance' ,'',16,'Activities','Performance','',21,162);	
		
--Vehicles Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO117','Vehicles','Vehicles Listing','A','/Images/AutoCare/icons-suv.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (17,17);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Units' ,'',17,'AutoCare/Vehicles','Index','',21,171),	
		('Models' ,'',17,'AutoCare/VehicleModels','Index','',21,172);	

--After Sales
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO118','Post Sales','Job Post Sales','A','/Images/AutoCare/icons-callback.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (18,18);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values ('Post Sales' ,'',18,'JobPostSales','Index','',21,181);	 

--Appointments
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO119','Appointments','Job Appointments','A','/Images/AutoCare/icons-call.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (19,19);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values  ('Appointment' ,'',19,'AutoCare/Appointments','Index','',21,191),
		('Slots' ,'',19,'AutoCare/AppointmentSlots','Index','',21,192),
		('Availability' ,'',19,'AutoCare/Appointments','Availability','',21,193);	

		
--Cashier
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO120','Cashier','Cashier','A','/Images/AutoCare/icons-cash-register.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (20,20);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values ('Cashier' ,'',20,'Cashier','Index','',21,201);	 



insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(12,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

insert into SysAccessUsers([UserId],[SysMenuId],[Seqno]) values
('assalvatierra@gmail.com',1,1), 
('assalvatierra@gmail.com',2,2), 
('assalvatierra@gmail.com',3,3), 
('assalvatierra@gmail.com',4,4), 
('assalvatierra@gmail.com',5,5), 
('assalvatierra@gmail.com',6,6),  
('assalvatierra@gmail.com',7,7), 
('assalvatierra@gmail.com',8,8), 
('assalvatierra@gmail.com',9,9), 
('assalvatierra@gmail.com',10,10), 
('assalvatierra@gmail.com',11,11), 
('assalvatierra@gmail.com',12,12), 
('assalvatierra@gmail.com',13,13), 
('assalvatierra@gmail.com',14,14), 
('assalvatierra@gmail.com',15,15), 
('assalvatierra@gmail.com',16,16), 
('assalvatierra@gmail.com',17,17), 
('assalvatierra@gmail.com',18,18), 
('assalvatierra@gmail.com',19,19), 
('assalvatierra@gmail.com',20,20), 

('jahdielvillosa@gmail.com',1,1), 
('jahdielvillosa@gmail.com',2,2), 
('jahdielvillosa@gmail.com',3,3), 
('jahdielvillosa@gmail.com',4,4), 
('jahdielvillosa@gmail.com',5,5), 
('jahdielvillosa@gmail.com',6,6), 
('jahdielvillosa@gmail.com',7,7), 
('jahdielvillosa@gmail.com',8,8),
('jahdielvillosa@gmail.com',9,9),
('jahdielvillosa@gmail.com',10,10),
('jahdielvillosa@gmail.com',11,11),
('jahdielvillosa@gmail.com',12,12),
('jahdielvillosa@gmail.com',13,13),
('jahdielvillosa@gmail.com',14,14),
('jahdielvillosa@gmail.com',15,15),
('jahdielvillosa@gmail.com',16,16),
('jahdielvillosa@gmail.com',17,17), 
('jahdielvillosa@gmail.com',18,18), 
('jahdielvillosa@gmail.com',19,19), 
('jahdielvillosa@gmail.com',20,20), 

('Demo@gmail.com',1,1), 
('Demo@gmail.com',3,3), 
('Demo@gmail.com',5,5), 
('Demo@gmail.com',6,6), 
('Demo@gmail.com',17,17), 
('Demo@gmail.com',18,18), 
('Demo@gmail.com',19,19), 
('Demo@gmail.com',20,20); 



insert into SysAccessUsers([UserId],[SysMenuId],[Seqno]) values
('assalvatierra@gmail.com', 21, 21),
('assalvatierra@gmail.com', 22, 22),
('assalvatierra@gmail.com', 23, 23),
('assalvatierra@gmail.com', 24, 24),
('assalvatierra@gmail.com', 25, 25),
('assalvatierra@gmail.com', 26, 26),
('assalvatierra@gmail.com', 27, 27),
('assalvatierra@gmail.com', 28, 28),
('assalvatierra@gmail.com', 29, 29),
('assalvatierra@gmail.com', 30, 30),
('assalvatierra@gmail.com', 31, 31),
('assalvatierra@gmail.com', 32, 32),
('assalvatierra@gmail.com', 33, 33),
('assalvatierra@gmail.com', 34, 34),
('assalvatierra@gmail.com', 35, 35),
('assalvatierra@gmail.com', 36, 36),
('assalvatierra@gmail.com', 37, 37),
('assalvatierra@gmail.com', 38, 38),
('assalvatierra@gmail.com', 39, 39),
('assalvatierra@gmail.com', 40, 40),
('assalvatierra@gmail.com', 41, 41),
('assalvatierra@gmail.com', 42, 42),
('assalvatierra@gmail.com', 43, 43),
('assalvatierra@gmail.com', 44, 44),
('assalvatierra@gmail.com', 45, 45),
('assalvatierra@gmail.com', 46, 46),
('assalvatierra@gmail.com', 47, 47),
('assalvatierra@gmail.com', 48, 48),
('assalvatierra@gmail.com', 49, 49),
('assalvatierra@gmail.com', 50, 50),
('assalvatierra@gmail.com', 51, 51),
('assalvatierra@gmail.com', 52, 52),
('assalvatierra@gmail.com', 53, 53),
('assalvatierra@gmail.com', 54, 54),
('assalvatierra@gmail.com', 55, 55),
('assalvatierra@gmail.com', 56, 56),
('assalvatierra@gmail.com', 57, 57),
('assalvatierra@gmail.com', 58, 58),
('assalvatierra@gmail.com', 59, 59),
('assalvatierra@gmail.com', 60, 60),
('assalvatierra@gmail.com', 61, 61),
('assalvatierra@gmail.com', 62, 62),
('assalvatierra@gmail.com', 63, 63),
('assalvatierra@gmail.com', 64, 64),
('assalvatierra@gmail.com', 65, 65),
('assalvatierra@gmail.com', 66, 66),
('assalvatierra@gmail.com', 67, 67),
('assalvatierra@gmail.com', 68, 68),
('assalvatierra@gmail.com', 69, 69),
('assalvatierra@gmail.com', 70, 70),
('assalvatierra@gmail.com', 71, 71),
('assalvatierra@gmail.com', 72, 72),
('assalvatierra@gmail.com', 73, 73),
('assalvatierra@gmail.com', 74, 74),
('assalvatierra@gmail.com', 75, 75),
('assalvatierra@gmail.com', 76, 76),
('assalvatierra@gmail.com', 77, 77),
('assalvatierra@gmail.com', 78, 78),
('assalvatierra@gmail.com', 79, 79),
('assalvatierra@gmail.com', 80, 80),
('assalvatierra@gmail.com', 81, 81),
('assalvatierra@gmail.com', 82, 82),
('assalvatierra@gmail.com', 83, 83),
('assalvatierra@gmail.com', 84, 84),
('assalvatierra@gmail.com', 85, 85),
('assalvatierra@gmail.com', 86, 86),
('assalvatierra@gmail.com', 87, 87),
('assalvatierra@gmail.com', 88, 88),
('assalvatierra@gmail.com', 89, 89),
('assalvatierra@gmail.com', 90, 90),

('jahdielvillosa@gmail.com', 20, 20),
('jahdielvillosa@gmail.com', 21, 21),
('jahdielvillosa@gmail.com', 22, 22),
('jahdielvillosa@gmail.com', 23, 23),
('jahdielvillosa@gmail.com', 24, 24),
('jahdielvillosa@gmail.com', 25, 25),
('jahdielvillosa@gmail.com', 26, 26),
('jahdielvillosa@gmail.com', 27, 27),
('jahdielvillosa@gmail.com', 28, 28),
('jahdielvillosa@gmail.com', 29, 29),
('jahdielvillosa@gmail.com', 30, 30),
('jahdielvillosa@gmail.com', 31, 31),
('jahdielvillosa@gmail.com', 32, 32),
('jahdielvillosa@gmail.com', 33, 33),
('jahdielvillosa@gmail.com', 34, 34),
('jahdielvillosa@gmail.com', 35, 35),
('jahdielvillosa@gmail.com', 36, 36),
('jahdielvillosa@gmail.com', 37, 37),
('jahdielvillosa@gmail.com', 38, 38),
('jahdielvillosa@gmail.com', 39, 39),
('jahdielvillosa@gmail.com', 40, 40),
('jahdielvillosa@gmail.com', 41, 41),
('jahdielvillosa@gmail.com', 42, 42),
('jahdielvillosa@gmail.com', 43, 43),
('jahdielvillosa@gmail.com', 44, 44),
('jahdielvillosa@gmail.com', 45, 45),
('jahdielvillosa@gmail.com', 46, 46),
('jahdielvillosa@gmail.com', 47, 47),
('jahdielvillosa@gmail.com', 48, 48),
('jahdielvillosa@gmail.com', 49, 49),
('jahdielvillosa@gmail.com', 50, 50),
('jahdielvillosa@gmail.com', 51, 51),
('jahdielvillosa@gmail.com', 52, 52),
('jahdielvillosa@gmail.com', 53, 53),
('jahdielvillosa@gmail.com', 54, 54),
('jahdielvillosa@gmail.com', 55, 55),
('jahdielvillosa@gmail.com', 56, 56),
('jahdielvillosa@gmail.com', 57, 57),
('jahdielvillosa@gmail.com', 58, 58),
('jahdielvillosa@gmail.com', 59, 59),
('jahdielvillosa@gmail.com', 60, 60),
('jahdielvillosa@gmail.com', 61, 61),
('jahdielvillosa@gmail.com', 62, 62),
('jahdielvillosa@gmail.com', 63, 63),
('jahdielvillosa@gmail.com', 64, 64),
('jahdielvillosa@gmail.com', 65, 63),
('jahdielvillosa@gmail.com', 66, 66),
('jahdielvillosa@gmail.com', 67, 67),
('jahdielvillosa@gmail.com', 68, 68),
('jahdielvillosa@gmail.com', 69, 69),
('jahdielvillosa@gmail.com', 70, 70),
('jahdielvillosa@gmail.com', 71, 71),
('jahdielvillosa@gmail.com', 72, 72),
('jahdielvillosa@gmail.com', 73, 73),
('jahdielvillosa@gmail.com', 74, 74),
('jahdielvillosa@gmail.com', 75, 75),
('jahdielvillosa@gmail.com', 76, 76),
('jahdielvillosa@gmail.com', 77, 77),
('jahdielvillosa@gmail.com', 78, 78),
('jahdielvillosa@gmail.com', 79, 79),
('jahdielvillosa@gmail.com', 80, 80),
('jahdielvillosa@gmail.com', 81, 81),
('jahdielvillosa@gmail.com', 82, 82),
('jahdielvillosa@gmail.com', 83, 83),
('jahdielvillosa@gmail.com', 84, 84),
('jahdielvillosa@gmail.com', 85, 85),
('jahdielvillosa@gmail.com', 86, 86),
('jahdielvillosa@gmail.com', 87, 87),
('jahdielvillosa@gmail.com', 88, 88),
('jahdielvillosa@gmail.com', 89, 89),
('jahdielvillosa@gmail.com', 90, 90),



--account access--
('Demo@gmail.com', 21, 21),
('Demo@gmail.com', 22, 22),
('Demo@gmail.com', 23, 23),
('Demo@gmail.com', 24, 24),
--job order--
('Demo@gmail.com', 29, 29),
('Demo@gmail.com', 31, 31),
('Demo@gmail.com', 32, 32),
('Demo@gmail.com', 33, 33),
('Demo@gmail.com', 34, 34),
-- Customers --
('Demo@gmail.com', 41, 41),
('Demo@gmail.com', 42, 42),
-- Resources --
('Demo@gmail.com', 45, 45),
('Demo@gmail.com', 49, 49),
-- Vehicles --
('Demo@gmail.com', 76, 76),
('Demo@gmail.com', 77, 77),
-- Post Sale --
('Demo@gmail.com', 78, 78),
-- Appointments --
('Demo@gmail.com', 79, 79),
('Demo@gmail.com', 80, 80),
('Demo@gmail.com', 81, 81),
('Demo@gmail.com', 82, 82),
-- Post Sale --
('Demo@gmail.com', 83, 83); 

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('Reports','GMS AutoCare Reports',0,'RptGmsAuto','OilReport','',20,1);

--GMS Auto Report
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('RP112','GMS Auto Reports','Reports','A','/Images/Erp/icons/icons-report.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (88,21);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Oil Report' ,'',88,'RptGmsAuto','OilReport','',21,881);	        -- id: 62
-- Payment STatus -- 
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Payments Status Report' ,'Payments Status Report',88,'RptGmsAuto','JobStatusReport','',21,882);	        -- id: 62

-- Payment STatus -- 
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Referral Report' ,'Referral Report',88,'RptGmsAuto','JobStatusReport','',21,883);	        -- id: 62


-- Job Monitory --
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Job Monitor' ,'Job Monitor',3,'JobOrder','JobsMonitor','',21,882);	        -- id: 62
		
		