
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/31/2019 11:34:45
-- Generated from EDMX file: C:\Users\VILLOSA\Documents\GithubClassic\eJob20\JobsV1\Areas\Products\Models\ProdDB.edmx
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

IF OBJECT_ID(N'[dbo].[FK_SmProdStatusSmProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProducts] DROP CONSTRAINT [FK_SmProdStatusSmProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmProdDesc]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdDescs] DROP CONSTRAINT [FK_SmProductSmProdDesc];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmProdInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdInfoes] DROP CONSTRAINT [FK_SmProductSmProdInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_SmBranchSmProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProducts] DROP CONSTRAINT [FK_SmBranchSmProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_SmSupplierSmProdSupplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdSuppliers] DROP CONSTRAINT [FK_SmSupplierSmProdSupplier];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmProdSupplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdSuppliers] DROP CONSTRAINT [FK_SmProductSmProdSupplier];
GO
IF OBJECT_ID(N'[dbo].[FK_SmSupplierSmSupplierInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmSupplierInfoes] DROP CONSTRAINT [FK_SmSupplierSmSupplierInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_SmCategorySmProdCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdCats] DROP CONSTRAINT [FK_SmCategorySmProdCat];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmProdCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdCats] DROP CONSTRAINT [FK_SmProductSmProdCat];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmRates] DROP CONSTRAINT [FK_SmProductSmRate];
GO
IF OBJECT_ID(N'[dbo].[FK_SmRateSmRateUoM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmRates] DROP CONSTRAINT [FK_SmRateSmRateUoM];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmFiles] DROP CONSTRAINT [FK_SmProductSmFile];
GO
IF OBJECT_ID(N'[dbo].[FK_SmCategorySmProdAds]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdAds] DROP CONSTRAINT [FK_SmCategorySmProdAds];
GO
IF OBJECT_ID(N'[dbo].[FK_SmProductSmProdAds]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SmProdAds] DROP CONSTRAINT [FK_SmProductSmProdAds];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SmProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProducts];
GO
IF OBJECT_ID(N'[dbo].[SmProdDescs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProdDescs];
GO
IF OBJECT_ID(N'[dbo].[SmProdStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProdStatus];
GO
IF OBJECT_ID(N'[dbo].[SmBranches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmBranches];
GO
IF OBJECT_ID(N'[dbo].[SmSuppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmSuppliers];
GO
IF OBJECT_ID(N'[dbo].[SmProdSuppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProdSuppliers];
GO
IF OBJECT_ID(N'[dbo].[SmSupplierInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmSupplierInfoes];
GO
IF OBJECT_ID(N'[dbo].[SmProdInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProdInfoes];
GO
IF OBJECT_ID(N'[dbo].[SmCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmCategories];
GO
IF OBJECT_ID(N'[dbo].[SmProdCats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProdCats];
GO
IF OBJECT_ID(N'[dbo].[SmRates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmRates];
GO
IF OBJECT_ID(N'[dbo].[SmRateUoMs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmRateUoMs];
GO
IF OBJECT_ID(N'[dbo].[SmFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmFiles];
GO
IF OBJECT_ID(N'[dbo].[SmProdAds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SmProdAds];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SmProducts'
CREATE TABLE [dbo].[SmProducts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SmBranchId] int  NOT NULL,
    [Code] nvarchar(10)  NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [BranchId] int  NOT NULL,
    [ProdStatusId] int  NOT NULL,
    [ValidStart] datetime  NOT NULL,
    [ValidEnd] datetime  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Contracted] decimal(18,0)  NOT NULL,
    [SmProdStatusId] int  NOT NULL
);
GO

-- Creating table 'SmProdDescs'
CREATE TABLE [dbo].[SmProdDescs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SmProductId] int  NOT NULL,
    [SortNo] int  NOT NULL,
    [Description] nvarchar(180)  NULL,
    [SmProductId1] int  NOT NULL
);
GO

-- Creating table 'SmProdStatus'
CREATE TABLE [dbo].[SmProdStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'SmBranches'
CREATE TABLE [dbo].[SmBranches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'SmSuppliers'
CREATE TABLE [dbo].[SmSuppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'SmProdSuppliers'
CREATE TABLE [dbo].[SmProdSuppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ValidStart] datetime  NOT NULL,
    [ValidEnd] datetime  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Contracted] decimal(18,0)  NOT NULL,
    [SmSupplierId] int  NOT NULL,
    [SmProductId] int  NOT NULL
);
GO

-- Creating table 'SmSupplierInfoes'
CREATE TABLE [dbo].[SmSupplierInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SmSupplierId] int  NOT NULL,
    [Label] nvarchar(10)  NOT NULL,
    [Value] nvarchar(80)  NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'SmProdInfoes'
CREATE TABLE [dbo].[SmProdInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SmProductId] int  NOT NULL,
    [Label] nvarchar(10)  NOT NULL,
    [Value] nvarchar(80)  NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'SmCategories'
CREATE TABLE [dbo].[SmCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'SmProdCats'
CREATE TABLE [dbo].[SmProdCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SmCategoryId] int  NOT NULL,
    [SmProductId] int  NOT NULL
);
GO

-- Creating table 'SmRates'
CREATE TABLE [dbo].[SmRates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Qty] int  NOT NULL,
    [Rate] decimal(18,0)  NOT NULL,
    [DRate] decimal(18,0)  NOT NULL,
    [SmProductId] int  NOT NULL,
    [SmRateUoMId] int  NOT NULL
);
GO

-- Creating table 'SmRateUoMs'
CREATE TABLE [dbo].[SmRateUoMs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'SmFiles'
CREATE TABLE [dbo].[SmFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(80)  NOT NULL,
    [Link] nvarchar(200)  NOT NULL,
    [SmProductId] int  NOT NULL
);
GO

-- Creating table 'SmProdAds'
CREATE TABLE [dbo].[SmProdAds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Image] nvarchar(max)  NOT NULL,
    [Link] nvarchar(max)  NOT NULL,
    [SmCategoryId] int  NOT NULL,
    [SmProductId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SmProducts'
ALTER TABLE [dbo].[SmProducts]
ADD CONSTRAINT [PK_SmProducts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmProdDescs'
ALTER TABLE [dbo].[SmProdDescs]
ADD CONSTRAINT [PK_SmProdDescs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmProdStatus'
ALTER TABLE [dbo].[SmProdStatus]
ADD CONSTRAINT [PK_SmProdStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmBranches'
ALTER TABLE [dbo].[SmBranches]
ADD CONSTRAINT [PK_SmBranches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmSuppliers'
ALTER TABLE [dbo].[SmSuppliers]
ADD CONSTRAINT [PK_SmSuppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmProdSuppliers'
ALTER TABLE [dbo].[SmProdSuppliers]
ADD CONSTRAINT [PK_SmProdSuppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmSupplierInfoes'
ALTER TABLE [dbo].[SmSupplierInfoes]
ADD CONSTRAINT [PK_SmSupplierInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmProdInfoes'
ALTER TABLE [dbo].[SmProdInfoes]
ADD CONSTRAINT [PK_SmProdInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmCategories'
ALTER TABLE [dbo].[SmCategories]
ADD CONSTRAINT [PK_SmCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmProdCats'
ALTER TABLE [dbo].[SmProdCats]
ADD CONSTRAINT [PK_SmProdCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmRates'
ALTER TABLE [dbo].[SmRates]
ADD CONSTRAINT [PK_SmRates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmRateUoMs'
ALTER TABLE [dbo].[SmRateUoMs]
ADD CONSTRAINT [PK_SmRateUoMs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmFiles'
ALTER TABLE [dbo].[SmFiles]
ADD CONSTRAINT [PK_SmFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SmProdAds'
ALTER TABLE [dbo].[SmProdAds]
ADD CONSTRAINT [PK_SmProdAds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SmProdStatusId] in table 'SmProducts'
ALTER TABLE [dbo].[SmProducts]
ADD CONSTRAINT [FK_SmProdStatusSmProduct]
    FOREIGN KEY ([SmProdStatusId])
    REFERENCES [dbo].[SmProdStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProdStatusSmProduct'
CREATE INDEX [IX_FK_SmProdStatusSmProduct]
ON [dbo].[SmProducts]
    ([SmProdStatusId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmProdDescs'
ALTER TABLE [dbo].[SmProdDescs]
ADD CONSTRAINT [FK_SmProductSmProdDesc]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmProdDesc'
CREATE INDEX [IX_FK_SmProductSmProdDesc]
ON [dbo].[SmProdDescs]
    ([SmProductId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmProdInfoes'
ALTER TABLE [dbo].[SmProdInfoes]
ADD CONSTRAINT [FK_SmProductSmProdInfo]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmProdInfo'
CREATE INDEX [IX_FK_SmProductSmProdInfo]
ON [dbo].[SmProdInfoes]
    ([SmProductId]);
GO

-- Creating foreign key on [SmBranchId] in table 'SmProducts'
ALTER TABLE [dbo].[SmProducts]
ADD CONSTRAINT [FK_SmBranchSmProduct]
    FOREIGN KEY ([SmBranchId])
    REFERENCES [dbo].[SmBranches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmBranchSmProduct'
CREATE INDEX [IX_FK_SmBranchSmProduct]
ON [dbo].[SmProducts]
    ([SmBranchId]);
GO

-- Creating foreign key on [SmSupplierId] in table 'SmProdSuppliers'
ALTER TABLE [dbo].[SmProdSuppliers]
ADD CONSTRAINT [FK_SmSupplierSmProdSupplier]
    FOREIGN KEY ([SmSupplierId])
    REFERENCES [dbo].[SmSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmSupplierSmProdSupplier'
CREATE INDEX [IX_FK_SmSupplierSmProdSupplier]
ON [dbo].[SmProdSuppliers]
    ([SmSupplierId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmProdSuppliers'
ALTER TABLE [dbo].[SmProdSuppliers]
ADD CONSTRAINT [FK_SmProductSmProdSupplier]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmProdSupplier'
CREATE INDEX [IX_FK_SmProductSmProdSupplier]
ON [dbo].[SmProdSuppliers]
    ([SmProductId]);
GO

-- Creating foreign key on [SmSupplierId] in table 'SmSupplierInfoes'
ALTER TABLE [dbo].[SmSupplierInfoes]
ADD CONSTRAINT [FK_SmSupplierSmSupplierInfo]
    FOREIGN KEY ([SmSupplierId])
    REFERENCES [dbo].[SmSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmSupplierSmSupplierInfo'
CREATE INDEX [IX_FK_SmSupplierSmSupplierInfo]
ON [dbo].[SmSupplierInfoes]
    ([SmSupplierId]);
GO

-- Creating foreign key on [SmCategoryId] in table 'SmProdCats'
ALTER TABLE [dbo].[SmProdCats]
ADD CONSTRAINT [FK_SmCategorySmProdCat]
    FOREIGN KEY ([SmCategoryId])
    REFERENCES [dbo].[SmCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmCategorySmProdCat'
CREATE INDEX [IX_FK_SmCategorySmProdCat]
ON [dbo].[SmProdCats]
    ([SmCategoryId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmProdCats'
ALTER TABLE [dbo].[SmProdCats]
ADD CONSTRAINT [FK_SmProductSmProdCat]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmProdCat'
CREATE INDEX [IX_FK_SmProductSmProdCat]
ON [dbo].[SmProdCats]
    ([SmProductId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmRates'
ALTER TABLE [dbo].[SmRates]
ADD CONSTRAINT [FK_SmProductSmRate]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmRate'
CREATE INDEX [IX_FK_SmProductSmRate]
ON [dbo].[SmRates]
    ([SmProductId]);
GO

-- Creating foreign key on [SmRateUoMId] in table 'SmRates'
ALTER TABLE [dbo].[SmRates]
ADD CONSTRAINT [FK_SmRateSmRateUoM]
    FOREIGN KEY ([SmRateUoMId])
    REFERENCES [dbo].[SmRateUoMs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmRateSmRateUoM'
CREATE INDEX [IX_FK_SmRateSmRateUoM]
ON [dbo].[SmRates]
    ([SmRateUoMId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmFiles'
ALTER TABLE [dbo].[SmFiles]
ADD CONSTRAINT [FK_SmProductSmFile]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmFile'
CREATE INDEX [IX_FK_SmProductSmFile]
ON [dbo].[SmFiles]
    ([SmProductId]);
GO

-- Creating foreign key on [SmCategoryId] in table 'SmProdAds'
ALTER TABLE [dbo].[SmProdAds]
ADD CONSTRAINT [FK_SmCategorySmProdAds]
    FOREIGN KEY ([SmCategoryId])
    REFERENCES [dbo].[SmCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmCategorySmProdAds'
CREATE INDEX [IX_FK_SmCategorySmProdAds]
ON [dbo].[SmProdAds]
    ([SmCategoryId]);
GO

-- Creating foreign key on [SmProductId] in table 'SmProdAds'
ALTER TABLE [dbo].[SmProdAds]
ADD CONSTRAINT [FK_SmProductSmProdAds]
    FOREIGN KEY ([SmProductId])
    REFERENCES [dbo].[SmProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SmProductSmProdAds'
CREATE INDEX [IX_FK_SmProductSmProdAds]
ON [dbo].[SmProdAds]
    ([SmProductId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------