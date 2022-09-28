-- Creating table 'CarCategories'
CREATE TABLE [dbo].[CarCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Remarks] nvarchar(max)  NULL
);

-- Creating table 'CarUnits'
CREATE TABLE [dbo].[CarUnits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Remarks] nvarchar(max)  NULL,
    [CarCategoryId] int  NOT NULL
);

-- Creating table 'CarDestinations'
CREATE TABLE [dbo].[CarDestinations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CityId] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Kms] int  NOT NULL
);

-- Creating table 'CarRates'
CREATE TABLE [dbo].[CarRates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Daily] decimal(18,0)  NOT NULL,
    [Weekly] decimal(18,0)  NOT NULL,
    [Monthly] decimal(18,0)  NOT NULL,
    [KmFree] int  NOT NULL,
    [KmRate] int  NOT NULL,
    [CarUnitId] int  NOT NULL,
    [OtRate] decimal(18,0)  NOT NULL
);

-- Creating table 'CarReservations'
CREATE TABLE [dbo].[CarReservations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtTrx] datetime  NOT NULL,
    [CarUnitId] int  NOT NULL,
    [DtStart] nvarchar(max)  NOT NULL,
    [LocStart] nvarchar(max)  NULL,
    [DtEnd] nvarchar(max)  NOT NULL,
    [LocEnd] nvarchar(max)  NULL,
    [BaseRate] nvarchar(max)  NOT NULL,
    [Destinations] nvarchar(max)  NOT NULL,
    [UseFor] nvarchar(max)  NOT NULL,
    [RenterName] nvarchar(max)  NOT NULL,
    [RenterCompany] nvarchar(max)  NULL,
    [RenterEmail] nvarchar(max)  NOT NULL,
    [RenterMobile] nvarchar(max)  NOT NULL,
    [RenterAddress] nvarchar(max)  NULL,
    [RenterFbAccnt] nvarchar(max)  NULL,
    [RenterLinkedInAccnt] nvarchar(max)  NULL
);

-- Creating primary key on [Id] in table 'CarCategories'
ALTER TABLE [dbo].[CarCategories]
ADD CONSTRAINT [PK_CarCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'CarUnits'
ALTER TABLE [dbo].[CarUnits]
ADD CONSTRAINT [PK_CarUnits]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'CarDestinations'
ALTER TABLE [dbo].[CarDestinations]
ADD CONSTRAINT [PK_CarDestinations]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'CarRates'
ALTER TABLE [dbo].[CarRates]
ADD CONSTRAINT [PK_CarRates]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'CarReservations'
ALTER TABLE [dbo].[CarReservations]
ADD CONSTRAINT [PK_CarReservations]
    PRIMARY KEY CLUSTERED ([Id] ASC);

-- Creating foreign key on [CarCategoryId] in table 'CarUnits'
ALTER TABLE [dbo].[CarUnits]
ADD CONSTRAINT [FK_CarCategoryCarUnit]
    FOREIGN KEY ([CarCategoryId])
    REFERENCES [dbo].[CarCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_CarCategoryCarUnit'
CREATE INDEX [IX_FK_CarCategoryCarUnit]
ON [dbo].[CarUnits]
    ([CarCategoryId]);


-- Creating foreign key on [CityId] in table 'CarDestinations'
ALTER TABLE [dbo].[CarDestinations]
ADD CONSTRAINT [FK_CityCarDestination]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_CityCarDestination'
CREATE INDEX [IX_FK_CityCarDestination]
ON [dbo].[CarDestinations]
    ([CityId]);


-- Creating foreign key on [CarUnitId] in table 'CarRates'
ALTER TABLE [dbo].[CarRates]
ADD CONSTRAINT [FK_CarUnitCarRate]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarRate'
CREATE INDEX [IX_FK_CarUnitCarRate]
ON [dbo].[CarRates]
    ([CarUnitId]);


-- Creating foreign key on [CarUnitId] in table 'CarReservations'
ALTER TABLE [dbo].[CarReservations]
ADD CONSTRAINT [FK_CarUnitCarReservation]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarReservation'
CREATE INDEX [IX_FK_CarUnitCarReservation]
ON [dbo].[CarReservations]
    ([CarUnitId]);

