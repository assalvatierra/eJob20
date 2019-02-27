Insert into AsIncClients
(
    [ShortName], 
    [Name],
    [Company],
    [Contact1],
    [Contact2],
    [Address],
    [Remarks]
) values ( 'General' , 'General','','','','','');

insert into AsIncCategories (
    [Desc] ,
    [Remarks] 
) values ('Transport Services',''),('Tour Services',''),('Air Ticket',''),('Other Services','');

Insert into AsExpBillers
(
    [ShortName], 
    [FullName],
    [Address],
    [Contact],
    [Contact2],
    [Remarks]
) values ( 'Undefined' , 'Undefined','','','',''),
( 'Building' , 'Building rentals and maintenance','','','',''),
( 'Office' , 'Supplies, furnitures, utilities, etc','','','',''),
( 'Communication' , 'Internet, Mobile, load','','','',''),
( 'Vehicle' , 'Fuel, pms, maintenance','','','',''),
( 'General' , 'General','','','','')
;

-- Creating table 'AsCostCenters'
Insert into AsCostCenters (
    [ccName] ,
    [xxRemarks] 
) values ('Real Breeze', '' ),('AJ88 Davao Car Rental','');
 
insert into AsExpCategories (
    [Desc] ,
    [Remarks] 
) values ('Staff Expenses',''),('Operational Expense',''),('Office Expenses',''),('Allowances',''),('Others','');

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------