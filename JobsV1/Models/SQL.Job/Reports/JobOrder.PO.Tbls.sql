-- Creating table 'SrvActionCodes'
CREATE TABLE [dbo].[SrvActionCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CatCode] nvarchar(80)  NOT NULL,
    [SortNo] int  NOT NULL
);
GO

-- Creating table 'SrvActionItems'
CREATE TABLE [dbo].[SrvActionItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(180)  NULL,
    [SortNo] int  NOT NULL,
    [ServicesId] int  NOT NULL,
    [SrvActionCodeId] int  NOT NULL
);
GO


-- Creating table 'JobActions'
CREATE TABLE [dbo].[JobActions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobServicesId] int  NOT NULL,
    [AssignedTo] nvarchar(80)  NULL,
    [PerformedBy] nvarchar(80)  NULL,
    [DtAssigned] datetime  NULL,
    [DtPerformed] datetime  NULL,
    [SrvActionItemId] int  NOT NULL,
    [Remarks] nvarchar(150)  NULL
);
GO

-- Creating table 'InvItems'
CREATE TABLE [dbo].[InvItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemCode] nvarchar(30)  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [ImgPath] nvarchar(80)  NULL
);
GO

-- Creating table 'InvItemCats'
CREATE TABLE [dbo].[InvItemCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [ImgPath] nvarchar(150)  NULL,
    [SysCode] nvarchar(20)  NULL
);
GO

