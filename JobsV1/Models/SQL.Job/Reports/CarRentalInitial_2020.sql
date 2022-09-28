
insert into CarUnits ( Description, Remarks, CarCategoryId , SelfDrive, SortOrder) 
values ('Van (14 seater)'     ,'Toyota GL Grandia Tourer'       ,1 ,0 ,7);

insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (7,'toyota-tourer-2020-px.png'     ,'','MAIN');
	   
insert into CarViewPages (CarUnitId, Viewname)
values (7,'CarDetail_tourer');

insert into CarRates (Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate)
values (3500,2500,2250,100,5,7,300); --tourer


insert into CarUnitMetas(carUnitId,PageTitle,MetaDesc,HomeDesc)
values
	-- Toyota Tourer -- 
	(7, 'Toyota Hiace GL Grandia Tourer for Rent in Davao City, Reliable van rental company in Davao City',
	'Toyota Hiace GL Grandia Tourer is comfortable 10-14 seater van for business, tour and family travel needs. Prioritize comfort and safety.',
	'A very comfortable van that can accommodate 14pax with individual reclining seats. Very few rent-a-car company in Davao offers this type of vehicle.'
	);


Insert into CarRateUnitPackages (CarUnitId,CarRatePackageId,DailyRate,FuelLonghaul,FuelDaily,DailyAddon)
values

-- big van ( Nissan Premium)
( 7, 1, 0,  0,   0,   0 ), --+ selfdrive
( 7, 2, 0,  0, 500,   0 ), --+ city
( 7, 3, 0,  0, 800, 1200 ), --+ Countryside
( 7, 4, 0,  0, 800, 1200 ), --+ Samal

( 7, 5,	 0,    0, 500, 1200 ), --panabo
( 7, 6,  0,  200, 500, 1000 ), --tagum
( 7, 7,  0,  700, 700, 300 ), --davao del norte
( 7, 8,  0,  700, 700, 1800 ), --comval
( 7, 9,  0,  900, 600, 1000 ), --govgen
( 7, 10,  200,  900, 600, 1700 ), --Mati
( 7, 11, 1200, 2000, 700, 1000 ), --Davao Oriental
( 7, 12, 1700, 2500, 700, 1000 ), --Agusan del sur
( 7, 13, 2200, 2500, 700, 1000 ), --Agusan del Norte
( 7, 14, 1700, 2500, 700, 1000 ), --Surigao del sur
( 7, 15, 2700, 3500, 700, 1000 ), --Surigao del Norte

( 7, 16,    0,    0, 600, 1000 ), --Marilog
( 7, 17,    0,  400, 600, 1000 ), --Buda
( 7, 18,    0,  900, 600, 1000 ), --Valencia
( 7, 19, 1200, 1500, 600, 1000 ), --Malaybalay
( 7, 20, 1200, 2000, 600, 1000 ), --Manolo fortich
( 7, 21, 1700, 2500, 600, 1000 ), --Cagayan
( 7, 22, 2200, 3000, 700, 1000 ), --Misamis
( 7, 23, 2200, 3000, 700, 1000 ), --Iligan

( 7, 24, 0, 0, 600,1000 ), --Santa Cruz, davao del sur
( 7, 25, 0, 400, 600,1000 ), --Digos, davao del sur
( 7, 26, 0, 600, 700,1000 ), --davao del sur
( 7, 27, 0, 900, 600,1000 ), --Malita
( 7, 28, 700, 1000, 600,1000 ), --Don Marcelino
( 7, 29, 700, 1000, 600,1000 ), --General Santos
( 7, 30, 700, 1400, 700,1000 ), --Saranggani
( 7, 31, 700, 1500, 600,1000 ), --Koronadal
( 7, 32, 700, 1500, 700,1000 ), --Isulan
( 7, 33, 700, 1500, 700,1000 ), --Sultan Kudarat
( 7, 34,   0, 1000, 600,1000 ), --Kidapawan
( 7, 35, 700, 1000, 600,1000 ), --Ilomavis
( 7, 36, 700, 1000, 600,1000 ), --Kabacan
( 7, 37, 1200, 1200, 600,1000 ), --Arakan
( 7, 38, 1200, 1500, 600,1000 ), --Midsayap
( 7, 39, 1700, 2000, 600,1000 ), --Cotabato City
( 7, 40, 1200, 2000, 700,1000 ); --North Cotabato

