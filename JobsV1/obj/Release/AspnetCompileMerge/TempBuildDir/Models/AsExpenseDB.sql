IF OBJECT_ID(N'[dbo].[FK_AsCostCenterAsExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsExpenses] DROP CONSTRAINT [FK_AsCostCenterAsExpense];

IF OBJECT_ID(N'[dbo].[FK_AsExpCategoryAsExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsExpenses] DROP CONSTRAINT [FK_AsExpCategoryAsExpense];

IF OBJECT_ID(N'[dbo].[FK_AsExpBillerAsExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsExpenses] DROP CONSTRAINT [FK_AsExpBillerAsExpense];

IF OBJECT_ID(N'[dbo].[AsExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsExpenses];

IF OBJECT_ID(N'[dbo].[AsExpCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsExpCategories];

IF OBJECT_ID(N'[dbo].[AsExpBillers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsExpBillers];

IF OBJECT_ID(N'[dbo].[AsCostCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsCostCenters];


CREATE TABLE [dbo].[AsExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TrxDate] nvarchar(max)  NOT NULL,
    [TrxDesc] nvarchar(max)  NOT NULL,
    [Amount] nvarchar(max)  NOT NULL,
    [TrxRemarks] nvarchar(max)  NULL,
    [DateEntered] nvarchar(max)  NOT NULL,
    [AsCostCenterId] int  NOT NULL,
    [AsExpCategoryId] int  NOT NULL,
    [AsExpBillerId] int  NOT NULL
);

CREATE TABLE [dbo].[AsExpCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NULL
);

CREATE TABLE [dbo].[AsExpBillers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ShortName] nvarchar(max)  NOT NULL,
    [FullName] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [Contact2] nvarchar(max)  NULL,
    [Remarks] nvarchar(max)  NULL
);

CREATE TABLE [dbo].[AsCostCenters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ccName] nvarchar(max)  NOT NULL,
    [xxRemarks] nvarchar(max)  NULL
);


ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [PK_AsExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);



ALTER TABLE [dbo].[AsExpCategories]
ADD CONSTRAINT [PK_AsExpCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);


ALTER TABLE [dbo].[AsExpBillers]
ADD CONSTRAINT [PK_AsExpBillers]
    PRIMARY KEY CLUSTERED ([Id] ASC);



ALTER TABLE [dbo].[AsCostCenters]
ADD CONSTRAINT [PK_AsCostCenters]
    PRIMARY KEY CLUSTERED ([Id] ASC);


ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [FK_AsCostCenterAsExpense]
    FOREIGN KEY ([AsCostCenterId])
    REFERENCES [dbo].[AsCostCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;



CREATE INDEX [IX_FK_AsCostCenterAsExpense]
ON [dbo].[AsExpenses]
    ([AsCostCenterId]);



ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [FK_AsExpCategoryAsExpense]
    FOREIGN KEY ([AsExpCategoryId])
    REFERENCES [dbo].[AsExpCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;



CREATE INDEX [IX_FK_AsExpCategoryAsExpense]
ON [dbo].[AsExpenses]
    ([AsExpCategoryId]);



ALTER TABLE [dbo].[AsExpenses]
ADD CONSTRAINT [FK_AsExpBillerAsExpense]
    FOREIGN KEY ([AsExpBillerId])
    REFERENCES [dbo].[AsExpBillers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;



CREATE INDEX [IX_FK_AsExpBillerAsExpense]
ON [dbo].[AsExpenses]
    ([AsExpBillerId]);

