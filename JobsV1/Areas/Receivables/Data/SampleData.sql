
-- accounts 
insert into ArAccounts([Name],[Landline],[Email],[Mobile],[Company],[Address],[Remarks],[ArAccStatusId]) values
('NA' , '(132) 555 0100', 'travel@gmail.com', '-', 'RealBreeze','DAvao','',1),
('Mark Cruz', '(132) 333 1212', 'markCruz@gmail.com', '0911 135 8455','NewCompany','Manila','',1),
('Mary Lee' , '(132) 555 0100', 'marylee@gmail.com', '0927 898 8822','Microsoft','Makati','',1),
('Tony Stark', '(132) 222 2525', 'tonystark@gmail.com', '0926 333 3233','','Davao','',1),
('Peter Parker', '(132) 777 8989', 'peterparker@gmail.com', '0921 112 3238','','Davao','',1);


-- accounts 
insert into ArAccContacts([Name],[Email],[Mobile],[ArAccountId]) values
('New Contact' ,'na@gmail.com', '0922 656 9877',1),
('Mary Lee','marylee@gmail.com', '0921 898 3238',3),
('Peter Parker','peterparker@gmail.com', '0921 112 3238',5);

-- account credits
insert into ArAccntCredits([ArAccountId],[DtCredit],[CreditLimit],[OverLimitAllowed],[CreditWarning],[ApprovedBy],[ArCreditStatusId]) values
(2, '02/21/2022', 50000, 10000, 40000, 'admin', 1),
(3, '02/21/2022', 50000, 10000, 40000, 'admin', 1),
(4, '02/20/2022', 60000, 10000, 50000, 'admin', 1),
(5, '02/20/2022', 70000, 20000, 60000, 'admin', 1);


-- account terms
insert into ArAccntTerms([ArAccountId],[dtTerm],[NoOfDays],[Remarks],[ApprovedBy],[ArAccntTermStatusId]) values
(2, '02/21/2022', 15, null, 'admin', 1),
(3, '02/21/2022', 15, null, 'admin', 1),
(4, '02/21/2022', 15, null, 'admin', 1),
(5, '02/21/2022', 15, null, 'admin', 1);



--transactions
insert into ArTransactions([ArAccountId],[ArAccContactId],[InvoiceId],[DtInvoice],[Description],[DtEncoded],[DtDue],[Amount],[IsRepeating],[Remarks],[ArTransStatusId],[ArCategoryId],[DtService], DtServiceTo, InvoiceRef ) values
(2, 1, '101', '02/21/2022', 'Invoice 101', '02/21/2022', '05/15/2022', 50000, 0, null, 2, 1, '02/18/2022', '02/25/2022', 'INV001'),
(3, 1, '102', '02/18/2022', 'Invoice 102', '02/20/2022', '05/19/2022', 70000, 0, null, 2, 1, '02/10/2022', '02/14/2022', 'INV002'),
(4, 2, '103', '02/15/2022', 'Invoice 103', '02/19/2022', '05/25/2022', 80000, 0, null, 1, 1, '02/15/2022', '02/16/2022', NULL),
(5, 2, '104', '02/10/2022', 'Invoice 104', '02/20/2022', '05/12/2022', 50000, 1, null, 1, 1, '02/10/2022', '02/12/2022', NULL),
(6, 2, '106', '02/12/2022', 'Invoice 105', '02/19/2022', '05/24/2022', 45000, 1, null, 3, 1, '02/12/2022', '02/15/2022', NULL);

-- payments
insert into ArPayments([DtPayment],[Amount],[Remarks],[Reference],[ArAccountId],[ArPaymentTypeId]) values
('02/21/2022', 5000, null, null, 2, 1),
('02/22/2022', 5000, null, null, 2, 1),
('02/21/2022', 15000, null, null, 3, 1),
('02/21/2022', 12000, null, null, 4, 1);

-- transaction payments
insert into ArTransPayments([ArTransactionId],[ArPaymentId]) values 
(2,1),
(2,2),
(3,3),
(4,4);

