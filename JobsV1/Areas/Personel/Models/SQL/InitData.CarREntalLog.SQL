insert into crLogOwners([Name],[Mobile],[Remarks])
values 
('AJ88', '', null),
('RealBreeze', '', null);

insert into crLogUnits([Description],[crLogOwnerId])
values 
('RNY301 Innova' , 1),
('AAF8980 Innova', 1),
('LAF4920 Innova', 1),
('GAN7604 Innova', 1),
('LDP2264 Everest', 1),
('NEO380 Fortuner', 1),
('AAF9396 Montero', 1),
('LAB9873 City'  , 1),
('GAL8852 Rush'  , 1),
('AOA5108 Super' , 1),
('GAI3946 Premium', 1),
('P4W423 Tourer' , 1);



Insert into crLogDrivers([Name])
values
('Peter'),
('Kelvin'),
('Nick'),
('Danny'),
('Wilfredo'),
('Jehiel'),
('Lemwel'),
('Jeremiah'),
('Jonald');

Insert into crLogCompanies([Name],[isShuttle],[IsInternal])
values
('Realbreeze', 0, 1),
('Shimizu', 0, 0),
('UGC', 0, 0),
('Teleperformance', 0, 0),
('Sutherland', 0, 0),
('IBEX', 0, 0),
('AwesomeOS', 1, 0),
('VXi', 0, 0);

insert into crLogTypes([Type]) values 
('Fuel'), ('Maintenance'), ('Others');


insert into crCashReqStatus([Status]) values 
('Request'), ('Approved'),('Released'), ('Returned');

insert into crLogPaymentTypes([Type]) values 
('Cash'), ('Credit Card'),('PO'), ('Others'), ('NA');

insert into crLogCashTypes([Code],[Description]) values 
('SAL','Salary'), ('CA','CA'),('PAY','Payment');

insert into crLogPassengerMasters ([Name],[Contact],[PassAddress],[PickupPoint],[PickupTime],[DropPoint],[DropTime],[Remarks],[RestDays],[Area],[NextDay]) values 
('John','0912 654 8798','Matina','Centerpoint, Matina','7:30 PM','MTS','9:30 PM','','Sun,Sat','Matina',0),
('Mark','0923 782 0021','Boulevard','Boulevard','7:40 PM','MTS','9:30 PM','','Sun,Sat','Boulevard',0),
('Mary','0956-979-9143','Catalunan','Catalunan','7:20 PM','MTS','9:30 PM','','Sun,Sat','Catalunan',0),
('Lee','0978-977-1321','Mintal','Mintal','7:10 PM','MTS','9:30 PM','','Sun,Sat','Mintal',0),
('Jerry','0965 889 8789','Cabantian','Cabantian','6:50 PM','MTS','9:30 PM','','Sun,Sat','Cabantian',0);

insert into crLogPassengerAreas ([Name]) values 
('Agdao'),('Bunawan'), ('Buhangin'),('Boulevard'),('Calinan'),('Catalunan'),('Matina'),('Mintal'),('Times'),('Toril');

insert into crLogPassRemarks([Description],[RemarksFor]) values
('Passenger has private service', 1),
('Passenger on rest day', 1),
('Passenger commuted', 1),
('Passenger declined', 1),
('Passenger transferred to another van', 1),
('Passenger unspecified reason', 1),

('Passenger not anwering text/call', 2),
('Passenger not on the pickup location', 2),
('Passenger did not arrive on time', 2),
('Passenger will be late', 2),
('Passenger is absent / on-leave / rest day', 2),
('Unspecified reason', 2)
;

--insert into crLogPassRemarks([])

--select * from crLogTrips;
--select * from crLogDrivers;
--select * from crLogUnits;
--select * from crLogCompanies;

