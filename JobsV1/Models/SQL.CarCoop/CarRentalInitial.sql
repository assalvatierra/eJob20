
insert into CarCategories (Description, Remarks)
values ('Rental','');

insert into CarUnits ( Description, Remarks, CarCategoryId , SelfDrive) 
values ('Hyundai Starex ','Hyundai Starex',1,1),
	   ('Nissan Urban Premium ','Nissan NV350 Urban Premium',1,1),
	   ('Nissan Urban ','Nissan NV350 Urban',1,1);

insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (1,'../CarRental/POTTMPC/Van_Starex.png','','MAIN'),
       (2,'../CarRental/POTTMPC/Van_Nissan_Urban.png','','MAIN'),
       (3,'../CarRental/POTTMPC/Van_Nissan_Urban_premium.png','','MAIN');
	   
insert into CarViewPages (CarUnitId, Viewname)
values (1,'CarDetail_van_starex'),
	   (2,'CarDetail_van_urvan_premium'),
	   (3,'CarDetail_van_urvan');

insert into CarRates (Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate)
values (500,500,500,0,0,1,1500), --hyundai starex
	   (500,500,500,0,0,2,1500), --nissan urban premium
	   (500,500,500,0,0,3,1500); --nissan urban

insert into CarUnitMetas(carUnitId,PageTitle,MetaDesc,HomeDesc)
values
	-- Van -- 
	(1, 'Van (Hyundai Starex) for Rent in Palawan',
	'Hyundai Starex for rent in Palawan best suited for business and family occassions',
	'Comfortable 10 Seater van for rent in Palawan. Best Price for the budget.'
	),

	-- Nissan Premium -- 
	(2, 'Van (Nissan Urvan Premium) for Rent in Palawan',
	'Nissan Urvan Premium for rent in Palawan best suited for business,family, tour and travel.',
	'Highroof van that can accommodate 14pax with individual reclining seats.'
	),

	-- SUVs -- 
	(3, 'Van (Nissan Urvan) for Rent in Palawan',
	'Nissan Urvan for rent in Palawan best suited for business,family, tour and travel occassions',
	'Highroof van that can accommodate 14pax with individual reclining seats.'
	);

Insert into CarRatePackages (Description,Remarks,DailyMeals,DailyRoom,DaysMin)
values 
--City Proper--
('Custom', 'Fuel by renter', 0,0,0 ),
('Van Transfer', 'Van Pickup / Drop off', 0,0,0 ),
('City Tour', 'City tour for the first four hours, additional 1500 php for the succeding hours', 0,0,0),
('Fire Fily Watching', 'Iwahig', 0,0,0 ),
--North Bound--
('Honda Bay','Drop and Pickup',0,0,1), 
('Nagtabon','One round trip',0,0,1), 
('Talaudyong','One round trip',0,0,1), 
('Tagawayan','One round trip',0,0,1), 
('Napsan','One round trip',0,0,1), 
('Salvacion','One round trip',0,0,1), 
('Macarascas','One round trip',0,0,1), 
('Undergound River, Sabang','One round trip',0,0,1), 
('Manalo','One round trip',0,0,1), 
('Babuyan','One round trip',0,0,1), 
('Mauyon','One round trip',0,0,1),
('San Rafael', 'One round trip', 0,0,0 ),
('Binduyan', 'Seagull', 0,0,0 ),
('Langogan', 'One round trip', 0,0,0 ),
('Port Barton', 'One round trip', 0,0,0 ),
('Roxas Poblacion', 'One round trip', 0,0,0 ),
('San Vicente Pobalcion', 'One round trip', 0,0,0 ),
('Taytay Pobalcion', 'One round trip', 0,0,0 ),
('El Nido Pobalcion Drop-off', 'One way trip', 0,0,0 ),
('El Nido Pobalcion Drop-off/Pickup', 'One round trip', 0,0,0 ),
--South Bound--
('Mangigisda', 'One round trip', 0,0,0 ),
('Luzminidan', 'One round trip', 0,0,0 ),
('Inawagan', 'One round trip', 0,0,0 ),
('Aborlan', 'One round trip', 0,0,0 ),
('Narra', 'One round trip', 0,0,0 ),
('Calategas', 'One round trip', 0,0,0 ),
('Quezon', 'One round trip', 0,0,0 ),
('Berong', 'One round trip', 0,0,0 ),
('Rizal', 'One round trip', 0,0,0 ),
('Espanola', 'One round trip', 0,0,0 ),
('Brookes Point', 'One round trip', 0,0,0 ),
('Bataraza', 'Kidapawan. One round trip', 0,0,0 ),
('Rio Tuba', 'Ilomavis. One round trip', 0,0,0 )
;


