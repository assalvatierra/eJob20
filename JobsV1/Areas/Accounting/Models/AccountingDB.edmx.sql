
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/13/2016 16:32:07
-- Generated from EDMX file: D:\Data\Real\Apps\JobsV1\JobsV1\Areas\Accounting\Models\AccountingDB.edmx
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

IF OBJECT_ID(N'[dbo].[FK_AsCostCenterAsExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsExpenses] DROP CONSTRAINT [FK_AsCostCenterAsExpense];
GO
IF OBJECT_ID(N'[dbo].[FK_AsExpCategoryAsExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsExpenses] DROP CONSTRAINT [FK_AsExpCategoryAsExpense];
GO
IF OBJECT_ID(N'[dbo].[FK_AsExpBillerAsExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsExpenses] DROP CONSTRAINT [FK_AsExpBillerAsExpense];
GO
IF OBJECT_ID(N'[dbo].[FK_AsCostCenterAsSales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsSales] DROP CONSTRAINT [FK_AsCostCenterAsSales];
GO
IF OBJECT_ID(N'[dbo].[FK_AsIncCategoryAsSales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsSales] DROP CONSTRAINT [FK_AsIncCategoryAsSales];
GO
IF OBJECT_ID(N'[dbo].[FK_AsIncClientAsSales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsSales] DROP CONSTRAINT [FK_AsIncClientAsSales];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AsExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsExpenses];
GO
IF OBJECT_ID(N'[dbo].[AsExpCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsExpCategories];
GO
IF OBJECT_ID(N'[dbo].[AsExpBillers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsExpBillers];
GO
IF OBJECT_ID(N'[dbo].[AsCostCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsCostCenters];
GO
IF OBJECT_ID(N'[dbo].[AsIncCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsIncCategories];
GO
IF OBJECT_ID(N'[dbo].[AsIncClients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsIncClients];
GO
IF OBJECT_ID(N'[dbo].[AsSales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsSales];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AsExpenses'
CREATE TABLE [dbo].[AsExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TrxDate] datetime  NOT NULL,
    [TrxDesc] nvarchar(80)  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [TrxRemarks] nvarchar(250)  NULL,
    [DateEntered] datetime  NOT NULL,
    [AsCostCenterId] int  NOT NULL,
    [AsExpCategoryId] int  NOT NULL,
    [AsExpBillerId] int  NOT NULL
);
GO

-- Creating table 'AsExpCategories'
CREATE TABLE [dbo].[AsExpCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'AsExpBillers'
CREATE TABLE [dbo].[AsExpBillers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ShortName] nvarchar(50)  NOT NULL,
    [FullName] nvarchar(80)  NULL,
    [Address] nvarchar(80)  NULL,
    [Contact] nvarchar(80)  NULL,
    [Contact2] nvarchar(80)  NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'AsCostCenters'
CREATE TABLE [dbo].[AsCostCenters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ccName] nvarchar(80)  NOT NULL,
    [xxRemarks] nvarchar(250)  NULL
);
GO

-- Creating table 'AsIncCategories'
CREATE TABLE [dbo].[AsIncCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'AsIncClients'
CREATE TABLE [dbo].[AsIncClients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ShortName] nvarchar(50)  NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Company] nvarchar(80)  NULL,
    [Contact1] nvarchar(80)  NULL,
    [Contact2] nvarchar(80)  NULL,
    [Address] nvarchar(250)  NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'AsSales'
CREATE TABLE [dbo].[AsSales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TrxDate] datetime  NOT NULL,
    [TrxDesc] nvarchar(80)  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [DateEntered] datetime  NULL,
    [AsCostCenterId] int  NOT NULL,
    [AsIncCategoryId] int  NOT NULL,
    [AsIncClientId] int  NOT NULL,
    [OrRef] nvarchar(20)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AsExpenses'
ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [PK_AsExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsExpCategories'
ALTER TABLE [dbo].[AsExpCategories]
ADD CONSTRAINT [PK_AsExpCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsExpBillers'
ALTER TABLE [dbo].[AsExpBillers]
ADD CONSTRAINT [PK_AsExpBillers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsCostCenters'
ALTER TABLE [dbo].[AsCostCenters]
ADD CONSTRAINT [PK_AsCostCenters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsIncCategories'
ALTER TABLE [dbo].[AsIncCategories]
ADD CONSTRAINT [PK_AsIncCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsIncClients'
ALTER TABLE [dbo].[AsIncClients]
ADD CONSTRAINT [PK_AsIncClients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsSales'
ALTER TABLE [dbo].[AsSales]
ADD CONSTRAINT [PK_AsSales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AsCostCenterId] in table 'AsExpenses'
ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [FK_AsCostCenterAsExpense]
    FOREIGN KEY ([AsCostCenterId])
    REFERENCES [dbo].[AsCostCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AsCostCenterAsExpense'
CREATE INDEX [IX_FK_AsCostCenterAsExpense]
ON [dbo].[AsExpenses]
    ([AsCostCenterId]);
GO

-- Creating foreign key on [AsExpCategoryId] in table 'AsExpenses'
ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [FK_AsExpCategoryAsExpense]
    FOREIGN KEY ([AsExpCategoryId])
    REFERENCES [dbo].[AsExpCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AsExpCategoryAsExpense'
CREATE INDEX [IX_FK_AsExpCategoryAsExpense]
ON [dbo].[AsExpenses]
    ([AsExpCategoryId]);
GO

-- Creating foreign key on [AsExpBillerId] in table 'AsExpenses'
ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [FK_AsExpBillerAsExpense]
    FOREIGN KEY ([AsExpBillerId])
    REFERENCES [dbo].[AsExpBillers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AsExpBillerAsExpense'
CREATE INDEX [IX_FK_AsExpBillerAsExpense]
ON [dbo].[AsExpenses]
    ([AsExpBillerId]);
GO

-- Creating foreign key on [AsCostCenterId] in table 'AsSales'
ALTER TABLE [dbo].[AsSales]
ADD CONSTRAINT [FK_AsCostCenterAsSales]
    FOREIGN KEY ([AsCostCenterId])
    REFERENCES [dbo].[AsCostCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AsCostCenterAsSales'
CREATE INDEX [IX_FK_AsCostCenterAsSales]
ON [dbo].[AsSales]
    ([AsCostCenterId]);
GO

-- Creating foreign key on [AsIncCategoryId] in table 'AsSales'
ALTER TABLE [dbo].[AsSales]
ADD CONSTRAINT [FK_AsIncCategoryAsSales]
    FOREIGN KEY ([AsIncCategoryId])
    REFERENCES [dbo].[AsIncCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AsIncCategoryAsSales'
CREATE INDEX [IX_FK_AsIncCategoryAsSales]
ON [dbo].[AsSales]
    ([AsIncCategoryId]);
GO

-- Creating foreign key on [AsIncClientId] in table 'AsSales'
ALTER TABLE [dbo].[AsSales]
ADD CONSTRAINT [FK_AsIncClientAsSales]
    FOREIGN KEY ([AsIncClientId])
    REFERENCES [dbo].[AsIncClients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AsIncClientAsSales'
CREATE INDEX [IX_FK_AsIncClientAsSales]
ON [dbo].[AsSales]
    ([AsIncClientId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------