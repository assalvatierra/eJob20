﻿
insert into SysCmdIdRefs([CmdId],[Description],[Remarks]) values 
(20,'OPEN MENU','get submenus'), 
(21,'EXECUTE','Execute controller/action');

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('System Access','Access Control',0,'SysAccessUsers','ModuleList','',20,1),
('Sales Leads','Sales Lead',0,'SalesLeads','Index','',20,2),
('Job Orders','Job Orders',0,'JobOrder','Index','',20,3),
('Suppliers','List of Suppliers',0,'Suppliers','Index','',20,4),
('Customers','Job Orders',0,'CustEntMains','Index','',20,5),
('Equipments','Job Orders',0,'SupplierItems','Index','',20,6),
('Packages','Job Orders',0,'CarRatePackages','Index','',20,7),
('Reporting','Job Orders',0,'Reporting','Index','',20,8),
('Accounts','Accounts',0,'Accounting/AccntMains','Index','',20,10),
('HR','HRIS',0,'Personel/HrPersonels','Index','',20,11),
('Products','Products',0,'Products/SMProducts','Index','',20,12),
('Bookkeeping','Bookkeeping',0,'Accounting/AsMain','Index','span=30',20,13),
('Master List','Master List',0,'Cities','Index','span=30',20,14),
('Admin','Admin',0,'Admin','UserList','',20,15),
('Activities','Activities',0,'Activities','Index','',20,16),
('Vehicles','Vechicles',0,'AutoCare/Vehicles','Index','',20,17),
('Trip Logs','Trip Logs',0,'Personel/CarRentalLog','Index','',20,18),
('Receivables','Receivable Transactions',0,'Receivables/ArTransactions','Index','',20,19),
('Payables','Payables Transactions',0,'Payables/ApTransactions','Index','',20,20),
('Procurement','Payables Transactions',0,'Payables/ApTransactions','Index','',20,20)
;

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('Procurement','Procurement Leads',0,'Procurement','Index','',20,2);

insert into EntBusinesses([Name],[ShortName],[BussRegNo],[User]) values
('Real Breeze Davao','RBD','11-2233','abel');
--Added by jahdiel - 3/6/19

--System Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('SA101','System Access','Users Access Control','A','/Images/Erp/icons-key.png', 1,'key');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (1,1);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Modules','',1,'SysAccessUsers','ModuleList','',21,11), -- id: 13
	('Users','',1,'SysAccessUsers','UsersList','',21,12),    -- id: 14
	('Add Users','',1,'Account','Register','',21,13),		 -- id: 15
	('Password','',1,'Account','ForgotPassword','',21,14);   -- id: 16
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(1,1,'2019/1/1');

--Sales Leads
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('SL102','Sales Leads','Leads,Quotations,Reservations','A','/Images/Erp/icons-flag.png', 2, 'envelope');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (2,2);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Sales Leads','',2,'SalesLeads','Index','',21,21),		  -- id: 17
	('Quotations','',2,'JobMains','JobLeads','',21,22),		  -- id: 18
	('Reservations','',2,'CarReservations','Index','',21,23), -- id: 19
	('Online Reservations', '', 2, 'OnlineReservations','Index','',21,24);
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(2,1,'2019/1/1');

--Job Order
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('JO103','Job Orders','Jobs / Work in Progress','A','/Images/Erp/inprogress.png', 3, 'calendar-o');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (3,3);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Work in Progress','',3,'JobOrder','Index','',21,31),		-- id: 20
	('Status','',3,'JobOrder','jobStatus','',21,32),			-- id: 21
	('Quick List','',3,'JobMains','ActiveJobs','',21,33),		-- id: 21
	('Listing','',3,'JobOrder','JobListing','span=30',21,34),	-- id: 23
	('Job Table','',3,'JobMains','JobTable','span=30',21,35),	-- id: 24
	('Availability','',3,'InvItems','Availability','',21,36),	-- id: 24
	('Trip Listing','',3,'JobMains','TripListing' ,'',21,36);	-- id: 25
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(3,1,'2019/1/1');

