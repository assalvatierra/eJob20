
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/09/2019 14:52:44
-- Generated from EDMX file: D:\Data\Real\Apps\GitHub\eJob20\JobsV1\Areas\Products\Models\ProdDB.edmx
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(10)  NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [BranchId] int  NOT NULL,
    [ProdStatusId] int  NOT NULL,
    [ValidStart] datetime  NOT NULL,
    [ValidEnd] datetime  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Contracted] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'ProdDescs'
CREATE TABLE [dbo].[ProdDescs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [SortNo] int  NOT NULL,
    [Description] nvarchar(180)  NULL
);
GO

-- Creating table 'ProdStatus'
CREATE TABLE [dbo].[ProdStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Branches'
CREATE TABLE [dbo].[Branches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'ProdSuppliers'
CREATE TABLE [dbo].[ProdSuppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [ValidStart] datetime  NOT NULL,
    [ValidEnd] datetime  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Contracted] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'SupplierInfoes'
CREATE TABLE [dbo].[SupplierInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int  NOT NULL,
    [Label] nvarchar(10)  NOT NULL,
    [Value] nvarchar(80)  NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'ProdInfoes'
CREATE TABLE [dbo].[ProdInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Label] nvarchar(10)  NOT NULL,
    [Value] nvarchar(80)  NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProdDescs'
ALTER TABLE [dbo].[ProdDescs]
ADD CONSTRAINT [PK_ProdDescs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProdStatus'
ALTER TABLE [dbo].[ProdStatus]
ADD CONSTRAINT [PK_ProdStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [PK_Branches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProdSuppliers'
ALTER TABLE [dbo].[ProdSuppliers]
ADD CONSTRAINT [PK_ProdSuppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierInfoes'
ALTER TABLE [dbo].[SupplierInfoes]
ADD CONSTRAINT [PK_SupplierInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProdInfoes'
ALTER TABLE [dbo].[ProdInfoes]
ADD CONSTRAINT [PK_ProdInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ProductId] in table 'ProdDescs'
ALTER TABLE [dbo].[ProdDescs]
ADD CONSTRAINT [FK_ProductProdDesc]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProdDesc'
CREATE INDEX [IX_FK_ProductProdDesc]
ON [dbo].[ProdDescs]
    ([ProductId]);
GO

-- Creating foreign key on [ProdStatusId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProdStatusProduct]
    FOREIGN KEY ([ProdStatusId])
    REFERENCES [dbo].[ProdStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProdStatusProduct'
CREATE INDEX [IX_FK_ProdStatusProduct]
ON [dbo].[Products]
    ([ProdStatusId]);
GO

-- Creating foreign key on [BranchId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_BranchProduct]
    FOREIGN KEY ([BranchId])
    REFERENCES [dbo].[Branches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BranchProduct'
CREATE INDEX [IX_FK_BranchProduct]
ON [dbo].[Products]
    ([BranchId]);
GO

-- Creating foreign key on [SupplierId] in table 'ProdSuppliers'
ALTER TABLE [dbo].[ProdSuppliers]
ADD CONSTRAINT [FK_SupplierProdSupplier]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierProdSupplier'
CREATE INDEX [IX_FK_SupplierProdSupplier]
ON [dbo].[ProdSuppliers]
    ([SupplierId]);
GO

-- Creating foreign key on [ProductId] in table 'ProdSuppliers'
ALTER TABLE [dbo].[ProdSuppliers]
ADD CONSTRAINT [FK_ProductProdSupplier]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProdSupplier'
CREATE INDEX [IX_FK_ProductProdSupplier]
ON [dbo].[ProdSuppliers]
    ([ProductId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierInfoes'
ALTER TABLE [dbo].[SupplierInfoes]
ADD CONSTRAINT [FK_SupplierSupplierInfo]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierInfo'
CREATE INDEX [IX_FK_SupplierSupplierInfo]
ON [dbo].[SupplierInfoes]
    ([SupplierId]);
GO

-- Creating foreign key on [ProductId] in table 'ProdInfoes'
ALTER TABLE [dbo].[ProdInfoes]
ADD CONSTRAINT [FK_ProductProdInfo]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProdInfo'
CREATE INDEX [IX_FK_ProductProdInfo]
ON [dbo].[ProdInfoes]
    ([ProductId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------