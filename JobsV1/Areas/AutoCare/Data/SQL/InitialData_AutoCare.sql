﻿
insert into SysCmdIdRefs([CmdId],[Description],[Remarks]) values 
(20,'OPEN MENU','get submenus'), 
(21,'EXECUTE','Execute controller/action');

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('System Access','Access Control',0,'SysAccessUsers','ModuleList','',20,1),
('Job Orders','Job Orders',0,'JobOrder','Index','',20,2),
('Customers','Customers',0,'CustEntMains','Index','',20,3),
('Resources','Resources',0,'SupplierItems','Index','',20,4),
('Vehicles','Vechicles',0,'AutoCare/Vehicles','Index','',20,5),
('Post Sales','Job Post Sales',0,'JobPostSales','Index','',20,6),
('Appointments','Job Post Sales',0,'AutoCare/Appointments','Index','',20,7),
('Cashier','Cashier',0,'Cashier','Index','',20,8),
('Reports','GMS AutoCare Reports',0,'RptGmsAuto','OilReport','',20,9)
;


insert into EntBusinesses([Name],[ShortName],[BussRegNo],[User]) values
('Real Breeze Davao','RBD','11-2233','abel');
--Added by jahdiel - 3/6/19

--System Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SA101','System Access','Users Access Control','A','/Images/Erp/icons-key.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (1,1);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Modules','',1,'SysAccessUsers','ModuleList','',21,11), -- id: 10
	('Users','',1,'SysAccessUsers','UsersList','',21,12),    -- id: 11
	('Add Users','',1,'Account','Register','',21,13),		 -- id: 12
	('Password','',1,'Account','ForgotPassword','',21,14);   -- id: 13
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(1,1,'2019/1/1');


--Job Order
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('JO103','Job Orders','Jobs / Work in Progress','A','/Images/Erp/inprogress.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (2,2);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Work in Progress','',2,'JobOrder','Index','',21,21),		-- id: 14
	('Status','',2,'JobOrder','jobStatus','',21,22),			-- id: 15
	('Quick List','',2,'JobMains','ActiveJobs','',21,23),		-- id: 16
	('Listing','',2,'JobOrder','JobListing','span=30',21,24),	-- id: 17
	('Job Table','',2,'JobMains','JobTable','span=30',21,25),	-- id: 18
	('Availability','',2,'InvItems','Availability','',21,26),	-- id: 19
	('Trip Listing','',2,'JobMains','TripListing' ,'',21,27),	-- id: 20
	('Job Monitor' ,'Job Monitor',2,'JobOrder','JobsMonitor','',21,28);-- id: 21

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(2,1,'2019/1/1');

--Customers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('CU105','Customers','List of Customers','A','/Images/Erp/icons/icons-business.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (3,3);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Companies','',3,'CustEntMains','Index','',21,31),			-- id: 22
	('Contacts','',3,'Customers','Index','',21,32);				-- id: 23
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(3,1,'2019/1/1');

--Equipments /Resources
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('MI106','Master Items','List of Resources','A','/Images/Erp/icons/icons-trolley.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (4,4);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Items','',4,'InvItems','Index','',21,42),					-- id: 24
	('Service Types','',4,'SupplierItems','Index','',21,41),	-- id: 25
	('Services','',4,'Services','Index','',21,42),			    -- id: 26
	('Service Group','',4,'SvcGroups','Index','',21,43);		-- id: 27

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(4,1,'2019/1/1');

--Vehicles 
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO117','Vehicles','Vehicles Listing','A','/Images/AutoCare/icons-suv.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (5,5);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Units' ,'',5,'AutoCare/Vehicles','Index','',21,51),			-- id: 28
		('Models' ,'',5,'AutoCare/VehicleModels','Index','',21,52);	-- id: 29

--POst Sales
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO118','Post Sales','Job Post Sales','A','/Images/AutoCare/icons-callback.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (6,6);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values ('Post Sales' ,'',6,'JobPostSales','Index','',21,61);			-- id: 30

--Appointments
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO119','Appointments','Job Appointments','A','/Images/AutoCare/icons-call.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (7,7);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values  ('Appointment' ,'',7,'AutoCare/Appointments','Index','',21,71),			-- id:31
		('Slots' ,'',7,'AutoCare/AppointmentSlots','Index','',21,72),				-- id: 32
		('Availability' ,'',7,'AutoCare/Appointments','Availability','',21,733);	-- id: 33

		
