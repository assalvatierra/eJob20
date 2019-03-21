
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/21/2019 16:34:36
-- Generated from EDMX file: D:\Data\Real\Apps\GitHub\eJob20\JobsV1\Areas\Accounting\Models\AccountingDB.edmx
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
IF OBJECT_ID(N'[dbo].[FK_AccntTrxTypeAccntTrxHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntTrxHdrs] DROP CONSTRAINT [FK_AccntTrxTypeAccntTrxHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntTrxHdrAccntTrxDtl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntTrxDtls] DROP CONSTRAINT [FK_AccntTrxHdrAccntTrxDtl];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntLedgerAccntTrxDtl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntTrxDtls] DROP CONSTRAINT [FK_AccntLedgerAccntTrxDtl];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntTypeAccntCOA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntMains] DROP CONSTRAINT [FK_AccntTypeAccntCOA];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntMainAccntLedger]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntLedgers] DROP CONSTRAINT [FK_AccntMainAccntLedger];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntTrxDtlAccntTrxHist]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntTrxHists] DROP CONSTRAINT [FK_AccntTrxDtlAccntTrxHist];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntTypeAccntChart]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntCategories] DROP CONSTRAINT [FK_AccntTypeAccntChart];
GO
IF OBJECT_ID(N'[dbo].[FK_AccntCategoryAccntMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccntMains] DROP CONSTRAINT [FK_AccntCategoryAccntMain];
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
IF OBJECT_ID(N'[dbo].[AccntMains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntMains];
GO
IF OBJECT_ID(N'[dbo].[AccntTrxHdrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntTrxHdrs];
GO
IF OBJECT_ID(N'[dbo].[AccntTrxDtls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntTrxDtls];
GO
IF OBJECT_ID(N'[dbo].[AccntTrxTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntTrxTypes];
GO
IF OBJECT_ID(N'[dbo].[AccntLedgers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntLedgers];
GO
IF OBJECT_ID(N'[dbo].[AccntTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntTypes];
GO
IF OBJECT_ID(N'[dbo].[AccntTrxHists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntTrxHists];
GO
IF OBJECT_ID(N'[dbo].[AccntCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccntCategories];
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

-- Creating table 'AccntMains'
CREATE TABLE [dbo].[AccntMains] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(5)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(200)  NULL,
    [AccntTypeId] int  NOT NULL,
    [AccntCategoryId] int  NOT NULL
);
GO

-- Creating table 'AccntTrxHdrs'
CREATE TABLE [dbo].[AccntTrxHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccntTrxTypeId] int  NOT NULL,
    [DtTrx] datetime  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'AccntTrxDtls'
CREATE TABLE [dbo].[AccntTrxDtls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccntTrxHdrId] int  NOT NULL,
    [AccntLedgerId] int  NOT NULL,
    [Remarks] nvarchar(50)  NULL,
    [DbAmt] decimal(18,0)  NOT NULL,
    [CrAmt] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'AccntTrxTypes'
CREATE TABLE [dbo].[AccntTrxTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(3)  NOT NULL,
    [Remarks] nvarchar(30)  NULL
);
GO

-- Creating table 'AccntLedgers'
CREATE TABLE [dbo].[AccntLedgers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(4)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [AccntMainId] int  NOT NULL,
    [Remarks] nvarchar(200)  NULL
);
GO

-- Creating table 'AccntTypes'
CREATE TABLE [dbo].[AccntTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(5)  NOT NULL,
    [NormalForm] nvarchar(2)  NOT NULL
);
GO

-- Creating table 'AccntTrxHists'
CREATE TABLE [dbo].[AccntTrxHists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtHist] nvarchar(max)  NOT NULL,
    [AccntTrxDtlId] int  NOT NULL,
    [HistType] nvarchar(20)  NOT NULL,
    [OldData] nvarchar(250)  NOT NULL,
    [User] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'AccntCategories'
CREATE TABLE [dbo].[AccntCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(5)  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [OrderNo] int  NOT NULL,
    [AccntTypeId] int  NOT NULL
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

-- Creating primary key on [Id] in table 'AccntMains'
ALTER TABLE [dbo].[AccntMains]
ADD CONSTRAINT [PK_AccntMains]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntTrxHdrs'
ALTER TABLE [dbo].[AccntTrxHdrs]
ADD CONSTRAINT [PK_AccntTrxHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntTrxDtls'
ALTER TABLE [dbo].[AccntTrxDtls]
ADD CONSTRAINT [PK_AccntTrxDtls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntTrxTypes'
ALTER TABLE [dbo].[AccntTrxTypes]
ADD CONSTRAINT [PK_AccntTrxTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntLedgers'
ALTER TABLE [dbo].[AccntLedgers]
ADD CONSTRAINT [PK_AccntLedgers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntTypes'
ALTER TABLE [dbo].[AccntTypes]
ADD CONSTRAINT [PK_AccntTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntTrxHists'
ALTER TABLE [dbo].[AccntTrxHists]
ADD CONSTRAINT [PK_AccntTrxHists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccntCategories'
ALTER TABLE [dbo].[AccntCategories]
ADD CONSTRAINT [PK_AccntCategories]
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

-- Creating foreign key on [AccntTrxTypeId] in table 'AccntTrxHdrs'
ALTER TABLE [dbo].[AccntTrxHdrs]
ADD CONSTRAINT [FK_AccntTrxTypeAccntTrxHdr]
    FOREIGN KEY ([AccntTrxTypeId])
    REFERENCES [dbo].[AccntTrxTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntTrxTypeAccntTrxHdr'
CREATE INDEX [IX_FK_AccntTrxTypeAccntTrxHdr]
ON [dbo].[AccntTrxHdrs]
    ([AccntTrxTypeId]);
GO

-- Creating foreign key on [AccntTrxHdrId] in table 'AccntTrxDtls'
ALTER TABLE [dbo].[AccntTrxDtls]
ADD CONSTRAINT [FK_AccntTrxHdrAccntTrxDtl]
    FOREIGN KEY ([AccntTrxHdrId])
    REFERENCES [dbo].[AccntTrxHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntTrxHdrAccntTrxDtl'
CREATE INDEX [IX_FK_AccntTrxHdrAccntTrxDtl]
ON [dbo].[AccntTrxDtls]
    ([AccntTrxHdrId]);
GO

-- Creating foreign key on [AccntLedgerId] in table 'AccntTrxDtls'
ALTER TABLE [dbo].[AccntTrxDtls]
ADD CONSTRAINT [FK_AccntLedgerAccntTrxDtl]
    FOREIGN KEY ([AccntLedgerId])
    REFERENCES [dbo].[AccntLedgers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntLedgerAccntTrxDtl'
CREATE INDEX [IX_FK_AccntLedgerAccntTrxDtl]
ON [dbo].[AccntTrxDtls]
    ([AccntLedgerId]);
GO

-- Creating foreign key on [AccntTypeId] in table 'AccntMains'
ALTER TABLE [dbo].[AccntMains]
ADD CONSTRAINT [FK_AccntTypeAccntCOA]
    FOREIGN KEY ([AccntTypeId])
    REFERENCES [dbo].[AccntTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntTypeAccntCOA'
CREATE INDEX [IX_FK_AccntTypeAccntCOA]
ON [dbo].[AccntMains]
    ([AccntTypeId]);
GO

-- Creating foreign key on [AccntMainId] in table 'AccntLedgers'
ALTER TABLE [dbo].[AccntLedgers]
ADD CONSTRAINT [FK_AccntMainAccntLedger]
    FOREIGN KEY ([AccntMainId])
    REFERENCES [dbo].[AccntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntMainAccntLedger'
CREATE INDEX [IX_FK_AccntMainAccntLedger]
ON [dbo].[AccntLedgers]
    ([AccntMainId]);
GO

-- Creating foreign key on [AccntTrxDtlId] in table 'AccntTrxHists'
ALTER TABLE [dbo].[AccntTrxHists]
ADD CONSTRAINT [FK_AccntTrxDtlAccntTrxHist]
    FOREIGN KEY ([AccntTrxDtlId])
    REFERENCES [dbo].[AccntTrxDtls]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntTrxDtlAccntTrxHist'
CREATE INDEX [IX_FK_AccntTrxDtlAccntTrxHist]
ON [dbo].[AccntTrxHists]
    ([AccntTrxDtlId]);
GO

-- Creating foreign key on [AccntTypeId] in table 'AccntCategories'
ALTER TABLE [dbo].[AccntCategories]
ADD CONSTRAINT [FK_AccntTypeAccntChart]
    FOREIGN KEY ([AccntTypeId])
    REFERENCES [dbo].[AccntTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntTypeAccntChart'
CREATE INDEX [IX_FK_AccntTypeAccntChart]
ON [dbo].[AccntCategories]
    ([AccntTypeId]);
GO

-- Creating foreign key on [AccntCategoryId] in table 'AccntMains'
ALTER TABLE [dbo].[AccntMains]
ADD CONSTRAINT [FK_AccntCategoryAccntMain]
    FOREIGN KEY ([AccntCategoryId])
    REFERENCES [dbo].[AccntCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccntCategoryAccntMain'
CREATE INDEX [IX_FK_AccntCategoryAccntMain]
ON [dbo].[AccntMains]
    ([AccntCategoryId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------