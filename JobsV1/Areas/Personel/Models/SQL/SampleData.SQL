

insert into crLogFuels([dtRequest],[Amount],[crLogUnitId],[crLogDriverId],[dtFillup],[odoFillup],[orAmount],[crLogTypeId]) values 
('7/21/2020', 650, 1, 1, '7/21/2020', 0, 0, 1),
('7/22/2020', 500, 2, 2, '7/22/2020', 0, 0, 1),
('7/25/2020', 750, 3, 3, '7/25/2020', 0, 0, 1),
('7/25/2020', 800, 2, 1, '7/25/2020', 0, 0, 1);

insert into crLogFuelStatus([dtStatus],[crLogFuelId],[crCashReqStatusId]) values
('7/21/2020', 1, 2),
('7/22/2020', 2, 1),
('7/25/2020', 3, 3),
('7/25/2020', 4, 3);

