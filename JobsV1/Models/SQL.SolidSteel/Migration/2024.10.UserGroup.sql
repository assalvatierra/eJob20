
-- Creating table 'DataGroups'
CREATE TABLE [dbo].[DataGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(20)  NOT NULL
);

-- Creating primary key on [Id] in table 'DataGroups'
ALTER TABLE [dbo].[DataGroups]
ADD CONSTRAINT [PK_DataGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);


INSERT INTO [dbo].[DataGroups] (Name, Remarks) 
VALUES ('Default' , 'No Group / Public') 


ALTER TABLE [dbo].[CustEntMains] 
ADD [DataGroupId] int  NOT NULL DEFAULT(1)

-- Creating table 'DataGroupAssigns'
CREATE TABLE [dbo].[DataGroupAssigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [User] nvarchar(80)  NOT NULL,
    [DataGroupId] int  NOT NULL
);

-- Creating primary key on [Id] in table 'DataGroupAssigns'
ALTER TABLE [dbo].[DataGroupAssigns]
ADD CONSTRAINT [PK_DataGroupAssigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);

-- Creating non-clustered index for FOREIGN KEY 'FK_DataGroupCustEntMain'
CREATE INDEX [IX_FK_DataGroupCustEntMain]
ON [dbo].[CustEntMains]
    ([DataGroupId]);