
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/09/2022 15:17:44
-- Generated from EDMX file: C:\Users\Acer-PC\Documents\GitHub\eJob20\JobsV1\Areas\Personel\Models\CarRentalLogDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-JobsV1-20160528101923];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogTrip]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogTrips] DROP CONSTRAINT [FK_crLogDrivercrLogTrip];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogUnitcrLogTrip]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogTrips] DROP CONSTRAINT [FK_crLogUnitcrLogTrip];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogCompanycrLogTrip]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogTrips] DROP CONSTRAINT [FK_crLogCompanycrLogTrip];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogCashRelease]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashReleases] DROP CONSTRAINT [FK_crLogDrivercrLogCashRelease];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogClosingcrLogCashRelease]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashReleases] DROP CONSTRAINT [FK_crLogClosingcrLogCashRelease];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogClosingcrLogTrip]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogTrips] DROP CONSTRAINT [FK_crLogClosingcrLogTrip];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogUnitcrLogFuel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogFuels] DROP CONSTRAINT [FK_crLogUnitcrLogFuel];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogFuel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogFuels] DROP CONSTRAINT [FK_crLogDrivercrLogFuel];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogUnitcrLogOdo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogOdoes] DROP CONSTRAINT [FK_crLogUnitcrLogOdo];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogOdo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogOdoes] DROP CONSTRAINT [FK_crLogDrivercrLogOdo];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogFuelcrLogFuelStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogFuelStatus] DROP CONSTRAINT [FK_crLogFuelcrLogFuelStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_crCashReqStatuscrLogFuelStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogFuelStatus] DROP CONSTRAINT [FK_crCashReqStatuscrLogFuelStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogCashReleasecrLogCashStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashStatus] DROP CONSTRAINT [FK_crLogCashReleasecrLogCashStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_crCashReqStatuscrLogCashStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashStatus] DROP CONSTRAINT [FK_crCashReqStatuscrLogCashStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogTypecrLogFuel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogFuels] DROP CONSTRAINT [FK_crLogTypecrLogFuel];
GO
IF OBJECT_ID(N'[dbo].[FK_crRptUnitExpenseCrRptUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrRptUnits] DROP CONSTRAINT [FK_crRptUnitExpenseCrRptUnit];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogUnitCrRptUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrRptUnits] DROP CONSTRAINT [FK_crLogUnitCrRptUnit];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogPaymentTypecrLogFuel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogFuels] DROP CONSTRAINT [FK_crLogPaymentTypecrLogFuel];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogCashTypecrLogCashRelease]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashReleases] DROP CONSTRAINT [FK_crLogCashTypecrLogCashRelease];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogPassStatuscrLogPassenger]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogPassengers] DROP CONSTRAINT [FK_crLogPassStatuscrLogPassenger];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogTripcrLogPassenger]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogPassengers] DROP CONSTRAINT [FK_crLogTripcrLogPassenger];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogOwnercrLogUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogUnits] DROP CONSTRAINT [FK_crLogOwnercrLogUnit];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogTripcrLogTripJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogTripJobMains] DROP CONSTRAINT [FK_crLogTripcrLogTripJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogCompanycrLogCompanyRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCompanyRates] DROP CONSTRAINT [FK_crLogCompanycrLogCompanyRate];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogCashSalary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashSalaries] DROP CONSTRAINT [FK_crLogDrivercrLogCashSalary];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogCashGroupcrLogCashSalary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashGroups] DROP CONSTRAINT [FK_crLogCashGroupcrLogCashSalary];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogCashReleasecrLogCashGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogCashGroups] DROP CONSTRAINT [FK_crLogCashReleasecrLogCashGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogDriverPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogDriverPayments] DROP CONSTRAINT [FK_crLogDrivercrLogDriverPayment];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[crLogDrivers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogDrivers];
GO
IF OBJECT_ID(N'[dbo].[crLogUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogUnits];
GO
IF OBJECT_ID(N'[dbo].[crLogCompanies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCompanies];
GO
IF OBJECT_ID(N'[dbo].[crLogTrips]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogTrips];
GO
IF OBJECT_ID(N'[dbo].[crLogCashReleases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCashReleases];
GO
IF OBJECT_ID(N'[dbo].[crLogClosings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogClosings];
GO
IF OBJECT_ID(N'[dbo].[crLogFuels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogFuels];
GO
IF OBJECT_ID(N'[dbo].[crLogOdoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogOdoes];
GO
IF OBJECT_ID(N'[dbo].[crCashReqStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crCashReqStatus];
GO
IF OBJECT_ID(N'[dbo].[crLogFuelStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogFuelStatus];
GO
IF OBJECT_ID(N'[dbo].[crLogCashStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCashStatus];
GO
IF OBJECT_ID(N'[dbo].[crLogTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogTypes];
GO
IF OBJECT_ID(N'[dbo].[crRptUnitExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crRptUnitExpenses];
GO
IF OBJECT_ID(N'[dbo].[CrRptUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CrRptUnits];
GO
IF OBJECT_ID(N'[dbo].[crLogPaymentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogPaymentTypes];
GO
IF OBJECT_ID(N'[dbo].[crLogCashTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCashTypes];
GO
IF OBJECT_ID(N'[dbo].[crLogPassengers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogPassengers];
GO
IF OBJECT_ID(N'[dbo].[crLogPassStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogPassStatus];
GO
IF OBJECT_ID(N'[dbo].[crLogPassengerMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogPassengerMasters];
GO
IF OBJECT_ID(N'[dbo].[crLogPassengerAreas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogPassengerAreas];
GO
IF OBJECT_ID(N'[dbo].[crLogPassRemarks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogPassRemarks];
GO
IF OBJECT_ID(N'[dbo].[crLogOwners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogOwners];
GO
IF OBJECT_ID(N'[dbo].[crLogTripJobMains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogTripJobMains];
GO
IF OBJECT_ID(N'[dbo].[crLogCompanyRates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCompanyRates];
GO
IF OBJECT_ID(N'[dbo].[crLogCashGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCashGroups];
GO
IF OBJECT_ID(N'[dbo].[crLogCashSalaries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogCashSalaries];
GO
IF OBJECT_ID(N'[dbo].[crLogDriverPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogDriverPayments];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'crLogDrivers'
CREATE TABLE [dbo].[crLogDrivers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [contact1] nvarchar(max)  NULL,
    [contact2] nvarchar(max)  NULL,
    [OrderNo] int  NULL,
    [Status] nvarchar(3)  NULL
);
GO

-- Creating table 'crLogUnits'
CREATE TABLE [dbo].[crLogUnits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(30)  NOT NULL,
    [OrderNo] int  NULL,
    [Status] nvarchar(3)  NULL,
    [crLogOwnerId] int  NOT NULL
);
GO

-- Creating table 'crLogCompanies'
CREATE TABLE [dbo].[crLogCompanies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Status] nvarchar(10)  NULL,
    [IsShuttle] bit  NOT NULL,
    [IsInternal] bit  NOT NULL
);
GO

-- Creating table 'crLogTrips'
CREATE TABLE [dbo].[crLogTrips] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [crLogDriverId] int  NOT NULL,
    [crLogUnitId] int  NOT NULL,
    [crLogCompanyId] int  NOT NULL,
    [DtTrip] datetime  NOT NULL,
    [Rate] decimal(18,0)  NOT NULL,
    [Expenses] decimal(18,0)  NOT NULL,
    [DriverFee] decimal(18,0)  NOT NULL,
    [Remarks] nvarchar(50)  NULL,
    [crLogClosingId] int  NULL,
    [Addon] decimal(18,0)  NOT NULL,
    [OdoStart] int  NULL,
    [OdoEnd] int  NULL,
    [DriverOT] decimal(10,2)  NOT NULL,
    [StartTime] nvarchar(10)  NULL,
    [EndTime] nvarchar(10)  NULL,
    [TripHours] int  NULL,
    [OTRate] decimal(18,0)  NULL,
    [DriverOTRate] decimal(18,0)  NULL,
    [AddonOT] decimal(18,0)  NULL,
    [IsFinal] bit  NOT NULL,
    [AllowEdit] bit  NOT NULL,
    [TripTicket] bit  NULL
);
GO

-- Creating table 'crLogCashReleases'
CREATE TABLE [dbo].[crLogCashReleases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtRelease] datetime  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [crLogDriverId] int  NOT NULL,
    [crLogClosingId] int  NULL,
    [crLogCashTypeId] int  NOT NULL
);
GO

-- Creating table 'crLogClosings'
CREATE TABLE [dbo].[crLogClosings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtClose] datetime  NOT NULL
);
GO

-- Creating table 'crLogFuels'
CREATE TABLE [dbo].[crLogFuels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtRequest] datetime  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [crLogUnitId] int  NOT NULL,
    [crLogDriverId] int  NOT NULL,
    [dtFillup] datetime  NOT NULL,
    [odoFillup] int  NOT NULL,
    [orAmount] decimal(18,2)  NOT NULL,
    [crLogTypeId] int  NOT NULL,
    [odoStart] int  NULL,
    [odoEnd] int  NULL,
    [isFullTank] bit  NOT NULL,
    [crLogPaymentTypeId] int  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'crLogOdoes'
CREATE TABLE [dbo].[crLogOdoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [crLogUnitId] int  NOT NULL,
    [crLogDriverId] int  NOT NULL,
    [OdoCurrent] int  NOT NULL,
    [dtReading] datetime  NOT NULL,
    [Remarks] nvarchar(max)  NULL
);
GO

-- Creating table 'crCashReqStatus'
CREATE TABLE [dbo].[crCashReqStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'crLogFuelStatus'
CREATE TABLE [dbo].[crLogFuelStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtStatus] datetime  NOT NULL,
    [crLogFuelId] int  NOT NULL,
    [crCashReqStatusId] int  NOT NULL
);
GO

-- Creating table 'crLogCashStatus'
CREATE TABLE [dbo].[crLogCashStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtStatus] datetime  NOT NULL,
    [crLogCashReleaseId] int  NOT NULL,
    [crCashReqStatusId] int  NOT NULL
);
GO

-- Creating table 'crLogTypes'
CREATE TABLE [dbo].[crLogTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'crRptUnitExpenses'
CREATE TABLE [dbo].[crRptUnitExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RptName] nvarchar(30)  NOT NULL,
    [RptNo] int  NOT NULL,
    [Status] nvarchar(5)  NOT NULL
);
GO

-- Creating table 'CrRptUnits'
CREATE TABLE [dbo].[CrRptUnits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [crRptUnitExpenseId] int  NOT NULL,
    [crLogUnitId] int  NOT NULL,
    [RptSeqNo] int  NOT NULL
);
GO

-- Creating table 'crLogPaymentTypes'
CREATE TABLE [dbo].[crLogPaymentTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'crLogCashTypes'
CREATE TABLE [dbo].[crLogCashTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(10)  NOT NULL,
    [Description] nvarchar(30)  NULL
);
GO

-- Creating table 'crLogPassengers'
CREATE TABLE [dbo].[crLogPassengers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Contact] nvarchar(50)  NULL,
    [PassAddress] nvarchar(180)  NULL,
    [PickupPoint] nvarchar(150)  NOT NULL,
    [PickupTime] nvarchar(10)  NOT NULL,
    [DropPoint] nvarchar(150)  NOT NULL,
    [DropTime] nvarchar(10)  NOT NULL,
    [timeContacted] nvarchar(10)  NOT NULL,
    [timeBoarded] nvarchar(10)  NOT NULL,
    [timeDelivered] nvarchar(10)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [crLogPassStatusId] int  NOT NULL,
    [crLogTripId] int  NOT NULL,
    [Area] nvarchar(150)  NULL,
    [NextDay] bit  NOT NULL,
    [RestDay] nvarchar(80)  NULL
);
GO

-- Creating table 'crLogPassStatus'
CREATE TABLE [dbo].[crLogPassStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'crLogPassengerMasters'
CREATE TABLE [dbo].[crLogPassengerMasters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Contact] nvarchar(50)  NOT NULL,
    [PassAddress] nvarchar(150)  NULL,
    [PickupPoint] nvarchar(150)  NOT NULL,
    [PickupTime] nvarchar(150)  NOT NULL,
    [DropPoint] nvarchar(150)  NOT NULL,
    [DropTime] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [RestDays] nvarchar(150)  NULL,
    [Area] nvarchar(150)  NULL,
    [NextDay] bit  NOT NULL
);
GO

-- Creating table 'crLogPassengerAreas'
CREATE TABLE [dbo].[crLogPassengerAreas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NOT NULL
);
GO

-- Creating table 'crLogPassRemarks'
CREATE TABLE [dbo].[crLogPassRemarks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [RemarksFor] int  NOT NULL
);
GO

-- Creating table 'crLogOwners'
CREATE TABLE [dbo].[crLogOwners] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Mobile] nvarchar(30)  NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'crLogTripJobMains'
CREATE TABLE [dbo].[crLogTripJobMains] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [crLogTripId] int  NOT NULL
);
GO

-- Creating table 'crLogCompanyRates'
CREATE TABLE [dbo].[crLogCompanyRates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TripRate] decimal(18,0)  NOT NULL,
    [OTRate] decimal(18,0)  NOT NULL,
    [TripHours] int  NOT NULL,
    [DriverDailyRate] decimal(18,0)  NOT NULL,
    [DriverOTRate] decimal(18,0)  NOT NULL,
    [crLogCompanyId] int  NOT NULL
);
GO

-- Creating table 'crLogCashGroups'
CREATE TABLE [dbo].[crLogCashGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [crLogCashSalaryId] int  NOT NULL,
    [crLogCashReleaseId] int  NOT NULL
);
GO

-- Creating table 'crLogCashSalaries'
CREATE TABLE [dbo].[crLogCashSalaries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [crLogDriverId] int  NOT NULL,
    [ExcludeOT] bit  NOT NULL
);
GO

-- Creating table 'crLogDriverPayments'
CREATE TABLE [dbo].[crLogDriverPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [crLogDriverId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'crLogDrivers'
ALTER TABLE [dbo].[crLogDrivers]
ADD CONSTRAINT [PK_crLogDrivers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogUnits'
ALTER TABLE [dbo].[crLogUnits]
ADD CONSTRAINT [PK_crLogUnits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCompanies'
ALTER TABLE [dbo].[crLogCompanies]
ADD CONSTRAINT [PK_crLogCompanies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogTrips'
ALTER TABLE [dbo].[crLogTrips]
ADD CONSTRAINT [PK_crLogTrips]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCashReleases'
ALTER TABLE [dbo].[crLogCashReleases]
ADD CONSTRAINT [PK_crLogCashReleases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogClosings'
ALTER TABLE [dbo].[crLogClosings]
ADD CONSTRAINT [PK_crLogClosings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogFuels'
ALTER TABLE [dbo].[crLogFuels]
ADD CONSTRAINT [PK_crLogFuels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogOdoes'
ALTER TABLE [dbo].[crLogOdoes]
ADD CONSTRAINT [PK_crLogOdoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crCashReqStatus'
ALTER TABLE [dbo].[crCashReqStatus]
ADD CONSTRAINT [PK_crCashReqStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogFuelStatus'
ALTER TABLE [dbo].[crLogFuelStatus]
ADD CONSTRAINT [PK_crLogFuelStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCashStatus'
ALTER TABLE [dbo].[crLogCashStatus]
ADD CONSTRAINT [PK_crLogCashStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogTypes'
ALTER TABLE [dbo].[crLogTypes]
ADD CONSTRAINT [PK_crLogTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crRptUnitExpenses'
ALTER TABLE [dbo].[crRptUnitExpenses]
ADD CONSTRAINT [PK_crRptUnitExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CrRptUnits'
ALTER TABLE [dbo].[CrRptUnits]
ADD CONSTRAINT [PK_CrRptUnits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogPaymentTypes'
ALTER TABLE [dbo].[crLogPaymentTypes]
ADD CONSTRAINT [PK_crLogPaymentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCashTypes'
ALTER TABLE [dbo].[crLogCashTypes]
ADD CONSTRAINT [PK_crLogCashTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogPassengers'
ALTER TABLE [dbo].[crLogPassengers]
ADD CONSTRAINT [PK_crLogPassengers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogPassStatus'
ALTER TABLE [dbo].[crLogPassStatus]
ADD CONSTRAINT [PK_crLogPassStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogPassengerMasters'
ALTER TABLE [dbo].[crLogPassengerMasters]
ADD CONSTRAINT [PK_crLogPassengerMasters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogPassengerAreas'
ALTER TABLE [dbo].[crLogPassengerAreas]
ADD CONSTRAINT [PK_crLogPassengerAreas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogPassRemarks'
ALTER TABLE [dbo].[crLogPassRemarks]
ADD CONSTRAINT [PK_crLogPassRemarks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogOwners'
ALTER TABLE [dbo].[crLogOwners]
ADD CONSTRAINT [PK_crLogOwners]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogTripJobMains'
ALTER TABLE [dbo].[crLogTripJobMains]
ADD CONSTRAINT [PK_crLogTripJobMains]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCompanyRates'
ALTER TABLE [dbo].[crLogCompanyRates]
ADD CONSTRAINT [PK_crLogCompanyRates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCashGroups'
ALTER TABLE [dbo].[crLogCashGroups]
ADD CONSTRAINT [PK_crLogCashGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogCashSalaries'
ALTER TABLE [dbo].[crLogCashSalaries]
ADD CONSTRAINT [PK_crLogCashSalaries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'crLogDriverPayments'
ALTER TABLE [dbo].[crLogDriverPayments]
ADD CONSTRAINT [PK_crLogDriverPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [crLogDriverId] in table 'crLogTrips'
ALTER TABLE [dbo].[crLogTrips]
ADD CONSTRAINT [FK_crLogDrivercrLogTrip]
    FOREIGN KEY ([crLogDriverId])
    REFERENCES [dbo].[crLogDrivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogDrivercrLogTrip'
CREATE INDEX [IX_FK_crLogDrivercrLogTrip]
ON [dbo].[crLogTrips]
    ([crLogDriverId]);
GO

-- Creating foreign key on [crLogUnitId] in table 'crLogTrips'
ALTER TABLE [dbo].[crLogTrips]
ADD CONSTRAINT [FK_crLogUnitcrLogTrip]
    FOREIGN KEY ([crLogUnitId])
    REFERENCES [dbo].[crLogUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogUnitcrLogTrip'
CREATE INDEX [IX_FK_crLogUnitcrLogTrip]
ON [dbo].[crLogTrips]
    ([crLogUnitId]);
GO

-- Creating foreign key on [crLogCompanyId] in table 'crLogTrips'
ALTER TABLE [dbo].[crLogTrips]
ADD CONSTRAINT [FK_crLogCompanycrLogTrip]
    FOREIGN KEY ([crLogCompanyId])
    REFERENCES [dbo].[crLogCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogCompanycrLogTrip'
CREATE INDEX [IX_FK_crLogCompanycrLogTrip]
ON [dbo].[crLogTrips]
    ([crLogCompanyId]);
GO

-- Creating foreign key on [crLogDriverId] in table 'crLogCashReleases'
ALTER TABLE [dbo].[crLogCashReleases]
ADD CONSTRAINT [FK_crLogDrivercrLogCashRelease]
    FOREIGN KEY ([crLogDriverId])
    REFERENCES [dbo].[crLogDrivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogDrivercrLogCashRelease'
CREATE INDEX [IX_FK_crLogDrivercrLogCashRelease]
ON [dbo].[crLogCashReleases]
    ([crLogDriverId]);
GO

-- Creating foreign key on [crLogClosingId] in table 'crLogCashReleases'
ALTER TABLE [dbo].[crLogCashReleases]
ADD CONSTRAINT [FK_crLogClosingcrLogCashRelease]
    FOREIGN KEY ([crLogClosingId])
    REFERENCES [dbo].[crLogClosings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogClosingcrLogCashRelease'
CREATE INDEX [IX_FK_crLogClosingcrLogCashRelease]
ON [dbo].[crLogCashReleases]
    ([crLogClosingId]);
GO

-- Creating foreign key on [crLogClosingId] in table 'crLogTrips'
ALTER TABLE [dbo].[crLogTrips]
ADD CONSTRAINT [FK_crLogClosingcrLogTrip]
    FOREIGN KEY ([crLogClosingId])
    REFERENCES [dbo].[crLogClosings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogClosingcrLogTrip'
CREATE INDEX [IX_FK_crLogClosingcrLogTrip]
ON [dbo].[crLogTrips]
    ([crLogClosingId]);
GO

-- Creating foreign key on [crLogUnitId] in table 'crLogFuels'
ALTER TABLE [dbo].[crLogFuels]
ADD CONSTRAINT [FK_crLogUnitcrLogFuel]
    FOREIGN KEY ([crLogUnitId])
    REFERENCES [dbo].[crLogUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogUnitcrLogFuel'
CREATE INDEX [IX_FK_crLogUnitcrLogFuel]
ON [dbo].[crLogFuels]
    ([crLogUnitId]);
GO

-- Creating foreign key on [crLogDriverId] in table 'crLogFuels'
ALTER TABLE [dbo].[crLogFuels]
ADD CONSTRAINT [FK_crLogDrivercrLogFuel]
    FOREIGN KEY ([crLogDriverId])
    REFERENCES [dbo].[crLogDrivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogDrivercrLogFuel'
CREATE INDEX [IX_FK_crLogDrivercrLogFuel]
ON [dbo].[crLogFuels]
    ([crLogDriverId]);
GO

-- Creating foreign key on [crLogUnitId] in table 'crLogOdoes'
ALTER TABLE [dbo].[crLogOdoes]
ADD CONSTRAINT [FK_crLogUnitcrLogOdo]
    FOREIGN KEY ([crLogUnitId])
    REFERENCES [dbo].[crLogUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogUnitcrLogOdo'
CREATE INDEX [IX_FK_crLogUnitcrLogOdo]
ON [dbo].[crLogOdoes]
    ([crLogUnitId]);
GO

-- Creating foreign key on [crLogDriverId] in table 'crLogOdoes'
ALTER TABLE [dbo].[crLogOdoes]
ADD CONSTRAINT [FK_crLogDrivercrLogOdo]
    FOREIGN KEY ([crLogDriverId])
    REFERENCES [dbo].[crLogDrivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogDrivercrLogOdo'
CREATE INDEX [IX_FK_crLogDrivercrLogOdo]
ON [dbo].[crLogOdoes]
    ([crLogDriverId]);
GO

-- Creating foreign key on [crLogFuelId] in table 'crLogFuelStatus'
ALTER TABLE [dbo].[crLogFuelStatus]
ADD CONSTRAINT [FK_crLogFuelcrLogFuelStatus]
    FOREIGN KEY ([crLogFuelId])
    REFERENCES [dbo].[crLogFuels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogFuelcrLogFuelStatus'
CREATE INDEX [IX_FK_crLogFuelcrLogFuelStatus]
ON [dbo].[crLogFuelStatus]
    ([crLogFuelId]);
GO

-- Creating foreign key on [crCashReqStatusId] in table 'crLogFuelStatus'
ALTER TABLE [dbo].[crLogFuelStatus]
ADD CONSTRAINT [FK_crCashReqStatuscrLogFuelStatus]
    FOREIGN KEY ([crCashReqStatusId])
    REFERENCES [dbo].[crCashReqStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crCashReqStatuscrLogFuelStatus'
CREATE INDEX [IX_FK_crCashReqStatuscrLogFuelStatus]
ON [dbo].[crLogFuelStatus]
    ([crCashReqStatusId]);
GO

-- Creating foreign key on [crLogCashReleaseId] in table 'crLogCashStatus'
ALTER TABLE [dbo].[crLogCashStatus]
ADD CONSTRAINT [FK_crLogCashReleasecrLogCashStatus]
    FOREIGN KEY ([crLogCashReleaseId])
    REFERENCES [dbo].[crLogCashReleases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogCashReleasecrLogCashStatus'
CREATE INDEX [IX_FK_crLogCashReleasecrLogCashStatus]
ON [dbo].[crLogCashStatus]
    ([crLogCashReleaseId]);
GO

-- Creating foreign key on [crCashReqStatusId] in table 'crLogCashStatus'
ALTER TABLE [dbo].[crLogCashStatus]
ADD CONSTRAINT [FK_crCashReqStatuscrLogCashStatus]
    FOREIGN KEY ([crCashReqStatusId])
    REFERENCES [dbo].[crCashReqStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crCashReqStatuscrLogCashStatus'
CREATE INDEX [IX_FK_crCashReqStatuscrLogCashStatus]
ON [dbo].[crLogCashStatus]
    ([crCashReqStatusId]);
GO

-- Creating foreign key on [crLogTypeId] in table 'crLogFuels'
ALTER TABLE [dbo].[crLogFuels]
ADD CONSTRAINT [FK_crLogTypecrLogFuel]
    FOREIGN KEY ([crLogTypeId])
    REFERENCES [dbo].[crLogTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogTypecrLogFuel'
CREATE INDEX [IX_FK_crLogTypecrLogFuel]
ON [dbo].[crLogFuels]
    ([crLogTypeId]);
GO

-- Creating foreign key on [crRptUnitExpenseId] in table 'CrRptUnits'
ALTER TABLE [dbo].[CrRptUnits]
ADD CONSTRAINT [FK_crRptUnitExpenseCrRptUnit]
    FOREIGN KEY ([crRptUnitExpenseId])
    REFERENCES [dbo].[crRptUnitExpenses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crRptUnitExpenseCrRptUnit'
CREATE INDEX [IX_FK_crRptUnitExpenseCrRptUnit]
ON [dbo].[CrRptUnits]
    ([crRptUnitExpenseId]);
GO

-- Creating foreign key on [crLogUnitId] in table 'CrRptUnits'
ALTER TABLE [dbo].[CrRptUnits]
ADD CONSTRAINT [FK_crLogUnitCrRptUnit]
    FOREIGN KEY ([crLogUnitId])
    REFERENCES [dbo].[crLogUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogUnitCrRptUnit'
CREATE INDEX [IX_FK_crLogUnitCrRptUnit]
ON [dbo].[CrRptUnits]
    ([crLogUnitId]);
GO

-- Creating foreign key on [crLogPaymentTypeId] in table 'crLogFuels'
ALTER TABLE [dbo].[crLogFuels]
ADD CONSTRAINT [FK_crLogPaymentTypecrLogFuel]
    FOREIGN KEY ([crLogPaymentTypeId])
    REFERENCES [dbo].[crLogPaymentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogPaymentTypecrLogFuel'
CREATE INDEX [IX_FK_crLogPaymentTypecrLogFuel]
ON [dbo].[crLogFuels]
    ([crLogPaymentTypeId]);
GO

-- Creating foreign key on [crLogCashTypeId] in table 'crLogCashReleases'
ALTER TABLE [dbo].[crLogCashReleases]
ADD CONSTRAINT [FK_crLogCashTypecrLogCashRelease]
    FOREIGN KEY ([crLogCashTypeId])
    REFERENCES [dbo].[crLogCashTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogCashTypecrLogCashRelease'
CREATE INDEX [IX_FK_crLogCashTypecrLogCashRelease]
ON [dbo].[crLogCashReleases]
    ([crLogCashTypeId]);
GO

-- Creating foreign key on [crLogPassStatusId] in table 'crLogPassengers'
ALTER TABLE [dbo].[crLogPassengers]
ADD CONSTRAINT [FK_crLogPassStatuscrLogPassenger]
    FOREIGN KEY ([crLogPassStatusId])
    REFERENCES [dbo].[crLogPassStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogPassStatuscrLogPassenger'
CREATE INDEX [IX_FK_crLogPassStatuscrLogPassenger]
ON [dbo].[crLogPassengers]
    ([crLogPassStatusId]);
GO

-- Creating foreign key on [crLogTripId] in table 'crLogPassengers'
ALTER TABLE [dbo].[crLogPassengers]
ADD CONSTRAINT [FK_crLogTripcrLogPassenger]
    FOREIGN KEY ([crLogTripId])
    REFERENCES [dbo].[crLogTrips]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogTripcrLogPassenger'
CREATE INDEX [IX_FK_crLogTripcrLogPassenger]
ON [dbo].[crLogPassengers]
    ([crLogTripId]);
GO

-- Creating foreign key on [crLogOwnerId] in table 'crLogUnits'
ALTER TABLE [dbo].[crLogUnits]
ADD CONSTRAINT [FK_crLogOwnercrLogUnit]
    FOREIGN KEY ([crLogOwnerId])
    REFERENCES [dbo].[crLogOwners]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogOwnercrLogUnit'
CREATE INDEX [IX_FK_crLogOwnercrLogUnit]
ON [dbo].[crLogUnits]
    ([crLogOwnerId]);
GO

-- Creating foreign key on [crLogTripId] in table 'crLogTripJobMains'
ALTER TABLE [dbo].[crLogTripJobMains]
ADD CONSTRAINT [FK_crLogTripcrLogTripJobMain]
    FOREIGN KEY ([crLogTripId])
    REFERENCES [dbo].[crLogTrips]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogTripcrLogTripJobMain'
CREATE INDEX [IX_FK_crLogTripcrLogTripJobMain]
ON [dbo].[crLogTripJobMains]
    ([crLogTripId]);
GO

-- Creating foreign key on [crLogCompanyId] in table 'crLogCompanyRates'
ALTER TABLE [dbo].[crLogCompanyRates]
ADD CONSTRAINT [FK_crLogCompanycrLogCompanyRate]
    FOREIGN KEY ([crLogCompanyId])
    REFERENCES [dbo].[crLogCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogCompanycrLogCompanyRate'
CREATE INDEX [IX_FK_crLogCompanycrLogCompanyRate]
ON [dbo].[crLogCompanyRates]
    ([crLogCompanyId]);
GO

-- Creating foreign key on [crLogDriverId] in table 'crLogCashSalaries'
ALTER TABLE [dbo].[crLogCashSalaries]
ADD CONSTRAINT [FK_crLogDrivercrLogCashSalary]
    FOREIGN KEY ([crLogDriverId])
    REFERENCES [dbo].[crLogDrivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogDrivercrLogCashSalary'
CREATE INDEX [IX_FK_crLogDrivercrLogCashSalary]
ON [dbo].[crLogCashSalaries]
    ([crLogDriverId]);
GO

-- Creating foreign key on [crLogCashSalaryId] in table 'crLogCashGroups'
ALTER TABLE [dbo].[crLogCashGroups]
ADD CONSTRAINT [FK_crLogCashGroupcrLogCashSalary]
    FOREIGN KEY ([crLogCashSalaryId])
    REFERENCES [dbo].[crLogCashSalaries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogCashGroupcrLogCashSalary'
CREATE INDEX [IX_FK_crLogCashGroupcrLogCashSalary]
ON [dbo].[crLogCashGroups]
    ([crLogCashSalaryId]);
GO

-- Creating foreign key on [crLogCashReleaseId] in table 'crLogCashGroups'
ALTER TABLE [dbo].[crLogCashGroups]
ADD CONSTRAINT [FK_crLogCashReleasecrLogCashGroup]
    FOREIGN KEY ([crLogCashReleaseId])
    REFERENCES [dbo].[crLogCashReleases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogCashReleasecrLogCashGroup'
CREATE INDEX [IX_FK_crLogCashReleasecrLogCashGroup]
ON [dbo].[crLogCashGroups]
    ([crLogCashReleaseId]);
GO

-- Creating foreign key on [crLogDriverId] in table 'crLogDriverPayments'
ALTER TABLE [dbo].[crLogDriverPayments]
ADD CONSTRAINT [FK_crLogDrivercrLogDriverPayment]
    FOREIGN KEY ([crLogDriverId])
    REFERENCES [dbo].[crLogDrivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_crLogDrivercrLogDriverPayment'
CREATE INDEX [IX_FK_crLogDrivercrLogDriverPayment]
ON [dbo].[crLogDriverPayments]
    ([crLogDriverId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------