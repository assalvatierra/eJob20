-- Creating table 'PreDefinedNotes'
CREATE TABLE [dbo].[PreDefinedNotes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Note] nvarchar(250)  NULL
);
GO

-- Creating table 'JobNotes'
CREATE TABLE [dbo].[JobNotes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [Sort] int  NULL,
    [Note] nvarchar(250)  NULL
);
GO

-- Creating primary key on [Id] in table 'PreDefinedNotes'
ALTER TABLE [dbo].[PreDefinedNotes]
ADD CONSTRAINT [PK_PreDefinedNotes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobNotes'
ALTER TABLE [dbo].[JobNotes]
ADD CONSTRAINT [PK_JobNotes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [JobMainId] in table 'JobNotes'
ALTER TABLE [dbo].[JobNotes]
ADD CONSTRAINT [FK_JobMainJobNote]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobNote'
CREATE INDEX [IX_FK_JobMainJobNote]
ON [dbo].[JobNotes]
    ([JobMainId]);
GO
