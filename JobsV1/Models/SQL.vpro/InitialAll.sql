

-- ------------------------------------------------------------
-- Customer Configuration
-- ------------------------------------------------------------


insert into Cities(Name) values('Davao'),('Cebu'),('Makati'),('Manila');
insert into Branches(Name, CityId, Remarks, Address, Landline, Mobile) 
values ('Vpro',1,'Davao Main','Matina Davao City','082 297-1831','');

insert into JobStatus([Status]) values('INQUIRY'),('RESERVATION'),('CONFIRMED'),('CLOSED'),('CANCELLED'),('TEMPLATE');
insert into JobThrus([Desc]) values('PHONE'),('EMAIL'),('WALKIN');

insert into Banks([BankName],[BankBranch],[AccntName],[AccntNo])
 values ('Cash','Davao','Cash','0');

insert into Customers([Name],[Email],[Contact1],[Contact2],[Remarks],[Status]) values
    ('<< New Customer >>','--','--',' ',' ','ACT');

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
values ('admin@gmail.com');

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
insert into SalesStatusCodes([SeqNo],[Name])
values (1,'NEW'), (2,'EVALUATION'), (3, 'ACCEPTED'), (4, 'CHECKING'), (5, 'ACCEPTED'), (6, 'REJECTED'), (7, 'CLOSE');

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
('Item','Item','~/Images/CarRental/Repair101.png','PIPE'),
('Others','Other Types','~/Images/CarRental/Repair101.png','OTHER');

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
	('Stockiest/Trader'),('Supplier'),('Installer');

insert Into Suppliers([Name],[Contact1],[Details],[Email],[CityId],[SupplierTypeId],[Status],[CountryId],[Code] ) values
	('<< New Supplier >>','--',' ', '--', '1', '1', 'ACT', 175, 'SUP01');

-- ----------------------------
-- Supplier Items / Products
-- ----------------------------

insert into SupplierUnits([Unit])
values ('Meter'),('Inch'),('Feet'),('Box'),('Package');

insert into SupplierContactStatus([Name]) values ('Active'),('Resigned');

insert into SupplierActivityTypes([Type],[Points]) values 
('Procurement', 5), ('Job Order', 8), ('Meeting', 3), ('Others', 2), ('Revision', 1);

insert into SupplierActStatus([Status]) values 
('Open'),('In-Progress'),('Cancelled'),('Closed');

insert into SupDocuments([Description]) values 
	   ('Business Registration'),('Tax Registration'), ('Import Export Registration');

-- ----------------------------------------------
-- Supplier PO Configuration
-- ----------------------------------------------
insert into SupplierPoStatus ([Status],[OrderNo]) values 
('REQUEST',1),('APPROVED',2),('CANCELLED',2),('CONFIRMED',3),('DELIVERED',4),('CLOSE',5);

-- Customer and Companies Initial

insert into CustCategories([Name],[iconPath])
values 
	   ('PRIORITY','Images/Customers/Category/star-filled-40.png'),
	   ('ACTIVE','Images/Customers/Category/star-filled-40.png'),
	   ('INACTIVE','Images/Customers/Category/star-filled-40.png'),
	   ('BAD ACCOUNT','Images/Customers/Category/star-filled-40.png'),
	   ('SUSPENDED','Images/Customers/Category/star-filled-40.png'),
	   ('BILLING/TERMS','Images/Customers/Category/star-filled-40.png'),
	   ('ACCREDITATION ON PROCESS','Images/Customers/Category/star-filled-40.png');

insert into CustEntAccountTypes ([Name],[SysCode]) values
('Regular','REG01');

insert into CustEntMains([Name],[Address],[Contact1],[Contact2])
values ('NEW (not yet defined)',' ',' ',' ');

insert into Customers(Name, Email, Contact1, Contact2, Remarks, Status) 
values('Juan Dela Cruz','johndoe@gmail.com','09950753794','09950753794','Test User','ACT');

insert into CustEntMains(Name,Code, Address, Contact1, Contact2, iconPath, Website, Remarks, CityId, Status, AssignedTo, Mobile, Exclusive, CustEntAccountTypeId) 
values ('RealSys Davao','COP-001','Davao City','888-9888','888-9881','','google.com','',5,'ACT','admin@gmail.com','09126659987','PUBLIC', 1);
	 
insert into CustEntAssigns(Assigned, Remarks, CustEntMainId, Date)
values ('demo@gmail.com', '' , 2 , '11-30-2019');

-- Customers / Assigned Agent Types --
insert into CustAssocTypes([Type]) 
values ('Contact'), ('Agent');

insert into CustEntities(CustEntMainId, CustomerId, Position, CustAssocTypeId) 
values (1,1,'Staff',1);
		
insert into CustCats(CustomerId, CustCategoryId) 
values	(1,1);

insert into CustEntCats(CustEntMainId,CustCategoryId)
values (1,1); 

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

-------------------- SQL.SYS --------------------------------


-------------------- Jobs Expenses --------------------------

insert into ExpensesCategories(Description, Remarks)
values	('Material',''),
		('Others',''),
		('Suppliers','')
;

insert into Expenses ( Name, Remarks, SeqNo, ExpensesCategoryId )
values	
		('Others'	 ,'',15,5)
		;


--------- ACTIVITES ------------------
insert into CustEntActStatus([Status]) values 
('Open'),('For Client Comment'),('For Meeting'),('Awarded'),('Close');

-- POSt SALE --

insert into CustEntActPostSaleStatus([Status]) values 
('To Follow Up'),('Ongoing'),('Closed'),('Rejected');
