insert into Cities(Name) values('Davao'),('Cebu'),('Makati'),('Manila');
insert into Branches(Name, CityId, Remarks, Address, Landline, Mobile) 
values ('VSteel',1,'Davao Main','Matina Davao City','082 297-1831','');

insert into JobStatus([Status]) values('INQUIRY'),('RESERVATION'),('CONFIRMED'),('CLOSED'),('CANCELLED'),('TEMPLATE');
insert into JobThrus([Desc]) values('PHONE'),('EMAIL'),('WALKIN');

insert into Banks([BankName],[BankBranch],[AccntName],[AccntNo])
 values ('Cash','Davao','Cash','0'),
		('BDO','SM-Ecoland Davao','AJ88 Car Rental Services','00 086 072 9575'),
		('BPI','SM-Ecoland Davao','Abel S. Salvatierra','870 303 5125'),
		('Personal Guarantee','Realbreeze-Davao','Personal Guarantee','0'),
		('Paypal','RealWheels-Paypal','Paypal','0');

insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values
    ('<< New Customer >>','--','--',' ',' ','ACT'),
	('Mark Dalton','markdalton@gmail.com','0916-755-8473','','','ACT'),
	('John Doe','johndoe@gmail.com','09123456789','','','ACT'),
	('Jerry','Jerry@gmail.com','09654-987-9898','998-3321','','ACT'),
	('Stephen','stephen@gmail.com','0915-225-1221','','','ACT'),
	('Kevin','Kevin@gmail.com','0954-321-65454','084-9898','','ACT');
		
insert Into Destinations([Description],[Remarks],[CityId]) 
values 
('Eden Nature Park','','1 '),
('Philippine Eagle','','1 '),
('Malagos Garden'  ,'','1 '),
('Japanese Tunnel' ,'','1 '),
('Peoples Park' ,'','1 '),
('Jacks Ridge' ,'','1 '),
('Apo ni Lola' ,'','1 '),
('Aldivenco Shopping Center' ,'','1 ');

insert into Currencies([Name])
values ('PHP'),('USD');


-- ------------------------------------------------------------
-- Sales Lead Configuration
-- ------------------------------------------------------------

insert into SalesStatusTypes([Type])
values  ('ALL'),('SALES'),('PROCUREMENT');

insert into SalesStatusCodes([SeqNo],[Name],[SalesStatusTypeId],[OrderNo])
values	(1, 'INQUIRY',1,1), (2,'SALES',2,2), (3, 'PROCUREMENT',2,3), (4, 'FOR APPROVAL',1,4),
		(5, 'APPROVED',1,5), (6, 'AWARDED',1,6), (7, 'REJECTED',1,7), (8, 'CLOSE',1,8),
		(2, 'EVALUATION', 2,2), (2, 'ACCEPTED', 2,2), (5, 'QUOTATION SENT', 2,5),
		(3, 'EVALUATION', 3,3), (3, 'ACCEPTED', 3,3), (3, 'ITEM PROCUREMENT', 3,3),
		(4, 'APPROVED BY ALDRIN', 1,4), (4, 'APPROVED BY CHECKER', 1,4);

insert into SalesStatusStatus([Status])
values ('Active'), ('Inactive');

insert into SalesActCodes([Name],[Desc],[SysCode],[iconPath],[DefaultActStatus])
values 
('RFQ','Request for quotation', 'RFQ','~/Images/SalesLead/Quotation101.png',1), 
('CALL-REQUEST','Return Call request','CALL REQUEST','~/Images/SalesLead/Phone103.png',1),   
('EMAIL-REQUEST','Request to Check/reply Email','EMAIL REQUEST','~/Images/SalesLead/Email102.jpg',1),   
('CALL-DONE','Call is done', 'CALL DONE','~/Images/SalesLead/Phone103.png',2), 
('MEETING-REQUEST','Schedule an appointment','APPOINTMENT','~/Images/SalesLead/meeting102.jpg',1),   
('MEETING-DONE','Meeting done', 'APPOINTMENT_DONE','~/Images/SalesLead/meeting102.jpg',2); 

insert into SalesStatusAllowedUsers ([User])
values ('admin@gmail.com'),('jahdielsvillosa@gmail.com');

insert into SalesStatusRestrictions([SalesStatusCodeId],[SalesStatusAllowedUsersId])
values (15, 1), (16 ,2);

insert into SalesActStatus([Name])
values ('REQUEST'),('DONE'),('SUSPEND');

insert into CustEntActTypes([Type])
values ('Others'), ('Indicated Price'), ('Bidding Only'), ('Firm Inquiry'), ('Buying Inquiry');


insert into CustEntActivityTypes([Type],[Points])
values ('Quotation',8), ('Meeting',8), ('Sales',15),('Procurement',8), ('Calls/Email',2), ('Others',1);
   
insert into SalesLeadCatCodes([CatName],[SysCode],[iconPath])
values	('Priority','PRIORITY','~/Images/SalesLead/high-importance.png'), 
		('HighMargin','HIGHMARGIN','~/Images/SalesLead/GreenArrow.png'),
		('LongTerm','LONGTERM','~/Images/SalesLead/Longterm.png'), 
		('Corporate','CORPORATE ACCOUNT','~/Images/SalesLead/ShakeHands.png'), 
		('HardOne', 'HARDONE','~/Images/SalesLead/unhappy.jpg');
	 	
-- Procurement Sales Lead
--insert into SalesStatusCodes([SeqNo],[Name])
--values (1,'NEW'), (2,'EVALUATION'), (3, 'ACCEPTED'), (4, 'CHECKING'), (5, 'ACCEPTED'), (6, 'REJECTED'), (7, 'CLOSE');

insert into SupplierActActionStatus([ActionStatus])
values ('REQUEST'),('DONE'),('SUSPEND');

