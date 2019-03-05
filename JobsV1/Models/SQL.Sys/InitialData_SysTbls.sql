
insert into SysCmdIdRefs values 
(20,'OPEN MENU','get submenus'), 
(21,'EXECUTE','Execute controller/action');

insert into SysMenus values 
('Account','Account',0,'','','',20,1),
('Customer','Customer',0,'Customer','Index','',20,2),
('Supplier','Supplier',0,'Supplier','Index','',20,3);

insert into SysServices values('HD110','HelpDesk Beta 1.0','IT support system','A','../Images/Erp/HelpDeskIcon.jpg');
insert into SysServiceMenus values(1,1);
insert into SysMenus values 
	('Main','',1,'','','',21,1),
	('Staffs','',1,'','','',21,2);

insert into SysServices values('FD110','Invoice Dispatch Beta 1.0','Invoice Dispatching System','A','../Images/Erp/FdaDispatchingIcon.jpg');
insert into SysServiceMenus values(2,2);
insert into SysMenus values 
	('FDA Dispatches','',2,'FdaDispatch','Index','',21,1),
	('Registered Mail','',2,'FdaRegMail','Index','',21,2),
	('Addresses','',2,'FdaAddress','Index','',21,3);

insert into EntBusinesses values('Real Breeze Davao','RBD','11-2233','abel');
insert into EntServices values(1,1,'2019/1/1');
insert into EntServices values(2,1,'2019/1/1');

