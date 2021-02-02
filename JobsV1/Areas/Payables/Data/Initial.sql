
insert into ApTransStatus([Status]) values
('Requested'),('Approved'),('Released'),('Closed');

insert into ApAccStatus([Status]) values
('Active'),('Inactive'),('OnHold');

insert into ApPaymentTypes([Type]) values
('Cash'),('Check'),('Bank'),('PO'),('Others');

insert into ApPaymentStatus([Status]) values
('Pending'),('Accepted'),('Cancelled');

insert into ApActionItems([Action],[Remarks],[SortNo]) values
('Payment Request','',1),('Payment Sent','',2),('Payment Received','',3),('Payment Closed','',4),('Payment Cancelled','',5);

insert into ApAccounts([Name],[Landline],[Email],[Mobile],[ContactPerson],[Address],[Remarks],[ApAccStatusId]) values
('< New Account >', null, 'NA', 'NA', null, null, null, 1);

insert into ApTransCategories([Name],[Remarks]) values
('Others', '');