-- Creating table 'InvItemCategories'
CREATE TABLE [dbo].[InvItemCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemCatId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'JobServiceItems'
CREATE TABLE [dbo].[JobServiceItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobServicesId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'SupplierInvItems'
CREATE TABLE [dbo].[SupplierInvItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'JobNotificationRequests'
CREATE TABLE [dbo].[JobNotificationRequests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ServiceId] int  NOT NULL,
    [ReqDt] datetime  NOT NULL,
    [RefId] nvarchar(20)  NULL
);
GO

-- Creating table 'CustFiles'
CREATE TABLE [dbo].[CustFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(180)  NOT NULL,
    [Folder] nvarchar(150)  NOT NULL,
    [Path] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(180)  NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'SupplierPoHdrs'
CREATE TABLE [dbo].[SupplierPoHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PoDate] datetime  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [SupplierId] int  NOT NULL,
    [SupplierPoStatusId] int  NOT NULL,
    [RequestBy] nvarchar(max)  NOT NULL,
    [DtRequest] datetime  NOT NULL,
    [ApprovedBy] nvarchar(80)  NULL,
    [DtApproved] datetime  NULL
);
GO

-- Creating table 'SupplierPoDtls'
CREATE TABLE [dbo].[SupplierPoDtls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierPoHdrId] int  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [JobServicesId] int  NOT NULL
);
GO

-- Creating table 'SupplierPoStatus'
CREATE TABLE [dbo].[SupplierPoStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [OrderNo] int  NOT NULL
);
GO

-- Creating table 'SupplierPoItems'
CREATE TABLE [dbo].[SupplierPoItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierPoDtlId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'SrvActionCodes'
ALTER TABLE [dbo].[SrvActionCodes]
ADD CONSTRAINT [PK_SrvActionCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SrvActionItems'
ALTER TABLE [dbo].[SrvActionItems]
ADD CONSTRAINT [PK_SrvActionItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'JobActions'
ALTER TABLE [dbo].[JobActions]
ADD CONSTRAINT [PK_JobActions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [PK_InvItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemCats'
ALTER TABLE [dbo].[InvItemCats]
ADD CONSTRAINT [PK_InvItemCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemCategories'
ALTER TABLE [dbo].[InvItemCategories]
ADD CONSTRAINT [PK_InvItemCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobServiceItems'
ALTER TABLE [dbo].[JobServiceItems]
ADD CONSTRAINT [PK_JobServiceItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierInvItems'
ALTER TABLE [dbo].[SupplierInvItems]
ADD CONSTRAINT [PK_SupplierInvItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobNotificationRequests'
ALTER TABLE [dbo].[JobNotificationRequests]
ADD CONSTRAINT [PK_JobNotificationRequests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustFiles'
ALTER TABLE [dbo].[CustFiles]
ADD CONSTRAINT [PK_CustFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoHdrs'
ALTER TABLE [dbo].[SupplierPoHdrs]
ADD CONSTRAINT [PK_SupplierPoHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoDtls'
ALTER TABLE [dbo].[SupplierPoDtls]
ADD CONSTRAINT [PK_SupplierPoDtls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoStatus'
ALTER TABLE [dbo].[SupplierPoStatus]
ADD CONSTRAINT [PK_SupplierPoStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoItems'
ALTER TABLE [dbo].[SupplierPoItems]
ADD CONSTRAINT [PK_SupplierPoItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [JobServicesId] in table 'JobActions'
ALTER TABLE [dbo].[JobActions]
ADD CONSTRAINT [FK_JobServicesJobAction]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesJobAction'
CREATE INDEX [IX_FK_JobServicesJobAction]
ON [dbo].[JobActions]
    ([JobServicesId]);
GO

-- Creating foreign key on [InvItemCatId] in table 'InvItemCategories'
ALTER TABLE [dbo].[InvItemCategories]
ADD CONSTRAINT [FK_InvItemCatInvItemCategory]
    FOREIGN KEY ([InvItemCatId])
    REFERENCES [dbo].[InvItemCats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemCatInvItemCategory'
CREATE INDEX [IX_FK_InvItemCatInvItemCategory]
ON [dbo].[InvItemCategories]
    ([InvItemCatId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvItemCategories'
ALTER TABLE [dbo].[InvItemCategories]
ADD CONSTRAINT [FK_InvItemInvItemCategory]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemCategory'
CREATE INDEX [IX_FK_InvItemInvItemCategory]
ON [dbo].[InvItemCategories]
    ([InvItemId]);
GO

-- Creating foreign key on [JobServicesId] in table 'JobServiceItems'
ALTER TABLE [dbo].[JobServiceItems]
ADD CONSTRAINT [FK_JobServicesJobServiceItem]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesJobServiceItem'
CREATE INDEX [IX_FK_JobServicesJobServiceItem]
ON [dbo].[JobServiceItems]
    ([JobServicesId]);
GO

-- Creating foreign key on [InvItemId] in table 'JobServiceItems'
ALTER TABLE [dbo].[JobServiceItems]
ADD CONSTRAINT [FK_InvItemJobServiceItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemJobServiceItem'
CREATE INDEX [IX_FK_InvItemJobServiceItem]
ON [dbo].[JobServiceItems]
    ([InvItemId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierInvItems'
ALTER TABLE [dbo].[SupplierInvItems]
ADD CONSTRAINT [FK_SupplierSupplierInvItem]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierInvItem'
CREATE INDEX [IX_FK_SupplierSupplierInvItem]
ON [dbo].[SupplierInvItems]
    ([SupplierId]);
GO

-- Creating foreign key on [InvItemId] in table 'SupplierInvItems'
ALTER TABLE [dbo].[SupplierInvItems]
ADD CONSTRAINT [FK_InvItemSupplierInvItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSupplierInvItem'
CREATE INDEX [IX_FK_InvItemSupplierInvItem]
ON [dbo].[SupplierInvItems]
    ([InvItemId]);
GO

-- Creating foreign key on [ServicesId] in table 'SrvActionItems'
ALTER TABLE [dbo].[SrvActionItems]
ADD CONSTRAINT [FK_ServicesSrvActionItem]
    FOREIGN KEY ([ServicesId])
    REFERENCES [dbo].[Services]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServicesSrvActionItem'
CREATE INDEX [IX_FK_ServicesSrvActionItem]
ON [dbo].[SrvActionItems]
    ([ServicesId]);
GO

-- Creating foreign key on [SrvActionItemId] in table 'JobActions'
ALTER TABLE [dbo].[JobActions]
ADD CONSTRAINT [FK_SrvActionItemJobAction]
    FOREIGN KEY ([SrvActionItemId])
    REFERENCES [dbo].[SrvActionItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SrvActionItemJobAction'
CREATE INDEX [IX_FK_SrvActionItemJobAction]
ON [dbo].[JobActions]
    ([SrvActionItemId]);
GO

-- Creating foreign key on [SrvActionCodeId] in table 'SrvActionItems'
ALTER TABLE [dbo].[SrvActionItems]
ADD CONSTRAINT [FK_SrvActionCodeSrvActionItem]
    FOREIGN KEY ([SrvActionCodeId])
    REFERENCES [dbo].[SrvActionCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SrvActionCodeSrvActionItem'
CREATE INDEX [IX_FK_SrvActionCodeSrvActionItem]
ON [dbo].[SrvActionItems]
    ([SrvActionCodeId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustFiles'
ALTER TABLE [dbo].[CustFiles]
ADD CONSTRAINT [FK_CustomerCustFiles]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustFiles'
CREATE INDEX [IX_FK_CustomerCustFiles]
ON [dbo].[CustFiles]
    ([CustomerId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierPoHdrs'
ALTER TABLE [dbo].[SupplierPoHdrs]
ADD CONSTRAINT [FK_SupplierSupplierPoHdr]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierPoHdr'
CREATE INDEX [IX_FK_SupplierSupplierPoHdr]
ON [dbo].[SupplierPoHdrs]
    ([SupplierId]);
GO

-- Creating foreign key on [SupplierPoStatusId] in table 'SupplierPoHdrs'
ALTER TABLE [dbo].[SupplierPoHdrs]
ADD CONSTRAINT [FK_SupplierPoStatusSupplierPoHdr]
    FOREIGN KEY ([SupplierPoStatusId])
    REFERENCES [dbo].[SupplierPoStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierPoStatusSupplierPoHdr'
CREATE INDEX [IX_FK_SupplierPoStatusSupplierPoHdr]
ON [dbo].[SupplierPoHdrs]
    ([SupplierPoStatusId]);
GO

-- Creating foreign key on [SupplierPoHdrId] in table 'SupplierPoDtls'
ALTER TABLE [dbo].[SupplierPoDtls]
ADD CONSTRAINT [FK_SupplierPoHdrSupplierPoDtl]
    FOREIGN KEY ([SupplierPoHdrId])
    REFERENCES [dbo].[SupplierPoHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierPoHdrSupplierPoDtl'
CREATE INDEX [IX_FK_SupplierPoHdrSupplierPoDtl]
ON [dbo].[SupplierPoDtls]
    ([SupplierPoHdrId]);
GO

-- Creating foreign key on [JobServicesId] in table 'SupplierPoDtls'
ALTER TABLE [dbo].[SupplierPoDtls]
ADD CONSTRAINT [FK_JobServicesSupplierPoDtl]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesSupplierPoDtl'
CREATE INDEX [IX_FK_JobServicesSupplierPoDtl]
ON [dbo].[SupplierPoDtls]
    ([JobServicesId]);
GO

-- Creating foreign key on [SupplierPoDtlId] in table 'SupplierPoItems'
ALTER TABLE [dbo].[SupplierPoItems]
ADD CONSTRAINT [FK_SupplierPoDtlSupplierPoItem]
    FOREIGN KEY ([SupplierPoDtlId])
    REFERENCES [dbo].[SupplierPoDtls]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierPoDtlSupplierPoItem'
CREATE INDEX [IX_FK_SupplierPoDtlSupplierPoItem]
ON [dbo].[SupplierPoItems]
    ([SupplierPoDtlId]);
GO

-- Creating foreign key on [InvItemId] in table 'SupplierPoItems'
ALTER TABLE [dbo].[SupplierPoItems]
ADD CONSTRAINT [FK_InvItemSupplierPoItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSupplierPoItem'
CREATE INDEX [IX_FK_InvItemSupplierPoItem]
ON [dbo].[SupplierPoItems]
    ([InvItemId]);
GO