Insert into CarRateUnitPackages (CarUnitId,CarRatePackageId,DailyRate,FuelLonghaul,FuelDaily,DailyAddon)
values
-- van ( hyundai starex)
-- city proper
( 1, 1, 0,     0, 0, 0 ), --+ selfdrive
( 1, 2, 0,     0, 0, 0 ), --+ van transfer
( 1, 3, 0,	   0, 0, 0 ), --+ city tour
( 1, 4, 0,  0, 0, 1000), --+ Fire Fly Watching Iwahig
--North Bound
( 1, 5,	 0, 0, 0, 1000), --Honda Bay
( 1, 6,  1000, 0, 0, 1000), --nagtabon
( 1, 7,  1500, 0, 0, 1000), --talaudyong
( 1, 8,  1500, 0, 0, 1000), --tagkawayan
( 1, 9,  3500, 0, 0, 1000), --Napsan
( 1, 10, 1000, 0, 0, 1000), --Salvacion
( 1, 11, 1000, 0, 0, 1000), --Macarascas
( 1, 12, 2000, 0, 0, 1000), --Sabang Underground River
( 1, 13, 1000, 0, 0, 1000), --Manalo
( 1, 14, 1300, 0, 0, 1000), --Babuyan
( 1, 15, 1300, 0, 0, 1000), --Mauyon
( 1, 16, 1300, 0, 0, 1000), --San Rafael
( 1, 17, 1300, 0, 0, 1000), --Binduyan
( 1, 18, 1800, 0, 0, 1000), --langogan
( 1, 19, 4000, 0, 0, 1000), --Port Barton
( 1, 20, 3000, 0, 0, 1000), --Roxas
( 1, 21, 5500, 0, 0, 1000), --San Vicente
( 1, 22, 6500, 0, 0, 1000), --Taytay
( 1, 23, 10500,0, 0, 1000), --El nido one way drop-off
( 1, 24, 12500,0, 0, 1000), --El nido round trip w/ overnight waiting
--South Bound--
( 1, 25, 1500, 0, 0, 1000), --Mangingisda
( 1, 26, 1000, 0, 0, 1000), --luzviminda
( 1, 27, 1300, 0, 0, 1000), --inagawan
( 1, 28, 1500, 0, 0, 1000), --Aborlan
( 1, 29, 2000, 0, 0, 1000), --Narra
( 1, 30, 2500, 0, 0, 1000), --Calategas
( 1, 31, 4500, 0, 0, 1000), --Quezon
( 1, 32, 5500, 0, 0, 1000), --Berong
( 1, 33, 6500, 0, 0, 1000), --Rizal
( 1, 34, 4500, 0, 0, 1000), --Espanola
( 1, 35, 5500, 0, 0, 1000), --brookes point
( 1, 36, 6500, 0, 0, 1000), --bataraza
( 1, 37, 8500, 0, 0, 1000), --Rio Tuba

