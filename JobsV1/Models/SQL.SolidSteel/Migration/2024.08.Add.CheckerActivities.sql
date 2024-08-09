
-- Add Checker Activities
-- Date: Aug 07 2024

-- Creating table 'CheckerActivities'
CREATE TABLE [dbo].[CheckerActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtActivity] datetime  NOT NULL,
    [CheckedBy] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [SalesLeadId] int  NOT NULL,
    [CheckerActivityTypeId] int  NOT NULL
);


-- Creating table 'CheckerActivityTypes'
CREATE TABLE [dbo].[CheckerActivityTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(40)  NOT NULL,
    [iconPath] nvarchar(80)  NOT NULL,
    [Points] int  NOT NULL,
    [OrderNo] int  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);


-- Creating primary key on [Id] in table 'CheckerActivities'
ALTER TABLE [dbo].[CheckerActivities]
ADD CONSTRAINT [PK_CheckerActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CheckerActivityTypes'
ALTER TABLE [dbo].[CheckerActivityTypes]
ADD CONSTRAINT [PK_CheckerActivityTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [SalesLeadId] in table 'CheckerActivities'
ALTER TABLE [dbo].[CheckerActivities]
ADD CONSTRAINT [FK_SalesLeadCheckerActivity]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadCheckerActivity'
CREATE INDEX [IX_FK_SalesLeadCheckerActivity]
ON [dbo].[CheckerActivities]
    ([SalesLeadId]);
GO

-- Creating foreign key on [CheckerActivityTypeId] in table 'CheckerActivities'
ALTER TABLE [dbo].[CheckerActivities]
ADD CONSTRAINT [FK_CheckerActivityTypeCheckerActivity]
    FOREIGN KEY ([CheckerActivityTypeId])
    REFERENCES [dbo].[CheckerActivityTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CheckerActivityTypeCheckerActivity'
CREATE INDEX [IX_FK_CheckerActivityTypeCheckerActivity]
ON [dbo].[CheckerActivities]
    ([CheckerActivityTypeId]);
GO
