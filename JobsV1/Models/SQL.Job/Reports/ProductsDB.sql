


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(30)  NOT NULL,
    [TemplateId] int  NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [Sort] int  NOT NULL,
    [Status] nvarchar(3)  NOT NULL
);
GO

-- Creating table 'ProductPrices'
CREATE TABLE [dbo].[ProductPrices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Uom] nvarchar(20)  NOT NULL,
    [Qty] int  NOT NULL,
    [Rate] decimal(18,0)  NOT NULL,
    [Rate1] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'ProductImages1'
CREATE TABLE [dbo].[ProductImages1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Path] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'ProductConditions'
CREATE TABLE [dbo].[ProductConditions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Description] nvarchar(250)  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'ProductCategories'
CREATE TABLE [dbo].[ProductCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProductProdCats'
CREATE TABLE [dbo].[ProductProdCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductCategoryId] int  NOT NULL,
    [ProductId] int  NOT NULL
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

-- Creating primary key on [Id] in table 'ProductPrices'
ALTER TABLE [dbo].[ProductPrices]
ADD CONSTRAINT [PK_ProductPrices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductImages1'
ALTER TABLE [dbo].[ProductImages1]
ADD CONSTRAINT [PK_ProductImages1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductConditions'
ALTER TABLE [dbo].[ProductConditions]
ADD CONSTRAINT [PK_ProductConditions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductCategories'
ALTER TABLE [dbo].[ProductCategories]
ADD CONSTRAINT [PK_ProductCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductProdCats'
ALTER TABLE [dbo].[ProductProdCats]
ADD CONSTRAINT [PK_ProductProdCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------


-- Creating foreign key on [ProductId] in table 'ProductPrices'
ALTER TABLE [dbo].[ProductPrices]
ADD CONSTRAINT [FK_ProductProductPrice]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductPrice'
CREATE INDEX [IX_FK_ProductProductPrice]
ON [dbo].[ProductPrices]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductImages1'
ALTER TABLE [dbo].[ProductImages1]
ADD CONSTRAINT [FK_ProductProductImages]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductImages'
CREATE INDEX [IX_FK_ProductProductImages]
ON [dbo].[ProductImages1]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductConditions'
ALTER TABLE [dbo].[ProductConditions]
ADD CONSTRAINT [FK_ProductProductCondition]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductCondition'
CREATE INDEX [IX_FK_ProductProductCondition]
ON [dbo].[ProductConditions]
    ([ProductId]);
GO

-- Creating foreign key on [ProductCategoryId] in table 'ProductProdCats'
ALTER TABLE [dbo].[ProductProdCats]
ADD CONSTRAINT [FK_ProductCategoryProductProdCat]
    FOREIGN KEY ([ProductCategoryId])
    REFERENCES [dbo].[ProductCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategoryProductProdCat'
CREATE INDEX [IX_FK_ProductCategoryProductProdCat]
ON [dbo].[ProductProdCats]
    ([ProductCategoryId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductProdCats'
ALTER TABLE [dbo].[ProductProdCats]
ADD CONSTRAINT [FK_ProductProductProdCat]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductProdCat'
CREATE INDEX [IX_FK_ProductProductProdCat]
ON [dbo].[ProductProdCats]
    ([ProductId]);
GO