--Cashier
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO120','Cashier','Cashier','A','/Images/AutoCare/icons-cash-register.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (8,8);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values ('Cashier' ,'',8,'Cashier','Index','',21,81);								-- id: 34

-- GMS Reports -- 
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('RP112','GMS Auto Reports','Reports','A','/Images/Erp/icons/icons-report.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (9,9);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Oil Report' ,'',9,'RptGmsAuto','OilReport','',21,91),						-- id: 35
		('Payments Status Report' ,'',9,'RptGmsAuto','JobStatusReport','',21,92),   -- id: 36
		('Referral Report' ,'t',9,'RptGmsAuto','JobStatusReport','',21,93);			-- id: 37

insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(9,1,'2019/1/1');
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

('jahdielvillosa@gmail.com',1,1), 
('jahdielvillosa@gmail.com',2,2), 
('jahdielvillosa@gmail.com',3,3), 
('jahdielvillosa@gmail.com',4,4), 
('jahdielvillosa@gmail.com',5,5), 
('jahdielvillosa@gmail.com',6,6), 
('jahdielvillosa@gmail.com',7,7), 
('jahdielvillosa@gmail.com',8,8),
('jahdielvillosa@gmail.com',9,9),

('Demo@gmail.com',1,1), 
('Demo@gmail.com',2,2), 
('Demo@gmail.com',3,3), 
('Demo@gmail.com',4,4), 
('Demo@gmail.com',5,5), 
('Demo@gmail.com',6,6), 
('Demo@gmail.com',7,7), 
('Demo@gmail.com',8,8), 
('Demo@gmail.com',9,9);



insert into SysAccessUsers([UserId],[SysMenuId],[Seqno]) values
('assalvatierra@gmail.com', 10, 10),
('assalvatierra@gmail.com', 11, 11),
('assalvatierra@gmail.com', 12, 12),
('assalvatierra@gmail.com', 13, 13),
('assalvatierra@gmail.com', 14, 14),
('assalvatierra@gmail.com', 15, 15),
('assalvatierra@gmail.com', 16, 16),
('assalvatierra@gmail.com', 17, 17),
('assalvatierra@gmail.com', 18, 18),
('assalvatierra@gmail.com', 19, 19),
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

('jahdielvillosa@gmail.com', 10, 10),
('jahdielvillosa@gmail.com', 11, 11),
('jahdielvillosa@gmail.com', 12, 12),
('jahdielvillosa@gmail.com', 13, 13),
('jahdielvillosa@gmail.com', 14, 14),
('jahdielvillosa@gmail.com', 15, 15),
('jahdielvillosa@gmail.com', 16, 16),
('jahdielvillosa@gmail.com', 17, 17),
('jahdielvillosa@gmail.com', 18, 18),
('jahdielvillosa@gmail.com', 19, 19),
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



('Demo@gmail.com', 10, 10),
('Demo@gmail.com', 11, 11),
('Demo@gmail.com', 12, 12),
('Demo@gmail.com', 13, 13),
('Demo@gmail.com', 14, 14),
('Demo@gmail.com', 15, 15),
('Demo@gmail.com', 16, 16),
('Demo@gmail.com', 17, 17),
('Demo@gmail.com', 18, 18),
('Demo@gmail.com', 19, 19),
('Demo@gmail.com', 20, 20),
('Demo@gmail.com', 21, 21),
('Demo@gmail.com', 22, 22),
('Demo@gmail.com', 23, 23),
('Demo@gmail.com', 24, 24),
('Demo@gmail.com', 25, 25),
('Demo@gmail.com', 26, 26),
('Demo@gmail.com', 27, 27),
('Demo@gmail.com', 28, 28),
('Demo@gmail.com', 29, 29),
('Demo@gmail.com', 30, 30),
('Demo@gmail.com', 31, 31),
('Demo@gmail.com', 32, 32),
('Demo@gmail.com', 33, 33),
('Demo@gmail.com', 34, 34),
('Demo@gmail.com', 35, 35),
('Demo@gmail.com', 36, 36),
('Demo@gmail.com', 37, 37);