--Suppliers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('SU104','Suppliers','List of Suppliers','A','/Images/Erp/icons/icons-exchange.png',4, 'group');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (4,4);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Master List','',4,'Suppliers','Index','',21,41),					-- id: 26
	('Items','',4,'InvItems','Index','',21,42),					-- id: 27
	('PO List','',4,'SupplierPoHdrs','Index','',21,43),			-- id: 28
	('Coop Members','',4,'CoopMembers','Index','',21,44);		-- id: 29
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(4,1,'2019/1/1');

--Customers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('CU105','Customers','List of Customers','A','/Images/Erp/icons/icons-business.png', 5, 'id-card');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (5,5);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Companies','',5,'CustEntMains','Index','',21,51),			-- id: 31
	('Contacts','',5,'Customers','Index','',21,52),				-- id: 30
	('Email Blaster','',5,'EmailBlaster','Index','',21,53),  	-- id: 32
	('Customer Portal','',5,'PortalCustomers','Index','',21,54); -- id: 33
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(5,1,'2019/1/1');

--Equipments
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('EQ106','Equipments','List of Equipments','A','/Images/Erp/icons/icons-trolley.png', 6, 'dropbox');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (6,6);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Items','',6,'InvItems','Index','',21,62),					-- id: 34
	('Availability','',6,'InvItems','Availability','',21,63),	-- id: 35
	('Gate Control','',6,'InvCarGateControls','Index','',21,64),-- id: 36
	('Maintenance','',6,'InvCarRecords','Index','',21,65),		-- id: 37
	('Items Master','',6,'SupplierItems','Index','',21,61),		-- id: 38
	('Maintenance Summary','',6,'InvItems','Maintenance','',21,61);-- id: 39
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(6,1,'2019/1/1');
--Packages
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('PK107','Packages','Reports','A','/Images/Erp/icons/icons-box.png', 7, 'tags');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (7,7);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Package Unit','',7,'CarRatePackages','Index','',21,71),	 -- id: 39
	('Package List','',7,'CarRateUnitPackages','Index','',21,72),-- id: 40
	('Unit Rates','',7,'CarRates','Index','',21,73),			 -- id: 41
	('Groups','',7,'RateGroups','Index','',21,73);				 -- id: 42

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(7,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Reporting
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('RP108','Reporting','Reports','A','/Images/Erp/icons/icons-report.png', 8, 'line-chart');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (8,8);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Job Listing','',8,'Reporting','Index','',21,81),			-- id: 43
	('Payment','',8,'Reporting','Index','',21,82),				-- id: 44
	('Package Rates','',8,'Reporting','Index','',21,83);		-- id: 45
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(8,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Accounts
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO110','Accounts','Accounting','A','/Images/Erp/icons/icons-accounting.png', 10, 'money');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (10,10);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Main Accounts','',10,'Accounting/AccntMains','Index','',21,101),			-- id: 48
		('Ledgers','',10,'Accounting/Accntledgers','Index','',21,102),				-- id: 49
		('Account Category','',10,'Accounting/AccntCategories','Index','',21,103),	-- id: 50
		('Transactions','',10,'Accounting/AccntTrxHdrs','Index','',21,103);			-- id: 51
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values	
(10,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--HRIS -- Personnel
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO111','HR','Human Resources','A','/Images/Erp/icons/icons-employee.png', 11, 'address-book');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (11,11);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Personnel Lists','',11,'Personel/HrPersonels','Index','',21,111);		-- id: 52

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(11,1,'2019/1/1');
insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');

--Products
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO112','Products','Products','A','/Images/Erp/icons/icons-products.png', 12,'tag');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (12,12);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Products','',12,'Products/SmProducts','Index','',21,121),		-- id: 53
		('Suppliers' ,'',12,'Products/SmSuppliers','Index','',21,122), 	-- id: 54
		('Ads' ,'',12,'Products/SmProdAds','Index','',21,123);	        -- id: 55
		
--Bookkeeping
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO113','Book keeping','Book Keeping','A','/Images/Erp/icons/icons-bookkeeping.png',13,'book');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (13,13);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Expense','',13,'Accounting/AsExpenses','ParamForm','',21,131),		-- id: 56
		('Sales' ,'',13,'Accounting/AsSales','ParamForm','',21,133);	        -- id: 57
		
