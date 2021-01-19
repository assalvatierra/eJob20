
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/12/2021 13:14:26
-- Generated from EDMX file: C:\Users\Acer-PC\Documents\GitHub\Receivable20\ArModels\Models\ArDB.edmx
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

IF OBJECT_ID(N'[dbo].[FK_ArTransactionArTransPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArTransPosts] DROP CONSTRAINT [FK_ArTransactionArTransPost];
GO
IF OBJECT_ID(N'[dbo].[FK_ArTransStatusArTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArTransactions] DROP CONSTRAINT [FK_ArTransStatusArTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ArAccountArTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArTransactions] DROP CONSTRAINT [FK_ArAccountArTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ArAccStatusArAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArAccounts] DROP CONSTRAINT [FK_ArAccStatusArAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_ArTransactionArAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArActions] DROP CONSTRAINT [FK_ArTransactionArAction];
GO
IF OBJECT_ID(N'[dbo].[FK_ArActionItemArAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArActions] DROP CONSTRAINT [FK_ArActionItemArAction];
GO
IF OBJECT_ID(N'[dbo].[FK_ArAccountArPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArPayments] DROP CONSTRAINT [FK_ArAccountArPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_ArTransactionArTransPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArTransPayments] DROP CONSTRAINT [FK_ArTransactionArTransPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_ArPaymentArTransPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArTransPayments] DROP CONSTRAINT [FK_ArPaymentArTransPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_ArPaymentTypeArPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArPayments] DROP CONSTRAINT [FK_ArPaymentTypeArPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_ArCategoryArTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArTransactions] DROP CONSTRAINT [FK_ArCategoryArTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ArAccountArAccntCredit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArAccntCredits] DROP CONSTRAINT [FK_ArAccountArAccntCredit];
GO
IF OBJECT_ID(N'[dbo].[FK_ArCreditStatusArAccntCredit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArAccntCredits] DROP CONSTRAINT [FK_ArCreditStatusArAccntCredit];
GO
IF OBJECT_ID(N'[dbo].[FK_ArAccountArAccntPaymentTerm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArAccntTerms] DROP CONSTRAINT [FK_ArAccountArAccntPaymentTerm];
GO
IF OBJECT_ID(N'[dbo].[FK_ArAccntTermStatusArAccntPaymentTerm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArAccntTerms] DROP CONSTRAINT [FK_ArAccntTermStatusArAccntPaymentTerm];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ArTransactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArTransactions];
GO
IF OBJECT_ID(N'[dbo].[ArTransPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArTransPosts];
GO
IF OBJECT_ID(N'[dbo].[ArPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArPayments];
GO
IF OBJECT_ID(N'[dbo].[ArTransStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArTransStatus];
GO
IF OBJECT_ID(N'[dbo].[ArAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArAccounts];
GO
IF OBJECT_ID(N'[dbo].[ArAccStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArAccStatus];
GO
IF OBJECT_ID(N'[dbo].[ArActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArActions];
GO
IF OBJECT_ID(N'[dbo].[ArActionItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArActionItems];
GO
IF OBJECT_ID(N'[dbo].[ArTransPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArTransPayments];
GO
IF OBJECT_ID(N'[dbo].[ArPaymentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArPaymentTypes];
GO
IF OBJECT_ID(N'[dbo].[ArCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArCategories];
GO
IF OBJECT_ID(N'[dbo].[ArAccntCredits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArAccntCredits];
GO
IF OBJECT_ID(N'[dbo].[ArCreditStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArCreditStatus];
GO
IF OBJECT_ID(N'[dbo].[ArAccntTerms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArAccntTerms];
GO
IF OBJECT_ID(N'[dbo].[ArAccntTermStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArAccntTermStatus];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ArTransactions'
CREATE TABLE [dbo].[ArTransactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceId] int  NOT NULL,
    [DtInvoice] datetime  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [DtEncoded] datetime  NOT NULL,
    [DtDue] datetime  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [Interval] int  NOT NULL,
    [IsRepeating] bit  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [ArTransStatusId] int  NOT NULL,
    [ArAccountId] int  NOT NULL,
    [ArCategoryId] int  NOT NULL,
    [DtService] datetime  NOT NULL,
    [DtServiceTo] datetime  NULL,
    [PrevRef] int  NULL,
    [NextRef] int  NULL,
    [InvoiceRef] nvarchar(20)  NULL,
    [RepeatCount] int  NULL
);
GO

-- Creating table 'ArTransPosts'
CREATE TABLE [dbo].[ArTransPosts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPost] datetime  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [Balance] decimal(20,2)  NOT NULL,
    [ArTransactionId] int  NOT NULL
);
GO

-- Creating table 'ArPayments'
CREATE TABLE [dbo].[ArPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPayment] datetime  NOT NULL,
    [Amount] decimal(20,2)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [Reference] nvarchar(80)  NULL,
    [ArAccountId] int  NOT NULL,
    [ArPaymentTypeId] int  NOT NULL
);
GO

-- Creating table 'ArTransStatus'
CREATE TABLE [dbo].[ArTransStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ArAccounts'
CREATE TABLE [dbo].[ArAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Landline] nvarchar(40)  NULL,
    [Email] nvarchar(80)  NOT NULL,
    [Mobile] nvarchar(40)  NOT NULL,
    [Company] nvarchar(80)  NULL,
    [Address] nvarchar(120)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [ArAccStatusId] int  NOT NULL,
    [Landline2] nvarchar(20)  NULL,
    [Mobile2] nvarchar(20)  NULL
);
GO

-- Creating table 'ArAccStatus'
CREATE TABLE [dbo].[ArAccStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'ArActions'
CREATE TABLE [dbo].[ArActions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPerformed] datetime  NOT NULL,
    [PreformedBy] nvarchar(80)  NOT NULL,
    [ArTransactionId] int  NOT NULL,
    [ArActionItemId] int  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'ArActionItems'
CREATE TABLE [dbo].[ArActionItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Action] nvarchar(40)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [SortNo] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'ArTransPayments'
CREATE TABLE [dbo].[ArTransPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ArTransactionId] int  NOT NULL,
    [ArPaymentId] int  NOT NULL
);
GO

-- Creating table 'ArPaymentTypes'
CREATE TABLE [dbo].[ArPaymentTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(40)  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'ArCategories'
CREATE TABLE [dbo].[ArCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [SortNo] int  NOT NULL
);
GO

-- Creating table 'ArAccntCredits'
CREATE TABLE [dbo].[ArAccntCredits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ArAccountId] int  NOT NULL,
    [DtCredit] datetime  NOT NULL,
    [CreditLimit] decimal(18,0)  NOT NULL,
    [OverLimitAllowed] decimal(18,0)  NOT NULL,
    [CreditWarning] decimal(18,0)  NOT NULL,
    [ApprovedBy] nvarchar(80)  NULL,
    [ArCreditStatusId] int  NOT NULL
);
GO

-- Creating table 'ArCreditStatus'
CREATE TABLE [dbo].[ArCreditStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'ArAccntTerms'
CREATE TABLE [dbo].[ArAccntTerms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtTerm] datetime  NOT NULL,
    [NoOfDays] int  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [ArAccountId] int  NOT NULL,
    [ApprovedBy] nvarchar(max)  NOT NULL,
    [ArAccntTermStatusId] int  NOT NULL
);
GO

-- Creating table 'ArAccntTermStatus'
CREATE TABLE [dbo].[ArAccntTermStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ArTransactions'
ALTER TABLE [dbo].[ArTransactions]
ADD CONSTRAINT [PK_ArTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArTransPosts'
ALTER TABLE [dbo].[ArTransPosts]
ADD CONSTRAINT [PK_ArTransPosts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArPayments'
ALTER TABLE [dbo].[ArPayments]
ADD CONSTRAINT [PK_ArPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArTransStatus'
ALTER TABLE [dbo].[ArTransStatus]
ADD CONSTRAINT [PK_ArTransStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArAccounts'
ALTER TABLE [dbo].[ArAccounts]
ADD CONSTRAINT [PK_ArAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArAccStatus'
ALTER TABLE [dbo].[ArAccStatus]
ADD CONSTRAINT [PK_ArAccStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArActions'
ALTER TABLE [dbo].[ArActions]
ADD CONSTRAINT [PK_ArActions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArActionItems'
ALTER TABLE [dbo].[ArActionItems]
ADD CONSTRAINT [PK_ArActionItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArTransPayments'
ALTER TABLE [dbo].[ArTransPayments]
ADD CONSTRAINT [PK_ArTransPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArPaymentTypes'
ALTER TABLE [dbo].[ArPaymentTypes]
ADD CONSTRAINT [PK_ArPaymentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArCategories'
ALTER TABLE [dbo].[ArCategories]
ADD CONSTRAINT [PK_ArCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArAccntCredits'
ALTER TABLE [dbo].[ArAccntCredits]
ADD CONSTRAINT [PK_ArAccntCredits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArCreditStatus'
ALTER TABLE [dbo].[ArCreditStatus]
ADD CONSTRAINT [PK_ArCreditStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArAccntTerms'
ALTER TABLE [dbo].[ArAccntTerms]
ADD CONSTRAINT [PK_ArAccntTerms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArAccntTermStatus'
ALTER TABLE [dbo].[ArAccntTermStatus]
ADD CONSTRAINT [PK_ArAccntTermStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ArTransactionId] in table 'ArTransPosts'
ALTER TABLE [dbo].[ArTransPosts]
ADD CONSTRAINT [FK_ArTransactionArTransPost]
    FOREIGN KEY ([ArTransactionId])
    REFERENCES [dbo].[ArTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArTransactionArTransPost'
CREATE INDEX [IX_FK_ArTransactionArTransPost]
ON [dbo].[ArTransPosts]
    ([ArTransactionId]);
GO

-- Creating foreign key on [ArTransStatusId] in table 'ArTransactions'
ALTER TABLE [dbo].[ArTransactions]
ADD CONSTRAINT [FK_ArTransStatusArTransaction]
    FOREIGN KEY ([ArTransStatusId])
    REFERENCES [dbo].[ArTransStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArTransStatusArTransaction'
CREATE INDEX [IX_FK_ArTransStatusArTransaction]
ON [dbo].[ArTransactions]
    ([ArTransStatusId]);
GO

-- Creating foreign key on [ArAccountId] in table 'ArTransactions'
ALTER TABLE [dbo].[ArTransactions]
ADD CONSTRAINT [FK_ArAccountArTransaction]
    FOREIGN KEY ([ArAccountId])
    REFERENCES [dbo].[ArAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArAccountArTransaction'
CREATE INDEX [IX_FK_ArAccountArTransaction]
ON [dbo].[ArTransactions]
    ([ArAccountId]);
GO

-- Creating foreign key on [ArAccStatusId] in table 'ArAccounts'
ALTER TABLE [dbo].[ArAccounts]
ADD CONSTRAINT [FK_ArAccStatusArAccount]
    FOREIGN KEY ([ArAccStatusId])
    REFERENCES [dbo].[ArAccStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArAccStatusArAccount'
CREATE INDEX [IX_FK_ArAccStatusArAccount]
ON [dbo].[ArAccounts]
    ([ArAccStatusId]);
GO

-- Creating foreign key on [ArTransactionId] in table 'ArActions'
ALTER TABLE [dbo].[ArActions]
ADD CONSTRAINT [FK_ArTransactionArAction]
    FOREIGN KEY ([ArTransactionId])
    REFERENCES [dbo].[ArTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArTransactionArAction'
CREATE INDEX [IX_FK_ArTransactionArAction]
ON [dbo].[ArActions]
    ([ArTransactionId]);
GO

-- Creating foreign key on [ArActionItemId] in table 'ArActions'
ALTER TABLE [dbo].[ArActions]
ADD CONSTRAINT [FK_ArActionItemArAction]
    FOREIGN KEY ([ArActionItemId])
    REFERENCES [dbo].[ArActionItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArActionItemArAction'
CREATE INDEX [IX_FK_ArActionItemArAction]
ON [dbo].[ArActions]
    ([ArActionItemId]);
GO

-- Creating foreign key on [ArAccountId] in table 'ArPayments'
ALTER TABLE [dbo].[ArPayments]
ADD CONSTRAINT [FK_ArAccountArPayment]
    FOREIGN KEY ([ArAccountId])
    REFERENCES [dbo].[ArAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArAccountArPayment'
CREATE INDEX [IX_FK_ArAccountArPayment]
ON [dbo].[ArPayments]
    ([ArAccountId]);
GO

-- Creating foreign key on [ArTransactionId] in table 'ArTransPayments'
ALTER TABLE [dbo].[ArTransPayments]
ADD CONSTRAINT [FK_ArTransactionArTransPayment]
    FOREIGN KEY ([ArTransactionId])
    REFERENCES [dbo].[ArTransactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArTransactionArTransPayment'
CREATE INDEX [IX_FK_ArTransactionArTransPayment]
ON [dbo].[ArTransPayments]
    ([ArTransactionId]);
GO

-- Creating foreign key on [ArPaymentId] in table 'ArTransPayments'
ALTER TABLE [dbo].[ArTransPayments]
ADD CONSTRAINT [FK_ArPaymentArTransPayment]
    FOREIGN KEY ([ArPaymentId])
    REFERENCES [dbo].[ArPayments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArPaymentArTransPayment'
CREATE INDEX [IX_FK_ArPaymentArTransPayment]
ON [dbo].[ArTransPayments]
    ([ArPaymentId]);
GO

-- Creating foreign key on [ArPaymentTypeId] in table 'ArPayments'
ALTER TABLE [dbo].[ArPayments]
ADD CONSTRAINT [FK_ArPaymentTypeArPayment]
    FOREIGN KEY ([ArPaymentTypeId])
    REFERENCES [dbo].[ArPaymentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArPaymentTypeArPayment'
CREATE INDEX [IX_FK_ArPaymentTypeArPayment]
ON [dbo].[ArPayments]
    ([ArPaymentTypeId]);
GO

-- Creating foreign key on [ArCategoryId] in table 'ArTransactions'
ALTER TABLE [dbo].[ArTransactions]
ADD CONSTRAINT [FK_ArCategoryArTransaction]
    FOREIGN KEY ([ArCategoryId])
    REFERENCES [dbo].[ArCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArCategoryArTransaction'
CREATE INDEX [IX_FK_ArCategoryArTransaction]
ON [dbo].[ArTransactions]
    ([ArCategoryId]);
GO

-- Creating foreign key on [ArAccountId] in table 'ArAccntCredits'
ALTER TABLE [dbo].[ArAccntCredits]
ADD CONSTRAINT [FK_ArAccountArAccntCredit]
    FOREIGN KEY ([ArAccountId])
    REFERENCES [dbo].[ArAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArAccountArAccntCredit'
CREATE INDEX [IX_FK_ArAccountArAccntCredit]
ON [dbo].[ArAccntCredits]
    ([ArAccountId]);
GO

-- Creating foreign key on [ArCreditStatusId] in table 'ArAccntCredits'
ALTER TABLE [dbo].[ArAccntCredits]
ADD CONSTRAINT [FK_ArCreditStatusArAccntCredit]
    FOREIGN KEY ([ArCreditStatusId])
    REFERENCES [dbo].[ArCreditStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArCreditStatusArAccntCredit'
CREATE INDEX [IX_FK_ArCreditStatusArAccntCredit]
ON [dbo].[ArAccntCredits]
    ([ArCreditStatusId]);
GO

-- Creating foreign key on [ArAccountId] in table 'ArAccntTerms'
ALTER TABLE [dbo].[ArAccntTerms]
ADD CONSTRAINT [FK_ArAccountArAccntPaymentTerm]
    FOREIGN KEY ([ArAccountId])
    REFERENCES [dbo].[ArAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArAccountArAccntPaymentTerm'
CREATE INDEX [IX_FK_ArAccountArAccntPaymentTerm]
ON [dbo].[ArAccntTerms]
    ([ArAccountId]);
GO

-- Creating foreign key on [ArAccntTermStatusId] in table 'ArAccntTerms'
ALTER TABLE [dbo].[ArAccntTerms]
ADD CONSTRAINT [FK_ArAccntTermStatusArAccntPaymentTerm]
    FOREIGN KEY ([ArAccntTermStatusId])
    REFERENCES [dbo].[ArAccntTermStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArAccntTermStatusArAccntPaymentTerm'
CREATE INDEX [IX_FK_ArAccntTermStatusArAccntPaymentTerm]
ON [dbo].[ArAccntTerms]
    ([ArAccntTermStatusId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------