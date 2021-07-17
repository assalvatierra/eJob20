
-- accounts 
insert into ArAccounts([Name],[Landline],[Email],[Mobile],[Company],[Address],[Remarks],[ArAccStatusId]) values
('John Doe' , '(082) 555 0100', 'johndoe@gmail.com', '0922 656 9877', 'Acer Ph Inc','Makati','',1),
('Mark Cruz', '(082) 333 1212', 'markCruz@gmail.com', '0911 085 8455','NewCompany','Manila','',1),
('Mary Lee' , '(082) 555 0100', 'marylee@gmail.com', '0927 898 8822','Microsoft','Makati','',1),
('Tony Stark', '(082) 222 2525', 'tonystark@gmail.com', '0926 333 3233','','Davao','',1),
('Peter Parker', '(082) 777 8989', 'peterparker@gmail.com', '0921 112 3238','','Davao','',1);

-- account credits
insert into ArAccntCredits([ArAccountId],[DtCredit],[CreditLimit],[OverLimitAllowed],[CreditWarning],[ApprovedBy],[ArCreditStatusId]) values
(2, '07/21/2020', 50000, 10000, 40000, 'admin', 1),
(3, '07/21/2020', 50000, 10000, 40000, 'admin', 1),
(4, '07/20/2020', 60000, 10000, 50000, 'admin', 1),
(5, '07/20/2020', 70000, 20000, 60000, 'admin', 1);


-- account terms
insert into ArAccntTerms([ArAccountId],[dtTerm],[NoOfDays],[Remarks],[ApprovedBy],[ArAccntTermStatusId]) values
(2, '07/21/2020', 15, null, 'admin', 1),
(3, '07/21/2020', 15, null, 'admin', 1),
(4, '07/21/2020', 15, null, 'admin', 1),
(5, '07/21/2020', 15, null, 'admin', 1);



--transactions
insert into ArTransactions([ArAccountId],[InvoiceId],[DtInvoice],[Description],[DtEncoded],[DtDue],[Amount],[Interval],[IsRepeating],[Remarks],[ArTransStatusId],[ArCategoryId],[DtService], DtServiceTo, PrevRef, NextRef, InvoiceRef ) values
(2, '101', '07/21/2020', 'Invoice 101', '07/21/2020', '08/15/2020', 50000, 0, 0, null, 2, 1, '07/18/2020', '07/25/2020',0,0, 'INV001'),
(3, '102', '07/18/2020', 'Invoice 102', '07/20/2020', '08/19/2020', 70000, 0, 0, null, 2, 1, '07/10/2020', '07/14/2020',0,0, 'INV002'),
(4, '103', '07/15/2020', 'Invoice 103', '07/19/2020', '08/25/2020', 80000, 0, 0, null, 1, 1, '07/15/2020', '07/16/2020',0,0, NULL),
(5, '104', '07/10/2020', 'Invoice 104', '07/20/2020', '08/12/2020', 50000, 30, 1, null, 1, 1, '07/10/2020', '07/12/2020',0,0, NULL),
(6, '106', '07/12/2020', 'Invoice 105', '07/19/2020', '08/24/2020', 45000, 15, 1, null, 3, 1, '07/12/2020', '07/15/2020',0,0, NULL);

-- payments
insert into ArPayments([DtPayment],[Amount],[Remarks],[Reference],[ArAccountId],[ArPaymentTypeId]) values
('07/21/2020', 5000, null, null, 2, 1),
('07/22/2020', 5000, null, null, 2, 1),
('07/21/2020', 15000, null, null, 3, 1),
('07/21/2020', 12000, null, null, 4, 1);

-- transaction payments
insert into ArTransPayments([ArTransactionId],[ArPaymentId]) values 
(1,1),
(1,2),
(2,3),
(3,4);

