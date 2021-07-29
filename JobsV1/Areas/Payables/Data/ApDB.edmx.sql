
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/21/2021 10:39:02
-- Generated from EDMX file: C:\Users\Acer-PC\Documents\GitHub\Payable20\ApModels\Models\ApDB.edmx
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

IF OBJECT_ID(N'[dbo].[FK_ApAccStatusApAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApAccounts] DROP CONSTRAINT [FK_ApAccStatusApAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_ApAccountApPayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApPayments] DROP CONSTRAINT [FK_ApAccountApPayments];
GO
IF OBJECT_ID(N'[dbo].[FK_ApPaymentTypeApPayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApPayments] DROP CONSTRAINT [FK_ApPaymentTypeApPayments];
GO
IF OBJECT_ID(N'[dbo].[FK_ApAccountApTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransactions] DROP CONSTRAINT [FK_ApAccountApTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ApPaymentsApTransPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransPayments] DROP CONSTRAINT [FK_ApPaymentsApTransPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransactionApTransPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransPayments] DROP CONSTRAINT [FK_ApTransactionApTransPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransCategoryApTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransactions] DROP CONSTRAINT [FK_ApTransCategoryApTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransStatusApTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransactions] DROP CONSTRAINT [FK_ApTransStatusApTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransactionApAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApActions] DROP CONSTRAINT [FK_ApTransactionApAction];
GO
IF OBJECT_ID(N'[dbo].[FK_ApActionItemApAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApActions] DROP CONSTRAINT [FK_ApActionItemApAction];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransactionApTransPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransPosts] DROP CONSTRAINT [FK_ApTransactionApTransPost];
GO
IF OBJECT_ID(N'[dbo].[FK_ApPaymentStatusApPayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApPayments] DROP CONSTRAINT [FK_ApPaymentStatusApPayments];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransPrintReqApPrintRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransPrintReqs] DROP CONSTRAINT [FK_ApTransPrintReqApPrintRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransPrintReqApTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransPrintReqs] DROP CONSTRAINT [FK_ApTransPrintReqApTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransactionApTransItems]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransItems] DROP CONSTRAINT [FK_ApTransactionApTransItems];
GO
IF OBJECT_ID(N'[dbo].[FK_ApTransactionApTransRepeat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApTransRepeats] DROP CONSTRAINT [FK_ApTransactionApTransRepeat];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ApTransactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransactions];
GO
IF OBJECT_ID(N'[dbo].[ApAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApAccounts];
GO
IF OBJECT_ID(N'[dbo].[ApAccStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApAccStatus];
GO
IF OBJECT_ID(N'[dbo].[ApPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApPayments];
GO
IF OBJECT_ID(N'[dbo].[ApPaymentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApPaymentTypes];
GO
IF OBJECT_ID(N'[dbo].[ApTransPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransPayments];
GO
IF OBJECT_ID(N'[dbo].[ApTransStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransStatus];
GO
IF OBJECT_ID(N'[dbo].[ApTransCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransCategories];
GO
IF OBJECT_ID(N'[dbo].[ApActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApActions];
GO
IF OBJECT_ID(N'[dbo].[ApActionItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApActionItems];
GO
IF OBJECT_ID(N'[dbo].[ApTransPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransPosts];
GO
IF OBJECT_ID(N'[dbo].[ApPaymentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApPaymentStatus];
GO
IF OBJECT_ID(N'[dbo].[ApTransPrintReqs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransPrintReqs];
GO
IF OBJECT_ID(N'[dbo].[ApPrintRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApPrintRequests];
GO
IF OBJECT_ID(N'[dbo].[ApTransItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransItems];
GO
IF OBJECT_ID(N'[dbo].[ApTransRepeats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApTransRepeats];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ApTransactions'
CREATE TABLE [dbo].[ApTransactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceNo] nvarchar(max)  NULL,
    [DtInvoice] datetime  NOT NULL,
    [DtEncoded] datetime  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [DtDue] datetime  NOT NULL,
    [DtService] datetime  NOT NULL,
    [DtServiceTo] datetime  NOT NULL,
    [Remarks] nvarchar(180)  NULL,
    [ApAccountId] int  NOT NULL,
    [ApTransStatusId] int  NOT NULL,
    [ApTransCategoryId] int  NOT NULL,
    
    [IsRepeating] bit  NOT NULL,
    [RepeatCount] int  NOT NULL,
    [RepeatNo] int  NOT NULL,
    [Interval] int  NULL,
    [NextRef] int  NOT NULL,
    [PrevRef] int  NOT NULL,

    [IsPrinted] bit  NOT NULL,
    [BudgetAmt] decimal(20,2)  NULL,
    [ReleaseAmt] decimal(20,2)  NULL,
    [DtRelease] datetime  NULL,
    [JobRef] int  NULL
);
GO

-- Creating table 'ApAccounts'
CREATE TABLE [dbo].[ApAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(180)  NOT NULL,
    [Landline] nvarchar(20)  NULL,
    [Mobile] nvarchar(20)  NOT NULL,
    [Email] nvarchar(80)  NULL,
    [ContactPerson] nvarchar(180)  NULL,
    [Address] nvarchar(180)  NULL,
    [Remarks] nvarchar(150)  NULL,
    [ApAccStatusId] int  NOT NULL,
    [Landline2] nvarchar(20)  NULL,
    [Mobile2] nvarchar(20)  NULL
);
GO

-- Creating table 'ApAccStatus'
CREATE TABLE [dbo].[ApAccStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'ApPayments'
CREATE TABLE [dbo].[ApPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPayment] datetime  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [ApAccountId] int  NOT NULL,
    [ApPaymentTypeId] int  NOT NULL,
    [ApPaymentStatusId] int  NOT NULL
);
GO

-- Creating table 'ApPaymentTypes'
CREATE TABLE [dbo].[ApPaymentTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(50)  NULL
);
GO

-- Creating table 'ApTransPayments'
CREATE TABLE [dbo].[ApTransPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApPaymentsId] int  NOT NULL,
    [ApTransactionId] int  NOT NULL
);
GO

-- Creating table 'ApTransStatus'
CREATE TABLE [dbo].[ApTransStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(20)  NOT NULL,
    [Code] int  NOT NULL
);
GO

-- Creating table 'ApTransCategories'
CREATE TABLE [dbo].[ApTransCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(180)  NULL
);
GO

-- Creating table 'ApActions'
CREATE TABLE [dbo].[ApActions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPerformed] datetime  NOT NULL,
    [PerformedBy] nvarchar(180)  NOT NULL,
    [ApTransactionId] int  NOT NULL,
    [ApActionItemId] int  NOT NULL
);
GO

-- Creating table 'ApActionItems'
CREATE TABLE [dbo].[ApActionItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Action] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(180)  NULL,
    [SortNo] int  NOT NULL
);
GO

-- Creating table 'ApTransPosts'
CREATE TABLE [dbo].[ApTransPosts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPost] datetime  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [Balance] decimal(20,2)  NOT NULL,
    [ApTransactionId] int  NOT NULL
);
GO

-- Creating table 'ApPaymentStatus'
CREATE TABLE [dbo].[ApPaymentStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ApTransPrintReqs'
CREATE TABLE [dbo].[ApTransPrintReqs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApPrintRequestId] int  NOT NULL,
    [ApTransactionId] int  NOT NULL
);
GO

-- Creating table 'ApPrintRequests'
CREATE TABLE [dbo].[ApPrintRequests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtRequest] datetime  NOT NULL,
    [RequestBy] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'ApTransItems'
CREATE TABLE [dbo].[ApTransItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [ApTransactionId] int  NOT NULL
);
GO

-- Creating table 'ApTransRepeats'
CREATE TABLE [dbo].[ApTransRepeats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RepeatCount] nvarchar(max)  NOT NULL,
    [RepeatNo] nvarchar(max)  NOT NULL,
    [NextRef] nvarchar(max)  NOT NULL,
    [PrevRef] nvarchar(max)  NOT NULL,
    [ApTransactionId] int  NOT NULL,
    [ApTransaction_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ApTransactions'
ALTER TABLE [dbo].[ApTransactions]
ADD CONSTRAINT [PK_ApTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApAccounts'
ALTER TABLE [dbo].[ApAccounts]
ADD CONSTRAINT [PK_ApAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApAccStatus'
ALTER TABLE [dbo].[ApAccStatus]
ADD CONSTRAINT [PK_ApAccStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApPayments'
ALTER TABLE [dbo].[ApPayments]
ADD CONSTRAINT [PK_ApPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApPaymentTypes'
ALTER TABLE [dbo].[ApPaymentTypes]
ADD CONSTRAINT [PK_ApPaymentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransPayments'
ALTER TABLE [dbo].[ApTransPayments]
ADD CONSTRAINT [PK_ApTransPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransStatus'
ALTER TABLE [dbo].[ApTransStatus]
ADD CONSTRAINT [PK_ApTransStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransCategories'
ALTER TABLE [dbo].[ApTransCategories]
ADD CONSTRAINT [PK_ApTransCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApActions'
ALTER TABLE [dbo].[ApActions]
ADD CONSTRAINT [PK_ApActions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApActionItems'
ALTER TABLE [dbo].[ApActionItems]
ADD CONSTRAINT [PK_ApActionItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransPosts'
ALTER TABLE [dbo].[ApTransPosts]
ADD CONSTRAINT [PK_ApTransPosts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApPaymentStatus'
ALTER TABLE [dbo].[ApPaymentStatus]
ADD CONSTRAINT [PK_ApPaymentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransPrintReqs'
ALTER TABLE [dbo].[ApTransPrintReqs]
ADD CONSTRAINT [PK_ApTransPrintReqs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApPrintRequests'
ALTER TABLE [dbo].[ApPrintRequests]
ADD CONSTRAINT [PK_ApPrintRequests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransItems'
ALTER TABLE [dbo].[ApTransItems]
ADD CONSTRAINT [PK_ApTransItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApTransRepeats'
ALTER TABLE [dbo].[ApTransRepeats]
ADD CONSTRAINT [PK_ApTransRepeats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ApAccStatusId] in table 'ApAccounts'
ALTER TABLE [dbo].[ApAccounts]
ADD CONSTRAINT [FK_ApAccStatusApAccount]
    FOREIGN KEY ([ApAccStatusId])
    REFERENCES [dbo].[ApAccStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApAccStatusApAccount'
CREATE INDEX [IX_FK_ApAccStatusApAccount]
ON [dbo].[ApAccounts]
    ([ApAccStatusId]);
GO

-- Creating foreign key on [ApAccountId] in table 'ApPayments'
ALTER TABLE [dbo].[ApPayments]
ADD CONSTRAINT [FK_ApAccountApPayments]
    FOREIGN KEY ([ApAccountId])
    REFERENCES [dbo].[ApAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApAccountApPayments'
CREATE INDEX [IX_FK_ApAccountApPayments]
ON [dbo].[ApPayments]
    ([ApAccountId]);
GO

-- Creating foreign key on [ApPaymentTypeId] in table 'ApPayments'
ALTER TABLE [dbo].[ApPayments]
ADD CONSTRAINT [FK_ApPaymentTypeApPayments]
    FOREIGN KEY ([ApPaymentTypeId])
    REFERENCES [dbo].[ApPaymentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApPaymentTypeApPayments'
CREATE INDEX [IX_FK_ApPaymentTypeApPayments]
ON [dbo].[ApPayments]
    ([ApPaymentTypeId]);
GO

-- Creating foreign key on [ApAccountId] in table 'ApTransactions'
ALTER TABLE [dbo].[ApTransactions]
ADD CONSTRAINT [FK_ApAccountApTransaction]
    FOREIGN KEY ([ApAccountId])
    REFERENCES [dbo].[ApAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApAccountApTransaction'
CREATE INDEX [IX_FK_ApAccountApTransaction]
ON [dbo].[ApTransactions]
    ([ApAccountId]);
GO

-- Creating foreign key on [ApPaymentsId] in table 'ApTransPayments'
ALTER TABLE [dbo].[ApTransPayments]
ADD CONSTRAINT [FK_ApPaymentsApTransPayment]
    FOREIGN KEY ([ApPaymentsId])
    REFERENCES [dbo].[ApPayments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApPaymentsApTransPayment'
CREATE INDEX [IX_FK_ApPaymentsApTransPayment]
ON [dbo].[ApTransPayments]
    ([ApPaymentsId]);
GO

-- Creating foreign key on [ApTransactionId] in table 'ApTransPayments'
ALTER TABLE [dbo].[ApTransPayments]
ADD CONSTRAINT [FK_ApTransactionApTransPayment]
    FOREIGN KEY ([ApTransactionId])
    REFERENCES [dbo].[ApTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransactionApTransPayment'
CREATE INDEX [IX_FK_ApTransactionApTransPayment]
ON [dbo].[ApTransPayments]
    ([ApTransactionId]);
GO

-- Creating foreign key on [ApTransCategoryId] in table 'ApTransactions'
ALTER TABLE [dbo].[ApTransactions]
ADD CONSTRAINT [FK_ApTransCategoryApTransaction]
    FOREIGN KEY ([ApTransCategoryId])
    REFERENCES [dbo].[ApTransCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransCategoryApTransaction'
CREATE INDEX [IX_FK_ApTransCategoryApTransaction]
ON [dbo].[ApTransactions]
    ([ApTransCategoryId]);
GO

-- Creating foreign key on [ApTransStatusId] in table 'ApTransactions'
ALTER TABLE [dbo].[ApTransactions]
ADD CONSTRAINT [FK_ApTransStatusApTransaction]
    FOREIGN KEY ([ApTransStatusId])
    REFERENCES [dbo].[ApTransStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransStatusApTransaction'
CREATE INDEX [IX_FK_ApTransStatusApTransaction]
ON [dbo].[ApTransactions]
    ([ApTransStatusId]);
GO

-- Creating foreign key on [ApTransactionId] in table 'ApActions'
ALTER TABLE [dbo].[ApActions]
ADD CONSTRAINT [FK_ApTransactionApAction]
    FOREIGN KEY ([ApTransactionId])
    REFERENCES [dbo].[ApTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransactionApAction'
CREATE INDEX [IX_FK_ApTransactionApAction]
ON [dbo].[ApActions]
    ([ApTransactionId]);
GO

-- Creating foreign key on [ApActionItemId] in table 'ApActions'
ALTER TABLE [dbo].[ApActions]
ADD CONSTRAINT [FK_ApActionItemApAction]
    FOREIGN KEY ([ApActionItemId])
    REFERENCES [dbo].[ApActionItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApActionItemApAction'
CREATE INDEX [IX_FK_ApActionItemApAction]
ON [dbo].[ApActions]
    ([ApActionItemId]);
GO

-- Creating foreign key on [ApTransactionId] in table 'ApTransPosts'
ALTER TABLE [dbo].[ApTransPosts]
ADD CONSTRAINT [FK_ApTransactionApTransPost]
    FOREIGN KEY ([ApTransactionId])
    REFERENCES [dbo].[ApTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransactionApTransPost'
CREATE INDEX [IX_FK_ApTransactionApTransPost]
ON [dbo].[ApTransPosts]
    ([ApTransactionId]);
GO

-- Creating foreign key on [ApPaymentStatusId] in table 'ApPayments'
ALTER TABLE [dbo].[ApPayments]
ADD CONSTRAINT [FK_ApPaymentStatusApPayments]
    FOREIGN KEY ([ApPaymentStatusId])
    REFERENCES [dbo].[ApPaymentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApPaymentStatusApPayments'
CREATE INDEX [IX_FK_ApPaymentStatusApPayments]
ON [dbo].[ApPayments]
    ([ApPaymentStatusId]);
GO

-- Creating foreign key on [ApPrintRequestId] in table 'ApTransPrintReqs'
ALTER TABLE [dbo].[ApTransPrintReqs]
ADD CONSTRAINT [FK_ApTransPrintReqApPrintRequest]
    FOREIGN KEY ([ApPrintRequestId])
    REFERENCES [dbo].[ApPrintRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransPrintReqApPrintRequest'
CREATE INDEX [IX_FK_ApTransPrintReqApPrintRequest]
ON [dbo].[ApTransPrintReqs]
    ([ApPrintRequestId]);
GO

-- Creating foreign key on [ApTransactionId] in table 'ApTransPrintReqs'
ALTER TABLE [dbo].[ApTransPrintReqs]
ADD CONSTRAINT [FK_ApTransPrintReqApTransaction]
    FOREIGN KEY ([ApTransactionId])
    REFERENCES [dbo].[ApTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransPrintReqApTransaction'
CREATE INDEX [IX_FK_ApTransPrintReqApTransaction]
ON [dbo].[ApTransPrintReqs]
    ([ApTransactionId]);
GO

-- Creating foreign key on [ApTransactionId] in table 'ApTransItems'
ALTER TABLE [dbo].[ApTransItems]
ADD CONSTRAINT [FK_ApTransactionApTransItems]
    FOREIGN KEY ([ApTransactionId])
    REFERENCES [dbo].[ApTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransactionApTransItems'
CREATE INDEX [IX_FK_ApTransactionApTransItems]
ON [dbo].[ApTransItems]
    ([ApTransactionId]);
GO

-- Creating foreign key on [ApTransaction_Id] in table 'ApTransRepeats'
ALTER TABLE [dbo].[ApTransRepeats]
ADD CONSTRAINT [FK_ApTransactionApTransRepeat]
    FOREIGN KEY ([ApTransaction_Id])
    REFERENCES [dbo].[ApTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApTransactionApTransRepeat'
CREATE INDEX [IX_FK_ApTransactionApTransRepeat]
ON [dbo].[ApTransRepeats]
    ([ApTransaction_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------