insert into SupplierActActionCodes([Name],[Desc],[SysCode],[IconPath],[DefaultActStatus],[SeqNo])
values 
('REQ-PROC','Request for Procurement', 'RFP','~/Images/SalesLead/Quotation101.png',1,1), 
('PROC-DONE','Procurement Done','PROC-FONE' ,'~/Images/SalesLead/Quotation101.png',2,2),   
('SALES-CLARIFY','Sales Clarification Done' ,'SALES CLARIFICATION','~/Images/SalesLead/ShakeHands.png',1,3),   
('CALL-DONE','Supplier Call Done', 'SUP-CALL-DONE','~/Images/SalesLead/Phone103.png',2,4), 
('EMAIL-DONE','Supplier Email Done','SUP-MAIL-DONE','~/Images/SalesLead/Email102.jpg',1,5),   
('MEETING-DONE','Supplier Meeting done', 'SUP-MEETING-DONE','~/Images/SalesLead/meeting102.jpg',3,6),   
('AWARDED','Awarded', 'AWARDED','~/Images/SalesLead/Awarded.png',4,7),   
('FOR-APPROVAL','For Approval by MGT', 'FOR-APPROVAL-MGT','~/Images/SalesLead/ShakeHands.png',4,8),   
('CLOSED','Closed', 'CLOSED','~/Images/SalesLead/meeting102.jpg',2,9),
('STATUS','Status Update','Status','~/Images/SalesLead/Quotation101.png',1,10); 

insert into SalesLeadQuotedItemStatus([Status])
values ('PENDING'), ('ACCEPTED'), ('CANCELLED');

-- ----------------------------------------------
-- Inventory Configuration 
-- ----------------------------------------------
insert into InvItemCats([Name],[Remarks],[ImgPath],[SysCode])
Values
('Pipe','Pipes','~/Images/CarRental/Repair101.png','PIPE'),
('Others','Other Types','~/Images/CarRental/Repair101.png','OTHER');


insert into InvItems ([ItemCode],[Description],[Remarks],[ContactInfo],[ViewLabel],[OrderNo] )
values
('CSTPP','Carbon Steel Pipe','Carbon Steel Pipe','','',100),
('CSTPL','Carbon Steel Plate','Carbon Steel Plate','','',100),
('SSTPP','Stainless Steel Pipe','Stainless Steel Pipe','','',100),
('SSTPL','Stainless Steel Plate','Stainless Steel Plate','','',100);

insert into InvItemCategories([InvItemId],[InvItemCatId])
values
(1,1);



