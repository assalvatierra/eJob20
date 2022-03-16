insert into ArTransStatus([Status]) values
('New'),('For Approval'),('Approved'),('Sent'),('Settlement'),('Closed');

insert into ArAccStatus([Status]) values
('Active'),('Inactive'),('OnHold');

insert into ArPaymentTypes([Type]) values
('Cash'),('Check'),('Bank'),('PO'),('Others');

insert into ArActionItems([Action],[Remarks],[SortNo]) values
('New Bill','',1),('Bill For Approval','',2),('Bill Approved','',3),('Bill Sent','',4),('Bill For Settlement','',5),('Bill Closed','',6),('Bill Payment','',7),
('1st Reminder Sent','',11),('2nd Reminder Sent','',12),('3rd Reminder Sent','',13),
('Edit Transaction','',21);


insert into ArAccounts([Name],[Landline],[Email],[Mobile],[Company],[Address],[Remarks],[ArAccStatusId]) values
('< New Account >', null, 'NA', 'NA', null, null, null, 1);

insert into ArCategories([Name],[Remarks],[SortNo]) values
('Others', '', 100);

insert into ArCreditStatus([Status]) values ('Active'),('Pending'),('Expired');

insert into ArAccntTermStatus([Status]) values ('Active'),('Pending'),('Expired');


insert into ArDepositBanks([AccountName]) values ('BDO-AJ88'),('BPI-Realbreeze'),('BPI-VIA');
