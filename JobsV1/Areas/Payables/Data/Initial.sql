
insert into ApTransStatus([Status]) values
('Requested'),('Approved'),('Released'),('Closed');

insert into ApAccStatus([Status]) values
('Active'),('Inactive'),('OnHold');

insert into ApPaymentTypes([Type]) values
('Cash'),('Check'),('Bank'),('PO'),('Others');

insert into ApPaymentStatus([Status]) values
('Pending'),('Accepted'),('Cancelled');

insert into ApActionItems([Action],[Remarks],[SortNo]) values
('Payables Request','',1),('Payables Approved','',2),('Payables Released','',3),('Payables Closed','',4),('Payables Cancelled','',5),
('Payment Pending','',6),('Payment Approved','',7),('Payment Cancelled','',8),('Payment Edit','',9),('Payment Delete','',10),
('Payables Edit','',11), ('Payables Deleted','',12),
('Payables Repeat Created','',13), ('Payables Repeat Canclled','',14);

insert into ApAccounts([Name],[Landline],[Email],[Mobile],[ContactPerson],[Address],[Remarks],[ApAccStatusId]) values
('< New Account >', null, 'NA', 'NA', null, null, null, 1);

insert into ApTransCategories([Name],[Remarks]) values
('Others', '');