-- 
-- Dumping data for table Countries( Code, Name)
-- 
INSERT INTO Countries( Code, Name) VALUES ( 'AF', 'Afghanistan'),
( 'AL', 'Albania'),( 'DZ', 'Algeria'),( 'DS', 'American Samoa'),( 'AD', 'Andorra'),
( 'AO', 'Angola'),( 'AI', 'Anguilla'),( 'AQ', 'Antarctica'),( 'AG', 'Antigua and Barbuda'),
( 'AR', 'Argentina'),( 'AM', 'Armenia'),( 'AW', 'Aruba'),( 'AU', 'Australia'),
( 'AT', 'Austria'),( 'AZ', 'Azerbaijan'),( 'BS', 'Bahamas'),( 'BH', 'Bahrain'),( 'BD', 'Bangladesh'),
( 'BB', 'Barbados'),( 'BY', 'Belarus'),( 'BE', 'Belgium'),( 'BZ', 'Belize'),( 'BJ', 'Benin'),
( 'BM', 'Bermuda'),( 'BT', 'Bhutan'),( 'BO', 'Bolivia'),( 'BA', 'Bosnia and Herzegovina'),
( 'BW', 'Botswana'),( 'BV', 'Bouvet Island'),( 'BR', 'Brazil'),( 'IO', 'British Indian Ocean Territory'),
( 'BN', 'Brunei Darussalam'),( 'BG', 'Bulgaria'),( 'BF', 'Burkina Faso'),( 'BI', 'Burundi'),
( 'KH', 'Cambodia'),( 'CM', 'Cameroon'),( 'CA', 'Canada'),( 'CV', 'Cape Verde'),( 'KY', 'Cayman Islands'),
( 'CF', 'Central African Republic'),( 'TD', 'Chad'),( 'CL', 'Chile'),( 'CN', 'China'),( 'CX', 'Christmas Island'),
( 'CC', 'Cocos (Keeling) Islands'),( 'CO', 'Colombia'),( 'KM', 'Comoros'),( 'CD', 'Democratic Republic of the Congo'),
( 'CG', 'Republic of Congo'),( 'CK', 'Cook Islands'),( 'CR', 'Costa Rica'),( 'HR', 'Croatia (Hrvatska)'),
( 'CU', 'Cuba'),( 'CY', 'Cyprus'),( 'CZ', 'Czech Republic'),( 'DK', 'Denmark'),( 'DJ', 'Djibouti'),( 'DM', 'Dominica'),
( 'DO', 'Dominican Republic'),( 'TP', 'East Timor'),( 'EC', 'Ecuador'),( 'EG', 'Egypt'),( 'SV', 'El Salvador'),
( 'GQ', 'Equatorial Guinea'),( 'ER', 'Eritrea'),( 'EE', 'Estonia'),( 'ET', 'Ethiopia'),( 'FK', 'Falkland Islands (Malvinas)'),
( 'FO', 'Faroe Islands'),( 'FJ', 'Fiji'),( 'FI', 'Finland'),( 'FR', 'France'),( 'FX', 'France, Metropolitan'),( 'GF', 'French Guiana'),
( 'PF', 'French Polynesia'),( 'TF', 'French Southern Territories'),( 'GA', 'Gabon'),( 'GM', 'Gambia'),( 'GE', 'Georgia'),
( 'DE', 'Germany'),( 'GH', 'Ghana'),( 'GI', 'Gibraltar'),( 'GK', 'Guernsey'),( 'GR', 'Greece'),( 'GL', 'Greenland'),
( 'GD', 'Grenada'),( 'GP', 'Guadeloupe'),( 'GU', 'Guam'),( 'GT', 'Guatemala'),( 'GN', 'Guinea'),( 'GW', 'Guinea-Bissau'),( 'GY', 'Guyana'),
( 'HT', 'Haiti'),( 'HM', 'Heard and Mc Donald Islands'),( 'HN', 'Honduras'),( 'HK', 'Hong Kong'),( 'HU', 'Hungary'),( 'IS', 'Iceland'),( 'IN', 'India'),
( 'IM', 'Isle of Man'),( 'ID', 'Indonesia'),( 'IR', 'Iran (Islamic Republic of)'),( 'IQ', 'Iraq'),( 'IE', 'Ireland'),( 'IL', 'Israel'),( 'IT', 'Italy'),
( 'CI', 'Ivory Coast'),( 'JE', 'Jersey'),( 'JM', 'Jamaica'),( 'JP', 'Japan'),( 'JO', 'Jordan'),( 'KZ', 'Kazakhstan'),( 'KE', 'Kenya'),
( 'KI', 'Kiribati'),( 'KP', 'Korea, Democratic People''s Republic of'),( 'KR', 'Korea, Republic of'),( 'XK', 'Kosovo'),( 'KW', 'Kuwait'),
( 'KG', 'Kyrgyzstan'),( 'LA', 'Lao People''s Democratic Republic'),( 'LV', 'Latvia'),( 'LB', 'Lebanon'),( 'LS', 'Lesotho'),
( 'LR', 'Liberia'),( 'LY', 'Libyan Arab Jamahiriya'),( 'LI', 'Liechtenstein'),( 'LT', 'Lithuania'),( 'LU', 'Luxembourg'),
( 'MO', 'Macau'),( 'MK', 'North Macedonia'),( 'MG', 'Madagascar'),( 'MW', 'Malawi'),( 'MY', 'Malaysia'),( 'MV', 'Maldives'),
( 'ML', 'Mali'),( 'MT', 'Malta'),( 'MH', 'Marshall Islands'),( 'MQ', 'Martinique'),( 'MR', 'Mauritania'),( 'MU', 'Mauritius'),( 'TY', 'Mayotte'),
( 'MX', 'Mexico'),( 'FM', 'Micronesia, Federated States of'),( 'MD', 'Moldova, Republic of'),( 'MC', 'Monaco'),( 'MN', 'Mongolia'),
( 'ME', 'Montenegro'),( 'MS', 'Montserrat'),( 'MA', 'Morocco'),( 'MZ', 'Mozambique'),
( 'MM', 'Myanmar'),( 'NA', 'Namibia'),( 'NR', 'Nauru'),( 'NP', 'Nepal'),( 'NL', 'Netherlands'),( 'AN', 'Netherlands Antilles'),
( 'NC', 'New Caledonia'),( 'NZ', 'New Zealand'),( 'NI', 'Nicaragua'),( 'NE', 'Niger'),( 'NG', 'Nigeria'),( 'NU', 'Niue'),
( 'NF', 'Norfolk Island'),( 'MP', 'Northern Mariana Islands'),( 'NO', 'Norway'),( 'OM', 'Oman'),( 'PK', 'Pakistan'),
( 'PW', 'Palau'),( 'PS', 'Palestine'),( 'PA', 'Panama'),( 'PG', 'Papua New Guinea'),( 'PY', 'Paraguay'),( 'PE', 'Peru'),
( 'PH', 'Philippines'),( 'PN', 'Pitcairn'),( 'PL', 'Poland'),( 'PT', 'Portugal'),( 'PR', 'Puerto Rico'),( 'QA', 'Qatar'),
( 'RE', 'Reunion'),( 'RO', 'Romania'),( 'RU', 'Russian Federation'),( 'RW', 'Rwanda'),( 'KN', 'Saint Kitts and Nevis'),
( 'LC', 'Saint Lucia'),( 'VC', 'Saint Vincent and the Grenadines'),( 'WS', 'Samoa'),( 'SM', 'San Marino'),
( 'ST', 'Sao Tome and Principe'),( 'SA', 'Saudi Arabia'),( 'SN', 'Senegal'),( 'RS', 'Serbia'),
( 'SC', 'Seychelles'),( 'SL', 'Sierra Leone'),( 'SG', 'Singapore'),( 'SK', 'Slovakia'),( 'SI', 'Slovenia'),
( 'SB', 'Solomon Islands'),( 'SO', 'Somalia'),( 'ZA', 'South Africa'),( 'GS', 'South Georgia South Sandwich Islands'),
( 'SS', 'South Sudan'),( 'ES', 'Spain'),( 'LK', 'Sri Lanka'),( 'SH', 'St. Helena'),( 'PM', 'St. Pierre and Miquelon'),
( 'SD', 'Sudan'),( 'SR', 'Suriname'),( 'SJ', 'Svalbard and Jan Mayen Islands'),( 'SZ', 'Swaziland'),( 'SE', 'Sweden'),
( 'CH', 'Switzerland'),( 'SY', 'Syrian Arab Republic'),( 'TW', 'Taiwan'),( 'TJ', 'Tajikistan'),
( 'TZ', 'Tanzania, United Republic of'),( 'TH', 'Thailand'),( 'TG', 'Togo'),( 'TK', 'Tokelau'),
( 'TO', 'Tonga'),( 'TT', 'Trinidad and Tobago'),( 'TN', 'Tunisia'),( 'TR', 'Turkey'),( 'TM', 'Turkmenistan'),
( 'TC', 'Turks and Caicos Islands'),( 'TV', 'Tuvalu'),( 'UG', 'Uganda'),( 'UA', 'Ukraine'),( 'AE', 'United Arab Emirates'),
( 'GB', 'United Kingdom'),( 'US', 'United States'),( 'UM', 'United States minor outlying islands'),( 'UY', 'Uruguay'),
( 'UZ', 'Uzbekistan'),( 'VU', 'Vanuatu'),( 'VA', 'Vatican City State'),( 'VE', 'Venezuela'),( 'VN', 'Vietnam'),
( 'VG', 'Virgin Islands (British)'),( 'VI', 'Virgin Islands (U.S.)'),( 'WF', 'Wallis and Futuna Islands'),( 'EH', 'Western Sahara'),
( 'YE', 'Yemen'),( 'ZM', 'Zambia'),( 'ZW', 'Zimbabwe');

-- ---------------------------------------- 
-- Suppliers Configuration
-- ---------------------------------------- 
insert into SupplierTypes(Description) values
	('Stockiest /Trader'),('Supplier'),('Installer');
insert Into Suppliers([Name],[Contact1],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId],[Code] ) values('<< New Supplier >>','--',' ', '--','1','1','ACT',175,'SUP01');
insert Into Suppliers([Name],[Contact1],[Contact2],[Contact3],[Website],[Address],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId],[Code] )
	values('American Steels CO., INC','(082) 223-6659','(082 223-6650)','0912-564-9877','americansteel.com','Manila', '','amsteels.supply@gmail.com','1','1','ACT',175,'SUP02'),
	      ('Green Steels Industries LTD','(082) 333-8845','(082 333-8840)','0915-654-1125','greensteels.com','Davao City', '','green.steels@gmail.com','1','1','ACT',175,'SUP03'),
	      ('Asia Metals and Steels INC','(0086) 11236548','(0086 11238840)','-','asia-metals-steels.com','Davao City', '','asia.metals.sales@gmail.com','1','1','ACT',175,'SUP04');

