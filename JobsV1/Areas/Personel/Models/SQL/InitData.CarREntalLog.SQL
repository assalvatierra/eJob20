insert into crLogUnits([Description])
values 
('RNY301 Innova'),
('AAF8980 Innova'),
('LAF4920 Innova'),
('GAN7604 Innova'),
('LDP2264 Everest'),
('NEO380 Fortuner'),
('AAF9396 Montero'),
('LAB9873 City'),
('GAL8852 Rush'),
('AOA5108 Super'),
('GAI3946 Premium'),
('P4W423 Tourer');


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

Insert into crLogCompanies([Name])
values
('Hubei'),
('Shimizu'),
('UGC'),
('Teleperformance'),
('Sutherland'),
('IBEX'),
('AwesomeOS'),
('VXi');

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