--Master List
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO114','Master List','Master List of Items','A','/Images/Erp/icons/icons-masterlist.png', 14, 'list');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (14,14);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Cities' ,'',14,'Cities','Index','',21,141),			-- id: 58
		('Categories' ,'',14,'CustCategories','Index','',21,142),-- id: 59
		('Documents' ,'',14,'SupDocuments','Index','',21,143);	 -- id: 60
		
--Admin Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO115','Admin','Admin Access','A','/Images/Erp/icons/icons-admin.png', 15, 'lock');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (15,15);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Agent Assignment' ,'',15,'Admin','Index','',21,151);	 -- id: 61

--Activities Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO116','Activities','User Activities Listing','A','/Images/Erp/icons/icons-activities.png', 16,'horn');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (16,16);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Activities' ,'',16,'Activities','Index','',21,161),
		('Performance' ,'',16,'Activities','Performance','',21,162);	 -- id: 61
		
--AutoCare Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('NO117','AutoCare','Vehicles Listing','A','/Images/Erp/icons/icons-activities.png', 17, 'car');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (17,17);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Vehicles' ,'',17,'AutoCare/Vehicles','Index','',21,171),	--72
		('Vehicle Models' ,'',17,'AutoCare/VehicleModels','Index','',21,172);	 -- id: 73
		
--Trip Logs --
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('TL1001','Trip Logs','Trip Logs','A','/Images/Erp/icons/icons-report.png', 18,'random');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (18,18);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Trip Logs' ,'',18,'AutoCare/Vehicles','Index','',21,151);	 -- id: 73


-- Receivables --
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('AR001','Recievables','Receivables and Collections','A','/Images/Erp/icons/icons-receivables.png', 19, 'bookmark');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (19,19);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Receivables' ,'',19,'Receivables/ArTransactions','Index','',21,191),	-- id: 74
	    ('Accounts' ,'',19,'Receivables/ArAccounts','Index','',21,192),			-- id: 74
	    ('Manage' ,'',19,'Receivables/ArMgt','Index','',21,193),				-- id: 74
	    ('For Approval' ,'',19,'Receivables/ArMgt','Approval','',21,194),				-- id: 74,
	    ('For Settlement' ,'',19,'Receivables/ArMgt','Settlement','',21,195),				-- id: 74,
	    ('Reports' ,'',19,'Receivables/ArReports','Index','',21,196);				-- id: 74;	
		
-- Payables --
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath],[SeqNo],[IconFA]) values
('AR001','Payables','Payables','A','/Images/Erp/icons/icons-receivables.png', 20, 'bookmakr-o');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (20,20);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Payables' ,'',20,'Payables/ApTransactions','Index','',21,201),  -- id: 74
	    ('Accounts' ,'',20,'Payables/ApAccounts','Index','',21,202),   	  -- id: 74
	    ('Reports' ,'',20,'Payables/ApReports','Index','',21,206);		  -- id: 74;	


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

('Demo@gmail.com',1,1), 
('Demo@gmail.com',3,3), 
('Demo@gmail.com',5,5), 
('Demo@gmail.com',6,6), 
('Demo@gmail.com',17,17),
('Demo@gmail.com', 18, 18),
('Demo@gmail.com', 19, 19); 



insert into SysAccessUsers([UserId],[SysMenuId],[Seqno]) values
('assalvatierra@gmail.com', 20, 20),
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


('Demo@gmail.com', 20, 20),
('Demo@gmail.com', 21, 21),
('Demo@gmail.com', 26, 26),
('Demo@gmail.com', 27, 27),
('Demo@gmail.com', 28, 28),
('Demo@gmail.com', 29, 29),
('Demo@gmail.com', 30, 30),
('Demo@gmail.com', 31, 31),
('Demo@gmail.com', 32, 32),
('Demo@gmail.com', 37, 37),
('Demo@gmail.com', 38, 38),
('Demo@gmail.com', 39, 39),
('Demo@gmail.com', 40, 40),
('Demo@gmail.com', 41, 41),
('Demo@gmail.com', 42, 42),
('Demo@gmail.com', 43, 43),
('Demo@gmail.com', 44, 44),
('Demo@gmail.com', 45, 45),
('Demo@gmail.com', 72, 72),
('Demo@gmail.com', 73, 73); 