-- ----------------------------
-- Supplier Items / Products
-- ----------------------------

insert into SupplierItems([Description],[SupplierId],[Remarks],[InCharge],[Status]) values 
	('Default','1','Item by supplier','Supplier','ACT');

insert into SupplierUnits([Unit])
values ('Meter'),('Inch'),('Feet'),('Box'),('Package');

insert into SupplierInvItems([SupplierId],[InvItemId]) values
(2,1),(2,2),
(3,1),
      (4,2),(4,3);

insert into SupplierItemRates([SupplierInvitemId],[ItemRate],[SupplierUnitId],[Remarks],[DtEntered],[DtValidFrom],[DtValidTo],[Particulars],[Material],[By],[ProcBy],[TradeTerm],[Tolerance]) values
	(1,2500,2,'','06/14/2022','01/16/2022','06/16/2023','Pipe' ,'Carbon Steel','John','Ann','',''),
	(2,3300,3,'','06/10/2022','01/10/2022','06/30/2023','Plate','Carbon Steel','John','Ann','',''),
	(3,2500,2,'','07/18/2022','01/05/2022','08/30/2023','Pipe' ,'Carbon Steel','Dalton','Mark','',''),
	(4,3600,3,'','07/09/2022','12/18/2022','12/30/2023','Plate','Carbon Steel','Mike','Ann','',''),
	(5,4000,3,'','12/09/2022','12/10/2022','06/30/2023','Pipe' ,'Stainless Steel','Mike','Ann','','');

insert into SupplierContactStatus([Name]) values ('Active'),('Resigned');

insert into SupplierContacts([SupplierId],[Name],[Mobile],[Landline],[Email],[SkypeId],[ViberId],[WhatsApp],[WeChat],[Remarks],[Position],[Department],[SupplierContactStatusId])
values (2,'John Doe','0995-987-4561','(082) 333 6588','johndoe@gmail.com','','','','','','Staff','Sales',1),
	   (3,'Dalton Gray','0912-112-5477','(082) 235 1125','daltongray@gmail.com','','','','','','Sales','Sales',1),
	   (3,'Elise Rhodes','0909-545-1335','(082) 235 1112','eliserhodes@gmail.com','','','','','','Procurement','Accounting',1),
	   (4,'Mike De Guzman','0915-587-9512','(082) 545 3212','mike.dg@gmail.com','','','','','','Sales','Sales',1);

insert into SupplierActivityTypes([Type],[Points]) values 
('Procurement', 5), ('Job Order', 8), ('Meeting', 3), ('Others', 2), ('Revision', 1);

insert into SupplierActStatus([Status]) values 
('Open'),('In-Progress'),('Cancelled'),('Closed');

insert into SupDocuments([Description]) values 
	   ('Business Registration'),('Tax Registration'), ('Import Export Registration');

insert into Services([Name],[Description],[Status]) values
	('Car Rental','Bus, Car, Van and other Transportation arrangements','1'),
	('Boat Rental','Boat Arrangement, Island Hopping','1'),
	('Tour Package','Tour Package, Land arrangements','1'),
	('AirTicket','Airline Ticket','1'),
	('Accommodation','Hotels, Rooms, Houses, etc','1'),
	('Activity','Water Rafting, Scuba Diving, Caving','1'),
	('Other','Other types of services','1');

insert into SrvActionCodes([CatCode],[SortNo]) values
('Arrangement',1),('Partial Payment',2),('Notification',3),('OnGoing',4),('Billing',5),('Full Payment',6),('Closing',7);

insert into SrvActionItems([Desc],[Remarks],[SortNo],[SrvActionCodeId],[ServicesId])
values
-- Car Rental Activities --
('Arrange Vehicle','',1,1,1),
('Partial Payment','',2,2,1),
('Notify Driver',''  ,3,3,1),
('Notify Operator','',4,3,1),
('Notify Guest',''   ,5,3,1),
('On Progress',''    ,6,4,1),
('Payment',''        ,7,6,1),
('Closing',''        ,8,7,1),

-- Boat Rental --
('Special Request','',1,1,2),
('Partial Payment','',2,2,2),
('Notify Operator','',3,3,2),
('Notify Guest',''   ,4,3,2),
('On Progress',''    ,5,4,2),
('Full Payment',''   ,6,6,2),
('Closing',''        ,7,7,2),

-- Tour Package Activities --
('Special Request',''    ,1,1,3),
('Partial Payment',''    ,1,1,3),
('Book Airticket',''     ,1,1,3),
('Book Transportation','',1,1,3),
('Book Hotel',''         ,1,1,3),
('Book Destinations',''  ,1,1,3),
('Book Meals',''         ,1,1,3),
('Book Tour Guide',''    ,1,1,3),
('Notify Driver',''      ,2,3,3),
('Notify Tour Guide',''  ,3,3,3),
('Notify Guest',''       ,4,3,3),
('On progress',''        ,5,4,3),
('Full Payment',''       ,6,6,3),
('Closing',''            ,7,7,3);


insert into SrvActionItems([Desc],[Remarks],[SortNo],[SrvActionCodeId],[ServicesId])
values

-- Air Ticket --
('Special Request','',1,1,4),
('Partial Payment','',2,2,4),
('Notify Operator','',3,3,4),
('Notify Guest',''   ,4,3,4),
('On Progress',''    ,5,4,4),
('Full Payment',''   ,6,6,4),
('Closing',''        ,7,7,4),

-- Accommodation --
('Special Request','',1,1,5),
('Partial Payment','',2,2,5),
('Notify Operator','',3,3,5),
('Notify Guest',''   ,4,3,5),
('On Progress',''    ,5,4,5),
('Full Payment',''   ,6,6,5),
('Closing',''        ,7,7,5),

-- Activity --
('Special Request','',1,1,6),
('Partial Payment','',2,2,6),
('Notify Operator','',3,3,6),
('Notify Guest',''   ,4,3,6),
('On Progress',''    ,5,4,6),
('Full Payment',''   ,6,6,6),
('Closing',''        ,7,7,6),


-- Others --
('Special Request','',1,1,7),
('Partial Payment','',2,2,7),
('Notify Operator','',3,3,7),
('Notify Guest',''   ,4,3,7),
('On Progress',''    ,5,4,7),
('Full Payment',''   ,6,6,7),
('Closing',''        ,7,7,7);


