
/** Car Rental Ads **/

INSERT SmProducts (SmBranchId, Code, Name, Remarks, BranchId, ProdStatusId, ValidStart, ValidEnd, Price, Contracted, SmProdStatusId ) 
values (1,'CR-SDN-19','Car Rental: Sedan','Car Rental',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',3000,3000,1),
	   (1,'CR-MPV-19','Car Rental: MPV','Car Rental',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',3500,3500,1),
	   (1,'CR-SUV-19','Car Rental: SUV','Car Rental',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',4000,4000,1),
	   (1,'CR-VAN-19','Car Rental: VAN','Car Rental',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',4000,4000,1),

	   (1,'CR-DVOCT','Car Rental: Davao City Tour','City Tour',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',3500,3500,1),
	   (1,'CR-DVOCS','Car Rental: Country Side Tour','CountrySide Tour',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',4000,4000,1),
	   (1,'CR-SMLT','Car Rental: Samal Tour','Samal Tour',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',4000,4000,1),
	   (1,'CR-DVOAP','Car Rental: Airport Pickup','City Tour',1,1,'7/26/2019 12:00:00 AM','7/26/2020 12:00:00 AM',1000,1000,1)
;


INSERT SmProdDescs (SmProductId,SortNo,Description,SmProductId1)
values (1,1,'10hrs Davao City Area Only',1),(1,1,'Driver & Fuel Included',1),(1,1,'Fuel on Renters Account',1),
       (2,1,'10hrs Davao City Area Only',2),(2,1,'Driver & Fuel Included',2),(2,1,'Fuel on Renters Account',2),
	   (3,1,'10hrs Davao City Area Only',3),(3,1,'Driver & Fuel Included',3),(3,1,'Fuel on Renters Account',3),
	   (4,1,'10hrs Davao City Area Only',4),(4,1,'Driver & Fuel Included',4),(4,1,'Fuel on Renters Account',4),

	   (4,1,'Davao City Tour Only',4),(4,1,'Driver Included',4),(4,1,'Fuel on Renters Account',4),
	   (4,1,'Davao City Tour Only',4),(4,1,'Driver Included',4),(4,1,'Fuel on Renters Account',4),
	   (4,1,'Davao City Tour Only',4),(4,1,'Driver Included',4),(4,1,'Fuel on Renters Account',4),
	   (4,1,'Davao City Tour Only',4),(4,1,'Driver Included',4),(4,1,'Fuel on Renters Account',4);

INSERT SmProdAds (Image,Link,SmCategoryId,SmProductId)
values ('Ads-sedan-2019.png','Form?tourCode=CR-SDN-19',11,1),
       ('Ads-mpv-2019.png','Form?tourCode=CR-MPV-19',11,2),
       ('Ads-suv-2019.png','Form?tourCode=CR-SUV-19',11,3),
       ('Ads-van-2019.png','Form?tourCode=CR-VAN-19',11,4)
	   ;

INSERT SmProdCats(SmCategoryId,SmProductID)
values (11,1),(11,2),(11,3),(11,4);