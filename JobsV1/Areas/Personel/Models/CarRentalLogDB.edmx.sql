
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/21/2020 16:58:19
-- Generated from EDMX file: C:\Users\VILLOSA\Documents\GitHub\eJob20\JobsV1\Areas\Personel\Models\CarRentalLogDB.edmx
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
    ALTER TABLE [dbo].[crLogOdoes1] DROP CONSTRAINT [FK_crLogUnitcrLogOdo];
GO
IF OBJECT_ID(N'[dbo].[FK_crLogDrivercrLogOdo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[crLogOdoes1] DROP CONSTRAINT [FK_crLogDrivercrLogOdo];
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
IF OBJECT_ID(N'[dbo].[crLogOdoes1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[crLogOdoes1];
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

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'crLogDrivers'
CREATE TABLE [dbo].[crLogDrivers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'crLogUnits'
CREATE TABLE [dbo].[crLogUnits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'crLogCompanies'
CREATE TABLE [dbo].[crLogCompanies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
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
    [Addon] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'crLogCashReleases'
CREATE TABLE [dbo].[crLogCashReleases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtRelease] datetime  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Remarks] nvarchar(30)  NULL,
    [crLogDriverId] int  NOT NULL,
    [crLogClosingId] int  NULL
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
    [Amount] decimal(18,0)  NOT NULL,
    [crLogUnitId] int  NOT NULL,
    [crLogDriverId] int  NOT NULL,
    [dtFillup] datetime  NOT NULL,
    [odoFillup] int  NOT NULL,
    [orAmount] decimal(18,0)  NOT NULL
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------