-- ----------------------------------------------
-- Supplier PO Configuration
-- ----------------------------------------------
insert into SupplierPoStatus ([Status],[OrderNo]) values ('REQUEST',1),('APPROVED',2),('CANCELLED',2),('CONFIRMED',3),('DELIVERED',4),('CLOSE',5);

-- ----------------------------------------------
-- Paypal Accounts
-- ----------------------------------------------
insert into PaypalAccounts ([SysCode],[Key],[Secret])
values
('Realwheels', 'ASTv_oxNk66nZW4tVTbt78dtocU-70VVoDDmgtdMSzv1Aqmw8QK6lJ01vzn6lO6jPio3DbfbT_6G6F6b' , 'EAYtPcgQYKu5UfA4WV5lzE_iPj1WFiGnPC_8XvSgYrjoISJEnZAezmdcofe5oRyJZzPToJO6QUMlgmS2');

insert into JobMains([JobDate],[CustomerId],[Description],[NoOfPax],[NoOfDays],[JobRemarks],[JobStatusId],[StatusRemarks],[BranchId],[JobThruId],[AgreedAmt])
values
('11-15-2019',1,'Test Job 101',10,1,'TEST DATA 0101',3,'N/A',1,1,5000),
('11-27-2019',1,'Item scheduling',3,1,'TEST DATA 0102',3,'N/A',1,1,3000),
('11-27-2019',1,'Davao City Tour',3,2,'Template Test',6,'N/A',1,1,3500);


insert into JobServices([JobMainId],[ServicesId],[SupplierId],[Particulars],[QuotedAmt],[SupplierAmt],[ActualAmt],[Remarks],[SupplierItemId],[DtStart],[DtEnd])
values
(1,1,2,'Car Rental sample data R1',5000,5000,5000,'Sample only. Disregard once seen on production',1,'11-15-2019','11-22-2019'),
(1,1,2,'Car Rental sample data R2',3000,3000,3000,'Sample only. Disregard once seen on production',1,'11-24-2019','11-28-2019'),
(2,1,2,'SUV Rental R1',2000,2000,2000,'Sample only. Disregard once seen on production',1,'11-27-2019','11-28-2019'),
(2,1,2,'SUV Rental R2',1000,1000,1000,'Sample only. Disregard once seen on production',1,'11-29-2019','11-30-2019'),
(3,1,2,'Day 1: Country Side Tour',2000,2000,2000,'Sample only. Disregard once seen on production',1,'11-27-2019','11-27-2019'),
(3,1,2,'Day 1: Country Side Tour',1500,1500,1000,'Sample only. Disregard once seen on production',1,'11-28-2019','11-28-2019');

insert into JobItineraries([JobMainId],[DestinationId],[ActualRate],[Remarks],[ItiDate],[SvcId])
values
(3,1,0,'','11-27-2019',5),
(3,2,0,'','11-27-2019',5),
(3,5,0,'','11-28-2019',6),
(3,6,0,'','11-28-2019',6);

--insert into InvItems([ItemCode],[Description],[Remarks])
--values ('RNY301','Toyota Innova E M/T 2013 Dsl',''),
--('EOK873','Honda City A/T 2018 1.5E',''),
--('ADP2264','Ford Everest A/T 2016 2.2',''),
--('AbelS','Abel Salvatierra','');

insert into InvItemCategories([InvItemId],[InvItemCatId])
values (1,1), (2,1), (3,1), (4,2);

Insert into JobServiceItems([JobServicesId],[InvItemId])
values(1,2),(1,3),
(2,3),(2,4),
(3,3),(3,4),
(4,3),(4,4);

-- Supplier PO Samples
insert into SupplierPoHdrs([PoDate],[Remarks],[SupplierId],[SupplierPoStatusId],[RequestBy],[DtRequest])
values ('11-25-2018','Test Po',1,1,'Abel','11-25-2018');

insert into SupplierPoDtls([SupplierPoHdrId],[Remarks],[Amount],[JobServicesId])
values (1,'10 seater vehicle',3500,1), (1,'14 seater vehicle',4000,1);


-- Customer and Companies Initial

insert into CustCategories([Name],[iconPath])
values 
	   --('PRIORITY','Images/Customers/Category/star-filled-40.png'),
	   --('ACTIVE','Images/Customers/Category/star-filled-40.png'),
	   --('INACTIVE','Images/Customers/Category/star-filled-40.png'),
	   --('BAD ACCOUNT','Images/Customers/Category/star-filled-40.png'),
	   --('SUSPENDED','Images/Customers/Category/star-filled-40.png'),
	   --('BILLING/TERMS','Images/Customers/Category/star-filled-40.png'),
	   --('ACCREDITATION ON PROCESS','Images/Customers/Category/star-filled-40.png'),
	   ('General Engg','Images/Customers/Category/star-filled-40.png'),
	   ('Electrical','Images/Customers/Category/Active-30.png'),
	   ('Mechanical','Images/Customers/Category/suspended-64.png'),
	   ('Civil','Images/Customers/Category/cancel-40.png'),
	   ('Structural','Images/Customers/Category/Active-30.png'),
	   ('Plumbing and Sanitary Work','Images/Customers/Category/star-filled-40.png'),
	   ('Mechanical Piping','Images/Customers/Category/Active-30.png'),
	   ('Airconditioning and Refrigeration','Images/Customers/Category/star-filled-40.png'),
	   ('Fire Protection','Images/Customers/Category/star-filled-40.png'),
	   ('Well Drilling Works','Images/Customers/Category/star-filled-40.png'); 

insert into CustEntAccountTypes ([Name],[SysCode]) values
('Regular','REG01');

--insert into CustEntMains([Name],[Address],[Contact1],[Contact2])
--values ('NEW (not yet defined)',' ',' ',' ');

insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
values('Juan Dela Cruz','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

insert into CustEntMains(Name,Code, Address, Contact1, Contact2, iconPath, Website, Remarks, CityId, Status, AssignedTo, Mobile, Exclusive, CustEntAccountTypeId) 
values ('Google.Inc','COP-001','Davao City','888-9888','888-9881','','google.com','',5,'ACT','admin@gmail.com','09126659987','PUBLIC', 1),
	   ('Acer.Inc.Ph','COP-002','Quezon City','111-9878','','','acer.com.ph','',5,'ACT','demo@gmail.com','0915-123-6548','PUBLIC', 1),
	   ('Silicon Valley','COP-003','Makati City','321-6689','888-3215','','admin@gmail.com','',5,'ACT','silicon.valley@gmail.com','0912-654-9879','PUBLIC', 1),
	   ('HP Davao','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',5,'ACT','demo@gmail.com','0999-987-9858','PUBLIC', 1),
	   
	   -- Priority -- Exclusive
	   ('AYES Food Corp','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','demo@gmail.com','0999-987-9858','EXCLUSIVE', 1),
	   ('San Miguel Brewery Corp','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','demo@gmail.com','0999-987-9858','EXCLUSIVE', 1),
	   ('Coca-cola Corp.','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','demo@gmail.com','0999-987-9858','EXCLUSIVE', 1),
	   ('SM Megamalls','COP-004','Davao City','0916-232-1134','','','hpe.com.ph','',1,'PRI','demo@gmail.com','0999-987-9858','EXCLUSIVE', 1)
	   ;

insert into CustEntAssigns(Assigned, Remarks, CustEntMainId, Date)
values ('demo@gmail.com', '' , 2 , '11-30-2019');

-- Customers / Assigned Agent Types --
insert into CustAssocTypes([Type]) 
values ('Contact'), ('Agent');

insert into CustEntities(CustEntMainId, CustomerId, Position, CustAssocTypeId) 
values  (2,2,'Staff',1), 
	    (2,3,'Manager', 1),
		(3,4,'Procurement', 1), 
		(4,5,'Purchaser', 1),
		(5,6,'Assistant Procurement', 1);
		
insert into CustCats(CustomerId, CustCategoryId) 
values	(1,3),(2,2),
		(3,2),(4,2),(5,2);

insert into CustEntCats(CustEntMainId,CustCategoryId)
values (1,2),(2,3),(3,5),(4,2),(5,1); 

insert into CustEntActStatus([Status]) values 
('Open'),('For Client Comment'),('For Meeting'),('Awarded'),('Close');


insert into CustEntActActionCodes([Name],[Desc],[SysCode],[IconPath],[DefaultActStatus],[SeqNo])
values 
('RFQ','Request for quotation', 'RFQ','~/Images/SalesLead/Quotation101.png',1,1), 
('PROCUREMENT','Request for Procurement', 'PROCUREMENT','~/Images/SalesLead/Quotation101.png',1,2), 
('QUOTATION-DONE','Quotation Done', 'RFQ','~/Images/SalesLead/Quotation101.png',2,3),  
('CALL-DONE','Client Call done', 'CALL DONE','~/Images/SalesLead/Phone103.png',2,4), 
('EMAIL-DONE','Client Meeting done', 'APPOINTMENT_DONE','~/Images/SalesLead/meeting102.jpg',2,5),
('MEETING-DONE','Client Meeting done', 'APPOINTMENT_DONE','~/Images/SalesLead/meeting102.jpg',2,5),
('FOR-MGT','For Management Approval','FOR APPROVAL','~/Images/SalesLead/meeting102.jpg',1,6),
('PO-READY','Ready for PO','PO READY','~/Images/SalesLead/Quotation101.png',1,7),
('AWARDED','Awarded','AWARDED','~/Images/SalesLead/Awarded.png',2,8),
('CLOSED','Closed','CLOSED','~/Images/SalesLead/Closed.png',2,9),
('STATUS','Status Update','Status','~/Images/SalesLead/Quotation101.png',1,10); 

insert into CustEntActActionStatus([ActionStatus])
values ('REQUEST'),('DONE'),('SUSPEND');

update CustCategories set iconPath = 'Images/Customers/Category/star-filled-40.png' where Id = 1; 
update CustCategories set iconPath = 'Images/Customers/Category/Active-30.png' where Id = 2; 
update CustCategories set iconPath = 'Images/Customers/Category//suspended-64.png' where Id = 3; 
update CustCategories set iconPath = 'Images/Customers/Category/cancel-40.png' where Id = 4;  


-- Car Rental Initial
insert into CarCategories (Description, Remarks)
values ('Rental','');

insert into CarUnits ( Description, Remarks, CarCategoryId , SelfDrive, SortOrder) 
values ('Van (10 seater)','Gl Grandia'    ,1,1,4),
	   ('Van (14 seater)','Nissan Premium',1,1,5),
	   ('SUV'            ,'Ford Everest'  ,1,0,3),
	   ('MPV/AUV/MiniVan','Toyota Innova' ,1,0,2),
	   ('Sedan'          ,'Honda City'    ,1,0,1),
	   ('Pickup'         ,'Pickups'       ,1,0,6),
	   ('Van (14 Seater)','Nissan Premium',1,0,6),
	   ('MPV'            ,'Toyota Rush'   ,1,0,6);

insert into CarImages ( CarUnitId, ImgUrl, Remarks, SysCode)
values (1,'glgrandia/Toyota-Grandia-side.jpg','','MAIN'),
       (2,'nissanPremium/Nissan-Premium-2018.jpg','','MAIN'),
       (3,'ford/ford-everest-front.jpg'		 ,'','MAIN'),
       (4,'innova/toyota-innova.jpg'		 ,'','MAIN'),
       (5,'hondacity/honda-city-front.jpg'   ,'','MAIN'),
       (6,'pickup/Toyota-Hilux-Main.jpg'	 ,'','MAIN'),
       (7,'tourer/Toyota-Grandia-Tourer-2019-side.jpg' ,'','MAIN'),
       (8,'rush/Toyota-Rush-2019.jpg'        ,'','MAIN');
	   
insert into CarViewPages (CarUnitId, Viewname)
values (1,'CarDetail_van'),
	   (2,'CarDetail_van'),
	   (3,'CarDetail_suv'),
	   (4,'CarDetail_mpv'),
	   (5,'CarDetail_sedan'),
	   (6,'CarDetail_pickup'),
	   (6,'CarDetail_van'),
	   (6,'CarDetail_mpv');

insert into CarRates (Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate)
values (3000,2500,2250,100,5,1,300), --grandia
	   (3500,2500,2250,100,5,2,300), --premium
	   (3000,2500,2250,100,5,3,300), --everest
	   (2500,1800,1500,100,5,4,300), --innova
	   (2500,1800,1500,100,5,5,300), --honda
	   (3500,2500,2250,100,5,6,300), --honda
	   (3500,2500,2250,100,5,7,300), --honda
	   (2500,2500,2250,100,5,8,300); --pickup

insert into CarUnitMetas(carUnitId,PageTitle,MetaDesc,HomeDesc)
values
	-- Van -- 
	(1, 'Vehicle (Toyota Grandia GL) for Rent in Davao City, Reliable car rental company in Davao',
	'Toyota Grandia GL is 10-12 seater van for business, tour and family travel needs. Car Rental offers affordable and flexible rental rates',
	'Comfortable 10 Seater van for rent for business and family occasions. Popular unit for Rent-A-Car in Davao.'
	),

	-- Nissan Premium -- 
	(2, 'Vehicle (Nissan Urvan Premium) for Rent in Davao City, Reliable van rental company in Davao City',
	'Nissan Urvan Premium is comfortable 10-14 seater van for business, tour and family travel needs. Very few rent-a-car company in Davao offers this type of vehicle',
	'Highroof van that can accommodate 14pax with individual reclining seats. No jump seats. Very few rent-a-car company in Davao offers this type of vehicle.'
	),

	-- SUVs -- 
	(3, 'SUV (Sports Utility Vehicle) for Rent in Davao City, Rent-A-Car company in Davao City',
	'SUV for rent in Davao City for your business needs and personal needs that can accommodate 7 persons. Open for SelfDrive or with driver rental option. Available units: Ford Everest, Toyota Fortuner, Toyota Innova',
	'SUV for business and personal needs that can accommodate 7persons. Units: Ford Everest, Toyota Fortuner, Toyota Innova.'
	),

	-- AUVs-- 
	(4, 'MPV (Multi purpose vehicle) for Rent in Davao City, Rent-A-Car company in Davao City',
	'MPV/AUV - Toyota Innova - for rent in Davao City for your travel needs. Open for SelfDrive or with driver rental option.',
	'Toyota Innova is a versatile vehicle for family and business use. Accommodates like a small van or SUV yet rides like a car.'
	),

	-- Sedan -- 
	(5, 'Sedan for Rent in Davao City, Car Rental company in Davao City',
	'Sedan - Honda City - for rent in Davao City for your business and travel needs. Open for SelfDrive or with driver rental option.',
	'Sedan is a practical vehicle for light travel for personal and business use. Available units: Honda City, Toyota Vios'
	),

	-- Pickup -- 
	(6, 'Pickup for Rent in Davao City, 4x4 rental, rent-a-car',
	'Pickup 4x4 for rent in Davao City for difficult terrain or bigger luggages. Units: Mitsubishi strada 4x4, Toyota Hilux 4x4',
	'Pickup 4x4 is best for difficult and unknown terrains. Can also use in hauling huge luggages.'
	),

	-- Pickup -- 
	(7, 'Pickup for Rent in Davao City, 4x4 rental, rent-a-car',
	'Pickup 4x4 for rent in Davao City for difficult terrain or bigger luggages. Units: Mitsubishi strada 4x4, Toyota Hilux 4x4',
	'Pickup 4x4 is best for difficult and unknown terrains. Can also use in hauling huge luggages.'
	),

	-- Pickup -- 
	(8, 'Pickup for Rent in Davao City, 4x4 rental, rent-a-car',
	'Pickup 4x4 for rent in Davao City for difficult terrain or bigger luggages. Units: Mitsubishi strada 4x4, Toyota Hilux 4x4',
	'Pickup 4x4 is best for difficult and unknown terrains. Can also use in hauling huge luggages.'
	);

	Insert into CarRatePackages (Description,Remarks,DailyMeals,DailyRoom,DaysMin, Status)
	values 
	('Custom',           'Fuel by renter', 0,0,0,    'SYS'),
	('Davao City Tour',  'Car Rental for City (Downtown) Tour Package', 0,0,0,'ACT' ),
	('Davao CountrySide Tour', 'Eden Nature Park, Philipine Eagle, Malagos, Japanese Tunnel, Shrine, Jacks Ridge', 0,0,0,'ACT'),
	('Samal Tour',       'Bat cave, Hagimit falls, Maxima Aquafun, Penaplata', 0,0,0,'ACT' ),
	('Panabo',           'One round trip',300,400,1, 'ACT'), 
	('Tagum',            'One round trip',300,400,1, 'ACT'), 
	('Davao Del Norte',  'One round trip',300,400,1, 'ACT'), 
	('Comval',           'One round trip',300,400,1, 'ACT'), 
	('Governor Generoso','One round trip',300,400,1, 'INC'),
	('Mati City',        'One round trip',300,400,1, 'ACT'),
	('Davao Oriental',   'One round trip',300,400,1, 'INC'),
	('Agusan Del Sur',   'One round trip',300,400,1, 'INC'),
	('Agusan Del Norte', 'One round trip',300,400,1, 'INC'),
	('Surigao Del Sur',  'One round trip',300,400,1, 'INC'),
	('Surigao Del Norte','One round trip',300,400,1, 'INC'),

	('Marilog',          'One round trip', 0,0,0 ,   'ACT'),
	('Buda',             'Seagull',        0,0,0 ,   'ACT'),
	('Valencia',         'One round trip', 0,0,0 ,   'INC'),
	('Malaybalay',       'One round trip', 0,0,0 ,   'INC'),
	('Manolo fortich',   'One round trip', 0,0,0 ,   'INC'),
	('Cagayan De Oro',   'One round trip', 0,0,0 ,   'INC'),
	('Misamis Oriental', 'One round trip', 0,0,0 ,   'INC'),
	('Iligan',           'One round trip', 0,0,0 ,   'INC'),

	('Santa Cruz',       'Davao Del Sur. One round trip', 0,0,0 , 'ACT' ),
	('Digos',            'Davao Del Sur. One round trip', 0,0,0 , 'ACT' ),
	('Davao Del Sur',    'One round trip', 0,0,0 , 'ACT' ),
	('Malita',           'One round trip', 0,0,0 , 'ACT' ),
	('Don Marcelino',    'One round trip', 0,0,0 , 'INC' ),
	('General Santos',   'One round trip', 0,0,0 , 'ACT' ),
	('Sarangani',        'One round trip', 0,0,0 , 'INC' ),
	('Koronadal',        'One round trip', 0,0,0 , 'INC' ),
	('Isulan',           'One round trip', 0,0,0 , 'INC' ),
	('Sultan Kudarat',   'One round trip', 0,0,0 , 'INC' ),

	('Kidapawan',        'Kidapawan. One round trip',     0,0,0 , 'ACT' ),
	('Ilomavis',         'Ilomavis. One round trip',      0,0,0 , 'INC' ),
	('Kabacan',          'Kabacan. One round trip',       0,0,0 , 'INC' ),
	('Arakan',           'Arakan. One round trip',        0,0,0 , 'INC' ),
	('Midsayap',         'Midsayap. One round trip',      0,0,0 , 'INC' ),
	('Cotabato City',    'Cotabato City. One round trip', 0,0,0 , 'INC' ),
	('North Cotabato',   'Kidapawan. One round trip',     0,0,0 , 'INC' )
	;

-------------------- SQL.SYS --------------------------------


-------------------- Jobs Expenses --------------------------

insert into ExpensesCategories(Description, Remarks)
values	('Car Rental',''),
		('Tour' ,''),
		('Company',''),
		('Allowance',''),
		('Others',''),
		('Suppliers','')
;

insert into Expenses ( Name, Remarks, SeqNo, ExpensesCategoryId )
values	('Fuel'		 ,'',10,1),
		('Car Wash'  ,'',10,2),
		('Driver'	 ,'',10,3),
		('Allowance' ,'',10,4),
		('CA','Cash Advance',11,5),
		('Others'	 ,'',15,5),

		('Supplier Expense','',11,6),
		('Supplier Comi','',11,6),
		('Agency Fee','',12,4),
		('Tax','',11,3),
		('Entrances','',11,2),
		('Barge Fee','',11,2),
		('Car Rental','',11,1),
		('Security Deposit','',11,1)
		;


--------- ACTIVITES ------------------
--insert into CustEntActStatus([Status]) values 
--('Open'),('For Client Comment'),('For Meeting'),('Awarded'),('Close');

insert into CustEntActivities(CustEntMainId,Date,Assigned,ProjectName,SalesCode,Amount,Status,Type,ActivityType,Remarks,CustEntActStatusId,CustEntActActionCodesId,CustEntActActionStatusId)
values	(1,'1/20/2022','demo@gmail.com','building a project', 'SO-002'	,50000	,'Open'			,'Bidding Only'	,'Quotation','Meeting Lunch - Sales Activity', 1, 1, 1),
		(1,'2/05/2022','demo@gmail.com','building a project', 'SO-002'	,50000	,'Awarded'		,'Bidding Only'	,'Quotation','Presentation'		, 1, 1, 1),
		(2,'1/27/2022','demo@gmail.com','Supplier Materials', 'SO-005'	,25000	,'For Meeting'	,'Others'		,'Meeting'	,' '				, 1, 1, 1),
		(2,'2/18/2022','demo@gmail.com','Supplier Materials', 'SO-005'	,28000	,'For Meeting'	,'Others'		,'Meeting'	,'For Meeting'		, 1, 1, 1),
		(2,'3/05/2022','demo@gmail.com','Supplier Materials', 'SO-005'	,29000	,'For Meeting'	,'Others'		,'Meeting'	,'For Client Comment', 1, 1, 1),
		(3,'1/21/2022','demo@gmail.com','Davao Bridge'		, 'SO-009'	,75000	,'Awarded'		,'Firm Inquiry'	,'Quotation','Initial Meeting'	, 1, 1, 1),
		(3,'2/21/2022','demo@gmail.com','Davao Bridge'		, 'SO-009'	,75000	,'For Meeting'	,'Firm Inquiry'	,'Meeting'	,'Initial Meeting'	, 1, 1, 1);

insert into CustEntActivities(CustEntMainId,Date,Assigned,ProjectName,SalesCode,Amount,Status,Type,ActivityType,Remarks,CustEntActStatusId,CustEntActActionCodesId,CustEntActActionStatusId)
values	(2,'06/05/2022', 'demo@gmail.com', 'Buidling A'		,'SC-012'	,0		,'Open'			,'Bidding Only'		,'Quotation','', 1, 1, 1),
		(3,'06/20/2022', 'demo@gmail.com', 'Buidling C'		,'SC-013'	,2500	,'For Meeting'	,'Others'			,'Quotation','', 1, 1, 1),
		(2,'07/12/2022', 'demo@gmail.com', 'Buidling A'		,'SC-012'	,6500	,'For Meeting'	,'Others'			,'Meeting'	,'', 1, 1, 1),
		(2,'07/15/2022', 'demo@gmail.com', 'Buidling A'		,'SC-012'	,3500	,'For Meeting'	,'Others'			,'Meeting'	,'', 1, 1, 1),
		(3,'07/16/2022', 'demo@gmail.com', 'Buidling C'		,'SC-013'	,25000	,'Closed'		,'Buying Inquiry'	,'Sales'	,'', 1, 1, 1); 


		
insert into SupplierActivities([Code],[DtActivity],[Assigned],[Remarks],[SupplierId],[Amount],[Type],[ActivityType],[SupplierActStatusId],[SupplierActActionCodeId], SupplierActActionStatusId)
values ('CO-001','7/25/2022 4:17:58 PM','admin@gmail.com','Meeting'			,2,2000,'Bidding Only'	,'Procurement', 1, 1, 1),
	   ('CO-001','7/26/2022 3:30:00 PM','demo@gmail.com' ,'Sales Meeting'	,2,2000,'Bidding Only'	,'Meeting', 1, 1, 1),
	   ('CO-002','7/20/2022 3:30:00 PM','demo@gmail.com' ,'Sales Meeting'	,3,2000,'Others'		,'Meeting', 1, 1, 1),
	   ('CO-002','7/30/2022 9:10:00 AM','demo@gmail.com' ,'Sales Meeting'	,3,2000,'Firm Inquiry'	,'Meeting', 1, 1, 1),
	   ('CO-002','8/12/2022 1:30:00 PM','admin@gmail.com','Sales Meeting'	,3,2000,'Buying Inquiry','Job Order', 1, 1, 1),
	   ('CO-003','7/21/2022 1:00:20 PM','admin@gmail.com','Sales Meeting'	,4,2000,'Bidding Only'	,'Revision', 1, 1, 1);


-- POSt SALE --

insert into CustEntActPostSaleStatus([Status]) values 
('To Follow Up'),('Ongoing'),('Closed'),('Rejected');


-- SALES LEAD SAMPLE DATA --

insert into SalesLeads([Date],[Details],[Remarks],[CustomerId], [CustName], [DtEntered],[EnteredBy],[Price],[AssignedTo],[CustPhone],[CustEmail],[SalesCode]) 
values ('3/8/2021', 'Sales Lead Sample 01', '', 2, 'John Doe', '3/8/2021', 'admin', 75000, 'admin@gmail.com', '0912 345 6789', 'johndoe@gmail.com', 'SL-001');


