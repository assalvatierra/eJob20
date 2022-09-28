

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CustomerCustCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustCats] DROP CONSTRAINT [FK_CustomerCustCat];
GO
IF OBJECT_ID(N'[dbo].[FK_CustCategoryCustCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustCats] DROP CONSTRAINT [FK_CustCategoryCustCat];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerSalesLead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeads] DROP CONSTRAINT [FK_CustomerSalesLead];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesActCodeSalesActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesActivities] DROP CONSTRAINT [FK_SalesActCodeSalesActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesActivities] DROP CONSTRAINT [FK_SalesLeadSalesActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntities] DROP CONSTRAINT [FK_CustEntMainCustEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntities] DROP CONSTRAINT [FK_CustomerCustEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesActStatusSalesActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesActivities] DROP CONSTRAINT [FK_SalesActStatusSalesActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadCatCodeSalesLeadCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadCategories] DROP CONSTRAINT [FK_SalesLeadCatCodeSalesLeadCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesLeadCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadCategories] DROP CONSTRAINT [FK_SalesLeadSalesLeadCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadCatCodeCustSalesCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustSalesCategories] DROP CONSTRAINT [FK_SalesLeadCatCodeCustSalesCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustSalesCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustSalesCategories] DROP CONSTRAINT [FK_CustomerCustSalesCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesStatusCodeSalesStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesStatus] DROP CONSTRAINT [FK_SalesStatusCodeSalesStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesStatus] DROP CONSTRAINT [FK_SalesLeadSalesStatus];
GO
-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------
IF OBJECT_ID(N'[dbo].[CustCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustCategories];
GO
IF OBJECT_ID(N'[dbo].[CustCats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustCats];
GO
IF OBJECT_ID(N'[dbo].[CustEntMains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntMains];
GO
IF OBJECT_ID(N'[dbo].[SalesLeads]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeads];
GO
IF OBJECT_ID(N'[dbo].[SalesStatusCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesStatusCodes];
GO
IF OBJECT_ID(N'[dbo].[SalesStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesStatus];
GO
IF OBJECT_ID(N'[dbo].[SalesActCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesActCodes];
GO
IF OBJECT_ID(N'[dbo].[SalesActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesActivities];
GO
IF OBJECT_ID(N'[dbo].[CustEntities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntities];
GO
IF OBJECT_ID(N'[dbo].[SalesActStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesActStatus];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadCatCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadCatCodes];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadCategories];
GO
IF OBJECT_ID(N'[dbo].[CustSalesCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustSalesCategories];
GO
IF OBJECT_ID(N'[dbo].[CustFileRefs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustFileRefs];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadLinks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadLinks];
GO
-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------
-- Creating table 'CustCategories'
CREATE TABLE [dbo].[CustCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'CustCats'
CREATE TABLE [dbo].[CustCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [CustCategoryId] int  NOT NULL
);
GO

-- Creating table 'CustEntMains'
CREATE TABLE [dbo].[CustEntMains] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Address] nvarchar(180)  NULL,
    [Contact1] nvarchar(80)  NULL,
    [Contact2] nvarchar(80)  NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesLeads'
CREATE TABLE [dbo].[SalesLeads] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Details] nvarchar(250)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [CustomerId] int  NOT NULL,
    [CustName] nvarchar(80)  NULL,
    [DtEntered] datetime  NOT NULL,
    [EnteredBy] nvarchar(80)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [AssignedTo] nvarchar(80)  NULL,
    [CustPhone] nvarchar(20)  NULL,
    [CustEmail] nvarchar(80)  NULL
);
GO

-- Creating table 'SalesStatusCodes'
CREATE TABLE [dbo].[SalesStatusCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SeqNo] int  NULL,
    [Name] nvarchar(80)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesStatus'
CREATE TABLE [dbo].[SalesStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtStatus] datetime  NOT NULL,
    [SalesStatusCodeId] int  NOT NULL,
    [SalesLeadId] int  NOT NULL
);
GO

-- Creating table 'SalesActCodes'
CREATE TABLE [dbo].[SalesActCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Desc] nvarchar(40)  NOT NULL,
    [SysCode] nvarchar(20)  NULL,
    [iconPath] nvarchar(150)  NULL,
    [DefaultActStatus] int  NULL
);
GO

-- Creating table 'SalesActivities'
CREATE TABLE [dbo].[SalesActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadId] int  NOT NULL,
    [SalesActCodeId] int  NOT NULL,
    [Particulars] nvarchar(250)  NOT NULL,
    [DtActivity] datetime  NOT NULL,
    [SalesActStatusId] int  NOT NULL,
    [DtEntered] datetime  NOT NULL,
    [EnteredBy] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'CustEntities'
CREATE TABLE [dbo].[CustEntities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'SalesActStatus'
CREATE TABLE [dbo].[SalesActStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesLeadCatCodes'
CREATE TABLE [dbo].[SalesLeadCatCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CatName] nvarchar(80)  NOT NULL,
    [SysCode] nvarchar(20)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesLeadCategories'
CREATE TABLE [dbo].[SalesLeadCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadCatCodeId] int  NOT NULL,
    [SalesLeadId] int  NOT NULL
);
GO

-- Creating table 'CustSalesCategories'
CREATE TABLE [dbo].[CustSalesCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadCatCodeId] int  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'CustFileRefs'
CREATE TABLE [dbo].[CustFileRefs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RefTable] nvarchar(80)  NOT NULL,
    [RefId] int  NOT NULL,
    [CustFilesId] int  NOT NULL
);
GO

-- Creating table 'SalesLeadLinks'
CREATE TABLE [dbo].[SalesLeadLinks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadId] int  NOT NULL,
    [JobMainId] int  NOT NULL
);




-- Creating primary key on [Id] in table 'CustCategories'
ALTER TABLE [dbo].[CustCategories]
ADD CONSTRAINT [PK_CustCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustCats'
ALTER TABLE [dbo].[CustCats]
ADD CONSTRAINT [PK_CustCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntMains'
ALTER TABLE [dbo].[CustEntMains]
ADD CONSTRAINT [PK_CustEntMains]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeads'
ALTER TABLE [dbo].[SalesLeads]
ADD CONSTRAINT [PK_SalesLeads]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesStatusCodes'
ALTER TABLE [dbo].[SalesStatusCodes]
ADD CONSTRAINT [PK_SalesStatusCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [PK_SalesStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesActCodes'
ALTER TABLE [dbo].[SalesActCodes]
ADD CONSTRAINT [PK_SalesActCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [PK_SalesActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [PK_CustEntities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesActStatus'
ALTER TABLE [dbo].[SalesActStatus]
ADD CONSTRAINT [PK_SalesActStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadCatCodes'
ALTER TABLE [dbo].[SalesLeadCatCodes]
ADD CONSTRAINT [PK_SalesLeadCatCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadCategories'
ALTER TABLE [dbo].[SalesLeadCategories]
ADD CONSTRAINT [PK_SalesLeadCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustSalesCategories'
ALTER TABLE [dbo].[CustSalesCategories]
ADD CONSTRAINT [PK_CustSalesCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating primary key on [Id] in table 'CustFileRefs'
ALTER TABLE [dbo].[CustFileRefs]
ADD CONSTRAINT [PK_CustFileRefs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadLinks'
ALTER TABLE [dbo].[SalesLeadLinks]
ADD CONSTRAINT [PK_SalesLeadLinks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO



-- Creating foreign key on [CustomerId] in table 'CustCats'
ALTER TABLE [dbo].[CustCats]
ADD CONSTRAINT [FK_CustomerCustCat]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustCat'
CREATE INDEX [IX_FK_CustomerCustCat]
ON [dbo].[CustCats]
    ([CustomerId]);
GO

-- Creating foreign key on [CustCategoryId] in table 'CustCats'
ALTER TABLE [dbo].[CustCats]
ADD CONSTRAINT [FK_CustCategoryCustCat]
    FOREIGN KEY ([CustCategoryId])
    REFERENCES [dbo].[CustCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustCategoryCustCat'
CREATE INDEX [IX_FK_CustCategoryCustCat]
ON [dbo].[CustCats]
    ([CustCategoryId]);
GO

-- Creating foreign key on [CustomerId] in table 'SalesLeads'
ALTER TABLE [dbo].[SalesLeads]
ADD CONSTRAINT [FK_CustomerSalesLead]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerSalesLead'
CREATE INDEX [IX_FK_CustomerSalesLead]
ON [dbo].[SalesLeads]
    ([CustomerId]);
GO

-- Creating foreign key on [SalesActCodeId] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [FK_SalesActCodeSalesActivity]
    FOREIGN KEY ([SalesActCodeId])
    REFERENCES [dbo].[SalesActCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesActCodeSalesActivity'
CREATE INDEX [IX_FK_SalesActCodeSalesActivity]
ON [dbo].[SalesActivities]
    ([SalesActCodeId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [FK_SalesLeadSalesActivity]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesActivity'
CREATE INDEX [IX_FK_SalesLeadSalesActivity]
ON [dbo].[SalesActivities]
    ([SalesLeadId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [FK_CustEntMainCustEntity]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntity'
CREATE INDEX [IX_FK_CustEntMainCustEntity]
ON [dbo].[CustEntities]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [FK_CustomerCustEntity]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustEntity'
CREATE INDEX [IX_FK_CustomerCustEntity]
ON [dbo].[CustEntities]
    ([CustomerId]);
GO

-- Creating foreign key on [SalesActStatusId] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [FK_SalesActStatusSalesActivity]
    FOREIGN KEY ([SalesActStatusId])
    REFERENCES [dbo].[SalesActStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesActStatusSalesActivity'
CREATE INDEX [IX_FK_SalesActStatusSalesActivity]
ON [dbo].[SalesActivities]
    ([SalesActStatusId]);
GO

-- Creating foreign key on [SalesLeadCatCodeId] in table 'SalesLeadCategories'
ALTER TABLE [dbo].[SalesLeadCategories]
ADD CONSTRAINT [FK_SalesLeadCatCodeSalesLeadCategory]
    FOREIGN KEY ([SalesLeadCatCodeId])
    REFERENCES [dbo].[SalesLeadCatCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadCatCodeSalesLeadCategory'
CREATE INDEX [IX_FK_SalesLeadCatCodeSalesLeadCategory]
ON [dbo].[SalesLeadCategories]
    ([SalesLeadCatCodeId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesLeadCategories'
ALTER TABLE [dbo].[SalesLeadCategories]
ADD CONSTRAINT [FK_SalesLeadSalesLeadCategory]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesLeadCategory'
CREATE INDEX [IX_FK_SalesLeadSalesLeadCategory]
ON [dbo].[SalesLeadCategories]
    ([SalesLeadId]);
GO

-- Creating foreign key on [SalesLeadCatCodeId] in table 'CustSalesCategories'
ALTER TABLE [dbo].[CustSalesCategories]
ADD CONSTRAINT [FK_SalesLeadCatCodeCustSalesCategory]
    FOREIGN KEY ([SalesLeadCatCodeId])
    REFERENCES [dbo].[SalesLeadCatCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadCatCodeCustSalesCategory'
CREATE INDEX [IX_FK_SalesLeadCatCodeCustSalesCategory]
ON [dbo].[CustSalesCategories]
    ([SalesLeadCatCodeId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustSalesCategories'
ALTER TABLE [dbo].[CustSalesCategories]
ADD CONSTRAINT [FK_CustomerCustSalesCategory]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustSalesCategory'
CREATE INDEX [IX_FK_CustomerCustSalesCategory]
ON [dbo].[CustSalesCategories]
    ([CustomerId]);
GO

-- Creating foreign key on [SalesStatusCodeId] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [FK_SalesStatusCodeSalesStatus]
    FOREIGN KEY ([SalesStatusCodeId])
    REFERENCES [dbo].[SalesStatusCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesStatusCodeSalesStatus'
CREATE INDEX [IX_FK_SalesStatusCodeSalesStatus]
ON [dbo].[SalesStatus]
    ([SalesStatusCodeId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [FK_SalesLeadSalesStatus]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesStatus'
CREATE INDEX [IX_FK_SalesLeadSalesStatus]
ON [dbo].[SalesStatus]
    ([SalesLeadId]);
GO
-- Creating foreign key on [CustFilesId] in table 'CustFileRefs'
ALTER TABLE [dbo].[CustFileRefs]
ADD CONSTRAINT [FK_CustFilesCustFileRef]
    FOREIGN KEY ([CustFilesId])
    REFERENCES [dbo].[CustFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustFilesCustFileRef'
CREATE INDEX [IX_FK_CustFilesCustFileRef]
ON [dbo].[CustFileRefs]
    ([CustFilesId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesLeadLinks'
ALTER TABLE [dbo].[SalesLeadLinks]
ADD CONSTRAINT [FK_SalesLeadSalesLeadLink]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesLeadLink'
CREATE INDEX [IX_FK_SalesLeadSalesLeadLink]
ON [dbo].[SalesLeadLinks]
    ([SalesLeadId]);
GO

-- Creating foreign key on [JobMainId] in table 'SalesLeadLinks'
ALTER TABLE [dbo].[SalesLeadLinks]
ADD CONSTRAINT [FK_JobMainSalesLeadLink]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainSalesLeadLink'
CREATE INDEX [IX_FK_JobMainSalesLeadLink]
ON [dbo].[SalesLeadLinks]
    ([JobMainId]);
GO

