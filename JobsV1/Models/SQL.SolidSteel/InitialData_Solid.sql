
insert into SysCmdIdRefs([CmdId],[Description],[Remarks]) values 
(20,'OPEN MENU','get submenus'), 
(21,'EXECUTE','Execute controller/action');

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('System Access','Access Control',0,'SysAccessUsers','ModuleList','',20,1),
('Sales Leads','Sales Lead',0,'SalesLeads','Index','',20,2),
('Job Orders','Job Orders',0,'JobOrder','Index','',20,3),
('Suppliers','List of Suppliers',0,'Suppliers','Index','',20,4),
('Customers','Job Orders',0,'CustEntMains','Index','',20,5),
('Items Master','Job Orders',0,'SupplierItems','Index','',20,6),
('Master List','Master List',0,'Cities','Index','span=30',20,7),
('Admin','Admin',0,'Admin','UserList','',20,8),
('Activities','Activities',0,'Activities','Index','',20,9),
('Post Sales','Activities Post Sales',0,'Activities','ActivitiesPostSales','',20,10)
;


insert into EntBusinesses([Name],[ShortName],[BussRegNo],[User]) values
('Real Breeze Davao','RBD','11-2233','abel');

--System Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SA101','System Access','Users Access Control','A','/Images/Erp/icons-key.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (1,1);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Modules','',1,'SysAccessUsers','ModuleList','',21,11), -- id: 11
	('Users','',1,'SysAccessUsers','UsersList','',21,12),    -- id: 12
	('Add Users','',1,'Account','Register','',21,13),		 -- id: 13
	('Password','',1,'Account','ForgotPassword','',21,14);   -- id: 14
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(1,1,'2019/1/1');

--Sales Leads
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SL102','Sales Leads','Leads,Quotations,Reservations','A','/Images/Erp/icons-flag.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (2,2);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Sales Leads','',2,'SalesLeads','Index','',21,21),		  -- id: 15
	('Quotations','',2,'JobMains','JobLeads','',21,22),		  -- id: 16
	('Reservations','',2,'CarReservations','Index','',21,23); -- id: 17
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(2,1,'2019/1/1');

--Job Order
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('JO103','Job Orders','Jobs / Work in Progress','A','/Images/Erp/inprogress.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (3,3);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Work in Progress','',3,'JobOrder','Index','',21,31),		-- id: 18
	('Status','',3,'JobOrder','jobStatus','',21,32),			-- id: 19
	('Quick List','',3,'JobMains','ActiveJobs','',21,33),		-- id: 20
	('Listing','',3,'JobOrder','JobListing','span=30',21,34),	-- id: 21
	('Job Table','',3,'JobMains','JobTable','span=30',21,35),	-- id: 22
	('Availability','',3,'InvItems','Availability','',21,36);	-- id: 23
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(3,1,'2019/1/1');

--Suppliers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SU104','Suppliers','List of Suppliers','A','/Images/Erp/icons/icons-exchange.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (4,4);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Supplier List','',4,'Suppliers','Index','',21,41)			-- id: 24
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(4,1,'2019/1/1');

--Customers
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('CU105','Customers','List of Customers','A','/Images/Erp/icons/icons-business.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (5,5);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Companies','',5,'CustEntMains','Index','',21,51),			-- id: 26
	('Contacts','',5,'Customers','Index','',21,52);				-- id: 27
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(5,1,'2019/1/1');

--Items Master
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('EQ106','Items Master','List of Items','A','/Images/Erp/icons/icons-trolley.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (6,6);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Items','',6,'InvItems','Index','',21,62),					-- id: 28
	('Items Master','',6,'SupplierItems','Index','',21,61);		-- id: 29
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(6,1,'2019/1/1');

--Master List
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO114','Master List','Master List of Items','A','/Images/Erp/icons/icons-masterlist.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (7,7);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Cities' ,'',7,'Cities','Index','',21,71),					-- id: 30
		('Categories' ,'',7,'CustCategories','Index','',21,72),		-- id: 31
		('Documents' ,'',7,'SupDocuments','Index','',21,73);		-- id: 32
		
--Admin Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO115','Admin','Admin Access','A','/Images/Erp/icons/icons-admin.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (8,8);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Agent Assignment' ,'',8,'Admin','Index','',21,81);	 -- id: 33

--Activities Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO116','Activities','User Activities Listing','A','/Images/Erp/icons/icons-activities.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (9,9);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Activities' ,'',9,'Activities','Index','',21,91),			-- id: 34
		('Performance' ,'',9,'Activities','Performance','',21,92);	-- id: 25

		
--Activities Post Sales
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('NO116','Post Sales','Activities Post Sales','A','/Images/Erp/icons/icons-callback.png');

insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (10,10);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) 
values 	('Customer Post Sales' ,'',10,'Activities','ActivitiesPostSales','',21,101),			-- id: 36
		('Customer Status' ,'',10,'Activities','StatusActivities','',21,102);	-- id: 37
		
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(10,1,'2019/1/1');

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

('Demo@gmail.com',4,4), 
('Demo@gmail.com',5,5), 
('Demo@gmail.com',6,6), 
('Demo@gmail.com',7,7), 
('Demo@gmail.com',8,8), 
('Demo@gmail.com',9,9),		
('Demo@gmail.com',10,10); 



insert into SysAccessUsers([UserId],[SysMenuId],[Seqno]) values
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
