﻿

ALTER TABLE [dbo].[CustEntities]
ADD CustAssocTypeId int NOT NULL DEFAULT(1)

-- Creating table 'CustAssocTypes'
CREATE TABLE [dbo].[CustAssocTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL
);


-- Creating primary key on [Id] in table 'CustAssocTypes'
ALTER TABLE [dbo].[CustAssocTypes]
ADD CONSTRAINT [PK_CustAssocTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);


    
-- Creating foreign key on [CustAssocTypeId] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [FK_CustAssocTypeCustEntity]
    FOREIGN KEY ([CustAssocTypeId])
    REFERENCES [dbo].[CustAssocTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustAssocTypeCustEntity'
CREATE INDEX [IX_FK_CustAssocTypeCustEntity]
ON [dbo].[CustEntities]
    ([CustAssocTypeId]);