﻿

/**
	Accounts Payables Initial Data
*/

/* Sample Accounts */
insert into ApAccounts([Name],[Landline],[Email],[Mobile],[ContactPerson],[Address],[Remarks],[ApAccStatusId]) values
('PLDT', '082 232 6561', 'pldt@gmail.com', '0923 656 8885', 'John Doe',  null, null, 1),
('DCWD', '', 'dcwd@gmail.com', '0929 112 6505','Mark Cruz',  'Davao City', null, 1),
('Malagos', '082 333 5656', 'maryLee@gmail.com', '0922 363 2210', 'Mary Lee', 'Matina, Davao City', null, 1);

/* Sample Transaction */
insert into ApTransactions([ApAccountId],[InvoiceNo],[DtInvoice],[DtEncoded],[Description],[Amount],[DtService],[DtServiceTo],[DtDue],[Remarks]
                          ,[ApTransStatusId],[ApTransCategoryId],[Interval],[IsRepeating],[RepeatCount],[RepeatNo],[NextRef],[PrevRef],[IsPrinted]) values 
 (2,'PLDT001','07/25/2021','07/28/2021', 'Internet Billing', 2900, '07/01/2021', '07/30/2021', '07/02/2021', null, 1, 1, 30, 1, 2, 1, 0, 0, 0),
 (3,'W001','08/03/2021','08/02/2021', 'Water Billing', 800, '08/06/2021', '08/06/2021', '08/12/2021', null, 1, 1, 30, 1, 3, 1, 0, 0, 0),
 (4, null,'07/25/2021','07/28/2021', 'Tour Payables', 500, '07/20/2021', '07/20/2021', '07/30/2021', null, 1, 1, 0, 0, 0, 0, 0, 0, 0);