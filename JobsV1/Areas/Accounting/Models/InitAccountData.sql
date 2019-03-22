Insert into AccntTypes([Code], [NormalForm]) 
	values ( 'DB','DB'), ('CR','CR');

insert into AccntTrxTypes([Code],[Remarks])
 values ('GT','General Transaction'),('CR','Cash Receipt'),('CV','Cash Voucher'),('BK','Bank Transaction') ;


	

Insert into AccntCategories([Code],[Description],[OrderNo],[AccntTypeId])
values	( '1000','ASSETS',1,1),
		( '2000','LIABILITIES',2,2),
		( '3000','OWNERS EQUITY',3,2),
		( '4000','REVENUES',4,2),
		( '5000','EXPENSES',5,1),
		( '6000','NON-OPERATING GAINS',6,2),
		( '7000','NON-OPERATING LOSSES',7,1);

Insert into AccntMains([Code],[Name],[Remarks],[AccntTypeId],[AccntCategoryId])
values 
( '10100','CASH','',1,1),
( '10200','BANK','',1,1),
( '10300','RECEIVABLES','',1,1),
( '10400','VEHICLES','',1,1),
( '10500','FIXED ASSETS','',1,1),
( '10600','INTANGIBLE ASSETS','',1,1),

( '20100','SHORT-TERM PAYABLES','',2,2),
( '20200','LONG-TERM PAYABLES','',2,2),
( '20300','WAGES','',2,2),

( '30100','OWNERS EQUITY','',2,3),

( '40100','SALES','',2,4),
( '40200','RENTAL','',2,4),

( '50100','SALARIES','',1,5),
( '50200','RENTAL','',1,5),
( '50300','UTILITIES','',1,5),
( '50400','ADVERTISING','',1,5),
( '50500','OTHERS','',1,5),

( '60100','NON-OPERATING GAINS','',2,6),

( '70100','NON-OPERATING LOSSES','',1,7);


Insert into AccntLedgers([Code],[Name],[AccntMainId],[Remarks])
values 
( '1100','CASH',1,''), --10100
( '1200','BANK',2,''), --10200
( '1300','RECEIVABLES',3,''), --10300
( '1400','VEHICLES',4,''), --10400
( '1500','FIXED ASSETS',5,''), --10500
( '1600','INTANGIBLE ASSETS',6,''), --10600

( '2100','SHORT-TERM PAYABLES',7,''), --20100
( '2200','LONG-TERM PAYABLES',8,''), --20200
( '2300','WAGES',9,''), --20300

( '3100','OWNERS EQUITY',10,''), --30100

( '4100','SALES',11,''), --40100
( '4200','RENTAL',12,''), --40200

( '5100','SALARIES',13,''), --50100
( '5200','RENTAL',14,''), --50200
( '5300','UTILITIES',15,''), --50300
( '5400','ADVERTISING',16,''), --50400
( '5500','OTHERS',17,''), --50500

( '6100','NON-OPERATING GAINS',18,''), --60100

( '7100','NON-OPERATING LOSSES',19,''); --70100




-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------