-- van ( nissan urban premium )
-- city proper
( 2, 1, 0,     0, 0, 0 ), --+ selfdrive
( 2, 2, 0,     0, 0, 0 ), --+ van transfer
( 2, 3, 0,	   0, 0, 0 ), --+ city tour
( 2, 4, 0,  0, 0, 1000), --+ Fire Fly Watching Iwahig
--North Bound
( 2, 5,	 0, 0, 0, 1000), --Honda Bay
( 2, 6,  1000, 0, 0, 1000), --nagtabon
( 2, 7,  1500, 0, 0, 1000), --talaudyong
( 2, 8,  1500, 0, 0, 1000), --tagkawayan
( 2, 9,  3500, 0, 0, 1000), --Napsan
( 2, 10, 1000, 0, 0, 1000), --Salvacion
( 2, 11, 1000, 0, 0, 1000), --Macarascas
( 2, 12, 2000, 0, 0, 1000), --Sabang Underground River
( 2, 13, 1000, 0, 0, 1000), --Manalo
( 2, 14, 1300, 0, 0, 1000), --Babuyan
( 2, 15, 1300, 0, 0, 1000), --Mauyon
( 2, 16, 1300, 0, 0, 1000), --San Rafael
( 2, 17, 1300, 0, 0, 1000), --Binduyan
( 2, 18, 1800, 0, 0, 1000), --langogan
( 2, 19, 4000, 0, 0, 1000), --Port Barton
( 2, 20, 3000, 0, 0, 1000), --Roxas
( 2, 21, 5500, 0, 0, 1000), --San Vicente
( 2, 22, 6500, 0, 0, 1000), --Taytay
( 2, 23, 10500,0, 0, 1000), --El nido one way drop-off
( 2, 24, 12500,0, 0, 1000), --El nido round trip w/ overnight waiting
--South Bound--
( 2, 25, 1500, 0, 0, 1000), --Mangingisda
( 2, 26, 1000, 0, 0, 1000), --luzviminda
( 2, 27, 1300, 0, 0, 1000), --inagawan
( 2, 28, 1500, 0, 0, 1000), --Aborlan
( 2, 29, 2000, 0, 0, 1000), --Narra
( 2, 30, 2500, 0, 0, 1000), --Calategas
( 2, 31, 4500, 0, 0, 1000), --Quezon
( 2, 32, 5500, 0, 0, 1000), --Berong
( 2, 33, 6500, 0, 0, 1000), --Rizal
( 2, 34, 4500, 0, 0, 1000), --Espanola
( 2, 35, 5500, 0, 0, 1000), --brookes point
( 2, 36, 6500, 0, 0, 1000), --bataraza
( 2, 37, 8500, 0, 0, 1000), --Rio Tuba


-- van ( nissan urban premium )
-- city proper
( 3, 1, 0,     0, 0, 0 ), --+ selfdrive
( 3, 2, 0,     0, 0, 0 ), --+ van transfer
( 3, 3, 0,	   0, 0, 0 ), --+ city tour
( 3, 4, 0,  0, 0, 1000), --+ Fire Fly Watching Iwahig
--North Bound
( 3, 5,	 0, 0, 0, 1000), --Honda Bay
( 3, 6,  1000, 0, 0, 1000), --nagtabon
( 3, 7,  1500, 0, 0, 1000), --talaudyong
( 3, 8,  1500, 0, 0, 1000), --tagkawayan
( 3, 9,  3500, 0, 0, 1000), --Napsan
( 3, 10, 1000, 0, 0, 1000), --Salvacion
( 3, 11, 1000, 0, 0, 1000), --Macarascas
( 3, 12, 2000, 0, 0, 1000), --Sabang Underground River
( 3, 13, 1000, 0, 0, 1000), --Manalo
( 3, 14, 1300, 0, 0, 1000), --Babuyan
( 3, 15, 1300, 0, 0, 1000), --Mauyon
( 3, 16, 1300, 0, 0, 1000), --San Rafael
( 3, 17, 1300, 0, 0, 1000), --Binduyan
( 3, 18, 1800, 0, 0, 1000), --langogan
( 3, 19, 4000, 0, 0, 1000), --Port Barton
( 3, 20, 3000, 0, 0, 1000), --Roxas
( 3, 21, 5500, 0, 0, 1000), --San Vicente
( 3, 22, 6500, 0, 0, 1000), --Taytay
( 3, 23, 10500,0, 0, 1000), --El nido one way drop-off
( 3, 24, 12500,0, 0, 1000), --El nido round trip w/ overnight waiting
--South Bound--
( 3, 25, 1500, 0, 0, 1000), --Mangingisda
( 3, 26, 1000, 0, 0, 1000), --luzviminda
( 3, 27, 1300, 0, 0, 1000), --inagawan
( 3, 28, 1500, 0, 0, 1000), --Aborlan
( 3, 29, 2000, 0, 0, 1000), --Narra
( 3, 30, 2500, 0, 0, 1000), --Calategas
( 3, 31, 4500, 0, 0, 1000), --Quezon
( 3, 32, 5500, 0, 0, 1000), --Berong
( 3, 33, 6500, 0, 0, 1000), --Rizal
( 3, 34, 4500, 0, 0, 1000), --Espanola
( 3, 35, 5500, 0, 0, 1000), --brookes point
( 3, 36, 6500, 0, 0, 1000), --bataraza
( 3, 37, 8500, 0, 0, 1000)  --Rio Tuba
;

