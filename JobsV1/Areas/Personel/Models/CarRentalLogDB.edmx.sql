
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/21/2020 20:28:32
-- Generated from EDMX file: D:\Projects\eJob20\JobsV1\Areas\Personel\Models\CarRentalLogDB.edmx
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------