
insert into SysCmdIdRefs([CmdId],[Description],[Remarks]) values 
(20,'OPEN MENU','get submenus'), 
(21,'EXECUTE','Execute controller/action');

insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
('System Access','Access Control',0,'SysAccessUsers','Index','',20,101),
('Sales Lead','Sales Lead',0,'SalesLeads','Index','',20,102),
('Job Orders','Job Orders',0,'SysAccessUsers','Index','',20,103)
;
--('Account','Account',0,'','','',20,102),
--('Customer','Customer',0,'Customer','Index','',20,103),
--('Supplier','Supplier',0,'Supplier','Index','',20,104);

--insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values 
--('HD110','HelpDesk Beta 1.0','IT support system','A','../Images/Erp/HelpDeskIcon.jpg');
--insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (1,1);
--insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
--	('Main','',1,'','','',21,1),
--	('Staffs','',1,'','','',21,2);

--insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
--('FD110','Invoice Dispatch Beta 1.0','Invoice Dispatching System','A','../Images/Erp/FdaDispatchingIcon.jpg');
--insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (2,2);
--insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
--	('FDA Dispatches','',2,'FdaDispatch','Index','',21,1),
--	('Registered Mail','',2,'FdaRegMail','Index','',21,2),
--	('Addresses','',2,'FdaAddress','Index','',21,3);


insert into EntBusinesses([Name],[ShortName],[BussRegNo],[User]) values
('Real Breeze Davao','RBD','11-2233','abel');

--Added by jahdiel - 3/6/19

--System Access
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SA101','System Access Users','Access Control','A','../Images/Erp/inprogress.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (1,1);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Modules','',1,'FdaDispatch','Index','',21,1),
	('Users','',1,'SysAccessUsers','Index','',21,2),
	('Add Users','',1,'SysAccessUsers','Create','',21,3),
	('Password','',1,'Account','ForgotPassword','',21,4);
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(1,1,'2019/1/1');

--Sales Leads
insert into SysServices([SysCode],[Description],[Remarks],[Status],[IconPath]) values
('SL101','Sales Leads','Leads,Quotations,Reservations','A','../Images/Erp/inprogress.png');
insert into SysServiceMenus([SysMenuId],[SysServiceId]) values (2,2);
insert into SysMenus([Menu],[Remarks],[ParentId],[Controller],[Action],[Params],[CmdId],[Seqno]) values 
	('Quotations','',2,'JobMains','JobLeads','',21,1),
	('Reservations','',2,'CarReservations','Index','',21,2);
insert into EntServices([SysServiceId],[EntCompanyId],[Expiry]) values 
(2,1,'2019/1/1');


insert into SysSettings([SysKey],[SysValue],[Remarks]) values
('DatabaseState','Dev.DB','Development DB');


insert into SysAccessUsers([UserId],[SysMenuId],[Seqno]) values
('assalvatierra@gmail.com',1,1), 
('assalvatierra@gmail.com',2,2), 
('assalvatierra@gmail.com',3,3), 
('assalvatierra@gmail.com',4,4), 
('assalvatierra@gmail.com',5,5),

('jahdielvillosa@gmail.com',1,1), 
('jahdielvillosa@gmail.com',2,2), 
('jahdielvillosa@gmail.com',3,3), 
('jahdielvillosa@gmail.com',4,4), 
('jahdielvillosa@gmail.com',5,5); 
