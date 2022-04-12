
DECLARE @Datetoday DATETIME = GETDATE()
DECLARE @Dateyesterday DATETIME  = dateadd(DD,-1,GETDATE())
DECLARE @Date2DaysAgo DATETIME  = dateadd(DD,-2,GETDATE())

-- Vehicle Expenses Request Logs --
insert into crLogFuels([dtRequest],[Amount],[crLogUnitId],[crLogDriverId],[dtFillup],[odoFillup],[orAmount],[crLogTypeId],[isFullTank],[crLogPaymentTypeId]) values 
(@Datetoday, 1650, 1, 1, @Datetoday, 2831, 2000, 1,0,1),
(@Datetoday, 1500, 2, 1, @Datetoday, 3254, 1800, 1,1,2),
(@Datetoday, 1750, 3, 2, @Datetoday, 5211, 1800, 1,1,2),
(@Dateyesterday, 2500, 2, 2, @Dateyesterday, 0, 0, 2,0,5),
(@Dateyesterday, 2000, 2, 3, @Dateyesterday, 0, 0, 2,0,5),
(@Dateyesterday, 1750, 3, 3, @Dateyesterday, 0, 0, 1,0,5),
(@Dateyesterday, 1750, 3, 4, @Dateyesterday, 0, 0, 1,0,5),
(@Dateyesterday, 2000, 2, 4, @Dateyesterday, 0, 0, 1,0,5);

insert into crLogFuelStatus([dtStatus],[crLogFuelId],[crCashReqStatusId]) values
(@Dateyesterday, 1, 3),
(@Dateyesterday, 2, 3),
(@Dateyesterday, 3, 3),
(@Dateyesterday, 4, 2),
(@Dateyesterday, 1, 1),
(@Datetoday, 2, 2),
(@Datetoday, 3, 1),
(@Datetoday, 4, 1);

-- Trip Logs --

insert into crLogClosings([dtClose]) values 
(@Dateyesterday),
(@Dateyesterday),
(@Dateyesterday),
(@Datetoday),
(@Datetoday),
(@Datetoday);

insert into crLogTrips ([DtTrip],[crLogDriverId],[crLogCompanyId],[crLogUnitId],[Rate],[Expenses],[DriverFee],[Remarks],[crLogClosingId],[Addon],[OdoStart],[OdoEnd],[DriverOT],[IsFinal],[AllowEdit]) values 
--paid trips
(@Date2DaysAgo, 1, 1, 1, 2500, 0, 500, '', 1, 0, 1000, 1065, 0, 0, 0),
(@Date2DaysAgo, 2, 1, 2, 2500, 0, 500, '', 2, 0, 2400, 2480, 0, 0, 0),
(@Date2DaysAgo, 3, 1, 3, 2500, 0, 500, '', 3, 0, 2590, 2650, 200, 0, 0),
(@Date2DaysAgo, 4, 2, 4, 2500, 0, 500, '', 4, 0, 1880, 1950, 100, 0, 0),
(@Date2DaysAgo, 5, 2, 5, 2500, 0, 500, '', 5, 0, 3520, 3800, 0, 0, 0),
(@Date2DaysAgo, 6, 2, 6, 2500, 0, 500, '', 6, 0, 2672, 2690, 0, 0, 0),

--unpaid trips

(@Dateyesterday, 1, 2, 5, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),
(@Dateyesterday, 2, 2, 6, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),
(@Dateyesterday, 1, 2, 5, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),
(@Dateyesterday, 2, 2, 6, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),

(@Datetoday, 2, 1, 2, 2500, 0, 500, '', null, 0, 2500, 2580, 0, 0, 0),
(@Datetoday, 3, 2, 3, 3500, 0, 500, '', null, 0, 2500, 2580, 0, 0, 0),
(@Datetoday, 4, 2, 4, 3500, 0, 500, '', null, 0, 2500, 2580, 200, 0, 0),
(@Datetoday, 1, 3, 1, 3200, 0, 500, '', null, 0, 2500, 2580, 200, 0, 0);


insert into crLogTrips ([DtTrip],[crLogDriverId],[crLogCompanyId],[crLogUnitId],[Rate],[Expenses],[DriverFee],[Remarks],[crLogClosingId],[Addon],[OdoStart],[OdoEnd],[DriverOT],[IsFinal],[AllowEdit]) values 

('3/22/2022', 1, 1, 1, 2500, 0, 500, '', 1, 0, 1000, 1065, 0, 0, 0),
('3/22/2022', 2, 1, 2, 2500, 0, 500, '', 2, 0, 2400, 2480, 0, 0, 0),
('3/23/2022', 3, 1, 3, 2500, 0, 500, '', 3, 0, 2590, 2650, 200, 0, 0),
('3/23/2022', 4, 2, 4, 2500, 0, 500, '', 4, 0, 1880, 1950, 100, 0, 0),
('3/24/2022', 5, 2, 5, 2500, 0, 500, '', 5, 0, 3520, 3800, 0, 0, 0),
('3/24/2022', 6, 2, 6, 2500, 0, 500, '', 6, 0, 2672, 2690, 0, 0, 0),

('3/22/2022', 1, 2, 5, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),
('3/22/2022', 2, 2, 6, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),
('3/22/2022', 1, 2, 5, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),
('3/22/2022', 2, 2, 6, 2500, 0, 500, '', null, 0, null, null, 0, 0, 0),

('3/21/2022', 2, 1, 2, 2500, 0, 500, '', null, 0, 2500, 2580, 0, 0, 0),
('3/21/2022', 3, 2, 3, 3500, 0, 500, '', null, 0, 2500, 2580, 0, 0, 0),
('3/21/2022', 4, 2, 4, 3500, 0, 500, '', null, 0, 2500, 2580, 200, 0, 0),
('3/21/2022', 1, 3, 1, 3200, 0, 500, '', null, 0, 2500, 2580, 200, 0, 0);
-- Cash Release --
insert into crLogCashReleases([DtRelease],[Amount],[Remarks],[crLogDriverId],[crLogClosingId],[crLogCashTypeId]) values
(@Date2DaysAgo, 1000, '', 2, 2, 1 ),
(@Date2DaysAgo,  100, '', 3, 3, 1 ),
(@Dateyesterday, 500, '', 4, 4, 1 );

insert into crLogPassStatus([Status]) values 
('For Pickup'),('Contacted'),('Boarded'),('Dropped'),('Declined'),('Failed');


-- Company Rate --
insert into crLogCompanyRates ([TripRate],[OTRate],[TripHours],[DriverDailyRate],[DriverOTRate],[crLogCompanyId]) values
(3000, 200, 10, 500, 50, 1),
(3500, 200, 12, 600, 50, 2),
(3500, 200, 12, 600, 50, 3),
(3200, 200, 9,  500, 60, 4);

-- Driver Contributions 

insert into crLogDriverPayments ([Description],[Amount],[Remarks],[crLogDriverId]) values
('SSS', 300, '', 1),('Philhealth', 150, '', 1),('Pagibig', 150, '', 1),
('SSS', 300, '', 2),('Philhealth', 150, '', 2),('Pagibig', 150, '', 2),
('SSS', 300, '', 3),('Philhealth', 150, '', 3),('Pagibig', 150, '', 3);